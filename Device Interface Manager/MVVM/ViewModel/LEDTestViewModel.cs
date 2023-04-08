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

    public required int Session { get; init; }
    public required int LEDFirst { get; init; }
    public required int LEDLast { get; init; }
    public required string BoardType { get; init; }

    public LEDTestViewModel() { }

    [RelayCommand]
    private void LEDEnable()
    {
        _ = interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Enable(Session, (bool)(this.IsEnabled = !this.IsEnabled));
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
        _ = interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Session, (int)posLED, (bool)LEDs.FirstOrDefault(x => x.Position == int.Parse(posLED.ToString())).IsChecked);
    }

    [RelayCommand]
    private void AllLEDOnOff(string direction)
    {
        if (direction is "On")
        {
            foreach (var led in LEDs)
            {
                _ = interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Session, led.Position, (bool)(led.IsChecked = true));
            }
            return;
        }
        foreach (var led in LEDs)
        {
            _ = interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Session, led.Position, (bool)(led.IsChecked = false));
        }
    }
}