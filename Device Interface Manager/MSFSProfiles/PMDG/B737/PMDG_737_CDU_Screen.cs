using Device_Interface_Manager.MVVM.View;

namespace Device_Interface_Manager.MSFSProfiles.PMDG.B737
{
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

        public void Load(PMDG737CDU pMDG737CDU)
        {
            pMDG737CDU.Top = this.Top;
            pMDG737CDU.Left = this.Left;
            pMDG737CDU.Height = this.Height;
            pMDG737CDU.Width = this.Width;
            pMDG737CDU.marginTop = this.MarginTop;
            pMDG737CDU.marginBottom = this.MarginBottom;
            pMDG737CDU.marginLeft = this.MarginLeft;
            pMDG737CDU.marginRight = this.MarginRight;
            pMDG737CDU.CDUGrid.Width = this.GridWidth;
            pMDG737CDU.CDUGrid.Height = this.GridHeight;
            pMDG737CDU.fontSize = this.FontSize;
        }

        public void Save(PMDG737CDU pMDG737CDU)
        {
            this.Top = pMDG737CDU.Top;
            this.Left = pMDG737CDU.Left;
            this.Height = pMDG737CDU.Height;
            this.Width = pMDG737CDU.Width;
            this.MarginTop = pMDG737CDU.marginTop;
            this.MarginBottom = pMDG737CDU.marginBottom;
            this.MarginLeft = pMDG737CDU.marginLeft;
            this.MarginRight = pMDG737CDU.marginRight;
            this.GridWidth = pMDG737CDU.CDUGrid.Width;
            this.GridHeight = pMDG737CDU.CDUGrid.Height;
            this.FontSize = pMDG737CDU.fontSize;
        }
    }
}