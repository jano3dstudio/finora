from pathlib import Path
from PIL import Image
import json,sys
p=Path(sys.argv[1]);files=list((p/'exports').iterdir())
assert len(files)==13,[(x.name) for x in files]
for file in files:
    with Image.open(file) as im:
        im.load()
        assert im.format==('PNG' if file.suffix=='.png' else 'JPEG')
        if 'Large_Render' in file.name:assert im.size==(1280,738),im.size
        elif file.suffix=='.png':
            i=int(file.stem.split('_')[1]);assert im.size==(480+(i-1)*10,300)
            assert im.getpixel((0,0))[3]==0
        else:
            assert im.size==(480,300)
            assert min(im.getpixel((0,0)))>230,im.getpixel((0,0))
assert (p/'exports'/'Rendering_01_finish.png').read_bytes()==(p/'exports'/'Rendering_01_finish_2.png').read_bytes()
assert not list((p/'exports').glob('*.tmp'))
report={'files':len(files),'formats':'decoded JPG/PNG','size':'1280 x 738 with aspect ratio preserved','transparency':'PNG preserved; JPG white background','collision':'original and suffixed export byte-identical','partialFiles':0}
(p/'export-verification.json').write_text(json.dumps(report,indent=2),encoding='utf-8')
print('PASS independent Pillow verification:',json.dumps(report))
