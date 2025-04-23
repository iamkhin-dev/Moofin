# Moofin **v0.1.0 BETA**
<p align="center">
    <img height="150px" src="icon.svg" align="center" />
    <h2 align="center">Moofin</h2>
    <p align="center"><strong>Moofin</strong> is a .NET library designed to provide core functionalities for building study-focused applications.</p>
    <p align="center">
       <img src="https://upload.wikimedia.org/wikipedia/commons/b/bd/Logo_C_sharp.svg" alt="C# Logo" width="50"></a>
        <a href="https://dotnet.microsoft.com/en-us/download/dotnet/9.0">
  <img src="https://upload.wikimedia.org/wikipedia/commons/7/7d/Microsoft_.NET_logo.svg" alt=".NET Logo" width="50"></a>
           <a href="https://www.nuget.org/">
  <img src="https://upload.wikimedia.org/wikipedia/commons/2/25/NuGet_project_logo.svg" alt="NuGet Logo" width="50">
</a>
</a>
</p>

- ⚙️ [Installation Guide](#%EF%B8%8F-installation-guide) 
- 📑 [Documentation](#-documentation)
- 📜 [ChangeLog](#-changelog)      

>[!WARNING]
> The name Moofin was taken from: [Moofin](https://www.twitch.tv/moofin__), follow him ❤️

## ⚙️ Installation Guide

To install **Moofin** in your .NET project, follow the steps below depending on your platform.

1. **Prerequisites**
   - [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
   
2. **Run the Installer**:
   - Download the appropriate installer for your operating system (Windows, macOS, or Linux).
   - Follow the on-screen instructions to complete the installation.

3. **Install via NuGet or Terminal or MoofinCLI**:
      ##### **NuGet:**
   Go to Project → NuGet Package Manager → Browse → Search: Moofin

      ##### **Terminal:**
   
   | **OS**     | **Command**               |
   |------------|---------------------------|
   | **Windows**| dotnet add package Moofin |
   | **macOS**  | dotnet add package Moofin |
   | **Linux**  | dotnet add package Moofin |
   
      #### **MoofinCLI:**
    Windows x64: 
    [Download](https://github.com/iamkhin-dev/Moofin/raw/main/MoofinCLI/bin/Release/net9.0/win-x64.zip)

    Windows x86: 
    [Download](https://github.com/iamkhin-dev/Moofin/raw/main/MoofinCLI/bin/Release/net9.0/win-x86.zip)


## 📑 Documentation
### Index

#### Exceptions
- [MoofinException](./docs/exceptions/MoofinException.md)
- [SyncFailedException](./docs/exceptions/SyncFailedException.md)
  
#### Intefaces
- [INotification](./docs/intefaces/INotification.md)
- [IStorage](./docs/intefaces/IStorage.md)
- [ISyncProvider](./docs/intefaces/ISyncProvider.md)
  
#### Services
- [FlashcardService](./docs/services/FlashcardService.md) 
- [GoalService](./docs/services/GoalService.md)
- [ProgressTracker](./docs/services/ProgressTracker.md)
- [TimeManager](./docs/services/TimeManager.md)
- [Format](./docs/services/Format.md)

#### Utilis
- [SpacedRepetition](./docs/utils/SpacedRepetition.md)

## 📜 ChangeLog

v0.1.0 BETA
+ Added all library on GitHub
+ Added all library to NuGet
