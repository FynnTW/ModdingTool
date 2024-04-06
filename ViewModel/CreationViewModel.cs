using System.Collections.Generic;
using System.Linq;

namespace ModdingTool.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using static ModdingTool.Globals;

public class CreationViewModel : ObservableObject
{
    public string NameValue { get; set; } = "";
   // public string LocalizedNameValue { get; set; } = "";
    private string TabType { get; set; }
    public string BaseValue { get; set; } = string.Empty;
    public List<string> ValuesList { get; private set; } = new();
    
    public CreationViewModel(string tabType)
    {
        TabType = tabType;
        var creationType = GetCreationType(TabType);
        creationType?.FillLists(this);
    }

    private static ICreationType? GetCreationType(string tabType)
    {
        return tabType switch
        {
            "Units" => new UnitCreation(),
            "Model Entries" => new ModelCreation(),
            _ => (ICreationType?)null
        };
    }
    
    public (string, string, bool) Create()
    {
        var creationType = GetCreationType(TabType);
        creationType?.Create(this);
        return (TabType, NameValue, creationType != null);
    }

    private interface ICreationType
    {
        void FillLists(CreationViewModel viewModel);
        void Create(CreationViewModel viewModel);
    }

    private class UnitCreation : ICreationType
    {
        public void FillLists(CreationViewModel viewModel)
        {
            viewModel.BaseValue = ModData.Units.GetByIndex(0)?.Type ?? string.Empty;
            viewModel.ValuesList = ModData.Units.GetNames();
        }

        public void Create(CreationViewModel viewModel)
        {
            if (!ModData.Units.Contains(viewModel.BaseValue)) return;
            var newUnit = Unit.CloneUnit(viewModel.NameValue, viewModel.NameValue, ModData.Units.Get(viewModel.BaseValue)!);
            if (newUnit == null) return;
            ModData.Units.Add(newUnit);
        }
    }

    private class ModelCreation : ICreationType
    {
        public void FillLists(CreationViewModel viewModel)
        {
            viewModel.BaseValue = ModData.BattleModelDb.GetByIndex(0)?.Name ?? "";
            viewModel.ValuesList = ModData.BattleModelDb.GetNames();
        }

        public void Create(CreationViewModel viewModel)
        {
            if (!ModData.BattleModelDb.Contains(viewModel.BaseValue)) return;
            var newModel = BattleModel.CloneModel(viewModel.NameValue, ModData.BattleModelDb.Get(viewModel.BaseValue)!);
            if (newModel == null) return;
            ModData.BattleModelDb.Add(newModel);
        }
    }


}