using System.Windows;
using ModdingTool.ViewModel;

namespace ModdingTool.View.UserControls;

public partial class OptionsWindow : Window
{
    private OptionsViewModel ViewModel => (OptionsViewModel)DataContext;
    public OptionsWindow()
    {
        InitializeComponent();
        DataContext = new OptionsViewModel();
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.SaveOptions();
        Close();
    }

    private void Browse_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.BrowseDirectories();
        Directories.Items.Refresh();
    }
}