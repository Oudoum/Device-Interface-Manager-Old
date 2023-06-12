using System;
using System.Linq;
using System.Dynamic;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.interfaceIT.USB;

namespace Device_Interface_Manager.MSFSProfiles.PMDG;
public class PMDGProfile
{
    public event EventHandler<PMDGDataFieldChangedEventArgs> FieldChanged;

    private PMDG_NG3_SDK.PMDG_NG3_Data data = new();

    private readonly List<string> watchedFields = new();

    private readonly dynamic dynObject = new ExpandoObject();

    private readonly IDictionary<string, object> dynDict;

    public PMDGProfile(TestCreator[] testCreators, InterfaceIT_BoardInfo.Device[] devices)
    {
        dynDict = (IDictionary<string, object>)dynObject;

        FieldChanged += Instance_FieldChanged;

        CreateList(testCreators);

        Initialize();

        Iteration(new()
        {
            ADF_StandbyFrequency = 20,
            IRS_annunALIGN = new bool[2] { true, false }
        });
    }

    private void CreateList(TestCreator[] testCreators)
    {
        foreach (var item in testCreators)
        {
            foreach (var output in item.OutputCreator)
            {
                if (output.SelectedDataType == TestCreator.PMDG737 && output.PMDGDataFieldName is not null)
                {
                    string pMDGDataFieldName = output.PMDGDataFieldName;
                    if (output.PMDGStructArrayNum is not null)
                    {
                        pMDGDataFieldName = pMDGDataFieldName + '_' + output.PMDGStructArrayNum;
                    }
                    if (!watchedFields.Contains(pMDGDataFieldName))
                    { 
                        watchedFields.Add(pMDGDataFieldName);
                    }
                }
            }
        }
    }

    private void Initialize()
    {
        foreach (var field in typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields())
        {
            if (field.Name == "reserved")
            {
                continue;
            }

            if (field.FieldType.IsArray)
            {
                Array array = (Array)field.GetValue(data);
                if (array is null)
                {
                    if (field.GetCustomAttributes(typeof(MarshalAsAttribute), false).FirstOrDefault() is MarshalAsAttribute marshalAsAttribute)
                    {
                        array = Array.CreateInstance(field.FieldType.GetElementType(), marshalAsAttribute.SizeConst);
                        field.SetValue(data, array);
                    }
                }

                if (array is not null)
                {
                    int i = 0;
                    foreach (var item in array)
                    {
                        dynDict[field.Name + '_' + i] = item;
                        i++;
                    }
                }
            }

            dynDict[field.Name] = field.GetValue(data);
        }
    }

    private void Iteration(PMDG_NG3_SDK.PMDG_NG3_Data newData)
    {
        foreach (var field in typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields())
        {
            if (field.Name == "reserved")
            {
                continue;
            }

            string propertyName;
            object oldValue;
            object newValue;
            if (field.FieldType.IsArray)
            {
                if (field.GetValue(newData) is Array array)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        propertyName = field.Name + '_' + i;
                        if (!dynDict.TryGetValue(propertyName, out oldValue))
                        {
                            oldValue = null;
                        }
                        newValue = array.GetValue(i);
                        if (!Equals(oldValue, newValue) && watchedFields.Contains(propertyName))
                        {
                            dynDict[propertyName] = newValue;

                            FieldChanged?.Invoke(null, new PMDGDataFieldChangedEventArgs(propertyName, newValue));
                        }
                    }
                }
                continue;
            }

            propertyName = field.Name;
            if (!dynDict.TryGetValue(propertyName, out oldValue))
            {
                oldValue = null;
            }

            newValue = field.GetValue(newData);
            if (!Equals(oldValue, newValue) && watchedFields.Contains(propertyName))
            {
                dynDict[propertyName] = newValue;

                FieldChanged?.Invoke(null, new PMDGDataFieldChangedEventArgs(propertyName, newValue));
            }
        }
    }

    private void Instance_FieldChanged(object sender, PMDGDataFieldChangedEventArgs e)
    {
        switch (e.Value)
        {
            case bool boolValue:
                Logic(Convert.ToInt32(boolValue), e.PMDGDataName);
                break;

            case byte byteValue:
                Logic(Convert.ToInt32(byteValue), e.PMDGDataName);
                break;

            case ushort ushortValue:
                Logic(Convert.ToInt32(ushortValue), e.PMDGDataName);
                break;

            case short shortValue:
                Logic(Convert.ToInt32(shortValue), e.PMDGDataName);
                break;

            case uint uintValue:
                Logic(Convert.ToInt32(uintValue), e.PMDGDataName);
                break;

            case int intValue:
                Logic(intValue, e.PMDGDataName);
                break;

            case string stringValue:
                Logic(stringValue, e.PMDGDataName);
                break;

            case float floatValue:
                Logic(Convert.ToSingle(floatValue), e.PMDGDataName);
                break;

            default:
                throw new Exception(e.PMDGDataName);
        }
    }

    private void Logic<T>(T value, string pMDGDataName)
    {

    }
}

public class PMDGDataFieldChangedEventArgs : EventArgs
{
    public string PMDGDataName { get; }
    public object Value { get; }

    public PMDGDataFieldChangedEventArgs(string propertyName, object newValue)
    {
        PMDGDataName = propertyName;
        Value = newValue;
    }
}