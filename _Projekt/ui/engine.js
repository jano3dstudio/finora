/* Rendering Finish: deterministic sRGB pixel pipeline, no network. */
(function(root){
'use strict';
const clamp=(v,a=0,b=1)=>Math.max(a,Math.min(b,v));
const defaults=()=>({auto:0,autoMode:'individual',exposure:0,temperature:0,tint:0,contrast:0,highlights:0,shadows:0,saturation:0,look:'neutral',intensity:75});
function parseCube(text){
 let size=0,min=[0,0,0],max=[1,1,1],rows=[];
 for(const raw of text.replace(/^\uFEFF/,'').split(/\r?\n/)){
  const line=raw.split('#')[0].trim();if(!line)continue;const p=line.split(/\s+/),key=p[0];
  if(key==='TITLE')continue;
  if(key==='LUT_3D_SIZE'){if(size)throw Error('cube');size=Number(p[1]);if(!Number.isInteger(size)||size<2||size>65)throw Error('cube');continue;}
  if(key==='DOMAIN_MIN'||key==='DOMAIN_MAX'){const a=p.slice(1).map(Number);if(a.length!==3||!a.every(Number.isFinite))throw Error('cube');if(key==='DOMAIN_MIN')min=a;else max=a;continue;}
  if(/^[A-Za-z_]/.test(key))throw Error('cube');
  const a=p.map(Number);if(a.length!==3||!a.every(Number.isFinite)||rows.length>=65**3)throw Error('cube');rows.push(a);
 }
 if(!size||rows.length!==size**3||max.some((v,i)=>v<=min[i]))throw Error('cube');
 return {size,min,max,data:rows.flat()};
}
function lutAt(r,g,b,lut){
 const n=lut.size,coords=[r,g,b].map((v,i)=>clamp((v-lut.min[i])/(lut.max[i]-lut.min[i]))*(n-1));
 const lo=coords.map(Math.floor),hi=lo.map(v=>Math.min(v+1,n-1)),f=coords.map((v,i)=>v-lo[i]);const out=[0,0,0];
 for(let z=0;z<2;z++)for(let y=0;y<2;y++)for(let x=0;x<2;x++){
  const w=(x?f[0]:1-f[0])*(y?f[1]:1-f[1])*(z?f[2]:1-f[2]);const k=((z?hi[2]:lo[2])*n*n+(y?hi[1]:lo[1])*n+(x?hi[0]:lo[0]))*3;
  for(let c=0;c<3;c++)out[c]+=lut.data[k+c]*w;
 }return out;
}
function analyze(data){let sum=0,count=0;for(let i=0;i<data.length;i+=4){if(data[i+3]<128)continue;sum+=Math.log(.02+(.2126*data[i]+.7152*data[i+1]+.0722*data[i+2])/255);count++;}return count?clamp(Math.log2(.38/(Math.exp(sum/count)-.02)), -1,1):0;}
function transform(data,s,autoEV=0,lut=null,start=0,end=data.length){
 const gain=2**(s.exposure+autoEV*s.auto/100),warm=s.temperature/100,tint=s.tint/100,ct=1+s.contrast/100,sat=1+s.saturation/100,amount=s.intensity/100;
 for(let i=start;i<end;i+=4){
  let r=data[i]/255,g=data[i+1]/255,b=data[i+2]/255;
  r*=gain*(1+warm*.16+tint*.04);g*=gain*(1-tint*.12);b*=gain*(1-warm*.16+tint*.04);
  const l=clamp(.2126*r+.7152*g+.0722*b),shift=(s.shadows/100)*(1-l)**2*.32+(s.highlights/100)*l*l*.32;
  r=(r+shift-.5)*ct+.5;g=(g+shift-.5)*ct+.5;b=(b+shift-.5)*ct+.5;
  const gray=.2126*r+.7152*g+.0722*b;r=gray+(r-gray)*sat;g=gray+(g-gray)*sat;b=gray+(b-gray)*sat;
  let v=[r,g,b];
  if(s.look==='custom'&&lut)v=lutAt(r,g,b,lut);
  else if(s.look==='warm')v=[r*1.035+.015,g*1.005+.004,b*.955];
  else if(s.look==='cool')v=[r*.955,g*1.01,b*1.035+.012];
  else if(s.look==='soft')v=[r*.91+.05,g*.91+.05,b*.91+.05];
  else if(s.look==='crisp')v=[(r-.5)*1.13+.5,(g-.5)*1.13+.5,(b-.5)*1.13+.5];
  else if(s.look==='mono')v=[gray,gray,gray];
  data[i]=clamp(r+(v[0]-r)*amount)*255;data[i+1]=clamp(g+(v[1]-g)*amount)*255;data[i+2]=clamp(b+(v[2]-b)*amount)*255;
 }return data;
}
function filename(name,prefix,index,format){let stem=name.replace(/\.[^.]*$/,'');stem=(prefix?prefix+'_'+String(index+1).padStart(2,'0')+'_'+stem:stem)+'_finish';return stem.replace(/[<>:"/\\|?*\x00-\x1f]/g,'_').slice(0,150)+'.'+(format==='png'?'png':'jpg');}
const api={clamp,defaults,parseCube,lutAt,analyze,transform,filename};if(typeof module!=='undefined')module.exports=api;else root.FinishEngine=api;
})(typeof window!=='undefined'?window:globalThis);
