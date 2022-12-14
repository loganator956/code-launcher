using System.Diagnostics;
string[] arguments = args;
// Get the system PATH variable. Allows to pass through the system's PATH variable to spawned processes
string systemPathEnvVar = Environment.GetEnvironmentVariable("PATH") ?? String.Empty;

// Get paths to important directories
// this process
ProcessModule? module = Process.GetCurrentProcess().MainModule;
if (module is null)
    throw new NullReferenceException("MainModule of current process is null");
Console.WriteLine($"code-launcher version {module.FileVersionInfo.FileVersion}\n");
string currentExe = module.FileName ?? String.Empty;
string currentExeDirectory = Path.GetDirectoryName(currentExe) ?? String.Empty;
string workspaceRootDir = Path.GetDirectoryName(currentExeDirectory) ?? String.Empty;

string portableGitDir = Path.Combine(workspaceRootDir, "PortableGit");
string homeDir = Path.Combine(portableGitDir, "home");
// validate directories
// required directories
try
{
    if (!Directory.Exists(workspaceRootDir))
        throw new DirectoryNotFoundException($"Can't find workspace root directory at {workspaceRootDir}");
    if (!Directory.Exists(portableGitDir))
        throw new DirectoryNotFoundException($"Can't find PortableGit directory at {portableGitDir}");
    if (!Directory.Exists(homeDir))
        throw new DirectoryNotFoundException($"Can't find home directory at {homeDir}");
}
catch (DirectoryNotFoundException dirNotFound)
{
    PrintInColour(dirNotFound.ToString(), ConsoleColor.Red);
    Environment.ExitCode = 1;
    return;
}
// check vs code required directories
try
{
    if (!Directory.Exists(Path.Combine(currentExeDirectory, "bin")))
        throw new DirectoryNotFoundException($"Can't find VSCode bin directory at {Path.Combine(currentExeDirectory, "bin")}");
}
catch (DirectoryNotFoundException dirNotFound)
{
    PrintInColour(dirNotFound.ToString(), ConsoleColor.Red);
    Environment.ExitCode = 1;
    return;
}
// Setup the information required to start the code process
ProcessStartInfo codeProcess = new ProcessStartInfo(Path.Combine(currentExeDirectory, "Code.exe"));
codeProcess.EnvironmentVariables["HOME"] = homeDir;
codeProcess.EnvironmentVariables["PATH"] = Path.Combine(homeDir, "bin") +
    ";" + currentExeDirectory +
    ";" + portableGitDir +
    ";" + Path.Combine(portableGitDir, "usr", "bin") +
    ";" + Path.Combine(portableGitDir, "bin") +
    ";" + systemPathEnvVar;
codeProcess.ArgumentList.Add("--extensions-dir ");
codeProcess.ArgumentList.Add(Path.Combine(currentExeDirectory, "data", "extensions"));
foreach (string arg in arguments)
    codeProcess.ArgumentList.Add(arg);

// need to install the extensions every new place as vs code, although "portable" uses full file paths for the extensions and so they only work in the
// path where they're initially installed ＼（〇_ｏ）／
string extensionsFilePath = Path.Combine(currentExeDirectory, "extensionslist.txt");
if (File.Exists(extensionsFilePath))
{
    foreach (string line in File.ReadAllLines(extensionsFilePath))
    {
        ProcessStartInfo extensionInstallInfo = new ProcessStartInfo("cmd.exe");
        extensionInstallInfo.ArgumentList.Add("/C"); // tells cmd.exe to execute the following commands and terminate
        extensionInstallInfo.ArgumentList.Add(Path.Combine(currentExeDirectory, "bin", "code"));
        extensionInstallInfo.ArgumentList.Add("--install-extension");
        extensionInstallInfo.ArgumentList.Add(line);
        Console.WriteLine($"Installing extension: {line}");
        Process extensionProcess = Process.Start(extensionInstallInfo) ?? throw new NullReferenceException("Spawned extension process is null");
        extensionProcess.WaitForExit();
    }
}

Process.Start(codeProcess);

void PrintInColour(string message, ConsoleColor colour)
{
    ConsoleColor prevColour = Console.ForegroundColor;
    Console.ForegroundColor = colour;
    Console.WriteLine(message);
    Console.ForegroundColor = prevColour;
}