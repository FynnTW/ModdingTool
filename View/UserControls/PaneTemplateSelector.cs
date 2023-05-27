using ModdingTool.View.InterfaceData;
using System.Windows;
using System.Windows.Controls;

namespace ModdingTool.View.UserControls;

public class PaneTemplateSelector : DataTemplateSelector
{

    public DataTemplate UnitTabTemplate { get; set; }
    public DataTemplate ModelDbTabTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is UnitTab)
            return UnitTabTemplate;

        if (item is ModelDbTab)
            return ModelDbTabTemplate;

        return base.SelectTemplate(item, container);
    }

}