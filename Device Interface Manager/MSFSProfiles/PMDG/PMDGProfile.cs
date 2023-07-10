using System;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Device_Interface_Manager.MVVM.Model;
using Device_Interface_Manager.interfaceIT.USB;
using Device_Interface_Manager.MSFSProfiles.PMDG.B737;

namespace Device_Interface_Manager.MSFSProfiles.PMDG;
public class PMDGProfile
{
    private static PMDGProfile instance;

    public static PMDGProfile Instance
    {
        get
        {
            instance ??= new PMDGProfile();
            return instance;
        }
    }

    public static event EventHandler<PMDGDataFieldChangedEventArgs> FieldChanged;

    private PMDG_NG3_SDK.PMDG_NG3_Data data = new();

    private readonly List<string> watchedFields = new();

    private readonly IDictionary<string, object> dynDict = new ExpandoObject();

    private bool pMDG737Registered;

    private bool simConnectStarted;

    private void SimConnect_OnRecvClientData(Microsoft.FlightSimulator.SimConnect.SimConnect sender, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_RECV_CLIENT_DATA data)
    {
        if ((uint)PMDG_NG3_SDK.DATA_REQUEST_ID.DATA_REQUEST == data.dwRequestID)
        {
            Iteration((PMDG_NG3_SDK.PMDG_NG3_Data)data.dwData[0]);
        }
    }

    public void Stop()
    {
        fDS_USB_Drivers.ForEach(d => d.Stop());
        pMDG737Registered = false;
        simConnectStarted = false;
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
                TestCreator = profileCreatorModel
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
                    Initialize();
                }
            }
        }

        if (!simConnectStarted)
        {
            SimConnectClient.Instance.SimConnect.OnRecvClientData += SimConnect_OnRecvClientData;
            simConnectStarted = true;
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