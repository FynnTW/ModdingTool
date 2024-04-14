using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using CommunityToolkit.Mvvm.Input;
using ModdingTool.ViewModel;
using ModdingTool.ViewModel.InterfaceData;

namespace ModdingTool.View.UserControls;

public partial class HyperLinkCombo : UserControl
{
        
    public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
        nameof(LabelText), typeof(string), 
        typeof(HyperLinkCombo), 
        new PropertyMetadata(default(string)));
    
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource), typeof(IEnumerable), typeof(HyperLinkCombo), new PropertyMetadata(default(IEnumerable)));

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem), typeof(object), typeof(HyperLinkCombo), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
  
    public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register(
        nameof(IsEditable), typeof(object), typeof(HyperLinkCombo), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public IEnumerable ItemsSource
    {
        get => (IEnumerable) GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(ClickCommand), typeof(IRelayCommand<object>), typeof(HyperLinkCombo), new PropertyMetadata(null));

    public IRelayCommand<object> ClickCommand
    {
        get => (IRelayCommand<object>)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    private void Hyperlink_Click(object sender, RoutedEventArgs e)
    {
        ClickCommand?.Execute(sender);
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public bool IsEditable
    {
        get => (bool)GetValue(IsEditableProperty);
        set => SetValue(IsEditableProperty, value);
    }
    public HyperLinkCombo()
    {
        IsEditable = false;
        InitializeComponent();
    }
    
    private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var comboBox = (ComboBox)sender;
        if (string.IsNullOrEmpty(comboBox.Text))
        {
            comboBox.SelectedItem = string.Empty;
        }
    }

    private void SoldierGotoHyper_OnClick(object sender, RoutedEventArgs e)
    {
        var textblock = (sender as Hyperlink)?.Parent as TextBlock;
        var grid = textblock?.Parent as Grid;
        if (grid?.Parent is not HyperLinkCombo hyperLinkCombo) return;
        var attribute = "";
        attribute = hyperLinkCombo.Name;
        if (string.IsNullOrWhiteSpace(attribute)) return;
        var unit = textblock?.DataContext as UnitTab;
        var model = (string) unit?.SelectedUnit.GetType().GetProperty(attribute)?.GetValue(unit.SelectedUnit, null)!;
        if (string.IsNullOrWhiteSpace(model)) return;
        var newTab = new ModelDbTab(model);
        var window = (MainWindow)Application.Current.MainWindow;
        var dataTab = window?.FindName("DataTabLive") as DataTab;
        var dataViewModel = dataTab?.DataContext as DataTabViewModel;
        dataViewModel?.AddTab(newTab);
    }
    
    public string LabelText
    {
        get => (string) GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }
}
