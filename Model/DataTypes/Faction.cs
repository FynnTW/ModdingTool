using System.Collections.Generic;

namespace ModdingTool
{
    public class Faction : GameType
    {
        private string _culture = "";
        private Culture _cultureLink = new();
        private string _religion = "";
        private string _symbol = "";
        private string _rebelSymbol = "";
        private int _primaryColourR;
        private int _primaryColourG;
        private int _primaryColourB;
        private int _secondaryColourR;
        private int _secondaryColourG;
        private int _secondaryColourB;
        private string _loadingLogo = "";
        private string _standardIndex = "";
        private string _logoIndex = "";
        private string _smallLogoIndex = "";
        private int _triumphValue;
        private bool _customBattleAvailability;
        private bool _canSap;
        private bool _prefersNavalInvasions;
        private bool _canHavePrincess;
        private bool _disbandToPools = true;
        private bool _canBuildSiegeTowers = true;
        private bool _canTransmitPlague = true;
        private string _hasFamilyTree = "";
        private int _hordeMinUnits;
        private int _hordeMaxUnits;
        private int _hordeMaxUnitsReductionEveryHorde;
        private int _hordeUnitPerSettlementPopulation;
        private int _hordeMinNamedCharacters;
        private int _hordeMaxPercentArmyStack;
        private int _hordeDisbandPercentOnSettlementCapture;
        private List<string> _hordeUnits = new List<string>();
        private List<string> _unitOwnership = new List<string>();
        private string _logo = "";
        private string _localizedName = "";
        private bool _spawnedOnEvent = false;
        private string _spawnModifier = "";
        private string _spawnFaction = "";
        private bool _slaveFaction;
        private bool _papalFaction;
        private List<string> _periodsUnavailableInCustomBattle = new();

        public string Name { get => _name; set => _name = value; }
        public string Culture { get => _culture; set => _culture = value; }
        public string Religion { get => _religion; set => _religion = value; }
        public string Symbol { get => _symbol; set => _symbol = value; }
        public string RebelSymbol { get => _rebelSymbol; set => _rebelSymbol = value; }
        public string LoadingLogo { get => _loadingLogo; set => _loadingLogo = value; }
        public string StandardIndex { get => _standardIndex; set => _standardIndex = value; }
        public string LogoIndex { get => _logoIndex; set => _logoIndex = value; }
        public string SmallLogoIndex { get => _smallLogoIndex; set => _smallLogoIndex = value; }
        public int TriumphValue { get => _triumphValue; set => _triumphValue = value; }
        public bool CustomBattleAvailability { get => _customBattleAvailability; set => _customBattleAvailability = value; }
        public bool CanSap { get => _canSap; set => _canSap = value; }
        public bool PrefersNavalInvasions { get => _prefersNavalInvasions; set => _prefersNavalInvasions = value; }
        public bool CanHavePrincess { get => _canHavePrincess; set => _canHavePrincess = value; }
        public bool DisbandToPools { get => _disbandToPools; set => _disbandToPools = value; }
        public bool CanBuildSiegeTowers { get => _canBuildSiegeTowers; set => _canBuildSiegeTowers = value; }
        public bool CanTransmitPlague { get => _canTransmitPlague; set => _canTransmitPlague = value; }
        public string HasFamilyTree { get => _hasFamilyTree; set => _hasFamilyTree = value; }
        public int HordeMinUnits { get => _hordeMinUnits; set => _hordeMinUnits = value; }
        public int HordeMaxUnits { get => _hordeMaxUnits; set => _hordeMaxUnits = value; }
        public int HordeMaxUnitsReductionEveryHorde { get => _hordeMaxUnitsReductionEveryHorde; set => _hordeMaxUnitsReductionEveryHorde = value; }
        public int HordeUnitPerSettlementPopulation { get => _hordeUnitPerSettlementPopulation; set => _hordeUnitPerSettlementPopulation = value; }
        public int HordeMinNamedCharacters { get => _hordeMinNamedCharacters; set => _hordeMinNamedCharacters = value; }
        public int HordeMaxPercentArmyStack { get => _hordeMaxPercentArmyStack; set => _hordeMaxPercentArmyStack = value; }
        public int HordeDisbandPercentOnSettlementCapture { get => _hordeDisbandPercentOnSettlementCapture; set => _hordeDisbandPercentOnSettlementCapture = value; }
        public List<string> HordeUnits { get => _hordeUnits; set => _hordeUnits = value; }
        public string Logo { get => _logo; set => _logo = value; }
        public string LocalizedName { get => _localizedName; set => _localizedName = value; }
        public List<string> UnitOwnership { get => _unitOwnership; set => _unitOwnership = value; }
        public bool SpawnedOnEvent { get => _spawnedOnEvent; set => _spawnedOnEvent = value; }
        public int PrimaryColourR { get => _primaryColourR; set => _primaryColourR = value; }
        public int PrimaryColourG { get => _primaryColourG; set => _primaryColourG = value; }
        public int PrimaryColourB { get => _primaryColourB; set => _primaryColourB = value; }
        public int SecondaryColourR { get => _secondaryColourR; set => _secondaryColourR = value; }
        public int SecondaryColourG { get => _secondaryColourG; set => _secondaryColourG = value; }
        public int SecondaryColourB { get => _secondaryColourB; set => _secondaryColourB = value; }
        public string SpawnModifier { get => _spawnModifier; set => _spawnModifier = value; }
        public string SpawnFaction { get => _spawnFaction; set => _spawnFaction = value; }
        public bool SlaveFaction { get => _slaveFaction; set => _slaveFaction = value; }
        public bool PapalFaction { get => _papalFaction; set => _papalFaction = value; }
        public Dictionary<string, FactionCharacter> FactionCharacterTypes { get; set; } = new();
        public List<string> PeriodsUnavailableInCustomBattle { get => _periodsUnavailableInCustomBattle; set => _periodsUnavailableInCustomBattle = value; }
    }
}

public class FactionCharacter
{
    public string Name;
    public string[] Models = new string[11];
    public int Dictionary = 2;
    public string BattleModel = "";
    public string BattleEquip = "gladius, chainmail shirt helmet and rectangular shield";
}