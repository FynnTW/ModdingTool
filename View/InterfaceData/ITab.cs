using System;
using System.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ModdingTool.View.UserControls;

namespace ModdingTool.View.InterfaceData;

public interface ITab
{
    string Name { get; set; }
    ICommand CloseCommand { get; }
    event EventHandler CloseRequested;

    public abstract class Tab : ITab
    {
        public Tab()
        {
            CloseCommand = new RelayCommand(Close);
        }

        public string Name { get; set; }
        public ICommand CloseCommand { get; private set; }
        public event EventHandler CloseRequested;

        private void Close()
        {

            MainWindow window = (ModdingTool.MainWindow)App.Current.MainWindow;
            var datatab = window?.FindName("DataTabLive") as DataTab;
            // Remove the tab from the collection of tabs
            datatab.Tabs.Remove(this);

            // Trigger the CloseRequested event
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }



    }
}