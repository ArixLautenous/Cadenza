using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace RX_Client_WF.Services
{
    /// <summary>
    /// Bộ lọc Equalizer 10 băng tần sử dụng BiQuad Filters
    /// </summary>
    public class EqualizerProvider : ISampleProvider
    {
        private readonly ISampleProvider _source;
        private readonly BiQuadFilter[,] _filters;
        private readonly float[] _gains;
        private readonly float[] _bands = { 31, 62, 125, 250, 500, 1000, 2000, 4000, 8000, 16000 };
        private readonly int _channels;
        private bool _updated = true;

        public WaveFormat WaveFormat => _source.WaveFormat;

        public EqualizerProvider(ISampleProvider source)
        {
            _source = source;
            _channels = source.WaveFormat.Channels;
            _filters = new BiQuadFilter[_channels, _bands.Length];
            _gains = new float[_bands.Length]; // Default 0dB

            CreateFilters();
        }

        private void CreateFilters()
        {
            for (int band = 0; band < _bands.Length; band++)
            {
                for (int ch = 0; ch < _channels; ch++)
                {
                    if (band == 0)
                        _filters[ch, band] = BiQuadFilter.LowShelf(WaveFormat.SampleRate, _bands[band], 1, 0); // Bass
                    else if (band == _bands.Length - 1)
                        _filters[ch, band] = BiQuadFilter.HighShelf(WaveFormat.SampleRate, _bands[band], 1, 0); // Treble
                    else
                        _filters[ch, band] = BiQuadFilter.PeakingEQ(WaveFormat.SampleRate, _bands[band], 0.8f, 0); // Mids
                }
            }
        }

        public void UpdateGain(int bandIndex, float gainDb)
        {
            if (bandIndex < 0 || bandIndex >= _bands.Length) return;
            
            _gains[bandIndex] = gainDb;
            UpdateFilters(bandIndex);
        }

        private void UpdateFilters(int band)
        {
            // Re-create filter for specific band with new Gain
            for (int ch = 0; ch < _channels; ch++)
            {
                if (band == 0)
                    _filters[ch, band] = BiQuadFilter.LowShelf(WaveFormat.SampleRate, _bands[band], 1, _gains[band]);
                else if (band == _bands.Length - 1)
                    _filters[ch, band] = BiQuadFilter.HighShelf(WaveFormat.SampleRate, _bands[band], 1, _gains[band]);
                else
                    _filters[ch, band] = BiQuadFilter.PeakingEQ(WaveFormat.SampleRate, _bands[band], 0.8f, _gains[band]);
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = _source.Read(buffer, offset, count);

            // Apply filters pipeline
            for (int n = 0; n < samplesRead; n += _channels)
            {
                for (int ch = 0; ch < _channels; ch++)
                {
                    float sample = buffer[offset + n + ch];
                    
                    // Chạy qua chuỗi 10 bộ lọc
                    for (int band = 0; band < _bands.Length; band++)
                    {
                        sample = _filters[ch, band].Transform(sample);
                    }

                    buffer[offset + n + ch] = sample;
                }
            }

            return samplesRead;
        }
    }
}
