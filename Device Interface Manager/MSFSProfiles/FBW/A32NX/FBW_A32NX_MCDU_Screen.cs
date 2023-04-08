using Device_Interface_Manager.MVVM.View;

namespace Device_Interface_Manager.MSFSProfiles.FBW.A32NX;

public class FBW_A32NX_MCDU_Screen
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
    public byte Fullscreen { get; set; } = 0;

    public void Load(FBWA32NXMCDU fBWA32NXMCDU)
    {
        fBWA32NXMCDU.Top = Top;
        fBWA32NXMCDU.Left = Left;
        fBWA32NXMCDU.Height = Height;
        fBWA32NXMCDU.Width = Width;
        fBWA32NXMCDU.marginTop = MarginTop;
        fBWA32NXMCDU.marginBottom = MarginBottom;
        fBWA32NXMCDU.marginLeft = MarginLeft;
        fBWA32NXMCDU.marginRight = MarginRight;
        fBWA32NXMCDU.MCDUGrid.Width = GridWidth;
        fBWA32NXMCDU.MCDUGrid.Height = GridHeight;
    }

    public void Save(FBWA32NXMCDU fBWA32NXMCDU)
    {
        Top = fBWA32NXMCDU.Top;
        Left = fBWA32NXMCDU.Left;
        Height = fBWA32NXMCDU.Height;
        Width = fBWA32NXMCDU.Width;
        MarginTop = fBWA32NXMCDU.marginTop;
        MarginBottom = fBWA32NXMCDU.marginBottom;
        MarginLeft = fBWA32NXMCDU.marginLeft;
        MarginRight = fBWA32NXMCDU.marginRight;
        GridWidth = fBWA32NXMCDU.MCDUGrid.Width;
        GridHeight = fBWA32NXMCDU.MCDUGrid.Height;
        Fullscreen = (byte)fBWA32NXMCDU.WindowState;
    }
}