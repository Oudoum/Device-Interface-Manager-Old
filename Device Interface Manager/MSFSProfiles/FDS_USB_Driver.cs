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

            foreach (var item in ProfileCreatorModel.OutputCreator.Where(item => item.DataType == ProfileCreatorModel.MSFSSimConnect && !string.IsNullOrEmpty(item.Data) && item.IsActive))
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

        foreach (var item in ProfileCreatorModel.OutputCreator.Where(k => k.DataType == ProfileCreatorModel.MSFSSimConnect && k.Data == simVar.Name && k.IsActive))
        {
            ProfileEntryIteration(item, simVar);
            PrecomparisonIterartion(item);
        }
    }

    private void ProfileEntryIteration(OutputCreator outputCreator, SimConnectClient.SimVar simVar)
    {
        outputCreator.FlightSimValue = simVar.Data;
        outputCreator.OutputValue = null;
        if (outputCreator.Output is not null && CheckPrecomparison(outputCreator.Preconditions))
        {
            switch (outputCreator.OutputType)
            {
                case ProfileCreatorModel.LED or ProfileCreatorModel.DATALINE when outputCreator.ComparisonValue is null:
                    SetSendOuput(outputCreator, simVar.BData());
                    break;

                case ProfileCreatorModel.LED or ProfileCreatorModel.DATALINE when outputCreator.ComparisonValue is not null:
                    SetSendOuput(outputCreator, CheckComparison(outputCreator.ComparisonValue, outputCreator.Operator, simVar.Data));
                    break;

                case ProfileCreatorModel.SEVENSEGMENT when simVar.Data == Math.Truncate(simVar.Data):
                    SetStringValue(simVar.Data.ToString(".0", System.Globalization.CultureInfo.InvariantCulture), simVar.Name, outputCreator);
                    break;

                case ProfileCreatorModel.SEVENSEGMENT:
                    SetStringValue(simVar.Data.ToString(System.Globalization.CultureInfo.InvariantCulture), simVar.Name, outputCreator);
                    break;
            }
        }
    }

    private void PrecomparisonIterartion(OutputCreator outputCreator)
    {
        foreach (var itemContainsPrecondition in ProfileCreatorModel.OutputCreator.Where(k => k.FlightSimValue is not null && k.Preconditions.Length > 0 && k.IsActive && k.Preconditions.Any(l => l.ReferenceId == outputCreator.Id)))
        {
            if (itemContainsPrecondition.DataType == ProfileCreatorModel.MSFSSimConnect)
            {
                ProfileEntryIteration(itemContainsPrecondition, (SimConnectClient.SimVar)new(itemContainsPrecondition.Data, (double)itemContainsPrecondition.FlightSimValue));
            }
            else if (itemContainsPrecondition.DataType == ProfileCreatorModel.PMDG737)
            {
                ProfileEntryIteration(itemContainsPrecondition, (PMDGDataFieldChangedEventArgs)new(Profiles.ConvertDataToPMDGDataFieldName(itemContainsPrecondition), itemContainsPrecondition.FlightSimValue));
            }
        }
    }

    private void PMDGProfile_FieldChanged(object sender, PMDGDataFieldChangedEventArgs e)
    {
        foreach (var item in ProfileCreatorModel.OutputCreator
            .Where(k => k.DataType == ProfileCreatorModel.PMDG737 && k.IsActive && (k.PMDGDataArrayIndex is not null && k.PMDGData + '_' + k.PMDGDataArrayIndex == e.PMDGDataName) || k.PMDGData == e.PMDGDataName))
        {
            ProfileEntryIteration(item, e);
            PrecomparisonIterartion(item);
        }
    }

    private void ProfileEntryIteration(OutputCreator outputCreator, PMDGDataFieldChangedEventArgs e)
    {
        outputCreator.FlightSimValue = e.Value;
        outputCreator.OutputValue = null;
        if (outputCreator.Output is not null && CheckPrecomparison(outputCreator.Preconditions))
        {
            switch (e.Value)
            {
                //bool
                case bool valueBool:
                    SetSendOuput(outputCreator, valueBool);
                    break;

                //string
                case string valueString:
                    SetStringValue(valueString, e.PMDGDataName, outputCreator);
                    break;

                default:
                    //byte, ushort, short, uint, int, float
                    double valuedouble = Convert.ToDouble(e.Value);
                    if (outputCreator.ComparisonValue is not null)
                    {
                        SetSendOuput(outputCreator, CheckComparison(outputCreator.ComparisonValue, outputCreator.Operator, valuedouble));
                    }
                    else if (outputCreator.ComparisonValue is null)
                    {
                        SetStringValue(valuedouble.ToString(), e.PMDGDataName, outputCreator);
                    }
                    break;
            }
        }
    }

    private bool CheckPrecomparison(Precondition[] preconditions)
    {
        if (preconditions.Length == 0)
        {
            return true;
        }
        bool result = false;
        for (int i = 0; i < preconditions.Length; i++)
        {
            var item = preconditions[i];
            OutputCreator matchingOutputCreator = ProfileCreatorModel.OutputCreator.FirstOrDefault(oc => oc.Id == item.ReferenceId);
            if (matchingOutputCreator is null)
            {
                return false;
            }
            bool comparisonResult = true;
            if (item.IsActive)
            {
                comparisonResult = CheckComparison(item.ComparisonValue, item.Operator, matchingOutputCreator.FlightSimValue);
            }
            if (i == 0)
            {
                result = comparisonResult;
                continue;
            }
            if (preconditions[i - 1].IsOrOperator)
            {
                result = result || comparisonResult;
                continue;
            }
            result = result && comparisonResult;
        }
        return result;
    }

    private static bool CheckComparison(string sComparisonValue, char? charOperator, double value)
    {
        if (double.TryParse(sComparisonValue, out double comparisonValue))
        {
            return charOperator switch
            {
                '=' => value == comparisonValue,
                '≠' => value != comparisonValue,
                '<' => value < comparisonValue,
                '>' => value > comparisonValue,
                '≤' => value <= comparisonValue,
                '≥' => value >= comparisonValue,
                _ => false,
            };
        }
        return false;
    }


    //finish string
    private static bool CheckComparison(string sComparisonValue, char? charOperator, string svalue)
    {
        return double.TryParse(svalue, out double value) && CheckComparison(sComparisonValue, charOperator, value);
    }

    private static bool CheckComparison(string sComparisonValue, char? charOperator, object svalue)
    {
        string valueStr = svalue is bool boolValue ? (boolValue ? "1" : "0") : svalue.ToString();
        return double.TryParse(valueStr, out double value) && CheckComparison(sComparisonValue, charOperator, value);
    }

    private void SetSendOuput(OutputCreator item, bool valueBool)
    {
        valueBool = item.IsInverted ? !valueBool : valueBool;
        SetBooleanOuput(item, valueBool);
        item.OutputValue = valueBool;
    }

    private void SetBooleanOuput(OutputCreator item, bool valueBool)
    {
        if (item.OutputType == ProfileCreatorModel.LED)
        {
            interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_LED_Set(Device.Session, item.Output.Value, valueBool);
        }
        else if (item.OutputType == ProfileCreatorModel.DATALINE)
        {
            interfaceIT.USB.InterfaceITAPI_Data.interfaceIT_Dataline_Set(Device.Session, item.Output.Value, valueBool);
        }
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
        ButtonIteration(key, direction);
    }

    private void ButtonIteration(int key, uint direction)
    {
        foreach (var item in ProfileCreatorModel.InputCreator.Where(k => k.Input == key && k.IsActive))
        {
            //Precondition
            if (item.Preconditions.Length > 0 && !CheckPrecomparison(item.Preconditions))
            {
                continue;
            }

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