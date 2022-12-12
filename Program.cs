using System.Diagnostics;

string homeDir = Path.Combine(Path.GetDirectoryName(Directory.GetCurrentDirectory()), "PortableGit", "home");
string portableGitDir = Path.Combine(Path.GetDirectoryName(Directory.GetCurrentDirectory()), "PortableGit");
if (!Directory.Exists(homeDir))
    throw new DirectoryNotFoundException($"Can't find dir at {homeDir}");
ProcessStartInfo codeProcess = new ProcessStartInfo(Path.Combine(Directory.GetCurrentDirectory(), "Code.exe"));
codeProcess.EnvironmentVariables["HOME"] = homeDir;
codeProcess.EnvironmentVariables["PATH"] = Path.Combine(homeDir, "bin") +
    ";" + Directory.GetCurrentDirectory() +
    ";" + portableGitDir;
Process.Start(codeProcess);