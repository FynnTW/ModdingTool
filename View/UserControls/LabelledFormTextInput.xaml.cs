using System.Windows;
using System.Windows.Controls;

namespace ModdingTool.View.UserControls;

public partial class LabelledFormTextInput : UserControl
{
        
    public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
        nameof(LabelText), typeof(string), 
        typeof(LabelledFormTextInput), 
        new PropertyMetadata(default(string)));

    public static readonly DependencyProperty InputTextProperty = DependencyProperty.Register(
        nameof(InputText), typeof(string), 
        typeof(LabelledFormTextInput), 
        new FrameworkPropertyMetadata(default(string), 
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public LabelledFormTextInput()
    {
        InitializeComponent();
    }
    
    public string LabelText
    {
        get => (string) GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public string InputText
    {
        get => (string) GetValue(InputTextProperty);
        set => SetValue(InputTextProperty, value);
    }
}