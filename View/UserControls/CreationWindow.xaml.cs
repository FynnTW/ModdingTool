using System.Windows;
using System.Windows.Input;
using ModdingTool.ViewModel;

namespace ModdingTool.View.UserControls;

public partial class CreationWindow : Window
{
    private CreationViewModel ViewModel => (CreationViewModel)DataContext;
    
    public CreationWindow(string tabType)
    {
        InitializeComponent();
        this.DataContext = new CreationViewModel(tabType);
    }

    private void CreateWindowSelector_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
    }

    private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
    {
        var (type, name, created) = ViewModel.Create();
        Close();
        if (!created) return;
        var window = (MainWindow)Application.Current?.MainWindow!;
        if (window?.FindName("DataListLive") is DataList dataList)
            dataList.ViewModel.OpenNewTab(type, name);
        else
            throw new System.Exception("DataListLive not found");
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}