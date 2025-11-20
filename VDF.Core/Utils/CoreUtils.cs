// /*
//     Copyright (C) 2025 0x90d
//     This file is part of VideoDuplicateFinder
//     VideoDuplicateFinder is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Affero General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//     VideoDuplicateFinder is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Affero General Public License for more details.
//     You should have received a copy of the GNU Affero General Public License
//     along with VideoDuplicateFinder.  If not, see <http://www.gnu.org/licenses/>.
// */
//

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace VDF.Core.Utils {
	public static class CoreUtils {
		public static bool IsWindows;
		public static string CurrentFolder;
		static CoreUtils() {
			IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
			CurrentFolder = Path.GetDirectoryName(Environment.ProcessPath)!;
		}

		/// <summary>
		/// Checks if a path is on a network drive (Windows) or network mount (Linux/Mac)
		/// </summary>
		public static bool IsNetworkPath(string path) {
			if (string.IsNullOrEmpty(path))
				return false;

			if (IsWindows) {
				// Check if path starts with UNC path (\\server\share) or mapped network drive
				if (path.StartsWith(@"\\", StringComparison.Ordinal))
					return true;
				
				// Check if it's a mapped network drive (e.g., Z:\)
				if (path.Length >= 2 && path[1] == ':') {
					string driveLetter = path.Substring(0, 2);
					try {
						DriveInfo drive = new(driveLetter);
						return drive.DriveType == DriveType.Network;
					}
					catch {
						return false;
					}
				}
			}
			else {
				// On Linux/Mac, check if path is on a network mount
				// This is a simplified check - in practice, you'd need to check /proc/mounts or similar
				return path.StartsWith("/mnt/") || path.StartsWith("/media/") || path.StartsWith("/net/");
			}

			return false;
		}

		/// <summary>
		/// Checks if a network path is accessible with timeout
		/// </summary>
		public static bool IsNetworkPathAccessible(string path, int timeoutMs = 10000) {
			if (!IsNetworkPath(path))
				return true; // Not a network path, assume accessible

			try {
				// Try to get directory info with timeout
				string dirPath = Path.GetDirectoryName(path) ?? path;
				if (string.IsNullOrEmpty(dirPath))
					dirPath = path;

				// Use Task.Run with timeout to check accessibility
				var task = Task.Run(() => {
					try {
						if (Directory.Exists(dirPath)) {
							// Try to enumerate to verify actual access
							var dir = new DirectoryInfo(dirPath);
							_ = dir.GetFileSystemInfos("*", new EnumerationOptions {
								MaxRecursionDepth = 0,
								IgnoreInaccessible = true
							});
							return true;
						}
						return false;
					}
					catch (Exception ex) {
						Logger.Instance.Info($"Network path check failed for '{path}': {ex.Message}");
						return false;
					}
				});

				// Wait for task completion and check the result
				if (task.Wait(timeoutMs)) {
					try {
						return task.Result; // Check actual result
					}
					catch (AggregateException aggEx) {
						// Unwrap inner exception for better error reporting
						var innerEx = aggEx.InnerException ?? aggEx;
						Logger.Instance.Info($"Network path accessibility task failed for '{path}': {innerEx.Message}");
						return false;
					}
				}
				else {
					// Task didn't complete within timeout
					Logger.Instance.Info($"Network path accessibility check timeout for '{path}' (>{timeoutMs}ms)");
					return false;
				}
			}
			catch (Exception ex) {
				Logger.Instance.Info($"Network path accessibility check error for '{path}': {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// Checks if file exists with timeout (useful for network drives)
		/// </summary>
		public static bool FileExistsWithTimeout(string path, int timeoutMs = 10000) {
			if (string.IsNullOrEmpty(path))
				return false;

			// For network paths, use timeout check
			if (IsNetworkPath(path)) {
				if (!IsNetworkPathAccessible(path, timeoutMs))
					return false;
			}

			try {
				var task = Task.Run(() => File.Exists(path));
				if (task.Wait(timeoutMs)) {
					try {
						return task.Result;
					}
					catch (AggregateException aggEx) {
						var innerEx = aggEx.InnerException ?? aggEx;
						Logger.Instance.Info($"File.Exists task error for '{path}': {innerEx.Message}");
						return false;
					}
				}
				else {
					Logger.Instance.Info($"File.Exists timeout for '{path}' (>{timeoutMs}ms)");
					return false;
				}
			}
			catch (Exception ex) {
				Logger.Instance.Info($"File.Exists error for '{path}': {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// Checks if directory exists with timeout (useful for network drives)
		/// </summary>
		public static bool DirectoryExistsWithTimeout(string path, int timeoutMs = 10000) {
			if (string.IsNullOrEmpty(path))
				return false;

			// For network paths, use timeout check
			if (IsNetworkPath(path)) {
				if (!IsNetworkPathAccessible(path, timeoutMs))
					return false;
			}

			try {
				var task = Task.Run(() => Directory.Exists(path));
				if (task.Wait(timeoutMs)) {
					try {
						return task.Result;
					}
					catch (AggregateException aggEx) {
						var innerEx = aggEx.InnerException ?? aggEx;
						Logger.Instance.Info($"Directory.Exists task error for '{path}': {innerEx.Message}");
						return false;
					}
				}
				else {
					Logger.Instance.Info($"Directory.Exists timeout for '{path}' (>{timeoutMs}ms)");
					return false;
				}
			}
			catch (Exception ex) {
				Logger.Instance.Info($"Directory.Exists error for '{path}': {ex.Message}");
				return false;
			}
		}
	}
}
