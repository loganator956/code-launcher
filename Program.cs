using System.Diagnostics;

string currentExe = Process.GetCurrentProcess().MainModule.FileName;
string exeDir = Path.GetDirectoryName(currentExe);

if (exeDir.ToLower().Contains("portable apps master"))
    return; // exits if detects is my "master" copy.

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
codeProcess.ArgumentList.Add("--extensions-dir ");
codeProcess.ArgumentList.Add(Path.Combine(exeDir, "data", "extensions"));

// need to install the extensions every new place as vs code, although "portable" uses full file paths for the extensions and so they only work in the
// path where they're initially installed ＼（〇_ｏ）／
foreach (string line in File.ReadAllLines(Path.Combine(exeDir, "extensionslist.txt")))
{
    ProcessStartInfo extensionInstallInfo = new ProcessStartInfo("cmd.exe");
    extensionInstallInfo.ArgumentList.Add("/C");
    extensionInstallInfo.ArgumentList.Add(Path.Combine(exeDir, "bin", "code"));
    extensionInstallInfo.ArgumentList.Add("--install-extension");
    extensionInstallInfo.ArgumentList.Add(line);
    Console.WriteLine($"Installing extension: {line}");
    Process.Start(extensionInstallInfo).WaitForExit();
}

Process.Start(codeProcess);
