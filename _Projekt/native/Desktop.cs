using System;using System.IO;using System.Drawing;using System.Reflection;using System.Runtime.CompilerServices;using System.Runtime.InteropServices;using System.Security.Cryptography;using System.Threading.Tasks;using System.Windows.Forms;using System.Web.Script.Serialization;using System.Text.RegularExpressions;using Microsoft.Web.WebView2.Core;using Microsoft.Web.WebView2.WinForms;
namespace Jano.AppKit {
static class DesktopStart {
 [DllImport("kernel32.dll",CharSet=CharSet.Unicode)] static extern bool SetDllDirectory(string path);
 [DllImport("user32.dll")] static extern bool SetProcessDpiAwarenessContext(IntPtr value);
 public static string AssetRoot,ProfileRoot,CheckRoot;public static bool MouseTest;
 [STAThread] static int Main(string[] args){try{
 MouseTest=args.Length==2&&args[0]=="--mouse-test";CheckRoot=args.Length==2&&(args[0]=="--self-test"||MouseTest)?Path.GetFullPath(args[1]):null;
 string id;using(var hash=SHA256.Create())using(var exe=File.OpenRead(Assembly.GetExecutingAssembly().Location))id=BitConverter.ToString(hash.ComputeHash(exe)).Replace("-","").Substring(0,16);
 string data=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"JANO","RenderingFinish");
 AssetRoot=Path.Combine(CheckRoot??data,"runtime",id);ProfileRoot=Path.Combine(CheckRoot??data,"profile");Directory.CreateDirectory(AssetRoot);
 foreach(var name in Assembly.GetExecutingAssembly().GetManifestResourceNames()){if(!name.StartsWith("payload/"))continue;string output=Path.Combine(AssetRoot,name.Substring(8).Replace('/',Path.DirectorySeparatorChar));Directory.CreateDirectory(Path.GetDirectoryName(output));using(var stream=Assembly.GetExecutingAssembly().GetManifestResourceStream(name))using(var buffer=new MemoryStream()){stream.CopyTo(buffer);var bytes=buffer.ToArray();File.WriteAllBytes(output,bytes);}}
 SetDllDirectory(AssetRoot);AppDomain.CurrentDomain.AssemblyResolve+=(s,e)=>{var name=new AssemblyName(e.Name).Name;return name=="Microsoft.Web.WebView2.Core"||name=="Microsoft.Web.WebView2.WinForms"?Assembly.LoadFrom(Path.Combine(AssetRoot,name+".dll")):null;};
 try{SetProcessDpiAwarenessContext(new IntPtr(-4));}catch{}return Run();
 }catch(Exception e){if(CheckRoot!=null){Directory.CreateDirectory(CheckRoot);File.WriteAllText(Path.Combine(CheckRoot,"desktop-error.txt"),e.ToString());}else MessageBox.Show("Rendering Finish konnte nicht starten.\n"+e.Message);return 1;}}
 [MethodImpl(MethodImplOptions.NoInlining)] static int Run(){Application.EnableVisualStyles();Application.SetCompatibleTextRenderingDefault(false);using(var app=new DesktopWindow()){Application.Run(app);return app.ExitCode;}}
}
sealed class Request {public string channel{get;set;}public string op{get;set;}public string value{get;set;}public string name{get;set;}public string data{get;set;}public int id{get;set;}}
class DesktopWindow:JanoWindow {
 readonly WebView2 web=new WebView2();readonly Label loading;readonly JavaScriptSerializer json=new JavaScriptSerializer{MaxJsonLength=600000000};LookBridge looks;string outputFolder;bool writing;public int ExitCode;
 const string Origin="https://rendering-finish.local/";
 public DesktopWindow(){Text="Rendering Finish";AutoScaleMode=AutoScaleMode.None;BackColor=Tokens.Background;var area=Screen.PrimaryScreen.WorkingArea;ClientSize=new Size(Math.Min(1440,area.Width-80),Math.Min(920,area.Height-110));MinimumSize=new Size(800,600);StartPosition=FormStartPosition.CenterScreen;
 web.Dock=DockStyle.Fill;web.DefaultBackgroundColor=Tokens.Background;Controls.Add(web);loading=new Label{Dock=DockStyle.Fill,Text="Rendering Finish …",ForeColor=Tokens.Text,TextAlign=ContentAlignment.MiddleCenter};Controls.Add(loading);loading.BringToFront();Shown+=async(s,e)=>await Initialize();
 if(DesktopStart.CheckRoot!=null){var timeout=new Timer{Interval=120000};timeout.Tick+=(s,e)=>{timeout.Stop();Fail(new TimeoutException("Self-test exceeded 120 seconds"));};FormClosed+=(s,e)=>timeout.Dispose();timeout.Start();}}
 static bool IsLocal(string value){Uri u;return Uri.TryCreate(value,UriKind.Absolute,out u)&&u.Scheme=="https"&&u.Host=="rendering-finish.local"&&u.IsDefaultPort;}
 async Task Initialize(){try{var env=await CoreWebView2Environment.CreateAsync(null,DesktopStart.ProfileRoot);await web.EnsureCoreWebView2Async(env);var core=web.CoreWebView2;core.Settings.AreDevToolsEnabled=false;core.Settings.AreDefaultContextMenusEnabled=false;core.Settings.AreHostObjectsAllowed=false;core.Settings.IsStatusBarEnabled=false;core.SetVirtualHostNameToFolderMapping("rendering-finish.local",Path.Combine(DesktopStart.AssetRoot,"ui"),CoreWebView2HostResourceAccessKind.DenyCors);
 core.NavigationStarting+=(s,e)=>{if(!IsLocal(e.Uri))e.Cancel=true;};core.NewWindowRequested+=(s,e)=>e.Handled=true;core.PermissionRequested+=(s,e)=>e.State=CoreWebView2PermissionState.Deny;core.DownloadStarting+=(s,e)=>e.Cancel=true;
 core.AddWebResourceRequestedFilter("*",CoreWebView2WebResourceContext.All);core.WebResourceRequested+=(s,e)=>{if(!IsLocal(e.Request.Uri)&&!e.Request.Uri.StartsWith("blob:"))e.Response=env.CreateWebResourceResponse(new MemoryStream(),403,"Blocked","");};
 looks=new LookBridge(core,this,"rendering-finish",DesktopStart.CheckRoot);core.WebMessageReceived+=async(s,e)=>{if(!IsLocal(e.Source))return;var raw=e.WebMessageAsJson;if(looks.Handle(raw))return;await HandleRequest(raw);};
 bool done=false;core.NavigationCompleted+=async(s,e)=>{if(done)return;done=true;try{if(!e.IsSuccess)throw new Exception("Local UI failed: "+e.WebErrorStatus);loading.Visible=false;web.BringToFront();RefreshChrome();if(DesktopStart.CheckRoot!=null){await Check();Close();}}catch(Exception ex){Fail(ex);}};core.Navigate(Origin+"index.html");
 }catch(Exception e){Fail(e);}}
 async Task HandleRequest(string raw){Request r=null;try{r=json.Deserialize<Request>(raw);if(r==null||r.channel!="finish")return;object result=null;
 switch(r.op){
 case "language":L.English=r.value=="en";RefreshChrome();break;
 case "chooseFolder":using(var dialog=new FolderBrowserDialog{Description=L.English?"Choose output folder":"Ausgabeordner wählen",ShowNewFolderButton=true}){if(dialog.ShowDialog(this)==DialogResult.OK){outputFolder=Path.GetFullPath(dialog.SelectedPath);result=outputFolder;}}break;
 case "testFolder":if(DesktopStart.CheckRoot==null)throw new Exception("Unavailable");outputFolder=Path.Combine(DesktopStart.CheckRoot,"exports");Directory.CreateDirectory(outputFolder);result=outputFolder;break;
 case "save":if(writing||outputFolder==null)throw new Exception("No output folder selected");writing=true;try{string name=r.name,data=r.data,folder=outputFolder;result=await Task.Run(()=>Save(folder,name,data));}finally{writing=false;}break;
 default:throw new Exception("Unknown request");}
 web.CoreWebView2.PostWebMessageAsJson(json.Serialize(new{channel="finish",id=r.id,ok=true,result=result}));
 }catch(Exception e){web.CoreWebView2.PostWebMessageAsJson(json.Serialize(new{channel="finish",id=r==null?0:r.id,ok=false,error=e.Message}));}}
 static string Save(string folder,string name,string data){
 if(String.IsNullOrWhiteSpace(name)||name.Length>180||Path.GetFileName(name)!=name||name.IndexOfAny(Path.GetInvalidFileNameChars())>=0||Regex.IsMatch(name,@"^(CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9])(?:\.|$)",RegexOptions.IgnoreCase))throw new Exception("Invalid filename");
 bool png=name.EndsWith(".png",StringComparison.OrdinalIgnoreCase),jpg=name.EndsWith(".jpg",StringComparison.OrdinalIgnoreCase);string prefix=png?"data:image/png;base64,":"data:image/jpeg;base64,";if((!png&&!jpg)||data==null||!data.StartsWith(prefix)||data.Length>550000000)throw new Exception("Invalid image data");
 byte[] bytes=Convert.FromBase64String(data.Substring(prefix.Length));if(bytes.Length<8||(png&&(bytes[0]!=137||bytes[1]!=80||bytes[2]!=78||bytes[3]!=71))||(jpg&&(bytes[0]!=255||bytes[1]!=216)))throw new Exception("Invalid image signature");
 string stem=Path.GetFileNameWithoutExtension(name),ext=Path.GetExtension(name),temp=Path.Combine(folder,".finish-"+Guid.NewGuid().ToString("N")+".tmp");try{using(var stream=new FileStream(temp,FileMode.CreateNew,FileAccess.Write,FileShare.None)){stream.Write(bytes,0,bytes.Length);stream.Flush(true);}for(int n=0;n<10000;n++){string dest=Path.Combine(folder,stem+(n==0?"":"_"+(n+1))+ext);try{File.Move(temp,dest);return dest;}catch(IOException){if(!File.Exists(dest))throw;}}throw new IOException("Too many duplicate filenames");}finally{if(File.Exists(temp))File.Delete(temp);}
 }
 void Fail(Exception e){ExitCode=1;if(DesktopStart.CheckRoot!=null){File.WriteAllText(Path.Combine(DesktopStart.CheckRoot,"desktop-error.txt"),e.ToString());Close();}else{loading.Visible=true;loading.Text="Rendering Finish konnte nicht geöffnet werden.\n"+e.Message;}}
 async Task CaptureImage(string name){using(var stream=File.Create(Path.Combine(DesktopStart.CheckRoot,name)))await web.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png,stream);}
 async Task Check(){if(DesktopStart.MouseTest){await MouseWindowCheck.Run(this,DesktopStart.CheckRoot);return;}
 await Task.Delay(400);await CaptureImage("empty.png");await web.CoreWebView2.ExecuteScriptAsync("window.FinishQA.run().catch(e=>window.__qaResult={ok:false,error:e.stack})");string result="null";for(int n=0;n<100;n++){await Task.Delay(500);result=await web.CoreWebView2.ExecuteScriptAsync("window.__qaResult||null");if(result!="null")break;}File.WriteAllText(Path.Combine(DesktopStart.CheckRoot,"result.json"),result);if(!result.Contains("\"ok\":true"))throw new Exception("UI workflow failed: "+result);await Task.Delay(250);await CaptureImage("series.png");await web.CoreWebView2.ExecuteScriptAsync("document.querySelector('[data-tab=brand]').click()");await CaptureImage("branding.png");await web.CoreWebView2.ExecuteScriptAsync("document.querySelector('#lookOpen').click()");await Task.Delay(300);await CaptureImage("looks.png");await web.CoreWebView2.ExecuteScriptAsync("document.querySelector('#look-cancel').click()");
 var overflow=await web.CoreWebView2.ExecuteScriptAsync("document.documentElement.scrollWidth <= innerWidth");if(overflow!="true")throw new Exception("Horizontal overflow");
 Size=new Size(1000,720);await Task.Delay(200);await CaptureImage("compact.png");overflow=await web.CoreWebView2.ExecuteScriptAsync("document.documentElement.scrollWidth <= innerWidth");if(overflow!="true")throw new Exception("Compact horizontal overflow");
 var reload=new TaskCompletionSource<bool>();EventHandler<CoreWebView2NavigationCompletedEventArgs> handler=null;handler=(s,e)=>{web.CoreWebView2.NavigationCompleted-=handler;reload.SetResult(e.IsSuccess);};web.CoreWebView2.NavigationCompleted+=handler;web.CoreWebView2.Reload();if(!await reload.Task)throw new Exception("Reload failed");await Task.Delay(300);
 await web.CoreWebView2.ExecuteScriptAsync("FinishQA.templates().then(v=>window.__templateReload=v.map(x=>({name:x.name,logo:!!x.logo,lut:!!x.lut})))");await Task.Delay(300);string persisted=await web.CoreWebView2.ExecuteScriptAsync("window.__templateReload");File.WriteAllText(Path.Combine(DesktopStart.CheckRoot,"template-reload.json"),persisted);if(!persisted.Contains("QA Kundenvorlage")||!persisted.Contains("QA LUT")||!persisted.Contains("\"logo\":true")||!persisted.Contains("\"lut\":true"))throw new Exception("Template reload persistence failed");
 File.WriteAllText(Path.Combine(DesktopStart.CheckRoot,"PASS.txt"),"PASS real WebView2: PNG/JPG import; ten-image batch; individual and series correction; logo/text; embedded logo/LUT template persistence after reload; DE/EN data preservation; PNG alpha; JPG resize; cancellation; non-overwriting filename collision; untinted logo; original view; compact layout. Runtime "+web.CoreWebView2.Environment.BrowserVersionString);
 }
}
}

