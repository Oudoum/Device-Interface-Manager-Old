using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Device_Interface_Manager.Core;
using Device_Interface_Manager.Models;
using MahApps.Metro.Controls.Dialogs;
using Device_Interface_Manager.Devices.interfaceIT.USB;

namespace Device_Interface_Manager.ViewModels
{
    public abstract partial class BaseCreatorViewModel : ObservableObject, ICloseWindowsCheck
    {
        public BaseCreatorViewModel(object device)
        {
            Device = device;
            if (device is InterfaceIT_BoardInfo.Device iitdevice)
            {
                InterfaceITAPI_Data.EnableDeviceFeatures(iitdevice);
            }
        }

        public Action Close { get; set; }

        public object Device { get; set; }

        public abstract ObservableCollection<OutputCreator> OutputCreator { get; set; }

        private OutputCreator _selectedOutputCreator;

        public OutputCreator SelectedOutputCreator
        {
            get => _selectedOutputCreator;
            set
            {
                if (_selectedOutputCreator != value)
                {
                    _selectedOutputCreator = value;
                    if (value is not null && SelectedPrecondition is not null)
                    {
                        SelectedPrecondition.HasError = false;
                        SelectedPrecondition.ReferenceId = SelectedOutputCreator.Id;
                        SelectedPrecondition.Description = SelectedOutputCreator.Description;
                    }
                    OnPropertyChanged(nameof(SelectedOutputCreator));
                }
            }
        }

        public abstract ObservableCollection<PreconditionModel> Preconditions { get; set; }

        private PreconditionModel _selectedPrecondition;

        public PreconditionModel SelectedPrecondition
        {
            get => _selectedPrecondition;
            set
            {
                if (_selectedPrecondition != value)
                {
                    _selectedPrecondition = value;
                    OnPropertyChanged(nameof(SelectedPrecondition));
                    if (SelectedPrecondition is not null)
                    {
                        SelectedOutputCreator = OutputCreator.FirstOrDefault(oc => oc.Id == _selectedPrecondition.ReferenceId);
                    }
                }
            }
        }

        [ObservableProperty]
        private int _selectedPreconditionIndex;

        public static char[] Operators => OutputCreatorModel.Operators;

        [RelayCommand]
        private void AddPrecondition()
        {
            if (OutputCreator.Count > 0)
            {
                Preconditions ??= new();
                SelectedOutputCreator ??= OutputCreator[0];
                SelectedPrecondition = new(SelectedOutputCreator);
                Preconditions.Add(SelectedPrecondition);
                SelectedPreconditionIndex = Preconditions.Count - 1;
            }
        }

        [RelayCommand]
        private void RemovePrecondition()
        {
            Preconditions.Remove(SelectedPrecondition);
            SelectedPrecondition = null;
            SelectedPreconditionIndex = Preconditions.Count - 1;
        }

        [RelayCommand]
        private void ClearPreconditions()
        {
            Preconditions.Clear();
            SelectedPrecondition = null;
            SelectedPreconditionIndex = Preconditions.Count - 1;
        }

        [RelayCommand]
        private void ChangeLogicalOperator(string logicalOperator)
        {
            switch(logicalOperator)
            {
                case "AND":
                    SelectedPrecondition.IsOrOperator = false;
                    break;

                case "OR":
                    SelectedPrecondition.IsOrOperator = true;
                    break;
            }
        }

        public bool Save { get; set; }

        [RelayCommand]
        private void Ok()
        {
            Save = true;
            Close?.Invoke();
        }

        public virtual MessageDialogResult CanClose()
        {
            if (Device is not null)
            {
                if (Device is InterfaceIT_BoardInfo.Device iitdevice)
                {
                    InterfaceITAPI_Data.InterfaceITDisable(iitdevice);
                }
            }
            return MessageDialogResult.Affirmative;
        }

        [RelayCommand]
        private static void ComboboxMouseEnterLeave(RoutedEventArgs e)
        {
            if (e.RoutedEvent == UIElement.GotFocusEvent)
            {
                ((ComboBox)e.Source).IsDropDownOpen = true;
            }
            else if (e.RoutedEvent == UIElement.LostFocusEvent)
            {
                ((ComboBox)e.Source).IsDropDownOpen = false;
            }
        }
    }
}