﻿using Device_Interface_Manager.Devices.interfaceIT.USB;
using Device_Interface_Manager.Models;
using Device_Interface_Manager.SimConnectProfiles.PMDG;
using DynamicData;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Device_Interface_Manager.SimConnectProfiles;
public class Profiles
{
    private static readonly Profiles _instance = new();

    public static Profiles Instance { get => _instance; }

    public event EventHandler<PMDGDataFieldChangedEventArgs> FieldChanged;

    private PMDG_NG3_SDK.PMDG_NG3_Data data = new();

    public List<string> WatchedFields { get; private set; } = new();

    public IDictionary<string, object> DynDict { get; private set; } = new ExpandoObject();

    private bool simConnectStarted;

    private bool initialized;

    private void SimConnect_OnRecvClientData(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_CLIENT_DATA data)
    {
        if ((uint)PMDG.PMDG.DATA_REQUEST_ID.DATA_REQUEST == data.dwRequestID)
        {
            Iteration((PMDG_NG3_SDK.PMDG_NG3_Data)data.dwData[0]);
        }
    }

    public void Stop()
    {
        fDS_USB_Drivers?.ForEach(d => d.Close());
        arduino_Drivers?.ForEach(d => d.Close());
        fDS_USB_Drivers?.Clear();
        arduino_Drivers?.Clear();
        simConnectStarted = false;
        initialized = false;
        WatchedFields.Clear();
        DynDict.Clear();
    }

    private List<ProfileCreatorBase<InterfaceIT_BoardInfo.Device>> fDS_USB_Drivers;
    private List<ProfileCreatorBase<string>> arduino_Drivers;

    public async Task StartAsync(ProfileCreatorModel profileCreatorModel, string port)
    {
        if (profileCreatorModel.Driver == ProfileCreatorModel.Arduino && !string.IsNullOrEmpty(port))
        {
            arduino_Drivers ??= new();
            ProfileCreatorArduino profileCreatorArduino = new()
            {
                ProfileCreatorModel = profileCreatorModel
            };
            await profileCreatorArduino.StartAsync(port);
            arduino_Drivers.Add(profileCreatorArduino);

            Init(profileCreatorModel);
        }
    }

    public async Task StartAsync(ProfileCreatorModel profileCreatorModel, InterfaceIT_BoardInfo.Device device)
    {
        if (profileCreatorModel.Driver == ProfileCreatorModel.FDSUSB && device is not null)
        {
            fDS_USB_Drivers ??= new();
            ProfileCreator_FDS_USB fDS_USB_Driver = new()
            {
                ProfileCreatorModel = profileCreatorModel
            };
            await fDS_USB_Driver.StartAsync(device);
            fDS_USB_Driver.Start();
            fDS_USB_Drivers.Add(fDS_USB_Driver);

            Init(profileCreatorModel);
        }
    }

    private void Init(ProfileCreatorModel profileCreatorModel)
    {
        foreach (var output in profileCreatorModel.OutputCreator)
        {
            if (output.DataType == ProfileCreatorModel.PMDG737 && output.IsActive)
            {
                if (!string.IsNullOrEmpty(output.PMDGData))
                {
                    string pMDGDataFieldName = ConvertDataToPMDGDataFieldName(output);
                    if (!WatchedFields.Contains(pMDGDataFieldName))
                    {
                        WatchedFields.Add(pMDGDataFieldName);
                    }
                }
                if (!initialized)
                {
                    initialized = true;
                    Initialize<PMDG_NG3_SDK.PMDG_NG3_Data>();
                }
            }
        }

        if (!simConnectStarted)
        {
            SimConnectClient.Instance.SimConnectMSFS.OnRecvClientData += SimConnect_OnRecvClientData;
            simConnectStarted = true;
        }
    }


    public static string ConvertDataToPMDGDataFieldName(OutputCreator ouput)
    {
        string pMDGDataFieldName = ouput.PMDGData;
        if (ouput.PMDGDataArrayIndex is not null)
        {
            pMDGDataFieldName = pMDGDataFieldName + '_' + ouput.PMDGDataArrayIndex;
        }
        return pMDGDataFieldName;
    }

    private void Initialize<T>() where T : struct
    {
        foreach (FieldInfo field in typeof(T).GetFields())
        {
            if (field.Name == "reserved")
            {
                continue;
            }

            if (field.FieldType.IsArray)
            {
                Array array = (Array)field.GetValue(data);
                if (field.GetCustomAttributes(typeof(MarshalAsAttribute), false).FirstOrDefault() is MarshalAsAttribute marshalAsAttribute)
                {
                    array = Array.CreateInstance(field.FieldType.GetElementType(), marshalAsAttribute.SizeConst);
                    field.SetValue(data, array);
                }
                int i = 0;
                foreach (object item in array)
                {
                    DynDict[field.Name + '_' + i] = item;
                    i++;
                }
            }
            DynDict[field.Name] = field.GetValue(data);
        }
    }

    private void Iteration<T>(T newData) where T : struct
    {
        foreach (FieldInfo field in typeof(T).GetFields())
        {
            if (field.Name == "reserved")
            {
                continue;
            }

            if (field.FieldType.IsArray)
            {
                if (field.GetValue(newData) is Array array)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        CheckOldNewValue(field.Name + '_' + i, array.GetValue(i));
                    }
                }
                continue;
            }

            CheckOldNewValue(field.Name, field.GetValue(newData));
        }
    }

    private void CheckOldNewValue(string propertyName, object newValue)
    {
        if (!DynDict.TryGetValue(propertyName, out object oldValue))
        {
            oldValue = null;
        }

        if (!Equals(oldValue, newValue) && WatchedFields.Contains(propertyName))
        {
            DynDict[propertyName] = newValue;

            FieldChanged?.Invoke(null, new PMDGDataFieldChangedEventArgs(propertyName, newValue));
        }
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