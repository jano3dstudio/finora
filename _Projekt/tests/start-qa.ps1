param([string]$ExePath,[string]$OutputPath,[switch]$Mouse)
$ErrorActionPreference='Stop'
if(-not $ExePath){$ExePath=Join-Path (Split-Path (Split-Path $PSScriptRoot)) 'Rendering Finish.exe'}
if(-not $OutputPath){$OutputPath=Join-Path $PSScriptRoot ('output\qa-'+(Get-Date -Format yyyyMMdd-HHmmss))}
$qaOutput=[IO.Path]::GetFullPath($OutputPath)
New-Item -ItemType Directory -Path $qaOutput -Force | Out-Null
$mode=if($Mouse){'--mouse-test'}else{'--self-test'}
$process=Start-Process -FilePath $ExePath -ArgumentList @($mode,('"'+$qaOutput+'"')) -WindowStyle Hidden -PassThru
if(-not $process.WaitForExit(150000)){throw "QA timed out, PID $($process.Id)"}
if($process.ExitCode -ne 0){Get-Content (Join-Path $qaOutput 'desktop-error.txt') -ErrorAction SilentlyContinue;throw "QA failed: $($process.ExitCode)"}
Get-Content (Join-Path $qaOutput 'PASS.txt')
