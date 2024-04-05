using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ModdingTool.ViewModel;

public partial class ChangesViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<string> _changeList;
    
    public ChangesViewModel()
    {
        ChangeList = new ObservableCollection<string>();
        UpdateDisplayedItems(Changes.GetChangesString());
    }

    public void UpdateDisplayedItems(List<string> items)
    {
        ChangeList.Clear();
        foreach (var item in items)
        {
            ChangeList.Add(item);
        }
    }
    
    
    
}