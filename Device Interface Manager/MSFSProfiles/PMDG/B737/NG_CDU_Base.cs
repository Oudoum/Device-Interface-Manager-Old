using System.IO;
using System.Windows;
using System.Text.Json;
using System.Threading.Tasks;
using Device_Interface_Manager.MVVM.View;
using static Device_Interface_Manager.MSFSProfiles.PMDG.PMDG_NG3_SDK;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737;

public class NG_CDU_Base
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

    public void Load(PMDG737CDU pMDG737CDU)
    {
        pMDG737CDU.Top = Top;
        pMDG737CDU.Left = Left;
        pMDG737CDU.Height = Height;
        pMDG737CDU.Width = Width;
        pMDG737CDU.MarginTop = MarginTop;
        pMDG737CDU.MarginBottom = MarginBottom;
        pMDG737CDU.MarginLeft = MarginLeft;
        pMDG737CDU.MarginRight = MarginRight;
        pMDG737CDU.CDUGrid.Width = GridWidth;
        pMDG737CDU.CDUGrid.Height = GridHeight;
        pMDG737CDU.LabelFontSize = FontSize;
    }

    public void Save(PMDG737CDU pMDG737CDU)
    {
        Top = pMDG737CDU.Top;
        Left = pMDG737CDU.Left;
        Height = pMDG737CDU.Height;
        Width = pMDG737CDU.Width;
        MarginTop = pMDG737CDU.MarginTop;
        MarginBottom = pMDG737CDU.MarginBottom;
        MarginLeft = pMDG737CDU.MarginLeft;
        MarginRight = pMDG737CDU.MarginRight;
        GridWidth = pMDG737CDU.CDUGrid.Width;
        GridHeight = pMDG737CDU.CDUGrid.Height;
        FontSize = pMDG737CDU.LabelFontSize;
        Fullscreen = (byte)pMDG737CDU.WindowState;
    }
}

public class PMDG_737_CDU_StartupManager
{
    public string Settings { get; set; }

    public PMDG737CDU pMDG737CDU;

    private NG_CDU_Base pMDG_737_CDU_Screen = new();

    public async Task PMDG737CDUStartup(SimConnectClient simConnectClient)
    {
        PMDG737.RegisterPMDGDataEvents(simConnectClient.simConnect);
        await Application.Current.Dispatcher.InvokeAsync(delegate
        {
            pMDG737CDU = new();
        });
        pMDG737CDU.OnEditormodeOff += PMDG737CDU_EditormodeOff;
        pMDG737CDU.Closing += PMDG737CDU_Closing;
        await pMDG737CDU.Dispatcher.BeginInvoke(async delegate ()
        {
            await GetPMDG737CDUSettingsAsync();
            simConnectClient.TransmitEvent(2, PMDGEvents.EVT_CDU_L_BRITENESS);
            simConnectClient.TransmitEvent(1, PMDGEvents.EVT_CDU_L_BRITENESS);
            simConnectClient.TransmitEvent(0, PMDGEvents.EVT_CDU_L_BRITENESS);
            simConnectClient.TransmitEvent(1, PMDGEvents.EVT_CDU_L_BRITENESS);
            simConnectClient.TransmitEvent(2, PMDGEvents.EVT_CDU_R_BRITENESS);
            simConnectClient.TransmitEvent(1, PMDGEvents.EVT_CDU_R_BRITENESS);
            simConnectClient.TransmitEvent(0, PMDGEvents.EVT_CDU_R_BRITENESS);
            simConnectClient.TransmitEvent(1, PMDGEvents.EVT_CDU_R_BRITENESS);
            pMDG737CDU.Show();
            pMDG737CDU.WindowState = (WindowState)pMDG_737_CDU_Screen.Fullscreen;
            pMDG737CDU.CreatePMDGCDUCells();
        });
    }

    private async Task GetPMDG737CDUSettingsAsync()
    {
        if (File.Exists(Settings))
        {
            string fileContent = await File.ReadAllTextAsync(Settings);
            pMDG_737_CDU_Screen = JsonSerializer.Deserialize<NG_CDU_Base>(fileContent, new JsonSerializerOptions { NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals });
        }
        pMDG_737_CDU_Screen.Load(pMDG737CDU);
    }

    private async Task SaveScreenPropertiesAsync()
    {
        pMDG_737_CDU_Screen.Save(pMDG737CDU);
        using MemoryStream stream = new();
        await JsonSerializer.SerializeAsync(stream, pMDG_737_CDU_Screen, new JsonSerializerOptions { WriteIndented = true, NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals });
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

    private async void PMDG737CDU_EditormodeOff(object sender, System.EventArgs e)
    {
        await SaveScreenPropertiesAsync();
    }

    private async void PMDG737CDU_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        await SaveScreenPropertiesAsync();
    }
}