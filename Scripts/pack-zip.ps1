Remove-Item ..\QuickLook.Plugin.WoW64HookHelper.qlplugin -ErrorAction SilentlyContinue

$files = Get-ChildItem -Path ..\Build\Release\ -Exclude *.pdb,*.xml,*.exe
Compress-Archive $files ..\QuickLook.Plugin.WoW64HookHelper.zip
Move-Item ..\QuickLook.Plugin.WoW64HookHelper.zip ..\QuickLook.Plugin.WoW64HookHelper.qlplugin