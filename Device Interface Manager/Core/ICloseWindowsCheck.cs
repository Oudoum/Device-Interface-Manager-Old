using System;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace Device_Interface_Manager.Core;

interface ICloseWindowsCheck
{
    Action Close { get; set; }

    MessageDialogResult CanClose();
}