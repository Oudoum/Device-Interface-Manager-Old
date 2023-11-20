using System;
using System.Text;
using System.Linq;
using System.Data;
using Device_Interface_Manager.Models;
using System.IO;

namespace Device_Interface_Manager.SimConnectProfiles;
public abstract class ProfileCreatorBase<T> : ProfileBase<T>
{
    public required ProfileCreatorModel ProfileCreatorModel { get; init; }

    private bool isInit;

    public void Start()
    {
        if (Device is not null && !isInit)
        {
            Profiles.Instance.FieldChanged += PMDGProfile_FieldChanged;

            Profiles.Instance.WatchedFields.ForEach(x => PMDGProfile_FieldChanged(null, new PMDGDataFieldChangedEventArgs(x, Profiles.Instance.DynDict[x])));

            foreach (var item in ProfileCreatorModel.OutputCreator.Where(item => item.DataType == ProfileCreatorModel.MSFSSimConnect && !string.IsNullOrEmpty(item.Data) && item.IsActive))
            {
                simConnectClient.RegisterSimVar(item.Data, item.Unit);
            }

            foreach (var item in ProfileCreatorModel.InputCreator.Where(item => item.EventType == ProfileCreatorModel.KEVENT && !string.IsNullOrEmpty(item.Event) && item.IsActive))
            {
                simConnectClient.RegisterSimEvent(item.Event);
            }
            isInit = true;
        }
    }

    protected override void OnRecvOpen()
    {
        Start();
    }

    protected override void Stop()
    {
        Profiles.Instance.FieldChanged -= PMDGProfile_FieldChanged;
        StopDevice();
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

    protected abstract void StopDevice();

    protected abstract void SetBooleanOuput(OutputCreator item, bool valueBool);

    protected abstract void SetDisplayOutput(OutputCreator item, string outputValue);

    private void ProfileEntryIteration(OutputCreator outputCreator, SimConnectClient.SimVar simVar)
    {
        outputCreator.FlightSimValue = simVar.Data;
        outputCreator.OutputValue = null;
        if (CheckPrecomparison(outputCreator.Preconditions))
        {
            switch (outputCreator.OutputType)
            {
                case ProfileCreatorModel.LED or ProfileCreatorModel.DATALINE when outputCreator.ComparisonValue is null:
                    SetSendOuput(outputCreator, simVar.BData());
                    break;

                case ProfileCreatorModel.LED or ProfileCreatorModel.DATALINE:
                    SetSendOuput(outputCreator, CheckComparison(outputCreator.ComparisonValue, outputCreator.Operator, simVar.Data));
                    break;

                case ProfileCreatorModel.SEVENSEGMENT when simVar.Data == Math.Truncate(simVar.Data) && outputCreator.ComparisonValue is null:
                    SetStringValue(simVar.Data.ToString(".0", System.Globalization.CultureInfo.InvariantCulture), simVar.Name, outputCreator);
                    break;

                case ProfileCreatorModel.SEVENSEGMENT when outputCreator.ComparisonValue is null:
                    SetStringValue(simVar.Data.ToString(System.Globalization.CultureInfo.InvariantCulture), simVar.Name, outputCreator);
                    break;

                case ProfileCreatorModel.SEVENSEGMENT:
                    SetSendOuput(outputCreator, CheckComparison(outputCreator.ComparisonValue, outputCreator.Operator, simVar.Data));
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
        if (CheckPrecomparison(outputCreator.Preconditions))
        {
            switch (e.Value)
            {
                //bool
                case bool valueBool:
                    if (outputCreator.ComparisonValue is not null)
                    {
                        SetSendOuput(outputCreator, CheckComparison(outputCreator.ComparisonValue, outputCreator.Operator, valueBool));
                        break;
                    }
                    SetSendOuput(outputCreator, valueBool);
                    break;

                //string
                case string valueString:
                    if (outputCreator.ComparisonValue is not null)
                    {
                        SetSendOuput(outputCreator, CheckComparison(outputCreator.ComparisonValue, outputCreator.Operator, valueString));
                        break;
                    }
                    SetStringValue(valueString, e.PMDGDataName, outputCreator);
                    break;

                //byte, ushort, short, uint, int, float
                default:
                    double valuedouble = Convert.ToDouble(e.Value);
                    if (outputCreator.ComparisonValue is not null)
                    {
                        SetSendOuput(outputCreator, CheckComparison(outputCreator.ComparisonValue, outputCreator.Operator, valuedouble));
                        break;
                    }
                    SetStringValue(valuedouble.ToString(), e.PMDGDataName, outputCreator);
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

    private static bool CheckComparison(string comparisonValue, char? charOperator, object value)
    {
        string sValue = value switch
        {
            null => "0",
            bool boolValue => boolValue ? "1" : "0",
            _ => value.ToString()
        };
        string sComparisonValue = comparisonValue switch
        {
            string strValue when strValue.ToLower() == "true" => "1",
            string strValue when strValue.ToLower() == "false" => "0",
            _ => comparisonValue.ToString()
        };
        return CheckComparison(sComparisonValue, charOperator, sValue);
    }


    private static bool CheckComparison(string sComparisonValue, char? charOperator, string sValue)
    {
        if (double.TryParse(sComparisonValue, out double comparisonValue) && double.TryParse(sValue, out double value))
        {
            return NumericComparison(comparisonValue, charOperator, value);
        }

        return StringComparison(sComparisonValue, charOperator, sValue);
    }

    private static bool NumericComparison(double comparisonValue, char? charOperator, double value)
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

    private static bool StringComparison(string sComparisonValue, char? charOperator, string sValue)
    {
        return charOperator switch
        {
            '=' => sValue == sComparisonValue,
            '≠' => sValue != sComparisonValue,
            _ => false,
        };
    }

    private void SetSendOuput(OutputCreator item, bool valueBool)
    {
        valueBool = item.IsInverted ? !valueBool : valueBool;
        if (item.Output is not null && !string.IsNullOrEmpty(item.Output.Value.Key))
        {
            SetBooleanOuput(item, valueBool);
        }
        item.OutputValue = valueBool;
    }

    private void SetStringValue(string value, string name, OutputCreator item)
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
                PMDG.PMDG.SetPMDG737MCP(ref sb, name);
                PMDG.PMDG.SetPMDG737IRSDisplay(ref sb, name);
            }
        }
        if (item.DigitCount is not null)
        {
            sb.Append('0', item.DigitCount.Value - sb.Length);
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
        if (item.Output is not null && !string.IsNullOrEmpty(item.Output.Value.Key))
        {
            SetDisplayOutput(item, outputValue);
        }
        item.OutputValue = outputValue;
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

    protected void ButtonIteration(string key, uint direction)
    {
        foreach (var item in ProfileCreatorModel.InputCreator.Where(k => k.Input.Value.Key == key && k.IsActive))
        {
            //Precondition
            if (item.Preconditions.Length > 0 && !CheckPrecomparison(item.Preconditions))
            {
                continue;
            }

            //HTML Event(H:Event), Reverse Polish Notation (RPN)
            if (item.EventType == ProfileCreatorModel.RPN && !string.IsNullOrEmpty(item.Event) && Convert.ToBoolean(direction) == !item.OnRelease)
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

            //Simulation Variable (SimVar[A]), Local Variable (L:Var[L]), Key Event ID (K:Event[K])
            if (item.EventType == ProfileCreatorModel.MSFSSimConnect && !string.IsNullOrEmpty(item.Event))
            {
                simConnectClient.SetSimVar(direction, item.Event);
                continue;
            }

            if (item.EventType == ProfileCreatorModel.KEVENT &&  !string.IsNullOrEmpty(item.Event))
            {
                simConnectClient.TransmitSimEvent(direction, item.Event);
                continue;
            }

            //PMDG 737
            if (item.EventType == ProfileCreatorModel.PMDG737 && item.PMDGEvent is not null)
            {
                simConnectClient.TransmitEvent(direction, item.PMDGEvent);
            }
        }
    }
}