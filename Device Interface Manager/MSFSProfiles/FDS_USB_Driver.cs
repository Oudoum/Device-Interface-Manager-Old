using System;
using System.Text;
using System.Linq;
using System.Data;
using Microsoft.FlightSimulator.SimConnect;
using Device_Interface_Manager.MVVM.Model;

namespace Device_Interface_Manager.MSFSProfiles;
public class FDS_USB_Driver : USB
{
    public required ProfileCreatorModel ProfileCreatorModel { get; init; }

    private bool isInit;

    public void Start()
    {
        if (simConnectClient.IsConnected && Device is not null && !isInit)
        {
            Profiles.Instance.FieldChanged += PMDGProfile_FieldChanged;

            Profiles.Instance.WatchedFields.ForEach(x => PMDGProfile_FieldChanged(null, new PMDGDataFieldChangedEventArgs(x, Profiles.Instance.DynDict[x])));

            if (Device.DeviceInfo.DatalineCount > 0)
            {
                interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_Dataline_Set(Device.Session, Device.DeviceInfo.DatalineFirst, true);
            }

            foreach (var item in ProfileCreatorModel.OutputCreator.Where(item => item.DataType == ProfileCreatorModel.MSFSSimConnect && !string.IsNullOrEmpty(item.Data)))
            {
                simConnectClient.RegisterSimVar(item.Data, item.Unit);
            }
            isInit = true;
        }
    }

    protected override void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
    {
        Start();
    }

    public override void Stop()
    {
        base.Stop();
        Profiles.Instance.FieldChanged -= PMDGProfile_FieldChanged;
    }

    protected override void SimConnectClient_OnSimVarChanged(object sender, SimConnectClient.SimVar simVar)
    {
        base.SimConnectClient_OnSimVarChanged(sender, simVar);

        foreach (var item in ProfileCreatorModel.OutputCreator.Where(k => k.Data == simVar.Name))
        {
            item.FlightSimValue = simVar.Data;
            if (item.Output is not null)
            {
                switch(item.OutputType)
                {
                    case ProfileCreatorModel.LED when item.ComparisonValue is null:
                        SetSendOuput(item, simVar.BData());
                        break;

                    case ProfileCreatorModel.LED when item.ComparisonValue is not null:
                        SetSendOuput(item, CheckComparison(item, simVar.Data));
                        break;

                    case ProfileCreatorModel.SEVENSEGMENT when simVar.Data == Math.Truncate(simVar.Data):
                        SetStringValue(simVar.Data.ToString(".0", System.Globalization.CultureInfo.InvariantCulture), simVar.Name, item);
                        break;

                    case ProfileCreatorModel.SEVENSEGMENT:
                        SetStringValue(simVar.Data.ToString(System.Globalization.CultureInfo.InvariantCulture), simVar.Name, item);
                        break;
                }
            }
        }
    }

    private void PMDGProfile_FieldChanged(object sender, PMDGDataFieldChangedEventArgs e)
    {
        foreach (var item in ProfileCreatorModel.OutputCreator
            .Where(k => k.DataType == ProfileCreatorModel.PMDG737 && (k.PMDGDataArrayIndex is not null && k.PMDGData + '_' + k.PMDGDataArrayIndex == e.PMDGDataName) || k.PMDGData == e.PMDGDataName))
        {
            item.FlightSimValue = e.Value;
            if (item.Output is not null)
            {
                switch (e.Value)
                {
                    //bool
                    case bool valueBool:
                        SetSendOuput(item, valueBool);
                        break;

                    //string
                    case string valueString:
                        SetStringValue(valueString, e.PMDGDataName, item);
                        break;

                    default:
                        //byte, ushort, short, uint, int, float
                        double valuedouble = Convert.ToDouble(e.Value);
                        if (item.ComparisonValue is not null)
                        {
                            SetSendOuput(item, CheckComparison(item, valuedouble));
                        }
                        else if (item.ComparisonValue is null)
                        {
                            SetStringValue(valuedouble.ToString(), e.PMDGDataName, item);
                        }
                        break;
                }
            }
        }
    }

    private static bool CheckComparison(OutputCreator item, double value)
    {
        return item.Operator switch
        {
            "=" => value == item.ComparisonValue,
            "≠" => value != item.ComparisonValue,
            "<" => value < item.ComparisonValue,
            ">" => value > item.ComparisonValue,
            "≤" => value <= item.ComparisonValue,
            "≥" => value >= item.ComparisonValue,
            _ => false,
        };
    }

    private void SetSendOuput(OutputCreator item, bool valueBool)
    {
        valueBool = item.IsInverted ? !valueBool : valueBool;
        SetBooleanOuput(item, valueBool);
        item.OutputValue = valueBool;
    }

    private void SetBooleanOuput(OutputCreator item, bool valueBool)
    {
        interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.Output.Value, valueBool);
    }

    private void SetStringValue(string value, string name, OutputCreator item)
    {
        if (Device.DeviceInfo.SevenSegmentCount > 0)
        {
            StringBuilder sb = new(value);
            if (item.DigitCount is not null)
            {
                sb.Replace(".", "");
                if (sb.Length > item.DigitCount)
                {
                    byte digitCount = item.DigitCount.Value;
                    if (sb[digitCount] - '0' > 5)
                    {
                        sb.Length = digitCount;
                        int carry = 1;
                        for (int i = digitCount - 1; i >= 0; i--)
                        {
                            int digit = sb[i] - '0' + carry;
                            carry = digit / 10;
                            sb[i] = (char)(digit % 10 + '0');
                        }
                        if (carry > 0)
                        {
                            sb.Insert(0, carry);
                        }
                    }
                    else if (sb[digitCount] - '0' <= 5)
                    {
                        sb.Length = digitCount;
                    }
                }
            }
            if (item.IsPadded == true)
            {
                if (item.PaddingCharacter is not null && item.DigitCount is not null)
                {
                    int dotCount = 0;
                    for (int i = 0; i < sb.Length; i++)
                    {
                        if (sb[i] == '.')
                        {
                            dotCount++;
                        }
                    }
                    while (sb.Length < item.DigitCount + dotCount)
                    {
                        sb.Insert(0, item.PaddingCharacter);
                    }
                }
                else if (item.PaddingCharacter is null && item.DigitCount is null)
                {
                    PMDG.B737.PMDG737.SetPMDG737MCP(ref sb, name);
                    PMDG.B737.PMDG737.SetPMDG737IRSDisplay(ref sb, name);
                }
            }
            if (item.DigitCount is not null)
            {
                FormatString(ref sb, item);
            }
            if (item.SubstringStart is null && item.SubstringEnd is not null)
            {
                sb.Length = item.SubstringEnd is not null ? (int)(item.SubstringEnd + 1) : sb.Length;
            }
            else if (item.SubstringStart is not null && item.SubstringStart <= sb.Length - 1)
            {
                sb.Remove(0, (int)item.SubstringStart);
                if (item.SubstringEnd is not null)
                {
                    sb.Length = (int)(item.SubstringEnd - item.SubstringStart + 1);
                }
            }
            string outputValue = sb.ToString();
            SetDisplayOutput(item, outputValue);
            item.OutputValue = outputValue;
        }
    }

    private void SetDisplayOutput(OutputCreator item, string outputValue)
    {
        interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_7Segment_Display(Device.Session, outputValue, item.Output.Value);
    }

    public static void FormatString(ref StringBuilder sb, OutputCreator item)
    {
        if (item.DigitCheckedSum is null && item.DecimalPointCheckedSum is null)
        {
            return;
        }
        if (item.DigitCheckedSum is not null)
        {
            for (int i = 0; i < item.DigitCount; i++)
            {
                if ((item.DigitCheckedSum & (1 << i)) == 0)
                {
                    sb[i] = ' ';
                    continue;
                }
                if (sb.Length <= i)
                {
                    sb.Append(item.PaddingCharacter);
                }
            }
        }
        if (item.DecimalPointCheckedSum is not null)
        {
            byte decimalPointCount = 0;
            for (int i = 0; i < item.DigitCount + decimalPointCount; i++)
            {
                if ((item.DecimalPointCheckedSum & (1 << i - decimalPointCount)) != 0 && sb.Length > i)
                {
                    if (sb.Length > i + 1 && sb[i + 1] == '.')
                    {
                        i++;
                        decimalPointCount++;
                        continue;
                    }
                    sb.Insert(i + 1, '.');
                    i++;
                    decimalPointCount++;
                }
            }
        }
    }

    protected override void KeyPressedProc(uint session, int key, uint direction)
    {
        foreach (var item in ProfileCreatorModel.InputCreator.Where(k => k.Input == key))
        {
            //RPN
            if (item.EventType == ProfileCreatorModel.RPN && direction == 1)
            {
                simConnectClient.SendWASMEvent(item.Event);
                continue;
            }

            //Direction
            switch (direction)
            {
                case 0 when item.DataRelease is not null:
                    direction = item.DataRelease.Value;
                    break;

                case 1 when item.DataPress is not null:
                    direction = item.DataPress.Value;
                    break;

                case 0 when item.PMDGMouseRelease is not null:
                    direction = item.PMDGMouseRelease.Value.Value;
                    break;

                case 1 when item.PMDGMousePress is not null:
                    direction = item.PMDGMousePress.Value.Value;
                    break;

                default:
                    continue;
            }

            if (item.EventType == ProfileCreatorModel.MSFSSimConnect && !string.IsNullOrEmpty(item.Event))
            {
                simConnectClient.SetSimVar(direction, item.Event);
            }

            //PMDG 737
            else if (item.EventType == ProfileCreatorModel.PMDG737 && item.PMDGEvent is not null)
            {
                simConnectClient.TransmitEvent(direction, item.PMDGEvent);
            }
        }
    }
}