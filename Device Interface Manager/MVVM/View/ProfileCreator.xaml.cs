using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.MVVM.ViewModel;
using Device_Interface_Manager.Profiles.FENIX.A320;
using Device_Interface_Manager.Profiles.PMDG.B737;
using MobiFlight.HubHop;

namespace Device_Interface_Manager.MVVM.View
{
    public enum HubHopPanelMode
    {
        Output,
        Input
    } 

    public partial class ProfileCreator : Window
    {
        private readonly HubHopPanelMode Mode = HubHopPanelMode.Output;

        public String PresetFile { get; set; }
        public String PresetFileUser { get; set; }

        public const byte MINIMUM_SEARCH_STRING_LENGTH = 1;
        public const string Hubhoppreset = @"Presets\msfs2020_hubhop_presets.json";

        protected List<String> lVars = new();
        readonly MobiFlight.SimConnectMSFS.SimConnectCache simConnectCache = new();
        readonly Msfs2020HubhopPresetList PresetList = null;
        readonly Msfs2020HubhopPresetList FilteredPresetList = new();


        public static InterfaceITEthernet Test { get; set; } = new();

        public ProfileCreator()
        {
            InitializeComponent();

            PresetList = Msfs2020HubhopPresetListSingleton.Instance;

            PresetList.Load(Hubhoppreset);

            simConnectCache.Connect();
            simConnectCache.Start();
            simConnectCache.ReceiveSimConnectMessage();


            Msfs2020EventPresetList deprecatedPresets = new();
            deprecatedPresets.Load();

            FilteredPresetList.Load(Hubhoppreset);


            deprecatedPresets.FindCodeByEventId("A320_Neo_CDU_1_BTN_R");


            FilteredPresetListChanged();
            FilterPresetList();

            SimVarNameTextBox.TextChanged += SimVarNameTextBox_TextChanged;
            FilterTextBox.TextChanged += FilterTextBox_TextChanged;


            IP.Text = Test.Hostname;

        }

        private void InitializeComboBoxesWithPreset(Msfs2020HubhopPreset preset)
        {
            SetSelectedItemByValue(VendorComboBox, preset.vendor);
            SetSelectedItemByValue(AircraftComboBox, preset.aircraft);
            SetSelectedItemByValue(SystemComboBox, preset.system);
            PresetComboBox.SelectionChanged -= PresetComboBox_SelectionChanged;
            PresetComboBox.SelectedValue = preset.Id;
            DescriptionTextBlock.Text = preset?.description;
            PresetComboBox.SelectionChanged += PresetComboBox_SelectionChanged;
        }

        static private bool SetSelectedItemByValue(ComboBox comboBox, string value)
        {
            foreach (object item in comboBox.Items)
            {
                if ((item.ToString()) == value)
                {
                    comboBox.SelectedItem = item;
                    return true;
                }
            }
            return false;
        }

        private void PresetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PresetComboBox.SelectedValue == null) return;
            Msfs2020HubhopPreset selectedItem = PresetComboBox.SelectedItem as Msfs2020HubhopPreset;

            Msfs2020HubhopPreset selectedPreset = FilteredPresetList.Items.Find(x => x.Id == selectedItem.Id);
            if (selectedPreset == null) return;
            DescriptionTextBlock.Text = selectedPreset?.description;
            SimVarNameTextBox.Text = selectedPreset?.Code;

            InitializeComboBoxesWithPreset(selectedPreset);
        }

        private void SimVarNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SimVarNameTextBox.Text.Contains(":index"))
            {
                SimVarNameTextBox.Text = SimVarNameTextBox.Text.Replace(":index", ":" + 1);
            }

        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DescriptionTextBlock.Text = null;
            SimVarNameTextBox.Text = null;
            FilterPresetList();
        }

        private void FilterPresetList()
        {
            String SelectedVendor = null;
            String SelectedAircraft = null;
            String SelectedSystem = null;
            String FilterText = null;

            if (VendorComboBox.SelectedIndex > 0) SelectedVendor = VendorComboBox.SelectedItem.ToString();
            if (AircraftComboBox.SelectedIndex > 0) SelectedAircraft = AircraftComboBox.SelectedItem.ToString();
            if (SystemComboBox.SelectedIndex > 0) SelectedSystem = SystemComboBox.SelectedItem.ToString();
            if (FilterTextBox.Text != "" && FilterTextBox.Text.Length >= MINIMUM_SEARCH_STRING_LENGTH) FilterText = FilterTextBox.Text;

            FilteredPresetList.Items.Clear();
            FilteredPresetList.Items.Add(new Msfs2020HubhopPreset()
            {
                Label = "- Select Preset -",
                Id = "-",
                Code = "",
                description = "No Preset selected."
            });

            HubHopType hubhopType = HubHopType.Output;
            if (Mode == HubHopPanelMode.Input) hubhopType = HubHopType.AllInputs;

            FilteredPresetList.Items.AddRange(
                PresetList.Filtered(
                    hubhopType,
                    SelectedVendor,
                    SelectedAircraft,
                    SelectedSystem,
                    FilterText
                    )
            );

            // Substract 1 because of the static "select preset"-label
            int MatchesFound = FilteredPresetList.Items.Count - 1;
            MatchLabel.Content = String.Format(("{0} matches found."),
                                    MatchesFound);

            FilteredPresetListChanged();
        }

        private void FilteredPresetListChanged()
        {
            HubHopType hubhopType = HubHopType.Output;
            if (Mode == HubHopPanelMode.Input) hubhopType = HubHopType.AllInputs;

            UpdateValues(VendorComboBox, FilteredPresetList.AllVendors(hubhopType).ToArray());
            UpdateValues(AircraftComboBox, FilteredPresetList.AllAircraft(hubhopType).ToArray());
            UpdateValues(SystemComboBox, FilteredPresetList.AllSystems(hubhopType).ToArray());
            UpdatePresetComboBoxValues();
        }

        private void UpdatePresetComboBoxValues()
        {
            String SelectedValue = null;
            Msfs2020HubhopPreset selectedPreset = null;

            PresetComboBox.SelectionChanged -= PresetComboBox_SelectionChanged;
            if (PresetComboBox.SelectedIndex > 0)
            {
                selectedPreset = PresetComboBox.SelectedItem as Msfs2020HubhopPreset;
                SelectedValue = selectedPreset.Id;
                if (selectedPreset.Id == "0")
                {
                    FilteredPresetList.Items.Add(selectedPreset);
                }
            }

            PresetComboBox.ItemsSource = null;
            PresetComboBox.ItemsSource = FilteredPresetList.Items;
            PresetComboBox.SelectedValuePath = "id";
            PresetComboBox.DisplayMemberPath = "label";

            if (SelectedValue != null)
            {
                PresetComboBox.SelectedValue = SelectedValue;

                // we didn't find the preset within the current
                // list
                if (PresetComboBox.SelectedValue == null)
                    PresetComboBox.SelectedIndex = 0;
            }
            else
            {
                PresetComboBox.SelectedIndex = 0;
            }

            PresetComboBox.IsEnabled = (FilteredPresetList.Items.Count > 1);

            PresetComboBox.SelectionChanged += PresetComboBox_SelectionChanged;
        }

        private void UpdateValues(ComboBox cb, String[] valueList)
        {
            String SelectedValue = null;
            cb.SelectionChanged -= OnFilter_SelectionChanged;
            if (cb.SelectedIndex > 0)
            {
                SelectedValue = cb.SelectedItem.ToString();
            }

            cb.Items.Clear();
            cb.Items.Add("-show all -");
            cb.SelectedIndex = 0;
            foreach (String value in valueList)
            {
                cb.Items.Add(value);
            }

            if (SelectedValue != null)
            {
                if (cb.Items.IndexOf(SelectedValue) != -1)
                {
                    cb.SelectedIndex = cb.Items.IndexOf(SelectedValue);
                }
            }

            cb.SelectionChanged += OnFilter_SelectionChanged;
        }

        private void OnFilter_SelectionChanged(object sender, EventArgs e)
        {
            FilterPresetList();
        }

        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ResetFilter();
        }

        private void ResetFilter()
        {
            SuspendUpdateEvents();
            VendorComboBox.SelectedIndex = 0;
            AircraftComboBox.SelectedIndex = 0;
            SystemComboBox.SelectedIndex = 0;
            DescriptionTextBlock.Text = null;
            FilterTextBox.Text = null;
            SimVarNameTextBox.Text = null;

            FilterPresetList();

            ResumeUpdateEvents();
        }

        private void SuspendUpdateEvents()
        {
            VendorComboBox.SelectionChanged -= OnFilter_SelectionChanged;
            AircraftComboBox.SelectionChanged -= OnFilter_SelectionChanged;
            SystemComboBox.SelectionChanged -= OnFilter_SelectionChanged;
            FilterTextBox.TextChanged -= FilterTextBox_TextChanged;
        }

        private void ResumeUpdateEvents()
        {
            // make sure to remove potentially
            // registered events
            SuspendUpdateEvents();

            // And now really register
            // all required events
            VendorComboBox.SelectionChanged += OnFilter_SelectionChanged;
            AircraftComboBox.SelectionChanged += OnFilter_SelectionChanged;
            SystemComboBox.SelectionChanged += OnFilter_SelectionChanged;
            FilterTextBox.TextChanged += FilterTextBox_TextChanged;
        }

        public class SimVarPreset
        {
            public String Code { get; set; }
            public String Label { get; set; }
            public String Description { get; set; }
        }

        private void VendorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        private void AircraftComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void SystemComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        //Update Hubhop
        private void UpdateHubhop_Click(object sender, RoutedEventArgs e)
        {
            MobiFlight.SimConnectMSFS.WasmModuleUpdater wasmModuleUpdater = new();
            wasmModuleUpdater.AutoDetectCommunityFolder();
            wasmModuleUpdater.InstallWasmModule();
        }

        //Move Window
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        //Close Window
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }






        bool connected = false;
        bool flag = false;
        private async void Connect_Click(object sender, RoutedEventArgs e)
        {

            Test.Hostname = IP.Text;

            System.Net.NetworkInformation.Ping ping = new();
            try
            {
                ping.Send(Test.Hostname);
                LabelConnection.Content = null;
                connected = true;
            }
            catch (Exception ex)
            {
                connected = false;
                LabelConnection.Content = ex.Message;
            }
            finally
            {
                ping?.Dispose();
            }

            HomeModel.SimConnectStart();
            if (connected && !flag && HomeModel.simConnectCache.IsSimConnectConnected())
            {
                Test.InterfaceITEthernetConnection();
                if (Test.canread)
                {
                    Connect.Background = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                    Test.GetinterfaceITEthernetDataStart();
                    Test.GetinterfaceITEthernetInfo();


                    //HomeViewModel.SimConnectClient = new SimConnectClient();
                    //await Task.Run(() => HomeViewModel.SimConnectClient.SimConnect_Open());
                    //MainViewModel.HomeVM.SimConnectMessageThread = new Thread(() => SimConnectClient.ReceiveSimConnectMessage(MainViewModel.HomeVM.SimConnectMessageCancellationTokenSource.Token));
                    //MainViewModel.HomeVM.SimConnectMessageThread.Start();


                    //MSFS_PMDG_737_CDU_E.MSFS_PMDG_737_Captain_Events.ReceivedDataThread = new Thread(() => Test.GetinterfaceITEthernetData(MSFS_PMDG_737_CDU_E.MSFS_PMDG_737_Captain_Events.EthernetKeyNotifyCallback));
                    //MSFS_PMDG_737_CDU_E.MSFS_PMDG_737_Captain_Events.ReceivedDataThread.Start();


                    //MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_MCDU_Data.ReceivedDataThread = new Thread(MSFS_PMDG_737_CDU_E.MSFS_FENIX_A320_Captain_MCDU_Data.ReceiveDataThread);
                    //MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_MCDU_Data.ReceivedDataThread.Start();
                    //MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_Events.ReceivedDataThread = new Thread((o) => test.GetinterfaceITEthernetData(MSFS_PMDG_737_CDU_E.MSFS_FENIX_A320_Captain_Events.EthernetKeyNotifyCallback));
                    //MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_Events.ReceivedDataThread.Start();
                }
                flag = true;
            }
            else if (flag)
            {
                //MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_MCDU_Data.ReceivedDataThread?.Abort();
                //MSFS_FENIX_A320_MCDU_E.MSFS_FENIX_A320_Captain_Events.ReceivedDataThread?.Abort();


                //MSFS_PMDG_737_CDU_E.MSFS_PMDG_737_Captain_Events.ReceivedDataThread?.Abort();
                MainViewModel.HomeVM.SimConnectMessageCancellationTokenSource?.Cancel();
                HomeViewModel.SimConnectClient?.SimConnect_Close();
                HomeViewModel.SimConnectClient = null;


                Test.CloseStream();
                HomeModel.SimConnectStop();
                Connect.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                connected = false;
                flag = false;
                HomeModel.simConnectCache = null;
            }
            else
            {
                HomeModel.simConnectCache = null;
            }
        }

        bool flagg = false;
        private void LED_Click(object sender, RoutedEventArgs e)
        {
            if (!flagg && connected)
            {
                Test.LEDon();
                LED.Background = new SolidColorBrush(Color.FromRgb(0, 200, 0));
                flagg = true;
            }
            else if (flagg && connected)
            {
                Test.LEDoff();
                LED.Background = new SolidColorBrush(Color.FromRgb( 255, 255, 255));
                flagg = false;
            }
        }
    }
}