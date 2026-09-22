"""Package only product sources, the tested binary and its exact shared kit inputs."""
from pathlib import Path
import json,hashlib,zipfile,sys
root=Path(__file__).resolve().parents[1];kit=root.parent/'jano-app-kit'
exe=root/'Rendering Finish.exe';receipt=Path(str(exe)+'.kit-lock.json')
digest=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
lock=json.loads(receipt.read_text('utf-8-sig'));assert digest(exe)==lock['exeSha256']
files={}
for file in root.rglob('*'):
    relative=file.relative_to(root)
    if not file.is_file() or any(x in {'.git','_Pakete','output','review','__pycache__','registration-backups'} for x in relative.parts):continue
    if file.name in {'compiler-input.txt','delivery.json'} or file.suffix in {'.pyc','.tmp'}:continue
    files[root.name+'/'+relative.as_posix()]=file
for row in lock['kit']['files']:
    file=kit/row['source'];assert digest(file)==row['sha256'],str(file)
    files['jano-app-kit/'+row['source']]=file
for relative in ['modules.json','scripts/consumer.ps1','DESIGN_SYSTEM.md','SHARED_DEVELOPMENT.md','SHARED_LOOKS.md']:
    files['jano-app-kit/'+relative]=kit/relative
manifest={name:digest(file) for name,file in files.items()}
target=root/'_Pakete'/'Rendering-Finish-0.1.0.zip';target.parent.mkdir(exist_ok=True)
with zipfile.ZipFile(target,'w',zipfile.ZIP_DEFLATED) as z:
    for name,file in files.items():z.write(file,name)
    z.writestr('SHA256.json',json.dumps(manifest,indent=2))
    z.writestr('BUILD.txt','Keep rendering-finish and jano-app-kit as siblings. Run rendering-finish/_Projekt/build.ps1 on Windows. The EXE runs independently with WebView2 Runtime. No user profiles or customer images included.\n')
with zipfile.ZipFile(target) as z:
    for name,h in manifest.items():assert hashlib.sha256(z.read(name)).hexdigest()==h
print(json.dumps({'package':str(target),'files':len(files),'sha256':digest(target)}))
