'配布対象外のファイルを削除する'
if (Test-Path deploy.dll) {
  Remove-Item deploy.dll -Force
}

if (Test-Path CeVIO.Talk.RemoteService.dll) {
  Remove-Item CeVIO.Talk.RemoteService.dll -Force
}

if (Test-Path logs) {
  Remove-Item logs -Force -Recurse
}

Remove-Item -Force -Recurse -Path .\* -Include *.xml -Exclude *系*,preset-*
Remove-Item -Force -Recurse -Path .\* -Include *.pdb
Remove-Item -Force -Recurse -Path .\* -Include "Action icons",locale
'Done'

'配布ファイルをアーカイブする'
if (Test-Path deploy.zip) {
  Remove-Item deploy.zip -Force
}

$files = Get-ChildItem -Path .\ -Exclude deploy.ps1
Compress-Archive -CompressionLevel Optimal -Path $files -DestinationPath deploy.zip
'Done'

Read-Host "終了するには何かキーを教えてください..."
