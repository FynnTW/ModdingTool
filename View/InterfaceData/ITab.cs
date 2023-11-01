using CommunityToolkit.Mvvm.Input;
using ModdingTool.View.UserControls;
using ModdingTool.ViewModel;
using System;
using System.Windows.Input;

namespace ModdingTool.View.InterfaceData;

public interface ITab
{
    string Title { get; set; }
    ICommand CloseCommand { get; }
    event EventHandler CloseRequested;
    public InterfaceLogic iLogic { get; set; }

    public abstract class Tab : ITab
    {
        public Tab()
        {
            CloseCommand = new RelayCommand(Close);
            iLogic = new InterfaceLogic(this);
        }

        public Tab(string name)
        {
            CloseCommand = new RelayCommand(Close);
            iLogic = new InterfaceLogic(this);
            Title = name;
        }

        public string Title { get; set; }
        public ICommand CloseCommand { get; private set; }
        public event EventHandler CloseRequested;
        public InterfaceLogic iLogic { get; set; }

        private void Close()
        {

            MainWindow window = (ModdingTool.MainWindow)App.Current.MainWindow;
            var datatab = window?.FindName("DataTabLive") as DataTab;
            // Remove the tab from the collection of tabs
            var dataViewModel = datatab?.DataContext as DataTabViewModel;
            dataViewModel?.Tabs.Remove(this);

            // Trigger the CloseRequested event
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }



    }
}