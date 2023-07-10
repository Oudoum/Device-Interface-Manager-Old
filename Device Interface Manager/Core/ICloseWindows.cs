using System;

namespace Device_Interface_Manager.Core;

interface ICloseWindows
{
    Action Close { get; set; }
}