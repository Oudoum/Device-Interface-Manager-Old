using System.Linq;
using System.Windows;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.MVVM.View;
using Device_Interface_Manager.MVVM.ViewModel;

namespace Device_Interface_Manager.Core;
interface INavigationService
{
    void NavigateTo<TView>(object parameter) where TView : Window, new();

    void NavigateToInputCreator(InputCreator inputCreator, int?[] inputs, object device);

    void NavigateToOutputCreator(OutputCreator outputCreator, int?[] outputs, object device);
}

public class NavigationService : INavigationService
{
    public void NavigateTo<TView>(object parameter) where TView : Window, new()
    {
        TView view = new();
        if (view.DataContext is ProfileCreatorViewModel viewModel)
        {
            viewModel.Devices = parameter as InterfaceIT_BoardInfo.Device[];
        }
        view.Show();
    }

    public void NavigateToInputCreator(InputCreator inputCreator, int?[] inputs, object device)
    {
        InputCreatorViewModel viewModel = new()
        {
            InputCreatorModel = new()
            {
                Switches = inputs,
                SelectedSwitch = inputCreator.SelectedSwitch,
                SelectedEventType = inputCreator.SelectedEventType,
                PMDGEvent = inputCreator.PMDGEvent,
                PMDGMouseEventPress = inputCreator.PMDGMouseEventPress,
                PMDGMouseEventRelease = inputCreator.PMDGMouseEventRelease,
                Event = inputCreator.Event,
                EventDataPress = inputCreator.EventDataPress,
                EventDataRelease = inputCreator.EventDataRelease
            }
        };
        if (device is InterfaceIT_BoardInfo.Device)
        {
            viewModel.Device = device as InterfaceIT_BoardInfo.Device;
        }

        InputCreatorView view = new()
        {
            DataContext = viewModel,
            Owner = Application.Current.Windows.OfType<ProfileCreatorView>().FirstOrDefault(),
        };
        view.ShowDialog();
        if (viewModel.Save == true)
        {
            inputCreator.SelectedSwitch = viewModel.SelectedSwitch;
            inputCreator.SelectedEventType = viewModel.SelectedEventType;
            inputCreator.PMDGEvent = viewModel.PMDGEvent;
            inputCreator.PMDGMouseEventPress = viewModel.PMDGMouseEventPress;
            inputCreator.PMDGMouseEventRelease = viewModel.PMDGMouseEventRelease;
            inputCreator.Event = viewModel.Event;
            inputCreator.EventDataPress = viewModel.EventDataPress;
            inputCreator.EventDataRelease = viewModel.EventDataRelease;
        }
    }

    public void NavigateToOutputCreator(OutputCreator outputCreator, int?[] outputs, object device)
    {
        OutputCreatorViewModel viewModel = new()
        {
            OutputCreatorModel = new()
            {
                LEDs = outputs,
                SelectedLED = outputCreator.SelectedLED,
                SelectedDataType = outputCreator.SelectedDataType,
                PMDGDataFieldName = outputCreator.PMDGDataFieldName,
                PMDGStructArrayNum = outputCreator.PMDGStructArrayNum,
                ComparisonValue = outputCreator.ComparisonValue,
                Data = outputCreator.Data,
                IsInverted = outputCreator.IsInverted
            }
        };
        if (device is InterfaceIT_BoardInfo.Device)
        {
            viewModel.Device = device as InterfaceIT_BoardInfo.Device;
        }

        OutputCreatorView view = new()
        {
            DataContext = viewModel,
            Owner = Application.Current.Windows.OfType<ProfileCreatorView>().FirstOrDefault(),
        };
        view.ShowDialog();
        if (viewModel.Save == true)
        {
            outputCreator.SelectedLED = viewModel.SelectedLED;
            outputCreator.SelectedDataType = viewModel.SelectedDataType;
            outputCreator.PMDGDataFieldName = viewModel.PMDGDataFieldName;
            outputCreator.PMDGStructArrayNum = viewModel.PMDGStructArrayNum;
            outputCreator.ComparisonValue = viewModel.ComparisonValue;
            outputCreator.Data = viewModel.Data;
            outputCreator.IsInverted = viewModel.IsInverted;
        }
    }
}