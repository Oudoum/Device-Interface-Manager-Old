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

    void NavigateToInputCreator(InputCreator inputCreator, int?[] switches, object device);

    void NavigateToOutputCreator(OutputCreator outputCreator, int?[] lEDs, int?[] sevenSegments, object device);
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

    public void NavigateToInputCreator(InputCreator inputCreator, int?[] switches, object device)
    {
        InputCreatorViewModel viewModel = new()
        {
            InputCreatorModel = new()
            {
                Switches = switches,
                InputType = inputCreator.InputType,
                Input = inputCreator.Input,
                EventType = inputCreator.EventType,
                PMDGEvent = inputCreator.PMDGEvent,
                PMDGMousePress = inputCreator.PMDGMousePress,
                PMDGMouseRelease = inputCreator.PMDGMouseRelease,
                Event = inputCreator.Event,
                DataPress = inputCreator.DataPress,
                DataRelease = inputCreator.DataRelease
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
            inputCreator.InputType = viewModel.InputType;
            inputCreator.Input = viewModel.Input;
            inputCreator.EventType = viewModel.EventType;
            inputCreator.PMDGEvent = viewModel.PMDGEvent;
            inputCreator.PMDGMousePress = viewModel.PMDGMousePress;
            inputCreator.PMDGMouseRelease = viewModel.PMDGMouseRelease;
            inputCreator.Event = viewModel.Event;
            inputCreator.DataPress = viewModel.DataPress;
            inputCreator.DataRelease = viewModel.DataRelease;
        }
    }

    public void NavigateToOutputCreator(OutputCreator outputCreator, int?[] lEDs, int?[] sevenSegments, object device)
    {
        OutputCreatorViewModel viewModel = new()
        {
            OutputCreatorModel = new()
            {
                LEDs = lEDs,
                SevenSegments = sevenSegments,
                OutputType = outputCreator.OutputType,
                Output = outputCreator.Output,
                DataType = outputCreator.DataType,
                PMDGData = outputCreator.PMDGData,
                PMDGDataArrayIndex = outputCreator.PMDGDataArrayIndex,
                ComparisonValue = outputCreator.ComparisonValue,
                Data = outputCreator.Data,
                Unit = outputCreator.Unit,
                IsInverted = outputCreator.IsInverted,
                IsPadded = outputCreator.IsPadded,
                PaddingCharacter = outputCreator.PaddingCharacter,
                DigitCount = outputCreator.DigitCount,
                DigitCheckedSum = outputCreator.DigitCheckedSum,
                DecimalPointCheckedSum = outputCreator.DecimalPointCheckedSum,
                SubstringStart = outputCreator.SubstringStart,
                SubstringEnd = outputCreator.SubstringEnd,
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
            outputCreator.OutputType = viewModel.OutputType;
            outputCreator.Output = viewModel.Output;
            outputCreator.DataType = viewModel.DataType;
            outputCreator.PMDGData = viewModel.PMDGData;
            outputCreator.PMDGDataArrayIndex = viewModel.PMDGDataArrayIndex;
            outputCreator.ComparisonValue = viewModel.ComparisonValue;
            outputCreator.Data = viewModel.Data;
            outputCreator.Unit = viewModel.Unit;
            outputCreator.IsInverted = viewModel.IsInverted;
            outputCreator.IsPadded = viewModel.IsPadded;
            outputCreator.PaddingCharacter = viewModel.PaddingCharacter;
            outputCreator.DigitCount = viewModel.DigitCount;
            outputCreator.DigitCheckedSum = viewModel.DigitCheckedSum;
            outputCreator.DecimalPointCheckedSum = viewModel.DecimalPointCheckedSum;
            outputCreator.SubstringStart = viewModel.SubstringStart;
            outputCreator.SubstringEnd = viewModel.SubstringEnd;
        }
    }
}