using System;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MSFSProfiles.PMDG.B737;
using Device_Interface_Manager.MSFSProfiles.PMDG;
using System.Reflection;

namespace Device_Interface_Manager.MSFSProfiles;
public class Profiles
{
    private static Profiles instance;

    public static Profiles Instance
    {
        get
        {
            instance ??= new Profiles();
            return instance;
        }
    }

    public event EventHandler<PMDGDataFieldChangedEventArgs> FieldChanged;

    private PMDG_NG3_SDK.PMDG_NG3_Data data = new();

    private readonly List<string> watchedFields = new();

    private readonly IDictionary<string, object> dynDict = new ExpandoObject();

    private bool pMDG737Registered;

    private bool simConnectStarted;

    private bool initialized;

    private void SimConnect_OnRecvClientData(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_CLIENT_DATA data)
    {
        if ((uint)PMDG_NG3_SDK.DATA_REQUEST_ID.DATA_REQUEST == data.dwRequestID)
        {
            if (!simConnectStarted)
            {
                watchedFields.ForEach(x => FieldChanged?.Invoke(null, new PMDGDataFieldChangedEventArgs(x, dynDict[x])));
                simConnectStarted = true;
            }
            Iteration((PMDG_NG3_SDK.PMDG_NG3_Data)data.dwData[0]);
        }
    }

    public void Stop()
    {
        fDS_USB_Drivers.ForEach(d => d.Stop());
        fDS_USB_Drivers.Clear();
        pMDG737Registered = false;
        simConnectStarted = false;
        initialized = false;
        watchedFields.Clear();
        dynDict.Clear();
    }

    private List<FDS_USB_Driver> fDS_USB_Drivers;
    public async Task StartAsync(ProfileCreatorModel[] profileCreatorModel, InterfaceIT_BoardInfo.Device[] devices)
    {
        foreach (var item in profileCreatorModel)
        {
            await StartAsync(item, devices.FirstOrDefault(k => k.BoardType == item.DeviceName));
        }
    }

    public async Task StartAsync(ProfileCreatorModel profileCreatorModel, InterfaceIT_BoardInfo.Device device)
    {
        if (profileCreatorModel.SelectedDriver == ProfileCreatorModel.FDSUSB)
        {
            fDS_USB_Drivers ??= new();
            FDS_USB_Driver fDS_USB_Driver = new()
            {
                ProfileCreatorModel = profileCreatorModel
            };
            await fDS_USB_Driver.StartAsync(device);
            fDS_USB_Drivers.Add(fDS_USB_Driver);
        }

        if (!pMDG737Registered)
        {
            foreach (var input in profileCreatorModel.InputCreator)
            {
                if (input.SelectedEventType == ProfileCreatorModel.PMDG737)
                {
                    pMDG737Registered = true;
                    PMDG737.RegisterPMDGDataEvents(SimConnectClient.Instance.SimConnect);
                    break;
                }
            }
        }

        foreach (var output in profileCreatorModel.OutputCreator)
        {
            if (output.SelectedDataType == ProfileCreatorModel.PMDG737)
            {
                if (!string.IsNullOrEmpty(output.PMDGDataFieldName))
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
                if (!pMDG737Registered)
                {
                    pMDG737Registered = true;
                    PMDG737.RegisterPMDGDataEvents(SimConnectClient.Instance.SimConnect);
                }
                if (!initialized)
                {
                    initialized = true;
                    Initialize();
                }
            }
        }

        SimConnectClient.Instance.SimConnect.OnRecvClientData += SimConnect_OnRecvClientData;
    }

    private void Initialize()
    {
        foreach (FieldInfo field in typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields())
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
                    dynDict[field.Name + '_' + i] = item;
                    i++;
                }
            }
            dynDict[field.Name] = field.GetValue(data);
        }
    }

    private void Iteration(PMDG_NG3_SDK.PMDG_NG3_Data newData)
    {
        foreach (FieldInfo field in typeof(PMDG_NG3_SDK.PMDG_NG3_Data).GetFields())
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
        if (!dynDict.TryGetValue(propertyName, out object oldValue))
        {
            oldValue = null;
        }

        if (!Equals(oldValue, newValue) && watchedFields.Contains(propertyName))
        {
            dynDict[propertyName] = newValue;

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