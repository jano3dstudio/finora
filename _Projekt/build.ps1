param([string]$OutputPath,[string]$KitRoot)
$ErrorActionPreference='Stop'
if(-not $KitRoot){$ref=Get-Content -LiteralPath (Join-Path $PSScriptRoot 'kit.ref.json') -Raw | ConvertFrom-Json;$KitRoot=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot $ref.source))}
. (Join-Path $KitRoot 'scripts/consumer.ps1')
$kit=Get-JanoKitPlan -ProjectRoot $PSScriptRoot -KitRoot $KitRoot
if(-not $OutputPath){$OutputPath=Join-Path (Split-Path $PSScriptRoot) 'Rendering Finish.exe'}
$OutputPath=[IO.Path]::GetFullPath($OutputPath)
New-Item -ItemType Directory -Force -Path (Split-Path $OutputPath) | Out-Null
$compiler=Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
$compileArgs=@('/nologo','/target:winexe','/platform:x64','/optimize+','/main:Jano.AppKit.DesktopStart','/reference:System.Drawing.dll','/reference:System.Windows.Forms.dll','/reference:System.Core.dll','/reference:System.Web.Extensions.dll',('/win32icon:'+(Join-Path $PSScriptRoot 'icon.ico')))
foreach($name in @('Microsoft.Web.WebView2.Core.dll','Microsoft.Web.WebView2.WinForms.dll')){$compileArgs+=('/reference:'+(Join-Path $PSScriptRoot "deps\$name"))}
foreach($name in @('Microsoft.Web.WebView2.Core.dll','Microsoft.Web.WebView2.WinForms.dll','WebView2Loader.dll','LICENSE.txt')){$compileArgs+=('/resource:'+(Join-Path $PSScriptRoot "deps\$name")+',payload/'+$name)}
$uiRoot=Join-Path $PSScriptRoot 'ui'
foreach($file in Get-ChildItem -LiteralPath $uiRoot -Recurse -File){$relative=$file.FullName.Substring($uiRoot.Length+1).Replace('\','/');$compileArgs+=('/resource:'+$file.FullName+',payload/ui/'+$relative)}
foreach($file in Get-ChildItem (Join-Path $PSScriptRoot 'native') -Filter '*.cs'){$compileArgs+=$file.FullName}
Invoke-JanoCompile -Plan $kit -Arguments $compileArgs -OutputPath $OutputPath
