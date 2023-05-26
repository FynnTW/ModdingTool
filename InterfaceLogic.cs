using System.Collections.Generic;
using System.Windows.Controls;
using ModdingTool.View.InterfaceData;

namespace ModdingTool;

public class InterfaceLogic
{
    public List<Control>? Changes;
    private ITab tab;

    public InterfaceLogic(ITab tab)
    {
        this.tab = tab;
        Changes = new List<Control>();
    }

}