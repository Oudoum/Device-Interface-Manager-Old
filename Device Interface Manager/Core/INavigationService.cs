using System.Linq;
using System.Windows;
using Device_Interface_Manager.Devices.interfaceIT.USB;
using Device_Interface_Manager.Models;
using Device_Interface_Manager.Views;
using Device_Interface_Manager.ViewModels;

namespace Device_Interface_Manager.Core;
interface INavigationService
{
    void NavigateTo<TView>(object parameter) where TView : Window, new();

    void NavigateToInputCreator(InputCreator inputCreator, OutputCreator[] outputCreators, object device);

    void NavigateToOutputCreator(OutputCreator outputCreator, OutputCreator[] outputCreators, object device);
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

    public void NavigateToInputCreator(InputCreator inputCreator, OutputCreator[] outputCreators, object device)
    {
        InputCreatorViewModel viewModel = new(
            
            new()
            {
                InputType = inputCreator.InputType,
                Input = inputCreator.Input,
                EventType = inputCreator.EventType,
                PMDGEvent = inputCreator.PMDGEvent,
                PMDGMousePress = inputCreator.PMDGMousePress,
                PMDGMouseRelease = inputCreator.PMDGMouseRelease,
                Event = inputCreator.Event,
                OnRelease = inputCreator.OnRelease,
                DataPress = inputCreator.DataPress,
                DataRelease = inputCreator.DataRelease,
                OutputCreator = outputCreators,
                Preconditions = new(inputCreator.Preconditions.Select(precondition => new PreconditionModel(precondition, outputCreators)))
            },
            device);

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
            inputCreator.OnRelease = viewModel.OnRelease;
            inputCreator.DataPress = viewModel.DataPress;
            inputCreator.DataRelease = viewModel.DataRelease;
            inputCreator.Preconditions = viewModel.Preconditions.ToArray();
        }
    }

    public void NavigateToOutputCreator(OutputCreator outputCreator, OutputCreator[] outputCreators, object device)
    {
        OutputCreatorViewModel viewModel = new(
            new()
            {
                OutputType = outputCreator.OutputType,
                Output = outputCreator.Output,
                DataType = outputCreator.DataType,
                PMDGData = outputCreator.PMDGData,
                PMDGDataArrayIndex = outputCreator.PMDGDataArrayIndex,
                Operator = outputCreator.Operator,
                ComparisonValue = outputCreator.ComparisonValue,
                TrueValue = outputCreator.TrueValue,
                FalseValue = outputCreator.FalseValue,
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
                OutputCreator = outputCreators.Where(x => x != outputCreator).ToArray(),
                Preconditions = new(outputCreator.Preconditions.Select(precondition => new PreconditionModel(precondition, outputCreators)))
            }, 
            device);

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
            outputCreator.Operator = viewModel.Operator;
            outputCreator.ComparisonValue = viewModel.ComparisonValue;
            outputCreator.TrueValue = viewModel.TrueValue;
            outputCreator.FalseValue = viewModel.FalseValue;
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
            outputCreator.Preconditions = viewModel.Preconditions.ToArray();
        }
    }
}