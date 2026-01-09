import sys
import os
import subprocess
from pathlib import Path
import warnings
import shutil
import traceback
import logging

# Try to set torchaudio backend to soundfile to fix Windows save error
try:
    import torchaudio
    if hasattr(torchaudio, "set_audio_backend"):
        torchaudio.set_audio_backend("soundfile")
except:
    pass

# Suppress warnings
warnings.filterwarnings("ignore")

def convert_to_wav(input_path, output_wav):
    """
    Convert input audio to standard WAV (16-bit, 44100Hz) using ffmpeg.
    This fixes 'Format not recognised' errors for m4a/mp3 etc.
    """
    try:
        command = [
            "ffmpeg", "-y",
            "-i", str(input_path),
            "-ar", "44100", # Sample rate
            "-ac", "2",     # Stereo
            "-c:a", "pcm_s16le", # Codec PCM 16-bit int
            str(output_wav)
        ]
        # Run silently
        subprocess.run(command, check=True, stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
        return True
    except Exception as e:
        print(f"FFmpeg Conversion Error: {e}")
        return False

def split_audio(input_path, output_dir):
    """
    Splits audio using BS-Roformer (ViperX) via audio-separator (OpenVINO).
    Then transcribes vocals using Whisper (Small - Auto Lang).
    Optimized for Intel Hardware (OpenVINO).
    """

    input_file = Path(input_path)
    if not input_file.exists():
        print(f"Error: Input file '{input_path}' not found.")
        sys.exit(1)

    print(f"Processing: {input_file.name}")
    print("Hardware Acceleration: OpenVINO (Intel Optimized).")
    
    # Tao folder rieng
    track_folder = Path(output_dir) / "bs_roformer" / input_file.stem
    track_folder.mkdir(parents=True, exist_ok=True)

    # Temporary WAV file for processing (Fix m4a issues)
    temp_wav_path = track_folder / "temp_input.wav"
    
    # Convert first
    print("Pre-processing: Converting to WAV...")
    if not convert_to_wav(input_file, temp_wav_path):
        print("Error: Could not convert input file to WAV. Processing might fail.")
        process_file = input_file
    else:
        process_file = temp_wav_path

    vocals_path = track_folder / "vocals.wav"
    no_vocals_path = track_folder / "no_vocals.wav"

    # --- STEP 1: SEPARATION (BS-Roformer) ---
    print("-" * 30)
    print("Step 1: Separating Audio with BS-Roformer (ViperX)...")
    
    # Logger Config
    log_handler = logging.StreamHandler(sys.stdout)
    log_handler.setLevel(logging.INFO)
    log_handler.setFormatter(logging.Formatter('[BS-Roformer] %(message)s'))
    
    logger = logging.getLogger("audio_separator")
    logger.setLevel(logging.INFO)
    logger.addHandler(log_handler)
    
    try:
        from audio_separator.separator import Separator
        
        # Initialize Separator
        # OpenVINOExecutionProvider will be auto-detected by the library/onnxruntime
        sep = Separator(
            log_level=logging.INFO,
            output_dir=str(track_folder),
            output_format="wav"
        )
        
        # Load ViperX Model
        print("Loading Model: model_bs_roformer_ep_317_sdr_12.9755.ckpt")
        sep.load_model(model_filename='model_bs_roformer_ep_317_sdr_12.9755.ckpt')
        
        # Separate
        print("Running OpenVINO Inference...")
        output_files = sep.separate(str(process_file))
        
        # Rename logic
        for fname in output_files:
            full_path = track_folder / fname
            lower_name = fname.lower()
            
            if "(vocals)" in lower_name:
                if vocals_path.exists(): os.remove(vocals_path)
                os.rename(full_path, vocals_path)
            elif "(instrumental)" in lower_name:
                if no_vocals_path.exists(): os.remove(no_vocals_path)
                os.rename(full_path, no_vocals_path)
                
    except Exception as e:
        print("BS-Roformer Error Details:")
        traceback.print_exc()
        print(f"BS-Roformer Error: {e}")
        print("Critical Error: AI Separation failed.")
        sys.exit(1)
    finally:
        # Cleanup temp wav
        if temp_wav_path.exists():
            try: os.remove(temp_wav_path)
            except: pass

    if vocals_path.exists() and no_vocals_path.exists():
        print("Successfully separated tracks.")
        print("OUTPUT_PATHS:")
        print(f"VOCAL|{vocals_path.absolute()}")
        print(f"INSTRUMENTAL|{no_vocals_path.absolute()}")
        
        # --- STEP 2: TRANSCRIPTION (Whisper) ---
        print("-" * 30)
        print("Step 2: Transcribing lyrics with Whisper (Small) on CPU...")
        try:
            import whisper
            
            # OpenVINO usually runs Whisper on CPU fairly well
            model = whisper.load_model("small", device="cpu") 
            
            print(f"Transcribing (Auto Language Detection)...")
            result = model.transcribe(
                str(vocals_path), 
                fp16=False, 
                initial_prompt="Lyrics, Lời bài hát, Song text:"
            )
            lyrics_text = result["text"].strip()
            
            lyrics_file = track_folder / "lyrics.txt"
            with open(lyrics_file, "w", encoding="utf-8") as f:
                f.write(lyrics_text)
                
            print(f"LYRICS_FILE|{lyrics_file.absolute()}")
            
        except ImportError:
             print("Error: Whisper not installed.")
        except Exception as w_ex:
            print(f"Whisper Transcription Error: {w_ex}")

    else:
        print("Error: Output files not found.")
        sys.exit(1)

if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("Usage: split_audio.py <input> <output>")
        sys.exit(1)
    split_audio(sys.argv[1], sys.argv[2])
