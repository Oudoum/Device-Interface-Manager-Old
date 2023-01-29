using Device_Interface_Manager.MVVM.View;

namespace Device_Interface_Manager.MSFSProfiles.FBW.A32NX
{
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
            fBWA32NXMCDU.Top = this.Top;
            fBWA32NXMCDU.Left = this.Left;
            fBWA32NXMCDU.Height = this.Height;
            fBWA32NXMCDU.Width = this.Width;
            fBWA32NXMCDU.marginTop = this.MarginTop;
            fBWA32NXMCDU.marginBottom = this.MarginBottom;
            fBWA32NXMCDU.marginLeft = this.MarginLeft;
            fBWA32NXMCDU.marginRight = this.MarginRight;
            fBWA32NXMCDU.MCDUGrid.Width = this.GridWidth;
            fBWA32NXMCDU.MCDUGrid.Height = this.GridHeight;
        }

        public void Save(FBWA32NXMCDU fBWA32NXMCDU)
        {
            this.Top = fBWA32NXMCDU.Top;
            this.Left = fBWA32NXMCDU.Left;
            this.Height = fBWA32NXMCDU.Height;
            this.Width = fBWA32NXMCDU.Width;
            this.MarginTop = fBWA32NXMCDU.marginTop;
            this.MarginBottom = fBWA32NXMCDU.marginBottom;
            this.MarginLeft = fBWA32NXMCDU.marginLeft;
            this.MarginRight = fBWA32NXMCDU.marginRight;
            this.GridWidth = fBWA32NXMCDU.MCDUGrid.Width;
            this.GridHeight = fBWA32NXMCDU.MCDUGrid.Height;
            this.Fullscreen = (byte)fBWA32NXMCDU.WindowState;
        }
    }
}