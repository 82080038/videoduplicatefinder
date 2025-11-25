// /*
//     Copyright (C) 2021 0x90d
//     This file is part of VideoDuplicateFinder
//     VideoDuplicateFinder is free software: you can redistribute it and/or modify
//     it under the terms of the GPLv3 as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//     VideoDuplicateFinder is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//     You should have received a copy of the GNU General Public License
//     along with VideoDuplicateFinder.  If not, see <http://www.gnu.org/licenses/>.
// */
//

global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Threading.Tasks;
using Avalonia;
using Avalonia.ReactiveUI;

namespace VDF.GUI {
	class Program {
		// Initialization code. Don't use any Avalonia, third-party APIs or any
		// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
		// yet and stuff might break.
		[STAThread]
		public static void Main(string[] args) {
			// Disable .NET telemetry to prevent connection errors
			Environment.SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1");
			Environment.SetEnvironmentVariable("DOTNET_SKIP_FIRST_TIME_EXPERIENCE", "1");
			Environment.SetEnvironmentVariable("DOTNET_NOLOGO", "1");
			Environment.SetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en");
			Environment.SetEnvironmentVariable("NUGET_XMLDOC_MODE", "skip");
			Environment.SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_SESSIONID", "");
			Environment.SetEnvironmentVariable("DOTNET_ADD_GLOBAL_TOOLS_TO_PATH", "false");
			Environment.SetEnvironmentVariable("DOTNET_MULTILEVEL_LOOKUP", "0");
			
			// Disable NuGet automatic restore to prevent connection attempts
			Environment.SetEnvironmentVariable("NUGET_PACKAGES", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages"));
			Environment.SetEnvironmentVariable("NUGET_PLUGIN_PATHS", "");
			Environment.SetEnvironmentVariable("NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED", "false");
			
			// Disable all network-related .NET features
			Environment.SetEnvironmentVariable("DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER", "false");
			Environment.SetEnvironmentVariable("DOTNET_SYSTEM_NET_HTTP_SOCKETSHTTPHANDLER_HTTP2SUPPORT", "false");
			
			BuildAvaloniaApp()
				.StartWithClassicDesktopLifetime(args);
		}

		// Avalonia configuration, don't remove; also used by visual designer.
		public static AppBuilder BuildAvaloniaApp()
			=> AppBuilder.Configure<App>()
				.UsePlatformDetect()
				.With(new X11PlatformOptions {  UseDBusFilePicker = false })
				.With(new Win32PlatformOptions { UseWindowsUIComposition = true })
				.UseReactiveUI();
	}
}
