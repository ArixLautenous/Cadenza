
import sys
import os
import subprocess
from pathlib import Path
import warnings

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
        "demucs",
        "-n", "htdemucs", 
        "--two-stems", "vocals",
        str(input_file),
        "-o", output_dir
    ]

    try:
        subprocess.run(command, check=True)
        
        # Demucs output path logic
        model_name = "htdemucs"
        track_folder = Path(output_dir) / model_name / input_file.stem
        
        vocals_path = track_folder / "vocals.wav"
        no_vocals_path = track_folder / "no_vocals.wav"
        
        if vocals_path.exists() and no_vocals_path.exists():
            print("Successfully separated tracks.")
            print("OUTPUT_PATHS:")
            print(f"VOCAL|{vocals_path.absolute()}")
            print(f"INSTRUMENTAL|{no_vocals_path.absolute()}")
            
            # 2. RUN WHISPER (Transcribe Vocals)
            print("Transcribing lyrics with Whisper...")
            try:
                import whisper
                # Load model (base is faster, small is more accurate)
                model = whisper.load_model("base") 
                
                # Transcribe
                result = model.transcribe(str(vocals_path))
                lyrics_text = result["text"].strip()
                
                # Save to file
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
