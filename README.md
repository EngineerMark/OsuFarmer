# osu!Farmer

## About & Support

Basically the successor to osu!Achieved.

This application is build using the new .NET MAUI framework and .NET 6, supporting multiple platforms.

OS Support:
|OS|Supported|Minimum Version|Note|
|---|---|---|---|
|Windows|✅|10+| Requires WinUI3|
|Linux|❓|-| Community support does exist |
|MacOS|✅|10.13+| |
|iOS|✅|10+| |
|Android|✅|5.0 API 21| |

[Linux support](https://github.com/jsuarezruiz/maui-linux) *(Uses GTK, possibly could support Windows 7)*

## Building
*This is only for Visual Studio 2022, but may work on other IDEs.*

*I only have experience in building to Windows. If you got it working on other operating systems, please open an issue and explain the pipeline.*

### Windows
Simply build the project from Visual Studio.

If going for a release; also run ``` finish_windows_build.bat ```.
This will move all files into a subfolder and create a file for the end-user to launch osu!Farmer.

.NET MAUI produces a ton of waste files and going through those is a major pain. This is much easier.
