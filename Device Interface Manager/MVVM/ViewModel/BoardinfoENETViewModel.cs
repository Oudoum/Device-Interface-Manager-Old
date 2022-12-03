using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Device_Interface_Manager.MVVM.ViewModel
{
    class BoardinfoENETViewModel : ObservableObject
    {
        public ObservableCollection<ObservableCollection<string>> InterfaceITEthernetInfoTextCollection { get; set; } = new();

        public ObservableCollection<string> InterfaceITEthernetInfoIPCollection { get; set; } = new();

        public ObservableCollection<string> InterfaceITEthernetInfoText { get; set; } = new();

        private int _interfaceInfoID = -1;
        public int InterfaceInfoID
        {
            get => this._interfaceInfoID;
            set
            {
                this._interfaceInfoID = value;
                if (InterfaceITEthernetInfoTextCollection.Count >= value && InterfaceITEthernetInfoTextCollection.Count != 0)
                {
                    this.InterfaceITEthernetInfoText = this.InterfaceITEthernetInfoTextCollection[value];
                    OnPropertyChanged(nameof(InterfaceITEthernetInfoText));
                }
                OnPropertyChanged();
            }
        }

        public BoardinfoENETViewModel()
        {

        }
    }
}