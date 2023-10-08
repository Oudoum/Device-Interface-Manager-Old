using System;

namespace Device_Interface_Manager.Models
{
    public interface ISwitchLogChanged
    {
        Action SwitchLogChanged { get; set; }
    }
}