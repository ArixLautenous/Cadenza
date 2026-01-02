import sys
import os
import subprocess
from pathlib import Path
import warnings

# Try to set torchaudio backend to soundfile to fix Windows save error
try:
    import torchaudio
    if hasattr(torchaudio, "set_audio_backend"):
        torchaudio.set_audio_backend("soundfile")
except:
    pass

# Suppress warnings
warnings.filterwarnings("ignore")

def split_audio(input_path, output_dir):
    """
    Splits audio into vocals and instrumental using Demucs.
    Then transcribes vocals using Whisper.
    """
    input_file = Path(input_path)
    if not input_file.exists():
        print(f"Error: Input file '{input_path}' not found.")
        sys.exit(1)

    print(f"Processing: {input_file.name}...")
    
    # 1. RUN DEMUCS
    command = [
        sys.executable, "-m", "demucs",
        "-n", "htdemucs", 
        "--two-stems", "vocals",
        str(input_file),
        "-o", output_dir
    ]

    try:
        subprocess.run(command, check=True)
        
        # Initialize Separator
        sep = Separator(
            log_level='ERROR', 
            output_dir=str(track_folder),
            output_format="wav"
        )
        
        # Load ViperX Model
        print("Loading Model: model_bs_roformer_ep_317_sdr_12.9755.ckpt")
        sep.load_model(model_filename='model_bs_roformer_ep_317_sdr_12.9755.ckpt')
        
        # Separate
        print("Running inference... (Max Performance Mode)")
        output_files = sep.separate(str(input_file))
        
        # Rename output files to standard names
        for fname in output_files:
            full_path = track_folder / fname
            lower_name = fname.lower()
            
            # ViperX yields: 'filename_(Vocals)_...' and 'filename_(Instrumental)_...'
            if "(vocals)" in lower_name:
                if vocals_path.exists(): os.remove(vocals_path)
                os.rename(full_path, vocals_path)
            elif "(instrumental)" in lower_name:
                if no_vocals_path.exists(): os.remove(no_vocals_path)
                os.rename(full_path, no_vocals_path)
        
        vocals_path = track_folder / "vocals.wav"
        no_vocals_path = track_folder / "no_vocals.wav"
        
        if vocals_path.exists() and no_vocals_path.exists():
            print("Successfully separated tracks.")
            print("OUTPUT_PATHS:")
            print(f"VOCAL|{vocals_path.absolute()}")
            print(f"INSTRUMENTAL|{no_vocals_path.absolute()}")
            
            # 2. RUN WHISPER (Transcribe Vocals)
            print("-" * 30)
            print("Step 2: Transcribing lyrics with Whisper...")
            print("Note: First run might download the model (approx 150MB). Please wait...")
            try:
                import whisper
                print("Loading Whisper 'base' model...")
                model = whisper.load_model("base") 
                
                # Transcribe
                print(f"Transcribing file: {vocals_path.name}")
                # fp16=False de tranh loi tren CPU hoac GPU cu
                result = model.transcribe(str(vocals_path), fp16=False)
                lyrics_text = result["text"].strip()
                
                lyrics_file = track_folder / "lyrics.txt"
                with open(lyrics_file, "w", encoding="utf-8") as f:
                    f.write(lyrics_text)
                    
                print(f"LYRICS_FILE|{lyrics_file.absolute()}")
                
            except ImportError:
                print("Error: 'openai-whisper' python package is not installed. Skipping lyrics.")
            except Exception as w_ex:
                print(f"Whisper Transcription Error: {w_ex}")

        else:
            print("Error: Output files not found after processing.")
            sys.exit(1)

    except subprocess.CalledProcessError as e:
        print(f"Error running Demucs: {e}")
        sys.exit(1)
    except Exception as e:
        print(f"Unexpected error: {e}")
        sys.exit(1)

if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("Usage: python split_audio.py <input_audio_path> <output_directory>")
        sys.exit(1)

    input_audio = sys.argv[1]
    output_directory = sys.argv[2]
    
    split_audio(input_audio, output_directory)
