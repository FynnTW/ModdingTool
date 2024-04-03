using System.Collections.Generic;
using System.Linq;

namespace ModdingTool.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;

public class CreationViewModel : ObservableObject
{
    public string NameValue { get; set; } = "";
   // public string LocalizedNameValue { get; set; } = "";
    private string TabType { get; set; }
    public string BaseValue { get; set; } = Globals.UnitDataBase.First().Key;
    public List<string> ValuesList { get; private set; } = Globals.UnitDataBase.Keys.ToList();
    
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
            viewModel.BaseValue = Globals.UnitDataBase.First().Key;
            viewModel.ValuesList = Globals.UnitDataBase.Keys.ToList();
        }

        public void Create(CreationViewModel viewModel)
        {
            var newUnit = Unit.CloneUnit(viewModel.NameValue, viewModel.NameValue, Globals.UnitDataBase[viewModel.BaseValue]);
            if (newUnit == null) return;
            EduParser.AddUnit(newUnit);
        }
    }

    private class ModelCreation : ICreationType
    {
        public void FillLists(CreationViewModel viewModel)
        {
            viewModel.BaseValue = Globals.BattleModelDataBase.First().Key;
            viewModel.ValuesList = Globals.BattleModelDataBase.Keys.ToList();
        }

        public void Create(CreationViewModel viewModel)
        {
            var newModel = BattleModel.CloneModel(viewModel.NameValue, Globals.BattleModelDataBase[viewModel.BaseValue]);
            if (newModel == null) return;
            Globals.BattleModelDataBase.Add(viewModel.NameValue, newModel);
        }
    }


}