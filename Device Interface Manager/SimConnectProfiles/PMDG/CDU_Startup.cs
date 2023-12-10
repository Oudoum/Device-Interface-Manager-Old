using System.IO;
using System.Windows;
using System.Text.Json;
using System.Threading.Tasks;
using Device_Interface_Manager.Views;
using System;

namespace Device_Interface_Manager.SimConnectProfiles.PMDG;

public class CDU_Startup
{
    public double Top { get; set; } = 0;
    public double Left { get; set; } = 0;
    public double Height { get; set; } = 770;
    public double Width { get; set; } = 830;
    public double MarginTop { get; set; } = 0;
    public double MarginBottom { get; set; } = 0;
    public double MarginLeft { get; set; } = 0;
    public double MarginRight { get; set; } = 0;
    public double GridWidth { get; set; } = double.NaN;
    public double GridHeight { get; set; } = double.NaN;
    public double FontSize { get; set; } = 60;
    public byte Fullscreen { get; set; } = 0;

    public void Load(PMDGCDU pMDGCDU, string settings)
    {
        pMDGCDU.Title = $"{Path.GetDirectoryName(settings).Remove(0, 8).Replace("\\", string.Empty)} {Path.GetFileNameWithoutExtension(settings)}";
        pMDGCDU.Top = Top;
        pMDGCDU.Left = Left;
        pMDGCDU.Height = Height;
        pMDGCDU.Width = Width;
        pMDGCDU.MarginTop = MarginTop;
        pMDGCDU.MarginBottom = MarginBottom;
        pMDGCDU.MarginLeft = MarginLeft;
        pMDGCDU.MarginRight = MarginRight;
        pMDGCDU.CDUGrid.Width = GridWidth;
        pMDGCDU.CDUGrid.Height = GridHeight;
        pMDGCDU.TextBlockFontSize = FontSize;
        pMDGCDU.CreatePMDGCDUCells();
        pMDGCDU.Show();
        pMDGCDU.WindowState = (WindowState)Fullscreen;
    }

    public void Save(PMDGCDU pMDGCDU)
    {
        Top = pMDGCDU.Top;
        Left = pMDGCDU.Left;
        Height = pMDGCDU.Height;
        Width = pMDGCDU.Width;
        MarginTop = pMDGCDU.MarginTop;
        MarginBottom = pMDGCDU.MarginBottom;
        MarginLeft = pMDGCDU.MarginLeft;
        MarginRight = pMDGCDU.MarginRight;
        GridWidth = pMDGCDU.CDUGrid.Width;
        GridHeight = pMDGCDU.CDUGrid.Height;
        FontSize = pMDGCDU.TextBlockFontSize;
        Fullscreen = (byte)pMDGCDU.WindowState;
    }
}

public class PMDG_CDU_StartupManager
{
    public PMDG_CDU_StartupManager(string settings)
    {
        Settings = settings;
    }

    private string Settings { get; set; }

    private CDU_Startup pMDG_CDU_Screen = new();

    public PMDGCDU PMDGCDU { get; private set; }

    public async Task PMDG737CDUStartupAsync(SimConnectClient simConnectClient, Enum cDUBrightnessButton)
    {
        await Application.Current.Dispatcher.InvokeAsync(delegate
        {
            PMDGCDU = new();
        });
        PMDGCDU.OnEditormodeOff += PMDGCDU_EditormodeOff;
        PMDGCDU.Closing += PMDGCDU_Closing;
        await PMDGCDU.Dispatcher.InvokeAsync(async delegate ()
        {
            await GetPMDGCDUSettingsAsync();
            simConnectClient.TransmitEvent(2, cDUBrightnessButton);
            simConnectClient.TransmitEvent(1, cDUBrightnessButton);
            simConnectClient.TransmitEvent(0, cDUBrightnessButton);
            simConnectClient.TransmitEvent(1, cDUBrightnessButton);
        });
    }

    public async Task GetPMDGCDUSettingsAsync()
    {
        if (File.Exists(Settings))
        {
            string fileContent = await File.ReadAllTextAsync(Settings);
            pMDG_CDU_Screen = JsonSerializer.Deserialize<CDU_Startup>(fileContent, new JsonSerializerOptions { NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals });
        }
        pMDG_CDU_Screen.Load(PMDGCDU, Settings);
    }

    public async Task SaveScreenPropertiesAsync()
    {
        pMDG_CDU_Screen.Save(PMDGCDU);
        using MemoryStream stream = new();
        await JsonSerializer.SerializeAsync(stream, pMDG_CDU_Screen, new JsonSerializerOptions { WriteIndented = true, NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals });
        stream.Position = 0;
        using StreamReader reader = new(stream);
        string json = await reader.ReadToEndAsync();
        Directory.CreateDirectory(Path.GetDirectoryName(Settings));
        if (File.Exists(Settings))
        {
            string existingContent = await File.ReadAllTextAsync(Settings);
            if (existingContent != json)
            {
                await File.WriteAllTextAsync(Settings, json);
            }
            return;
        }
        await File.WriteAllTextAsync(Settings, json);
    }

    private async void PMDGCDU_EditormodeOff(object sender, EventArgs e)
    {
        await SaveScreenPropertiesAsync();
    }

    private async void PMDGCDU_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        await SaveScreenPropertiesAsync();
    }
}