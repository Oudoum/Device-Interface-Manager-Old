using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Device_Interface_Manager.Core;
public class ViewLocator
{
    public event Action Closed;

    public ViewLocator(ObservableObject data)
    {
        BuildWindow(data);
    }

    private void BuildWindow(ObservableObject data)
    {
        if (data is null)
        {
            MessageBox.Show("Data was null");
            return;
        }

        string name = data.GetType().FullName!.Replace("ViewModel", "View");
        Type type = Type.GetType(name);

        if (type is not null)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == type)
                {
                    window.Activate();
                    return;
                }
            }

            Window view = (Window)Activator.CreateInstance(type)!;
            view.DataContext = data;
            view.Show();
            view.Closed += (s, e) =>
            {
                Closed?.Invoke();
            };
            return;
        }
        MessageBox.Show("Not Found: " + name);
    }
}