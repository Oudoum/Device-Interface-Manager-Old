using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MSFSProfiles.PMDG;
using MahApps.Metro.Controls.Dialogs;
using GongSolutions.Wpf.DragDrop;
using MahApps.Metro.Controls;

namespace Device_Interface_Manager.MVVM.ViewModel;
public partial class ProfileCreatorViewModel : ObservableObject, IDropTarget
{
    private readonly IDialogCoordinator dialogCoordinator;

    public ProfileCreatorViewModel(IDialogCoordinator dialogCoordinator)
    {
        this.dialogCoordinator = dialogCoordinator;
    }

    [ObservableProperty]
    private ProfileCreatorModel _profileCreatorModel = new();

    public List<ProfileCreatorModel> ProfileCreatorModels { get; set; } = new();

    public string PreviousProfileName { get; private set; }

    public string ProfileName
    {
        get => ProfileCreatorModel.ProfileName;
        set
        {
            PreviousProfileName = ProfileCreatorModel.ProfileName;
            ProfileCreatorModel.ProfileName = value;
            OnPropertyChanged(nameof(ProfileName));
            UpdateButtons();
        }
    }

    private void UpdateButtons()
    {
        SaveProfileCommand.NotifyCanExecuteChanged();
        SaveProfileAsCommand.NotifyCanExecuteChanged();
        SortInputOutputCommand.NotifyCanExecuteChanged();
        ClearProfileCommand.NotifyCanExecuteChanged();
    }

    public string Driver
    {
        get => ProfileCreatorModel.SelectedDriver;
        set
        {
            ProfileCreatorModel.SelectedDriver = value;
            switch (value)
            {
                case ProfileCreatorModel.FDSUSB:
                    foreach (var device in Devices)
                    {
                        if (!DeviceNameCollection.Contains(device.BoardType))
                        {
                            DeviceNameCollection.Add(device.BoardType);
                        }
                    }
                    break;
            }
            OnPropertyChanged(nameof(Driver));
            UpdateButtons();
        }
    }

    public static string[] Drivers => ProfileCreatorModel.Drivers;

    public string DeviceName
    {
        get => ProfileCreatorModel.DeviceName;
        set
        {
            if (string.IsNullOrEmpty(ProfileCreatorModel.DeviceName) || result.Value && !ProfileCreatorModels.Any(s => s.DeviceName == value))
            {
                ProfileCreatorModels.Add(ProfileCreatorModel);
                SetDevice(value, ProfileCreatorModel);
            }
            else if (!ProfileCreatorModels.Any(s => s.DeviceName == value))
            {
                ProfileCreatorModel profileCreatorModel = new()
                {
                    ProfileName = ProfileCreatorModel.ProfileName,
                    SelectedDriver = ProfileCreatorModel.SelectedDriver,
                };
                SetDevice(value, profileCreatorModel);
                ProfileCreatorModels.Add(profileCreatorModel);
            }
            else if (result.Value && ProfileCreatorModels.Any(s => s.DeviceName == value))
            {
                if (OverrideDevice() == MessageDialogResult.Affirmative)
                {
                    int index = ProfileCreatorModels.FindIndex(x => x.DeviceName == value);
                    if (index != -1)
                    {
                        ProfileCreatorModels[index] = ProfileCreatorModel;
                    }
                }
            }
            ProfileCreatorModel = ProfileCreatorModels.Where(s => s.DeviceName == value).FirstOrDefault();
            ProfileCreatorModel.DeviceName = value;
            ProfileName = ProfileCreatorModel.ProfileName;
            Driver = ProfileCreatorModel.SelectedDriver;
            OnPropertyChanged(nameof(DeviceName));
        }
    }

    private MessageDialogResult OverrideDevice()
    {
        return dialogCoordinator.ShowModalMessageExternal(this, "Override", $"Are you sure you want to override the profile for {ProfileCreatorModel.DeviceName}?",
        MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
        {
            AnimateHide = false,
            AnimateShow = false,
            AffirmativeButtonText = "Yes",
            NegativeButtonText = "No"
        });
    }

    private void SetDevice(string deviceName, ProfileCreatorModel profileCreatorModel)
    {
        switch (Driver)
        {
            case ProfileCreatorModel.FDSUSB:
                InterfaceIT_BoardInfo.Device device = Devices.Where(s => s.BoardType == deviceName).FirstOrDefault();
                for (int i = device.DeviceInfo.nSwitchFirst; i <= device.DeviceInfo.nSwitchLast; i++)
                {
                    profileCreatorModel.Switches.Add(i);
                }
                for (int i = device.DeviceInfo.nLEDFirst; i <= device.DeviceInfo.nLEDLast; i++)
                {
                    profileCreatorModel.LEDs.Add(i);
                }
                InterfaceITAPI_Data.interfaceIT_LED_Enable(device.Session, true);
                InterfaceITAPI_Data.interfaceIT_Switch_Enable_Poll(device.Session, true);
                break;
        }
        profileCreatorModel.DeviceName = deviceName;
    }

    public ObservableCollection<string> DeviceNameCollection { get; set; } = new();

    public InterfaceIT_BoardInfo.Device[] Devices { get; set; }

    [ObservableProperty]
    private string _errorText;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveProfileCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveProfileAsCommand))]
    [NotifyCanExecuteChangedFor(nameof(SortInputOutputCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearProfileCommand))]
    private string _profileNameButtonContent = "Ok";

    private string OldFilePath => Path.Combine("Profiles", "Creator", PreviousProfileName + ".json");
    private string NewFilePath => Path.Combine("Profiles", "Creator", ProfileName + ".json");

    bool? result = false;

    [RelayCommand]
    private void LoadProfile()
    {
        Microsoft.Win32.OpenFileDialog dialog = new()
        {
            InitialDirectory = Environment.CurrentDirectory + Path.GetDirectoryName(NewFilePath),
            DefaultExt = ".json"
        };
        result = dialog.ShowDialog();

        if (result == true)
        {
            ProfileCreatorModel = JsonSerializer.Deserialize<ProfileCreatorModel>(File.ReadAllText(dialog.FileName));
            PreviousProfileName = ProfileName;
            DeviceName = ProfileCreatorModel.DeviceName;
            ProfileNameButtonContent = "Edit";
            ErrorText = ProfileName + " successfully loaded";
        }
        result = false;
    }

    [RelayCommand]
    private void SaveProfile()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(NewFilePath));
            File.WriteAllText(NewFilePath, JsonSerializer.Serialize(ProfileCreatorModel, new JsonSerializerOptions { WriteIndented = true }));
            ErrorText = ProfileName + " successfully saved";
        }
        catch (Exception e)
        {
            ErrorText = e.Message;
        }
    }

    [RelayCommand]
    private void ProfileNameSave()
    {
        if (ProfileNameButtonContent == "Ok" && !string.IsNullOrEmpty(ProfileName))
        {
            if (!string.IsNullOrEmpty(PreviousProfileName))
            {
                try
                {
                    File.Move(OldFilePath, NewFilePath);
                    string text = File.ReadAllText(NewFilePath);
                    text = text.Replace(PreviousProfileName, ProfileName);
                    File.WriteAllText(NewFilePath, text);
                    ErrorText = PreviousProfileName + " successfully renamed to " + ProfileName;
                }
                catch(Exception e)
                {
                    ErrorText = e.Message;
                }
            }
            ProfileNameButtonContent = "Edit";
        }
        else if (ProfileNameButtonContent == "Edit")
        {
            ProfileNameButtonContent = "Ok";
        }
    }

    [RelayCommand]
    private async Task SaveProfileAs()
    {
        string inputDialogResult = await dialogCoordinator.ShowInputAsync(this, "Profile name", "Please enter your profile name.",
            new MetroDialogSettings
            { 
                AnimateHide = false,
                AnimateShow = false,
                DefaultText = PreviousProfileName 
            });
        if (!string.IsNullOrEmpty(inputDialogResult))
        {
            ProfileName = inputDialogResult;
            SaveProfile();
            ProfileNameButtonContent = "Edit";
        }
    }

    [ObservableProperty]
    private bool? _isSortedAscending;

    [RelayCommand]
    private void SortInputOutput()
    {
        if (ProfileCreatorModel is not null)
        {
            IsSortedAscending ??= false;
            List<InputCreator> sortedInputList = ProfileCreatorModel.InputCreator.ToList();
            sortedInputList.Sort((x, y) => IsSortedAscending.Value ? Comparer<int?>.Default.Compare(y.SelectedSwitch, x.SelectedSwitch) : Comparer<int?>.Default.Compare(x.SelectedSwitch, y.SelectedSwitch));
            for (int i = 0; i < sortedInputList.Count; i++)
            {
                ProfileCreatorModel.InputCreator.Move(ProfileCreatorModel.InputCreator.IndexOf(sortedInputList[i]), i);
            }

            List<OutputCreator> sortedOutputList = ProfileCreatorModel.OutputCreator.ToList();
            sortedOutputList.Sort((x, y) => IsSortedAscending.Value ? Comparer<int?>.Default.Compare(y.SelectedLED, x.SelectedLED) : Comparer<int?>.Default.Compare(x.SelectedLED, y.SelectedLED));
            for (int i = 0; i < sortedOutputList.Count; i++)
            {
                ProfileCreatorModel.OutputCreator.Move(ProfileCreatorModel.OutputCreator.IndexOf(sortedOutputList[i]), i);
            }
            IsSortedAscending = !IsSortedAscending;
            ErrorText = ProfileName + " successfully sorted";
        }
    }

    [RelayCommand]
    private async Task ClearProfile()
    {
        if (ProfileCreatorModel is not null && MessageDialogResult.Affirmative == await dialogCoordinator
            .ShowMessageAsync(this, "Clear", "Are you sure you want to clear all inputs and outputs?",
            MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
            {
                AnimateHide = false,
                AnimateShow = false,
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            }))
        {
            ProfileCreatorModel.UsedSwitches.Clear();
            ProfileCreatorModel.InputCreator.Clear();

            ProfileCreatorModel.UsedLEDs.Clear();
            ProfileCreatorModel.OutputCreator.Clear();
        }
    }

    [RelayCommand]
    private void AddInput()
    {
        ProfileCreatorModel?.InputCreator.Add(new InputCreator());
    }

    [RelayCommand]
    private void AddOutput()
    {
        ProfileCreatorModel?.OutputCreator.Add(new OutputCreator());
    }

    private async Task<bool> ShowConfirmationDialog(string rowType)
    {
        MessageDialogResult result = await dialogCoordinator.ShowMessageAsync(this, "Delete", $"Are you sure you want to delete the selected {rowType}?",
            MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
            {
                AnimateHide = false,
                AnimateShow = false,
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            });
        return result == MessageDialogResult.Affirmative;
    }

    [RelayCommand]
    private async Task DeleteInputOutputCreatorRow(object creator)
    {
        if (creator is InputCreator inputCreator)
        {
            if (!await ShowConfirmationDialog("input row"))
            {
                return;
            }
            DeleteInputCreatorRow(inputCreator);
        }
        else if (creator is OutputCreator outputCreator)
        {
            if (!await ShowConfirmationDialog("output row"))
            {
                return;
            }
            DeleteOutputCreatorRow(outputCreator);
        }
        else if (creator is IList selectedItems)
        {
            bool isShown = false;
            foreach (var item in selectedItems.Cast<object>().ToList())
            {
                if (item is InputCreator input)
                {
                    if (!isShown)
                    {
                        if (!await ShowConfirmationDialog("input rows"))
                        {
                            return;
                        }
                    }
                    isShown = true;
                    DeleteInputCreatorRow(input);
                }
                else if (item is OutputCreator output)
                {
                    if (!isShown)
                    {
                        if (!await ShowConfirmationDialog("output rows"))
                        {
                            return;
                        }
                    }
                    isShown = true;
                    DeleteOutputCreatorRow(output);
                }
            }
        }
    }

    private void DeleteInputCreatorRow(InputCreator input)
    {
        ProfileCreatorModel.UsedSwitches.Remove(input.SelectedSwitch);
        ProfileCreatorModel.InputCreator.Remove(input);
    }

    private void DeleteOutputCreatorRow(OutputCreator output)
    {
        ProfileCreatorModel.UsedLEDs.Remove(output.SelectedLED);
        ProfileCreatorModel.OutputCreator.Remove(output);
    }

    [RelayCommand]
    private void DuplicateInputOutputCreatorRow(object creator)
    {
        if (creator is IList selectedItems)
        {
            foreach (var item in selectedItems.Cast<object>().ToList())
            {
                if (item is InputCreator input)
                {
                    ProfileCreatorModel.InputCreator.Add(input.Clone());
                }
                else if (item is OutputCreator output)
                {
                    ProfileCreatorModel.OutputCreator .Add(output.Clone());
                }
            }
        }
    }

    [RelayCommand]
    private void EditInputOutput(object inputOutputCreator)
    {
        Core.NavigationService navigationService = new();
        if (inputOutputCreator is InputCreator inputCreator)
        {
            ProfileCreatorModel.UsedSwitches.Remove(inputCreator.SelectedSwitch);
            navigationService.NavigateToInputCreator(inputCreator, ProfileCreatorModel.Switches.ToArray() ,Devices.FirstOrDefault(k => k.BoardType == ProfileCreatorModel.DeviceName));
            ProfileCreatorModel.UsedSwitches.Add(inputCreator.SelectedSwitch);
        }
        else if (inputOutputCreator is OutputCreator outputCreator)
        {
            ProfileCreatorModel.UsedLEDs.Remove(outputCreator.SelectedLED);
            navigationService.NavigateToOutputCreator(outputCreator, ProfileCreatorModel.LEDs.ToArray(), Devices.FirstOrDefault(k => k.BoardType == ProfileCreatorModel.DeviceName));
            ProfileCreatorModel.UsedLEDs.Add(outputCreator.SelectedLED);
        }
    }

    [ObservableProperty]
    private bool _isStarted;

    [RelayCommand]
    private async Task StartProfiles()
    {
        IsStarted = !IsStarted;
        if (!IsStarted)
        {
            PMDGProfile.Instance.Stop();
            ErrorText = "Profiles stopped";
        }
        foreach (var device in Devices)
        {
            InterfaceITAPI_Data.interfaceIT_LED_Enable(device.Session, !IsStarted);
            InterfaceITAPI_Data.interfaceIT_Switch_Enable_Poll(device.Session, !IsStarted);
        }
        if (IsStarted)
        {
            await PMDGProfile.Instance.StartAsync(ProfileCreatorModels.ToArray(), Devices.ToArray());
            ErrorText = "Profiles started";
        }
    }

    public void DragOver(IDropInfo dropInfo)
    {
        if ((dropInfo.Data is InputCreator && dropInfo.TargetItem is InputCreator) || (dropInfo.Data is OutputCreator && dropInfo.TargetItem is OutputCreator))
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = System.Windows.DragDropEffects.Move;
        }
    }

    public void Drop(IDropInfo dropInfo)
    {
        if (dropInfo.Data is InputCreator)
        {
            InputCreator item = (InputCreator)dropInfo.Data;
            ProfileCreatorModel.InputCreator.Remove(item);
            if (dropInfo.InsertIndex >= 0 && dropInfo.InsertIndex <= ProfileCreatorModel.InputCreator.Count)
            {
                ProfileCreatorModel.InputCreator.Insert(dropInfo.InsertIndex, item);
                return;
            }
            ProfileCreatorModel.InputCreator.Add(item);
        }
        else if (dropInfo.Data is OutputCreator item)
        {
            ProfileCreatorModel.OutputCreator.Remove(item);
            if (dropInfo.InsertIndex >= 0 && dropInfo.InsertIndex <= ProfileCreatorModel.OutputCreator.Count)
            {
                ProfileCreatorModel.OutputCreator.Insert(dropInfo.InsertIndex, item);
                return;
            }
            ProfileCreatorModel.OutputCreator.Add(item);
        }
    }
}