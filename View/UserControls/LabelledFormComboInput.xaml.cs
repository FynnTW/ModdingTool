using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ModdingTool.View.UserControls;

public partial class LabelledFormComboInput : UserControl
{
        
    public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
        nameof(LabelText), typeof(string), 
        typeof(LabelledFormComboInput), 
        new PropertyMetadata(default(string)));
    
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource), typeof(IEnumerable), typeof(LabelledFormComboInput), new PropertyMetadata(default(IEnumerable)));

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem), typeof(object), typeof(LabelledFormComboInput), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
  
    public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register(
        nameof(IsEditable), typeof(object), typeof(LabelledFormComboInput), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public IEnumerable ItemsSource
    {
        get => (IEnumerable) GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
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
    
    public LabelledFormComboInput()
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
    
    public string LabelText
    {
        get => (string) GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    private void SoldierGotoHyper_OnClick(object sender, RoutedEventArgs e)
    {
    }
}