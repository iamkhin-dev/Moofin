using System.Diagnostics;

namespace MoofinCLI
{
    internal sealed class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Welcome to Moofin CLI");
            while (true)
            {
                Console.Write("\n> ");
                var input = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(input)) continue;

                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var c = parts.Length >= 2 ? $"{parts[0]} {parts[1]}" : parts[0];

                switch (c.ToLowerInvariant())
                {
                    case "moofin help":
                        Console.WriteLine("Available Commands:");
                        Console.WriteLine("moofin help - Show available Commands");
                        Console.WriteLine("moofin nuget create-app {project-name} - Create your project with the Moofin library");
                        Console.WriteLine("moofin exit - Exit the program");
                        break;

                    case "moofin nuget":
                        if (parts.Length >= 4 && parts[2].Equals("create-app", StringComparison.OrdinalIgnoreCase))
                        {
                            string projectName = parts[3];
                            string? inputPath;
                            do
                            {
                                Console.Write("Where do you want to create the project (full path): ");
                                inputPath = Console.ReadLine()?.Trim();

                                if (string.IsNullOrWhiteSpace(inputPath))
                                {
                                    Console.WriteLine("You must specify a valid path.");
                                }
                            } while (string.IsNullOrWhiteSpace(inputPath));

                            string path = inputPath!;
                            await CreateNewProject(projectName, path);
                        }
                        else
                        {
                            Console.WriteLine("Usage: moofin nuget create-app {project-name}");
                        }
                        break;

                    case "moofin exit":
                        await Task.Delay(1200);
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Unrecognized command, type moofin help.");
                        break;
                }
            }
        }

        static async Task CreateNewProject(string projectName, string basePath)
        {
            string projectDir = Path.Combine(basePath, projectName);

            if (Directory.Exists(projectDir))
            {
                Console.WriteLine("The folder already exists.");
                return;
            }

            Directory.CreateDirectory(projectDir);
            Directory.CreateDirectory(Path.Combine(projectDir, "src"));

            File.WriteAllText(Path.Combine(projectDir, $"{projectName}.csproj"), $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>");

            File.WriteAllText(Path.Combine(projectDir, "src", "Program.cs"), $@"using Moofin.Core.Services;

namespace {projectName}
{{
    public static class Program
    {{
        public static void Main()
        {{
            // Write your code here with our library 
        }}
    }}
}}");

            File.WriteAllText(Path.Combine(projectDir, ".gitignore"), "bin/\nobj/");
            File.WriteAllText(Path.Combine(projectDir, "README.md"), $"# {projectName}\n\nProject generated with Moofin CLI.");

            Console.WriteLine($"Project '{projectName}' successfully created!");

            await AddNuGetPackage(projectDir, "Moofin");
        }

        static async Task AddNuGetPackage(string projectPath, string packageName)
        {
            Console.WriteLine($"Downloading NuGet package '{packageName}'...");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"add \"{Path.Combine(projectPath, $"{Path.GetFileName(projectPath)}.csproj")}\" package {packageName}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            var startTime = DateTime.Now;
            process.Start();
            var progressTask = ShowProgressBar(process);

            string output = await process.StandardOutput.ReadToEndAsync();
            string errors = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();

            await progressTask;

            var endTime = DateTime.Now;
            var elapsed = endTime - startTime;

            Console.WriteLine();
            Console.WriteLine(output);

            if (!string.IsNullOrEmpty(errors))
            {
                Console.WriteLine("Error adding the package:");
                Console.WriteLine(errors);
            }
            else
            {
                Console.WriteLine($"Package '{packageName}' successfully added in {elapsed.TotalSeconds:F1} seconds!");
            }
        }

        static async Task ShowProgressBar(Process process)
        {
            int totalBlocks = 20;
            int progress = 0;

            while (!process.HasExited)
            {
                progress = (progress + 1) % (totalBlocks + 1);

                string bar = new string('-', progress) + new string(' ', totalBlocks - progress);
                int percent = (progress * 100) / totalBlocks;

                Console.Write($"\rDownloading: [{bar}] {percent}%");

                await Task.Delay(200);
            }

            string finalBar = new string('-', totalBlocks);
            Console.Write($"\rDownloading: [{finalBar}] 100%\n");
        }
    }
}