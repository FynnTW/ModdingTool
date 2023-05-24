using ModdingTool.View.UserControls;
using static ModdingTool.Globals;

namespace ModdingTool.View.InterfaceData;

public class UnitTab : ITab.Tab
{
    public Unit SelectedUnit = null;

    public UnitTab()
    {
        SelectedUnit = AllUnits[Name];
    }


}