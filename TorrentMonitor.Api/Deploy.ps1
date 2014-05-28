$cmd = "$OctopusPackageDirectoryPath" + "\TorrentMonitor.Api.exe"

& $cmd "stop"
& $cmd "uninstall"
& $cmd "install"
& $cmd "start"


