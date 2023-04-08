using Device_Interface_Manager.MVVM.View;
using System.Text.Json.Serialization;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737;

public class PMDG_737_CDU_Screen
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
        pMDG737CDU.marginTop = MarginTop;
        pMDG737CDU.marginBottom = MarginBottom;
        pMDG737CDU.marginLeft = MarginLeft;
        pMDG737CDU.marginRight = MarginRight;
        pMDG737CDU.CDUGrid.Width = GridWidth;
        pMDG737CDU.CDUGrid.Height = GridHeight;
        pMDG737CDU.fontSize = FontSize;
    }

    public void Save(PMDG737CDU pMDG737CDU)
    {
        Top = pMDG737CDU.Top;
        Left = pMDG737CDU.Left;
        Height = pMDG737CDU.Height;
        Width = pMDG737CDU.Width;
        MarginTop = pMDG737CDU.marginTop;
        MarginBottom = pMDG737CDU.marginBottom;
        MarginLeft = pMDG737CDU.marginLeft;
        MarginRight = pMDG737CDU.marginRight;
        GridWidth = pMDG737CDU.CDUGrid.Width;
        GridHeight = pMDG737CDU.CDUGrid.Height;
        FontSize = pMDG737CDU.fontSize;
        Fullscreen = (byte)pMDG737CDU.WindowState;
    }
}