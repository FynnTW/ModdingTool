using System;
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
            Name = DataList.Selected;
        }

        public string Name { get; set; }
        public ICommand CloseCommand { get; private set; }
        public event EventHandler CloseRequested;

        private void Close()
        {
            // Your closing logic here

            // Trigger the CloseRequested event
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }



    }
}