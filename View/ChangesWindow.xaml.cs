using System.Linq;
using System.Windows;
using System.Windows.Input;
using ModdingTool.ViewModel;

namespace ModdingTool.View;

public partial class ChangesWindow : Window
{
    ChangesViewModel ViewModel => (ChangesViewModel) DataContext;
    public ChangesWindow()
    {
        InitializeComponent();
        DataContext = new ChangesViewModel();
    }

    private void ChangesLogBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var change = Changes.GetChanges().FirstOrDefault(x => x.ToString() == ChangesLogBox.SelectedItem.ToString());
        change?.Undo();
        ViewModel.UpdateDisplayedItems(Changes.GetChangesString());
    }
}