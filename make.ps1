$devenv="C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\devenv.com"
$startdir=Get-Location

$sln=Get-Item *.sln

'Replace License Key'
$codefile = ".\ACT.TTSYukkuri.Core\Yukkuri\AquesTalk.cs"
$codefileBack = ".\ACT.TTSYukkuri.Core\Yukkuri\AquesTalk.cs.back"

if (Test-Path .\AquesTalk.key) {
    $replacement = "#DEVELOPER_KEY_IS_HERE#"
    $key = $(Get-Content .\AquesTalk.key)
    Copy-Item -Force $codefile $codefileBack
    (Get-Content $codefile) | % { $_ -replace $replacement, $key } > $codefile
}

'Build Debug'
& $devenv $sln /project deploy\deploy.csproj /Rebuild Debug

'Build Release'
& $devenv $sln /project deploy\deploy.csproj /Rebuild Release

'Restore Backup'
if (Test-Path $codefileBack) {
    Copy-Item -Force $codefileBack $codefile
}

'Deploy Debug'
if (Test-Path .\deploy\bin\Debug) {
    Set-Location .\deploy\bin\Debug
    if (Test-Path .\deploy.ps1) {
        .\deploy.ps1
    }
    Set-Location $startdir
}

'Deploy Release'
if (Test-Path .\deploy\bin\Release) {
    Set-Location .\deploy\bin\Release
    if (Test-Path .\deploy.ps1) {
        .\deploy.ps1
    }
    Set-Location $startdir
}

Read-Host "èIóπÇ∑ÇÈÇ…ÇÕâΩÇ©ÉLÅ[Çã≥Ç¶ÇƒÇ≠ÇæÇ≥Ç¢..."
