---@diagnostic disable: missing-return, undefined-global, undefined-doc-param
---Function to get the databases the API is using
---@return Globals.Data mod path
function GetModData() end

---@class Globals.Data
Globals.Data = {

    ---@type BattleModelDb
    BattleModelDb = nil,

    ---@type UnitDb
    Units = nil,

    ---@type BuildingDb
    Buildings = nil,
}

---@class BattleModelDb
BattleModelDb = {

    ---@type table<list, string>
    UsedModels = nil,

}

---Adds a new BattleModel to the BattleModels dictionary.
---@param BattleModel model
function BattleModelDb:Add(model) end

---Retrieves the count of BattleModels in the BattleModels dictionary.
---@return integer num
function BattleModelDb:GetCount() end

---Checks if the BattleModels dictionary contains a specific key.
---@param name string
---@return boolean contains
function BattleModelDb:Contains(name) end

---Get a BattleModel by name.
---@param name string
---@return BattleModel|nil model
function BattleModelDb:Get(name) end

---Get a BattleModel by index.
---@param index integer
---@return BattleModel|nil model
function BattleModelDb:GetByIndex(index) end

---Imports a JSON file and deserializes it into the BattleModels dictionary.
---@param file string
function BattleModelDb:ImportJson(file) end

---Exports the BattleModels dictionary to a JSON file.
function BattleModelDb:ExportJson() end

---Retrieves a list of the names of all BattleModels in the BattleModels dictionary.
---@return table<integer, string> names
function BattleModelDb:GetNames() end

---Removes a BattleModel from the BattleModels dictionary.
---@param name string
function BattleModelDb:Remove(name) end

---Writes the BattleModels dictionary to the battlemodels.modeldb file in the mod folder.
function BattleModelDb:WriteFile() end

---Parses the battle_models.modeldb file and populates the BattleModels dictionary.
function BattleModelDb:ParseFile() end

---Checks the usage of models and mounts in the game files.
function BattleModelDb:CheckModelUsage() end

---@class BattleModel
BattleModel = {

    ---@type string
    Name = nil,

    ---@type number
    Scale = nil,

    ---@type integer
    LodCount = nil,

    ---@type table<integer, Lod>
    LodTable = nil,

    ---@type integer
    MainTexturesCount = nil,

    ---@type table<integer, Texture>
    MainTextures = nil,

    ---@type integer
    AttachTexturesCount = nil,

    ---@type table<integer, Texture>
    AttachTextures = nil,

    ---@type integer
    MountTypeCount = nil,

    ---@type table<integer, Animation>
    Animations = nil,

    ---@type integer
    TorchIndex = nil,

    ---@type number
    TorchBoneX = nil,

    ---@type number
    TorchBoneY = nil,

    ---@type number
    TorchBoneZ = nil,

    ---@type number
    TorchSTerteX = nil,

    ---@type number
    TorchSTerteY = nil,

    ---@type number
    TorchSTerteZ = nil,

    ---@type ModelUsage
    ModelUsage = nil,
}

---Adds a new level of detail (LOD) to the BattleModel. If the LOD already exists, updates the mesh and distance.
---@param meshName string
---@param distance integer
---@param index integer The index of the LOD in the LOD table. For example 0 would change the lod0 etc
---@param name string Name of the model
function BattleModel:AddLod(meshName, distance, index, name) end

---Adds a main texture to the BattleModel. If the texture already exists, updates the texture path, normal map, and sTerte.
---@param faction string
---@param texturePath string
---@param normal string
---@param sTerte string
---@param name string Name of the model
---@return Texture newTexture
function BattleModel:AddMainTexture(faction, texturePath, normal, sTerte, name) end

---@param texture Texture
function BattleModel:RemoveMainTexture(texture) end

---Adds a attach texture to the BattleModel. If the texture already exists, updates the texture path, normal map, and sTerte.
---@param faction string
---@param texturePath string
---@param normal string
---@param sTerte string
---@param name string Name of the model
---@return Texture newTexture
function BattleModel:AddAttachTexture(faction, texturePath, normal, sTerte, name) end

---@param texture Texture
function BattleModel:RemoveAttachTexture(texture) end

---Adds an animation to the BattleModel. If the entry for the provided mount type already exists, updates the Termary and secondary skeleton.
---@param mountType string
---@param TermarySkeleton string
---@param secondarySkeleton string
---@param name string Name of the model
---@return Animation newAnimation
function BattleModel:AddAnimation(mountType, TermarySkeleton, secondarySkeleton, name) end

---@param anim Animation
function BattleModel:RemoveAnimation(anim) end

---Writes the entry in a format compatible with the battle_modelds.modeldb and returns it.
---@return string entry
function BattleModel:WriteEntry() end

---Clones the BattleModel entry and returns a new BattleModel.
---@return BattleModel newModel
function BattleModel:CloneEntry(newModel) end

---@class Lod
Lod = {

    ---@type string
    Name = nil,

    ---@type string
    Mesh = nil,

    ---@type integer
    Distance = nil,

    ---@type integer
    Index = nil,

}

---@class Texture
Texture = {

    ---@type string
    Name = nil,

    ---@type boolean
    IsAttach = nil,

    ---@type string
    TexturePath = nil,

    ---@type string
    Normal = nil,

    ---@type string
    STerte = nil,

    ---@type string
    Faction = nil,
}

---@class Animation
Animation = {

    ---@type string
    Name = nil,

    ---@type string
    MountType = nil,

    ---@type string
    TermarySkeleton = nil,

    ---@type string
    SecondarySkeleton = nil,

    ---@type integer
    TerWeaponCount = nil,

    ---@type table<integer, string>
    TerWeapons = nil,

    ---@type string
    TerWeaponOne = nil,

    ---@type string
    TerWeaponTwo = nil,

    ---@type integer
    SecWeaponCount = nil,

    ---@type table<integer, string>
    SecWeapons = nil,

    ---@type string
    SecWeaponOne = nil,

    ---@type string
    SecWeaponTwo = nil,

}

---@class ModelUsage
ModelUsage = {

    ---@type table<integer, Unit>
    Units = nil,

    ---@type table<integer, string>
    DescrStratLines = nil,

    ---@type table<integer, string>
    CampaignScriptLines = nil,

    ---@type table<integer, string>
    CharacterEntries = nil,
}

---Checks if the model is used in the game files.
---@return boolean used
function ModelUsage:IsUsed() end

---Generates a string that represents the usage of the model.
---@return string usage
function ModelUsage:GetUsageString() end

---@class UnitDb
UnitDb = {

    ---@type table<integer, string>
    AttributeTypes = nil,
}

---Adds a new unit to the Units DB.
---@param unit Unit
function UnitDb:Add(unit) end

---Checks if a unit with the given name exists in the game.
---@param name string
---@return boolean contains
function UnitDb:Contains(name) end

---Retrieves a unit from the Units dictionary based on the unit type.
---@param name string
---@return Unit|nil unit
function UnitDb:Get(name) end

---Retrieves a unit from the Units dictionary based on its index.
---@param index integer
---@return Unit|nil unit
function UnitDb:GetByIndex(index) end

---Retrieves the total count of units in the Units dictionary.
---@return integer num
function UnitDb:GetUnitCount() end

---Imports a JSON file and deserializes it into a dictionary of units.
---@param file string
function UnitDb:ImportJson(file) end

---Exports the Units dictionary to a JSON file.
function UnitDb:ExportJson() end

---Retrieves a list of the names of all units in the Units dictionary.
---@return table<integer, string> names
function UnitDb:GetNames() end

---Removes a unit from the Units dictionary.
---@param name string
function UnitDb:Remove(name) end

---Writes the current state of the Units database to files.
function UnitDb:WriteFile() end

---Parses the game files to populate the Units dictionary.
function UnitDb:ParseFile() end

---@class Unit
Unit = {

    ---@type string
    LocalizedName = nil,

    ---@type string
    Type = nil,

    ---@type string
    Dictionary = nil,

    ---@type string
    Category = nil,

    ---@type string
    ClassType = nil,

    ---@type string
    VoiceType = nil,

    ---@type string
    Accent = nil,

    ---@type string
    BannerFaction = nil,

    ---@type string
    BannerUnit = nil,

    ---@type string
    BannerMain = nil,

    ---@type string
    BannerSecondary = nil,

    ---@type string
    BannerHoly = nil,

    ---@type string
    Soldier = nil,

    ---@type integer
    SoldierCount = nil,

    ---@type integer
    ExtrasCount = nil,

    ---@type number
    Mass = nil,

    ---@type number
    Radius = nil,

    ---@type number
    Height = nil,

    ---@type string
    Officer1 = nil,

    ---@type string
    Officer2 = nil,

    ---@type string
    Officer3 = nil,

    ---@type string
    MountedEngine = nil,

    ---@type string
    Mount = nil,

    ---@type string
    Ship = nil,

    ---@type string
    Engine = nil,

    ---@type string
    Animal = nil,

    ---@type table<integer, string>
    MountEffect = nil,

    ---@type table<integer, string>
    Attributes = nil,

    ---@type number
    SpacingWidth = nil,

    ---@type number
    SpacingDepth = nil,

    ---@type number
    SpacingWidthLoose = nil,

    ---@type number
    SpacingDepthLoose = nil,

    ---@type string
    SpecialFormation = nil,

    ---@type integer
    HitPoints = nil,

    ---@type integer
    MountHitPoints = nil,

    ---@type integer
    PriAttack = nil,

    ---@type integer
    PriCharge = nil,

    ---@type integer
    PriProjectile = nil,

    ---@type integer
    PriRange = nil,

    ---@type integer
    PriAmmunition = nil,

    ---@type string
    PriWeaponType = nil,

    ---@type string
    PriTechType = nil,

    ---@type string
    PriDamageType = nil,

    ---@type string
    PriSoundType = nil,

    ---@type integer
    PriAttDelay = nil,

    ---@type number
    PriSkelFactor = nil,

    ---@type table<integer, string>
    PriAttr = nil,

    ---@type integer
    SecAttack = nil,

    ---@type integer
    SecCharge = nil,

    ---@type integer
    SecProjectile = nil,

    ---@type integer
    SecRange = nil,

    ---@type integer
    SecAmmunition = nil,

    ---@type string
    SecWeaponType = nil,

    ---@type string
    SecTechType = nil,

    ---@type string
    SecDamageType = nil,

    ---@type string
    SecSoundType = nil,

    ---@type integer
    SecAttDelay = nil,

    ---@type number
    SecSkelFactor = nil,

    ---@type table<integer, string>
    SecAttr = nil,

    ---@type integer
    TerAttack = nil,

    ---@type integer
    TerCharge = nil,

    ---@type integer
    TerProjectile = nil,

    ---@type integer
    TerRange = nil,

    ---@type integer
    TerAmmunition = nil,

    ---@type string
    TerWeaponType = nil,

    ---@type string
    TerTechType = nil,

    ---@type string
    TerDamageType = nil,

    ---@type string
    TerSoundType = nil,

    ---@type integer
    TerAttDelay = nil,

    ---@type number
    TerSkelFactor = nil,

    ---@type table<integer, string>
    TerAttr = nil,

    ---@type string
    FormationStyle = nil,

    ---@type integer
    PriArmour = nil,

    ---@type integer
    PriDefense = nil,

    ---@type integer
    PriShield = nil,

    ---@type string
    PriDefSound = nil,

    ---@type integer
    SecArmour = nil,

    ---@type integer
    SecDefense = nil,

    ---@type string
    SecDefSound = nil,

    ---@type integer
    StatHeat = nil,

    ---@type integer
    StatScrub = nil,

    ---@type integer
    StatForest = nil,

    ---@type integer
    StatSnow = nil,

    ---@type integer
    StatSand = nil,

    ---@type integer
    Morale = nil,

    ---@type string
    Discipline = nil,

    ---@type string
    Training = nil,

    ---@type integer
    StatChargeDist = nil,

    ---@type integer
    StatFireDelay = nil,

    ---@type integer
    StatFood = nil,

    ---@type integer
    StatFoodSec = nil,

    ---@type integer
    RecruitTime = nil,

    ---@type integer
    RecruitCost = nil,

    ---@type integer
    Upkeep = nil,

    ---@type integer
    WpnCost = nil,

    ---@type integer
    ArmourCost = nil,

    ---@type integer
    CustomCost = nil,

    ---@type integer
    CustomLimit = nil,

    ---@type integer
    CustomIncrease = nil,

    ---@type number
    MoveSpeed = nil,

    ---@type integer
    StatStl = nil,

    ---@type string
    ArmourlvlBase = nil,

    ---@type string
    ArmourlvlOne = nil,

    ---@type string
    ArmourlvlTwo = nil,

    ---@type string
    ArmourlvlThree = nil,

    ---@type string
    ArmourModelBase = nil,

    ---@type string
    ArmourModelOne = nil,

    ---@type string
    ArmourModelTwo = nil,

    ---@type string
    ArmourModelThree = nil,

    ---@type table<integer, string>
    Ownership = nil,

    ---@type table<integer, string>
    EraZero = nil,

    ---@type table<integer, string>
    EraOne = nil,

    ---@type table<integer, string>
    EraTwo = nil,

    ---@type number
    RecruitPriorityOffset = nil,

    ---@type string
    InfoDict = nil,

    ---@type string
    CardDict = nil,

    ---@type number
    CrusadeUpkeep = nil,

    ---@type integer
    SpacingRanks = nil,

    ---@type boolean
    LockMorale = nil,

    ---@type string
    PriFireType = nil,

    ---@type string
    SecFireType = nil,

    ---@type string
    TerFireType = nil,

    ---@type string
    Descr = nil,

    ---@type string
    DescrShort = nil,

    ---@type string
    Card = nil,

    ---@type boolean
    MercenaryUnit = nil,

    ---@type boolean
    IsEopUnit = nil,

    ---@type boolean
    FilePath = nil,

    ---@type integer
    EduIndex = nil,

    ---@type number
    AiUnitValue = nil,

    ---@type number
    ValuePerCost = nil,

    ---@type number
    ValuePerUpkeep = nil,

    ---@type string
    CardInfo = nil,

    ---@type string
    FactionSymbol = nil,

}

---Add a mount effect
---@param effect string
function Unit:AddMountEffect(effect) end

---Remove a mount effect
---@param effect string
function Unit:RemoveMountEffect(effect) end

---Add an attribute
---@param attr string
function Unit:AddAttribute(attr) end

---Remove an attribute
---@param attr string
function Unit:RemoveAttribute(attr) end

---Add an attribute
---@param attr string
function Unit:AddPriAttribute(attr) end

---Remove an attribute
---@param attr string
function Unit:RemovePriAttribute(attr) end

---Add an attribute
---@param attr string
function Unit:AddSecAttribute(attr) end

---Remove an attribute
---@param attr string
function Unit:RemoveSecAttribute(attr) end

---Add an attribute
---@param attr string
function Unit:AddTerAttribute(attr) end

---Remove an attribute
---@param attr string
function Unit:RemoveTerAttribute(attr) end

---Add an ownership faction
---@param fac string
function Unit:AddOwnership(fac) end

---Remove an ownership faction
---@param fac string
function Unit:RemoveOwnership(fac) end

---Add an era faction
---@param fac string
function Unit:AddEraZero(fac) end

---Remove an era faction
---@param fac string
function Unit:RemoveEraZero(fac) end

---Add an era faction
---@param fac string
function Unit:AddEraOne(fac) end

---Remove an era faction
---@param fac string
function Unit:RemoveEraOne(fac) end

---Add an era faction
---@param fac string
function Unit:AddEraTwo(fac) end

---Remove an era faction
---@param fac string
function Unit:RemoveEraTwo(fac) end

---Check if unit uses a model
---@param model string
---@return boolean contains
function Unit:ContainsModel(model) end

---@param oldModel string
---@param newModel string
function Unit:UpdateModelUsage(oldModel, newModel) end

---Writes the entry in a format compatible with the export_descr_unit.txt and returns it.
---@return string entry
function Unit:WriteEntry() end

---@return number value
function Unit:CalculateUnitValue() end

---@return table<integer, string> cards
function Unit:GetCards() end

---@class BuildingDb
BuildingDb = {

}

---Adds a new building to the Buildings DB.
---@param building Building
function BuildingDb:Add(building) end

---Checks if a building with the given name exists in the game.
---@param name string
---@return boolean contains
function BuildingDb:Contains(name) end

---Retrieves a Building from the Building dictionary based on the Building name.
---@param name string
---@return Building|nil building
function BuildingDb:Get(name) end

---Retrieves a building from the Buildings dictionary based on its index.
---@param index integer
---@return Building|nil building
function BuildingDb:GetByIndex(index) end

---Retrieves the total count of buildings in the Buildings dictionary.
---@return integer num
function BuildingDb:GetCount() end

---Imports a JSON file and deserializes it into a dictionary of buildings.
---@param file string
function BuildingDb:ImportJson(file) end

---Exports the Building dictionary to a JSON file.
function BuildingDb:ExportJson() end

---Retrieves a list of the names of all buildings in the Buildings dictionary.
---@return table<integer, string> names
function BuildingDb:GetNames() end

---Removes a building from the Buildings dictionary.
---@param name string
function BuildingDb:Remove(name) end

function BuildingDb:WriteFile() end

---Parses the game files to populate the buildings dictionary.
function BuildingDb:ParseFiles() end

---@class Building
Building = {

    ---@type string
    Name = nil,

    ---@type string
    Religion = nil,

    ---@type string
    Classification = nil,

    ---@type string
    ConvertTo = nil,

    ---@type boolean
    IsGuild = nil,

    ---@type boolean
    IsConvert = nil,

    ---@type boolean
    IsHinterland = nil,

    ---@type boolean
    IsTemple = nil,

    ---@type boolean
    IsFarms = nil,

    ---@type table<integer, string>
    LevelNames = nil,

    ---@type table<integer, BuildingLevel>
    Levels = nil,

    ---@type integer
    LevelCount = nil,

    ---@type table<integer, Plugin>
    Plugins = nil,

    ---@type table<integer, string>
    Factions = nil,

    ---@type table<integer, string>
    Cultures = nil,

}

---Add a faction
---@param name string
function Building:AddFaction(name) end

---Add a culture
---@param name string
function Building:AddCulture(name) end

---Remove a faction
---@param name string
function Building:RemoveFaction(name) end

---Remove a culture
---@param name string
function Building:RemoveCulture(name) end

---Add a building level
---@param level BuildingLevel
function Building:AddBuildingLevel(level) end

---Add a plugin
---@param plugin Plugin
function Building:AddPlugin(plugin) end

---Remove a building level
---@param level BuildingLevel
function Building:RemoveBuildingLevel(level) end

---Remove a plugin
---@param plugin Plugin
function Building:RemovePlugin(plugin) end

---Get a building level
---@param index integer
---@return BuildingLevel level
function Building:GetBuildingLevel(index) end

---Get a plugin
---@param index integer
---@return Plugin plugin
function Building:GetPlugin(index) end

---@class BuildingLevel
BuildingLevel = {

    ---@type string
    Name = nil,

    ---@type boolean
    AvailableCity = nil,

    ---@type boolean
    AvailableCastle = nil,

    ---@type string
    Condition = nil,

    ---@type string
    ConvertTo = nil,

    ---@type string
    Material = nil,

    ---@type integer
    ConstructionTime = nil,

    ---@type integer
    ConstructionCost = nil,

    ---@type string
    SettlementMinLevel = nil,

    ---@type table<integer, Capability>
    Capabilities = nil,

    ---@type table<integer, Capability>
    FactionCapabilities = nil,

    ---@type table<integer, BuildingUpgrade>
    Upgrades = nil,

    ---@type integer
    CapabilityCount = nil,

    ---@type integer
    FactionCapabilityCount = nil,

    ---@type integer
    UpgradeCount = nil,

}

---Add a capability
---@param cap Capability
function BuildingLevel:AddCapability(cap) end

---Add a faction capability
---@param cap Capability
function BuildingLevel:AddFactionCapability(cap) end

---Add an upgrade
---@param upg BuildingUpgrade
function BuildingLevel:AddUpgrade(upg) end

---Add a capability
---@param type string
---@return Capability cap
function BuildingLevel:CreateCapability(type) end

---Add a faction capability
---@param type string
---@return Capability cap
function BuildingLevel:CreateFactionCapability(type) end

---Add an upgrade
---@param name string
---@param condition string
---@return BuildingUpgrade upg
function BuildingLevel:CreateUpgrade(name, condition) end

---Remove a capability
---@param cap Capability
function BuildingLevel:RemoveCapability(cap) end

---Remove a faction capability
---@param cap Capability
function BuildingLevel:RemoveFactionCapability(cap) end

---Remove an upgrade
---@param upg BuildingUpgrade
function BuildingLevel:RemoveUpgrade(upg) end

---@class Capability
Capability = {

    ---@type string
    Type = nil,

    ---@type string
    Condition = nil,

    ---@type integer
    Value = nil,

    ---@type boolean
    Bonus = nil,

    ---@type string
    AgentType = nil,

    ---@type string
    Unit = nil,

    ---@type number
    InitialPool = nil,

    ---@type number
    ReplenishmentRate = nil,

    ---@type number
    MaximumPool = nil,

    ---@type string
    InitialPoolString = nil,

    ---@type string
    ReplenishmentRateString = nil,

    ---@type string
    MaximumPoolString = nil,

    ---@type integer
    StartingExperience = nil,

}

---@class BuildingUpgrade
BuildingUpgrade = {

    ---@type string
    Name = nil,
    ---@type string
    Condition = nil,

}

---@class Plugin
Plugin = {

}
