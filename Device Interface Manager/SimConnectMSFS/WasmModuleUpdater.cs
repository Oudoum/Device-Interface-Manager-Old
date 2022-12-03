using MobiFlight.Base;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace MobiFlight.SimConnectMSFS
{
    public class WasmModuleUpdater
    {
        public const String WasmModuleFolder = @".\MSFS2020-module\mobiflight-event-module";
        
        public const String WasmEventsTxtUrl = @"https://hubhop-api-mgtm.azure-api.net/api/v1/export/presets?type=wasm";
        public const String WasmEventsTxtFolder = @"mobiflight-event-module\modules";
        public const String WasmEventsTxtFile = "events.txt";

        public const String WasmEventsCipUrl = @"https://hubhop-api-mgtm.azure-api.net/api/v1/export/presets?type=cip";
        public const String WasmEventsCipFolder = @".\presets";
        public const String WasmEventsCipFileName = @"msfs2020_eventids.cip";

        public const String WasmEventsSimVarsUrl = @"https://hubhop-api-mgtm.azure-api.net/api/v1/export/presets?type=simVars";
        public const String WasmEventsSimVarsFolder = @".\presets";
        public const String WasmEventsSimVarsFileName = @"msfs2020_simvars.cip";

        public const String WasmEventHubHHopUrl = @"https://hubhop-api-mgtm.azure-api.net/api/v1/presets?type=json";
        public const String WasmEventsHubHopFolder = @".\presets";
        public const String WasmEventsHubHopFileName = @"msfs2020_hubhop_presets.json";

        public const String WasmModuleName = @"MobiFlightWasmModule.wasm";
        public const String WasmModuleNameOld = @"StandaloneModule.wasm";

        public event EventHandler<ProgressUpdateEvent> DownloadAndInstallProgress;

        public String CommunityFolder { get; set; }

        private static String ExtractCommunityFolderFromUserCfg(String UserCfg)
        {
            string CommunityFolder = null;
            string line;
            string InstalledPackagesPath = "";
            StreamReader file = new(UserCfg);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("InstalledPackagesPath"))
                {
                    InstalledPackagesPath = line;
                }
            }

            if (InstalledPackagesPath == "")
                return CommunityFolder;

            InstalledPackagesPath = InstalledPackagesPath[23..];
            char[] charsToTrim = { '"' };
            
            InstalledPackagesPath = InstalledPackagesPath.TrimEnd(charsToTrim);
            
            if (Directory.Exists(InstalledPackagesPath + @"\Community"))
            {
                CommunityFolder = InstalledPackagesPath + @"\Community";
            }

            return CommunityFolder;
        }
        public bool AutoDetectCommunityFolder()
        {
            string searchpath = Environment.GetEnvironmentVariable("AppData") + @"\Microsoft Flight Simulator\UserCfg.opt";

            if (!File.Exists(searchpath))
            {
                searchpath = Environment.GetEnvironmentVariable("LocalAppData") + @"\Packages\Microsoft.FlightSimulator_8wekyb3d8bbwe\LocalCache\UserCfg.opt";
                if (!File.Exists(searchpath))
                {
                    return false;
                }
            }

            CommunityFolder = ExtractCommunityFolderFromUserCfg(searchpath);
            return true;
        }

        public bool InstallWasmModule()
        {
            if (!Directory.Exists(WasmModuleFolder))
            {
                return false;
            }

            if (!Directory.Exists(CommunityFolder))
            {
                return false;
            }

            String destFolder = CommunityFolder + @"\mobiflight-event-module";
            CopyFolder(new DirectoryInfo(WasmModuleFolder), new DirectoryInfo(destFolder));

            // Remove the old Wasm File
            DeleteOldWasmFile();

            return true;
        }

        private void DeleteOldWasmFile()
        {
            String installedWASM = CommunityFolder + $@"\mobiflight-event-module\modules\{WasmModuleNameOld}";
            if(File.Exists(installedWASM))
                File.Delete(installedWASM);
        }

        public static void CopyFolder(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyFolder(diSourceSubDir, nextTargetSubDir);
            }
        }

        static string CalculateMD5(string filename)
        {
            var md5 = MD5.Create();
            using var stream = File.OpenRead(filename);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public bool WasmModulesAreDifferent()
        {
            Console.WriteLine("Check if WASM module needs to be updated");

            string installedWASM;
            string mobiflightWASM;
            string installedEvents;
            string mobiflightEvents;

            if (CommunityFolder == null) return true;


            if (!File.Exists(CommunityFolder + $@"\mobiflight-event-module\modules\{WasmModuleName}"))
                return true;

            installedWASM = CalculateMD5(CommunityFolder + $@"\mobiflight-event-module\modules\{WasmModuleName}");
            mobiflightWASM = CalculateMD5($@".\MSFS2020-module\mobiflight-event-module\modules\{WasmModuleName}");

            installedEvents = CalculateMD5(CommunityFolder + $@"\mobiflight-event-module\modules\{WasmEventsTxtFile}");
            mobiflightEvents = CalculateMD5($@".\MSFS2020-module\mobiflight-event-module\modules\{WasmEventsTxtFile}");

            return (installedWASM != mobiflightWASM || installedEvents != mobiflightEvents);
        }

        public bool InstallWasmEvents()
        {
            String destFolder = Path.Combine(CommunityFolder, WasmEventsTxtFolder);

            try
            {

                if (!Directory.Exists(destFolder))
                {
                    return false;
                }

                if (!Directory.Exists(WasmModuleFolder))
                {
                    return false;
                }

                if (!Directory.Exists(CommunityFolder))
                {
                    return false;
                }

                if (!DownloadWasmEvents())
                {
                    return false;
                }

                BackupAndInstallWasmEventsTxt(destFolder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private static void BackupAndInstallWasmEventsTxt(string destFolder)
        {
            // Copy wasm events.txt to community folder
            // and create a backup file
            String sourceFile = Path.Combine(WasmModuleFolder, "modules", WasmEventsTxtFile);
            String destFile = Path.Combine(destFolder, WasmEventsTxtFile);

            File.Delete(destFile + ".bak");

            if (File.Exists(destFile))
                File.Move(destFile, destFile + ".bak");

            File.Copy(sourceFile, destFile);
        }

        public bool DownloadWasmEvents()
        {
            ProgressUpdateEvent progress = new()
            {
                ProgressMessage = "Downloading WASM events.txt (legacy)",
                Current = 5
            };
            DownloadAndInstallProgress?.Invoke(this, progress);

            if (!DownloadSingleFile(new Uri(WasmEventsTxtUrl), WasmEventsTxtFile, WasmModuleFolder + @"\modules")) return false;

            progress.ProgressMessage = "Downloading EventIDs (legacy)";
            progress.Current = 25;
            DownloadAndInstallProgress?.Invoke(this, progress);
            if (!DownloadSingleFile(new Uri(WasmEventsCipUrl), WasmEventsCipFileName, WasmEventsCipFolder)) return false;

            progress.ProgressMessage = "Downloading SimVars (legacy)";
            progress.Current = 50;
            DownloadAndInstallProgress?.Invoke(this, progress);
            if (!DownloadSingleFile(new Uri(WasmEventsSimVarsUrl), WasmEventsSimVarsFileName, WasmEventsSimVarsFolder)) return false;

            progress.ProgressMessage = "Downloading HubHop Presets";
            progress.Current = 75;
            DownloadAndInstallProgress?.Invoke(this, progress);
            if (!DownloadSingleFile(new Uri(WasmEventHubHHopUrl), WasmEventsHubHopFileName, WasmEventsHubHopFolder)) return false;

            progress.ProgressMessage = "Downloading done";
            progress.Current = 100;
            DownloadAndInstallProgress?.Invoke(this, progress);
            return true;
        }

        private static bool DownloadSingleFile(Uri uri, String filename, String targetPath)
        {
            SecurityProtocolType oldType = ServicePointManager.SecurityProtocol;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string tmpFile = Directory.GetCurrentDirectory() + targetPath + @"\" + filename + ".tmp";
            System.Net.Http.HttpClient httpClient = new();
            File.WriteAllText(tmpFile, httpClient.GetStringAsync(uri).Result);
            httpClient.Dispose();

            File.Delete($@"{targetPath}\{filename}");
            File.Move(tmpFile, $@"{targetPath}\{filename}");

            ServicePointManager.SecurityProtocol = oldType;
            return true;
        }
    }
}