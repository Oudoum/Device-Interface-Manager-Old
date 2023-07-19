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

    protected override void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
    {
        Profiles.Instance.FieldChanged += PMDGProfile_FieldChanged;

        Profiles.Instance.WatchedFields.ForEach(x => PMDGProfile_FieldChanged(null, new PMDGDataFieldChangedEventArgs(x, Profiles.Instance.DynDict[x])));

        if (Device.DeviceInfo.nDatalineCount > 0)
        {
            interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_Dataline_Set(Device.Session, Device.DeviceInfo.nDatalineFirst, true);
        }

        foreach (var item in ProfileCreatorModel.OutputCreator.Where(item => item.DataType == ProfileCreatorModel.MSFSSimConnect && !string.IsNullOrEmpty(item.Data)))
        {
            simConnectClient.RegisterSimVar(item.Data, item.Unit);
        }
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
            if (item.Output is not null)
            {
                if (simVar.Unit == "MHz" && item.OutputType == ProfileCreatorModel.SEVENSEGMENT)
                {
                    SetStringValue(simVar.Data.ToString(".000", System.Globalization.CultureInfo.InvariantCulture), null, item);
                }
                else if (simVar.Unit == "kHz" && item.OutputType == ProfileCreatorModel.SEVENSEGMENT)
                {
                    SetStringValue(simVar.Data.ToString(".0", System.Globalization.CultureInfo.InvariantCulture), null, item);
                }
                else if (string.IsNullOrEmpty(simVar.Unit))
                {
                    if (item.OutputType == ProfileCreatorModel.SEVENSEGMENT)
                    {
                        SetStringValue(simVar.Data.ToString(System.Globalization.CultureInfo.InvariantCulture), null, item);
                    }
                    else if (item.OutputType == ProfileCreatorModel.LED)
                    {
                        bool value = item.IsInverted ? !simVar.BData() : simVar.BData();
                        interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.Output.Value, value);
                    }
                }
            }
        }
    }


    private void PMDGProfile_FieldChanged(object sender, PMDGDataFieldChangedEventArgs e)
    {
        foreach (var item in ProfileCreatorModel.OutputCreator.Where(k => k.DataType == ProfileCreatorModel.PMDG737))
        {
            if ((item.PMDGDataArrayIndex is not null && item.PMDGData + '_' + item.PMDGDataArrayIndex == e.PMDGDataName) || item.PMDGData == e.PMDGDataName)
            {
                if (item.Output is null)
                {
                    return;
                }
                switch (e.Value)
                {
                    //bool
                    case bool valueBool:
                        interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.Output.Value, item.IsInverted ? !valueBool : valueBool);
                        break;

                    //byte
                    case byte valueByte when item.ComparisonValue is not null && item.ComparisonValue <= byte.MaxValue && item.ComparisonValue >= byte.MinValue:
                        interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.Output.Value, valueByte == Convert.ToByte(item.ComparisonValue));
                        break;

                    case byte valueByte when item.ComparisonValue is null && item.OutputType == ProfileCreatorModel.SEVENSEGMENT:
                        SetStringValue(valueByte.ToString(), e.PMDGDataName, item);
                        break;

                    //uint
                    case uint valueUint when item.ComparisonValue is not null && item.ComparisonValue <= uint.MaxValue && item.ComparisonValue >= uint.MinValue:
                        interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.Output.Value, valueUint == Convert.ToUInt32(item.ComparisonValue));
                        break;

                    case uint valueUint when item.ComparisonValue is null && item.OutputType == ProfileCreatorModel.SEVENSEGMENT:
                        SetStringValue(valueUint.ToString(), e.PMDGDataName, item);
                        break;

                    //int
                    case int valueInt when item.ComparisonValue is not null && item.ComparisonValue <= int.MaxValue && item.ComparisonValue >= int.MinValue:
                        interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.Output.Value, valueInt == item.ComparisonValue);
                        break;

                    case int valueInt when item.ComparisonValue is null && item.OutputType == ProfileCreatorModel.SEVENSEGMENT:
                        SetStringValue(valueInt.ToString(), e.PMDGDataName, item);
                        break;

                    //float
                    case float valueFLoat:
                        SetStringValue(valueFLoat.ToString(System.Globalization.CultureInfo.InvariantCulture), e.PMDGDataName, item);
                        break;

                    //ushort
                    case ushort valueUshort:
                        SetStringValue(valueUshort.ToString(), e.PMDGDataName, item);
                        break;

                    //short
                    case short valueShort:
                        SetStringValue(valueShort.ToString(), e.PMDGDataName, item);
                        break;

                    //string
                    case string valueString:
                        SetStringValue(valueString.ToString(), e.PMDGDataName, item);
                        break;
                }
            }
        }
    }

    private void SetStringValue(string value, string name, OutputCreator item)
    {
        if (Device.DeviceInfo.n7SegmentCount > 0)
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
            interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_7Segment_Display(Device.Session, sb.ToString(), item.Output.Value);
        }
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
            direction = direction switch
            {
                0 when item.DataRelease is not null => item.DataRelease.Value,

                1 when item.DataPress is not null => item.DataPress.Value,

                0 when item.PMDGMouseRelease is not null => item.PMDGMouseRelease.Value.Value,

                1 when item.PMDGMousePress is not null => item.PMDGMousePress.Value.Value,

                _ => int.MaxValue,
            };

            if (direction == int.MaxValue)
            {
                return;
            }

            //MSFS/SimConnect/LVar
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