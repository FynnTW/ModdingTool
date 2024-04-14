using System.Windows;
using System.Windows.Controls;

namespace ModdingTool.View.UserControls;

public partial class LabelNumberInput : UserControl
{

    public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
        nameof(LabelText), typeof(string),
        typeof(LabelNumberInput),
        new PropertyMetadata(default(string)));

    public static readonly DependencyProperty InputTextProperty = DependencyProperty.Register(
        nameof(InputText), typeof(string),
        typeof(LabelNumberInput),
        new FrameworkPropertyMetadata(default(string),
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public LabelNumberInput()
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
