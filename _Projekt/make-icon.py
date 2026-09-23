"""Export the canonical transparent icon; never recreate a background tile."""
from pathlib import Path
import subprocess
root=Path(__file__).resolve().parents[2]
subprocess.run(['node',str(root/'jano-app-kit/scripts/export-app-icons.cjs'),'rendering-finish'],check=True)
