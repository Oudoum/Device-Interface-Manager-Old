using System;
using System.IO;
using System.Security.Cryptography;

namespace Device_Interface_Manager.MVVM.Model;

public class WasmModuleUpdater
{
    private const string wasmModuleFolder = @"dim-event-module";
    private const string wasmModuleName = @"DIM_WASM_Module.wasm";

    public string CommunityFolder { get; private set; }

    public string InstallWasmModule()
    {
        if (!Directory.Exists(wasmModuleFolder))
        {
            return "Folder: \"" + wasmModuleFolder + "\" could not be loacted in the DIM directory!";
        }

        if (!AutoDetectCommunityFolder())
        {
            return "Community folder could not be located!";
        }

        if (!WasmModulesAreDifferent())
        {
            return "DIM Event WASM module is up to date!";
        }

        CopyFolder(new DirectoryInfo(wasmModuleFolder), new DirectoryInfo(Path.Combine(CommunityFolder, wasmModuleFolder)));

        return "DIM Event WASM module was successfully installed!";
    }

    public bool AutoDetectCommunityFolder()
    {
        string searchpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft Flight Simulator\UserCfg.opt");

        if (!File.Exists(searchpath))
        {
            searchpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Packages\Microsoft.FlightSimulator_8wekyb3d8bbwe\LocalCache\UserCfg.opt");
            if (!File.Exists(searchpath))
            {
                return false;
            }
        }

        CommunityFolder = ExtractCommunityFolderFromUserCfg(searchpath);

        return true;
    }

    private static string ExtractCommunityFolderFromUserCfg(string UserCfg)
    {
        string installedPackagesPath = null;

        using (StreamReader file = new(UserCfg))
        {
            string line;
            while ((line = file.ReadLine()) is not null)
            {
                if (line.Contains("InstalledPackagesPath"))
                {
                    installedPackagesPath = line[23..].TrimEnd('"');
                    break;
                }
            }
        }

        if (!string.IsNullOrEmpty(installedPackagesPath))
        {
            string communityFolderPath = Path.Combine(installedPackagesPath, "Community");
            if (Directory.Exists(communityFolderPath))
            {
                return communityFolderPath;
            }
        }

        return null;
    }

    private bool WasmModulesAreDifferent()
    {
        string installedWasmPath = Path.Combine(CommunityFolder, wasmModuleFolder, "modules", wasmModuleName);
        if (!File.Exists(installedWasmPath))
        {
            return true;
        }

        string dimWasmPath = Path.Combine(wasmModuleFolder, "modules", wasmModuleName);
        string installedWASM = CalculateMD5(installedWasmPath);
        string dimWASM = CalculateMD5(dimWasmPath);

        return installedWASM != dimWASM;
    }

    private static string CalculateMD5(string filename)
    {
        using FileStream stream = File.OpenRead(filename);
        return BitConverter.ToString(MD5.Create().ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
    }

    private void CopyFolder(DirectoryInfo source, DirectoryInfo target)
    {
        Directory.CreateDirectory(target.FullName);

        foreach (FileInfo fi in source.GetFiles())
        {
            fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
        }

        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
            CopyFolder(diSourceSubDir, nextTargetSubDir);
        }
    }
}