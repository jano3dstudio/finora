from PIL import Image, ImageDraw
from pathlib import Path
p=Path(__file__).parent
im=Image.new('RGBA',(256,256));d=ImageDraw.Draw(im)
d.rounded_rectangle((0,0,255,255),radius=52,fill='#15181b')
d.rectangle((50,70,150,170),outline='#9aa5af',width=10)
d.rectangle((86,98,206,198),fill='#1c2024',outline='#3cff91',width=10)
d.line([(92,178),(126,138),(152,164),(172,142),(200,176)],fill='#e7ebee',width=8)
d.line((190,42,190,82),fill='#3cff91',width=10);d.line((170,62,210,62),fill='#3cff91',width=10)
im.save(p/'icon.ico',sizes=[(16,16),(24,24),(32,32),(48,48),(64,64),(128,128),(256,256)])
im.save(p/'ui'/'icon.png')
