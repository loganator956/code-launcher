using System.Diagnostics;

string currentExe = Process.GetCurrentProcess().MainModule.FileName;
string exeDir = Path.GetDirectoryName(currentExe);

string homeDir = Path.Combine(Path.GetDirectoryName(exeDir), "PortableGit", "home");
string portableGitDir = Path.Combine(Path.GetDirectoryName(exeDir), "PortableGit");
if (!Directory.Exists(homeDir))
    throw new DirectoryNotFoundException($"Can't find dir at {homeDir}");
ProcessStartInfo codeProcess = new ProcessStartInfo(Path.Combine(exeDir, "Code.exe"));
codeProcess.EnvironmentVariables["HOME"] = homeDir;
codeProcess.EnvironmentVariables["PATH"] = Path.Combine(homeDir, "bin") +
    ";" + exeDir +
    ";" + portableGitDir +
    ";" + Path.Combine(portableGitDir, "usr", "bin") +
    ";" + Path.Combine(portableGitDir, "bin");
Process.Start(codeProcess);