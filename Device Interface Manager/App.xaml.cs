using System.Windows;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace Device_Interface_Manager
{
    public partial class App : Application
    {
        public ObservableCollection<string> LogMessages { get; set; } = new();

        public App()
        {
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddProvider(new Core.CollectionLoggerProvider(LogMessages));
                })
                .AddSingleton<MVVM.View.MainWindow>()
                .AddTransient<MVVM.ViewModel.MainViewModel>()
                .BuildServiceProvider());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MVVM.View.MainWindow mainWindow = Ioc.Default.GetService<MVVM.View.MainWindow>();
            mainWindow.Show();
        }
    }
}