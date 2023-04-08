using System;

namespace Device_Interface_Manager.MVVM.Model
{
    public interface ISwitchLogChanged
    {
        Action SwitchLogChanged { get; set; }
    }
}