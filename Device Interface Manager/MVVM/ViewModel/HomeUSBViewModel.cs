using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MSFSProfiles.PMDG;
using Device_Interface_Manager.MSFSProfiles.WASM;
using static Device_Interface_Manager.MVVM.ViewModel.MainViewModel;
using static Device_Interface_Manager.MVVM.Model.HomeUSBModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    partial class HomeUSBViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isSimConnectOpen;

        [ObservableProperty]
        private bool _isUSBEnabled = true;

        public ObservableCollection<string> BoardInfo { get; set; } = new();

        public ObservableCollection<string> BoardType { get; set; } = new();

        public ObservableCollection<Connection> Connections { get; set; } = new();

        public ObservableCollection<Profile> Profiles { get; set; } = new();

        private List<USBPMDG> ListPMDG { get; set; } = new();
        private List<USBWASM> ListWASM { get; set; } = new();

        public HomeUSBViewModel()
        {
            CreateProfiles();
            LoadUSBData();

            if (Properties.Settings.Default.AutoHide && Connections.Count > 0)
            {
                StartUSB();
            }
        }

        public void GetBoardInfo()
        {
            if (DeviceList.Count == 0)
            {
                this.BoardInfo.Clear();
                this.BoardType.Clear();
                return;
            }

            this.BoardInfo.Clear();

            this.BoardInfo.Add("Board " + DeviceList[GetSeletedController()].DeviceInfo.szBoardType + " manufactured on " + DeviceList[GetSeletedController()].DeviceInfo.szManufactureDate + " has the following features: ");


            if (DeviceList[GetSeletedController()].DeviceInfo.dwFeatures == InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_NONE)
                this.BoardInfo.Add("No features programmed. Please obtain the update patch for this board.");
            else this.BoardInfo.Add(string.Empty);

            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_LED) != 0)
                this.BoardInfo.Add(DeviceList[GetSeletedController()].DeviceInfo.nLEDCount + " | LEDs ( " + DeviceList[GetSeletedController()].DeviceInfo.nLEDFirst + " - " + DeviceList[GetSeletedController()].DeviceInfo.nLEDLast + " )");
            else this.BoardInfo.Add(string.Empty);


            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_INPUT_SWITCHES) != 0)
                this.BoardInfo.Add(DeviceList[GetSeletedController()].DeviceInfo.nSwitchCount + " | Switches ( " + DeviceList[GetSeletedController()].DeviceInfo.nSwitchFirst + " - " + DeviceList[GetSeletedController()].DeviceInfo.nSwitchLast + " )");
            else this.BoardInfo.Add(string.Empty);


            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_7SEGMENT) != 0)
                this.BoardInfo.Add(DeviceList[GetSeletedController()].DeviceInfo.n7SegmentCount + " | 7 Segments ( " + DeviceList[GetSeletedController()].DeviceInfo.n7SegmentFirst + " - " + DeviceList[GetSeletedController()].DeviceInfo.n7SegmentLast + " )");
            else this.BoardInfo.Add(string.Empty);


            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_DATALINE) != 0)
                this.BoardInfo.Add(DeviceList[GetSeletedController()].DeviceInfo.nDatalineCount + " | Datalines ( " + DeviceList[GetSeletedController()].DeviceInfo.nDatalineFirst + " - " + DeviceList[GetSeletedController()].DeviceInfo.nDatalineLast + " )");
            else this.BoardInfo.Add(string.Empty);


            //Not available
            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_OUTPUT_SERVO) != 0)
                this.BoardInfo.Add(DeviceList[GetSeletedController()].DeviceInfo.nServoController + " | Servos ( " + DeviceList[GetSeletedController()].DeviceInfo.nServoControllerFirst + " - " + DeviceList[GetSeletedController()].DeviceInfo.nServoControllerLast + " )");
            else this.BoardInfo.Add(string.Empty);


            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_BRIGHTNESS) != 0)
                this.BoardInfo.Add("Brightness control supported");
            else this.BoardInfo.Add(string.Empty);


            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG_INPUT) != 0)
                this.BoardInfo.Add("Analog input supported (Single Channel)");
            else this.BoardInfo.Add(string.Empty);


            if ((DeviceList[GetSeletedController()].DeviceInfo.dwFeatures & InterfaceIT_BoardInfo.Features.INTERFACEIT_FEATURE_SPECIAL_ANALOG16_INPUT) != 0)
                this.BoardInfo.Add("Analog input supported (16 Channels)");
            else this.BoardInfo.Add(string.Empty);


            this.BoardType.Clear();

            foreach (var field in typeof(InterfaceIT_BoardIDs).GetFields())
            {
                if ((string)field.GetValue(null) == DeviceList[GetSeletedController()].DeviceInfo.szBoardType)
                    this.BoardType.Add(field.Name.ToString().Replace('_', ' '));
            }
        }

        [RelayCommand]
        private async void StartUSB()
        {
            this.IsUSBEnabled = !this.IsUSBEnabled;

            if (!this.IsUSBEnabled)
            {
                foreach (var connection in this.Connections)
                {
                    switch (connection.Profile.Id)
                    {
                        case 1:
                            MSFSProfiles.WASM.FENIX.A320.MCDU_L_USB mCDU_L_USB = new();
                            await Task.Run(() => mCDU_L_USB.Start(DeviceList.FirstOrDefault(o => o.SerialNumber == connection.Serial)));
                            this.ListWASM.Add(mCDU_L_USB);
                            break;

                        case 2:
                            MSFSProfiles.WASM.FENIX.A320.MCDU_R_USB mCDU_RUSB = new();
                            await Task.Run(() => mCDU_RUSB.Start(DeviceList.FirstOrDefault(o => o.SerialNumber == connection.Serial)));
                            this.ListWASM.Add(mCDU_RUSB);
                            break;

                        case 3:
                            MSFSProfiles.WASM.FBW.A32NX.MCDU_L_USB mCDU_L_USB1 = new();
                            await Task.Run(() => mCDU_L_USB1.Start(DeviceList.FirstOrDefault(o => o.SerialNumber == connection.Serial)));
                            this.ListWASM.Add(mCDU_L_USB1);
                            break;

                        case 4:
                            MSFSProfiles.WASM.FBW.A32NX.MCDU_R_USB mCDU_R_USB1 = new();
                            await Task.Run(() => mCDU_R_USB1.Start(DeviceList.FirstOrDefault(o => o.SerialNumber == connection.Serial)));
                            this.ListWASM.Add(mCDU_R_USB1);
                            break;

                        case 5:
                            MSFSProfiles.PMDG.B737.NG_CDU_L_USB nG_CDU_L_USB = new();
                            await Task.Run(() => nG_CDU_L_USB.Start(DeviceList.FirstOrDefault(o => o.SerialNumber == connection.Serial)));
                            this.ListPMDG.Add(nG_CDU_L_USB);
                            break;

                        case 6:
                            MSFSProfiles.PMDG.B737.NG_CDU_R_USB nG_CDU_R_USB = new();
                            await Task.Run(() => nG_CDU_R_USB.Start(DeviceList.FirstOrDefault(o => o.SerialNumber == connection.Serial)));
                            this.ListPMDG.Add(nG_CDU_R_USB);
                            break;

                        case 7:
                            MSFSProfiles.PMDG.B737.NG_MCP_USB nG_MCP_USB = new();
                            await Task.Run(() => nG_MCP_USB.Start(DeviceList.FirstOrDefault(o => o.SerialNumber == connection.Serial)));
                            this.ListPMDG.Add(nG_MCP_USB);
                            break;

                        case 8:
                            MSFSProfiles.PMDG.B737.NG_MCP_777_USB nG_MCP_777_USB = new();
                            await Task.Run(() => nG_MCP_777_USB.Start(DeviceList.FirstOrDefault(o => o.SerialNumber == connection.Serial)));
                            this.ListPMDG.Add(nG_MCP_777_USB);
                            break;

                        default:
                            break;
                    }
                }
            }

            if (this.IsUSBEnabled)
            {
                this.ListWASM.ForEach(o => o.Stop());
                this.ListPMDG.ForEach(o => o.Stop());

                this.ListWASM.Clear();
                this.ListPMDG.Clear();

                this.SaveUSBData();
            }
        }

        [RelayCommand]
        private void DeleteRow(Connection connection)
        {
            foreach (var conn in Connections)
            {
                if (conn == connection)
                {
                    Connections.Remove(conn);
                    return;
                }
            }
        }

        [RelayCommand]
        private void AddRow()
        {
            Connections.Add(new Connection() { Id = 0, Name = "NAME", Serial = "TEST", Profile = Profiles[0] });
        }

        private void CreateProfiles()
        {
            Profiles.Add(new Profile { Id = 0, Name = "-- None --" });

            Profiles.Add(new Profile { Id = 1, Name = "Fenix A320 Left MCDU" });
            Profiles.Add(new Profile { Id = 2, Name = "Fenix A320 Right MCDU" });

            Profiles.Add(new Profile { Id = 3, Name = "FBW A32NX Left MCDU" });
            Profiles.Add(new Profile { Id = 4, Name = "FBW A32NX Right MCDU" });

            Profiles.Add(new Profile { Id = 5, Name = "PMDG 737NG Left CDU" });
            Profiles.Add(new Profile { Id = 6, Name = "PMDG 737NG Right CDU" });

            Profiles.Add(new Profile { Id = 7, Name = "PMDG 737NG MCP" });
            Profiles.Add(new Profile { Id = 8, Name = "PMDG 737NG MCP (777)" });

            //Profiles.Add(new Profile { Id = 7, Name = "PMDG 737MAX Left CDU" });
            //Profiles.Add(new Profile { Id = 8, Name = "PMDG 737MAX Right CDU" });

            //Profiles.Add(new Profile { Id = 9, Name = "PMDG 777 Left CDU" });
            //Profiles.Add(new Profile { Id = 10, Name = "PMDG 777 Right CDU" });
            //Profiles.Add(new Profile { Id = 11, Name = "PMDG 777 Center CDU" });

            //Profiles.Add(new Profile { Id = 12, Name = "PMDG 747 Left CDU" });
            //Profiles.Add(new Profile { Id = 13, Name = "PMDG 747 Right CDU" });
            //Profiles.Add(new Profile { Id = 14, Name = "PMDG 747 Center CDU" });
        }

        [RelayCommand]
        private void ResetScreens()
        {
            this.ListPMDG.ForEach(o => { o.pMDG737CDU.Top = 0; o.pMDG737CDU.Left = 0; });
            this.ListWASM.ForEach(o => { o.fBWA32NXMCDU.Top = 0; o.fBWA32NXMCDU.Left = 0; });
        }

        private const string usb = @"Profiles\USB.json";
        private void LoadUSBData()
        {
            if (File.Exists(usb))
            {
                this.Connections = JsonConvert.DeserializeObject<ObservableCollection<Connection>>(File.ReadAllText(usb));
            }
        }

        public void SaveUSBData()
        {
            if (this.Connections.Count == 0)
            {
                if (Directory.Exists((usb.Remove(8))))
                {
                    File.Delete(usb);
                }
                return;
            }
            foreach (var status in this.Connections)
            {
                status.Status = 0;
            }
            Directory.CreateDirectory(usb.Remove(8));
            string json = JsonConvert.SerializeObject(this.Connections, Formatting.Indented);
            if (File.Exists(usb))
            {
                if (File.ReadAllText(usb) != json)
                {
                    File.WriteAllText(usb, json);
                }
                return;
            }
            File.WriteAllText(usb, json);
        }
    }
}