using System.Linq;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Device_Interface_Manager.MVVM.ViewModel;

public partial class LEDTestViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isEnabled;

    public ObservableCollection<Model.LEDTestModel.DeviceLED> LEDs { get; set; } = new();

    public required uint Session { get; init; }
    public required int LEDFirst { get; init; }
    public required int LEDLast { get; init; }
    public required string BoardType { get; init; }

    [RelayCommand]
    private void LEDEnable()
    {
        interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Enable(Session, IsEnabled = !IsEnabled);
        LEDs.Clear();
        for (int i = LEDFirst; i <= LEDLast; i++)
        {
            LEDs.Add(new Model.LEDTestModel.DeviceLED
            {
                Id = i - LEDFirst + 1,
                Name = "Board: " + BoardType + " [LED]",
                Position = i,
            });
        }
    }

    [RelayCommand]
    private void IsChecked(object posLED)
    {
        if (posLED is int position)
        {
            interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Session, position, LEDs.FirstOrDefault(x => x.Position == position).IsChecked);
        }
    }

    [RelayCommand]
    private void AllLEDOnOff(string direction)
    {
        foreach (var led in LEDs)
        {
            interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Session, led.Position, led.IsChecked = direction == "On");
        }
    }
}