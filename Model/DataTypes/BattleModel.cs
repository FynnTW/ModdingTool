using System.Collections.Generic;
using System.Linq;
using static ModdingTool.Globals;
// ReSharper disable RedundantDefaultMemberInitializer

namespace ModdingTool;

public class BattleModel : GameType
{
    #region Strings
    /**
     * <summary>
     * List of all mount types compatible in the battle_models.modeldb.
     * </summary>
     */
    public static IEnumerable<string> MountTypes { get; } =
        new List<string>{
            "horse", 
            "none", 
            "elephant", 
            "camel"
        };
    #endregion
    
    #region Properties
    /**
     * <summary>
     * Name of the entry.
     * </summary>
     */
    public string Name
    {
        get => _name;
        set {
            var old = _name;
            if (ModData.BattleModelDb.Contains(value))
                ErrorDb.AddError("Model name already exists in the database. " + value + " is not unique.");
            else
            {
                _name = value;
                if (string.IsNullOrWhiteSpace(old) || !ModData.BattleModelDb.Contains(old)) return;
                AddChange(nameof(Name), old, value);
                ModData.BattleModelDb.Remove(old);
                ModData.BattleModelDb.Add(this);
            }
        } 
    }
    
    /**
     * <summary>
     * Scale of the model.
     * </summary>
     */
    public float Scale
    {
        get => _scale;
        set
        {
            if (value < 0)
                ErrorDb.AddError("Scale must be positive. " + value + " is not valid.");
            else
            {
                AddChange(nameof(Scale), _scale, value);
                _scale = value;
            }
        }
    }
    
    public int LodCount
    {
        get => !IsParsing ? LodTable.Count : _lodCount;
        set => _lodCount = value;
    }
    public List<Lod> LodTable { get; set; } = new();
    public int MainTexturesCount
    {
        get => !IsParsing ? MainTextures.Count : _mainTexturesCount;
        set => _mainTexturesCount = value;
    }
    public List<Texture> MainTextures { get; private init; } = new();
    public int AttachTexturesCount
    {
        get => !IsParsing ? AttachTextures.Count : _attachTexturesCount;
        set => _attachTexturesCount = value;
    }
    public List<Texture> AttachTextures { get; private init; } = new ();
    public int MountTypeCount
    {
        get => !IsParsing ? Animations.Count : _mountTypeCount;
        set => _mountTypeCount = value;
    }
    public List<Animation> Animations { get; set; } = new ();
    
    /// <summary>
    /// Gets or sets the TorchIndex of the BattleModel.
    /// </summary>
    /// <value>
    /// The TorchIndex of the BattleModel.
    /// </value>
    /// <remarks>
    /// When setting this property, it logs the change using the AddChange method.
    /// </remarks>
    public int TorchIndex
    {
        get => _torchIndex;
        set
        {
            AddChange(nameof(TorchIndex), _torchIndex, value);
            _torchIndex = value;
        }
    }

    public float TorchBoneX
    {
        get => _torchBoneX;
        set
        {
            AddChange(nameof(TorchBoneX), _torchBoneX, value);
            _torchBoneX = value;
        }
    }

    public float TorchBoneY
    {
        get => _torchBoneY;
        set
        {
            AddChange(nameof(TorchBoneY), _torchBoneY, value);
            _torchBoneY = value;
        }
    }

    public float TorchBoneZ
    {
        get => _torchBoneZ;
        set
        {
            AddChange(nameof(TorchBoneZ), _torchBoneZ, value);
            _torchBoneZ = value;
        }
    }

    public float TorchSpriteX
    {
        get => _torchSpriteX;
        set
        {
            AddChange(nameof(TorchSpriteX), _torchSpriteX, value);
            _torchSpriteX = value;
        }
    }

    public float TorchSpriteY
    {
        get => _torchSpriteY;
        set
        {
            AddChange(nameof(TorchSpriteY), _torchSpriteY, value);
            _torchSpriteY = value;
        }
    }

    public float TorchSpriteZ
    { 
        get => _torchSpriteZ;
        set
        {
            AddChange(nameof(TorchSpriteZ), _torchSpriteZ, value);
            _torchSpriteZ = value;
        }
    }
    #endregion Properties

    #region Fields
    private float _scale = 1;
    private int _torchIndex = -1;
    private float _torchBoneX= 0;
    private float _torchBoneY = 0;
    private float _torchBoneZ  = 0;
    private float _torchSpriteX = 0;
    private float _torchSpriteY = 0;
    private float _torchSpriteZ = 0;
    private int _lodCount;
    private int _mainTexturesCount;
    private int _attachTexturesCount;
    private int _mountTypeCount;

    #endregion

    #region Methods
    /**
     * <summary>
     * Writes the entry in a format compatible with the battle_modelds.modeldb and returns it.
     * </summary>
     * <returns>Entry as string</returns>
     */
    public string WriteEntry()
    {
        var entry = "";
        entry += NumString(Name) + "\n";
        entry += FormatFloat(Scale) + " " + LodTable.Count + "\n";
        entry = LodTable.Aggregate(entry, (current, lod) => current + NumString(lod.Mesh) + " " + lod.Distance + "\n");
        entry += MainTextures.Count + "\n";
        foreach (var texture in MainTextures)
        {
            entry += NumString(texture.Faction) + "\n";
            entry += NumString(texture.TexturePath) + "\n";
            entry += NumString(texture.Normal) + "\n";
            entry += NumString(texture.Sprite) + "\n";
        }
        entry += AttachTextures.Count + "\n";
        foreach (var texture in AttachTextures)
        {
            entry += NumString(texture.Faction) + "\n";
            entry += NumString(texture.TexturePath) + "\n";
            entry += NumString(texture.Normal);
            if (!string.IsNullOrWhiteSpace(texture.Sprite) && texture.Sprite != "0")
                entry += "\n";
            else
                entry += " ";
            entry += NumString(texture.Sprite) + "\n";
        }
        entry += Animations.Count + "\n";
        foreach (var animation in Animations)
        {
            animation.PriWeapons = new List<string>();
            if (!string.IsNullOrWhiteSpace(animation.PriWeaponOne))
            {
                animation.PriWeapons.Add(animation.PriWeaponOne);
            }

            if (!string.IsNullOrWhiteSpace(animation.PriWeaponTwo))
            {
                animation.PriWeapons.Add(animation.PriWeaponTwo);
            }
            animation.SecWeapons = new List<string>();
            if (!string.IsNullOrWhiteSpace(animation.SecWeaponOne))
            {
                animation.SecWeapons.Add(animation.SecWeaponOne);
            }

            if (!string.IsNullOrWhiteSpace(animation.SecWeaponTwo))
            {
                animation.SecWeapons.Add(animation.SecWeaponTwo);
            }
            entry += NumString(animation.MountType) + "\n";
            entry += NumString(animation.PrimarySkeleton) + " ";
            entry += NumString(animation.SecondarySkeleton) + "\n";
            entry += animation.PriWeapons.Count;
            if (animation.PriWeapons.Count > 0)
            {
                entry += "\n";
            }
            else
            {
                entry += " ";
            }
            for (var i = 0; i < animation.PriWeapons.Count; i++)
            {
                entry += i switch
                {
                    0 => NumString(animation.PriWeaponOne) + "\n",
                    1 => NumString(animation.PriWeaponTwo) + "\n",
                    _ => NumString(animation.PriWeapons[i]) + "\n"
                };
            }

            entry += animation.SecWeapons.Count;
            if (animation.SecWeapons.Count > 0)
            {
                entry += "\n";
            }
            else
            {
                entry += " ";
            }
            for (var i = 0; i < animation.SecWeapons.Count; i++)
            {
                entry += i switch
                {
                    0 => NumString(animation.SecWeaponOne) + "\n",
                    1 => NumString(animation.SecWeaponTwo) + "\n",
                    _ => NumString(animation.SecWeapons[i]) + "\n"
                };
            }
        }
        entry += TorchIndex + " ";
        entry += FormatFloat(TorchBoneX) + " " + FormatFloat(TorchBoneY) + " " + FormatFloat(TorchBoneZ) + " ";
        entry += FormatFloat(TorchSpriteX) + " " + FormatFloat(TorchSpriteY) + " " + FormatFloat(TorchSpriteZ) + "\n";

        return entry;
    }

    /**
     * <summary>
     * Returns the length of the input string and the string itself.
     * </summary>
     * <param name="input">String to be counted</param>
     * <returns>Length of the string and the string itself</returns>
     */
    private static string NumString(string input) 
        => string.IsNullOrWhiteSpace(input) || input.Length < 1 ? "0" : input.Length + " " + input;
    
    /**
     * <summary>
     * Creates a new BattleModel with the given name and the same properties as the old model.
     * </summary>
     * <param name="name">New name of the entry</param>
     * <param name="oldModel">Old entry to get the data from</param>
     * <returns>The new BattleModel</returns>
     */
    public static BattleModel? CloneModel(string name, BattleModel oldModel)
    {
        if (ModData.BattleModelDb.Contains(name))
        {
            ErrorDb.AddError("Model name already exists in the database. " + name + " is not unique.");
            return null;
        }
        var newModel = new BattleModel
        {
            Name = name,
            Scale = oldModel.Scale,
            LodCount = oldModel.LodCount,
            LodTable = new List<Lod>(),
            MainTexturesCount = oldModel.MainTexturesCount,
            MainTextures = new List<Texture>(),
            AttachTexturesCount = oldModel.AttachTexturesCount,
            AttachTextures = new List<Texture>(),
            MountTypeCount = oldModel.MountTypeCount,
            Animations = new List<Animation>(),
            TorchIndex = oldModel.TorchIndex,
            TorchBoneX = oldModel.TorchBoneX,
            TorchBoneY = oldModel.TorchBoneY,
            TorchBoneZ = oldModel.TorchBoneZ,
            TorchSpriteX = oldModel.TorchSpriteX,
            TorchSpriteY = oldModel.TorchSpriteY,
            TorchSpriteZ = oldModel.TorchSpriteZ
        };
        foreach (var lod in oldModel.LodTable)
        {
            newModel.LodTable.Add(new Lod
            {
                Mesh = lod.Mesh,
                Distance = lod.Distance,
                Name = name
            });
        }
        foreach (var texture in oldModel.MainTextures)
        {
            newModel.MainTextures.Add(new Texture
            {
                Faction = texture.Faction,
                TexturePath = texture.TexturePath,
                Normal = texture.Normal,
                Sprite = texture.Sprite,
                Name = name
            });
        }
        foreach (var texture in oldModel.AttachTextures)
        {
            newModel.AttachTextures.Add(new Texture
            {
                Faction = texture.Faction,
                TexturePath = texture.TexturePath,
                Normal = texture.Normal,
                Sprite = texture.Sprite,
                Name = name
            });
        }
        foreach (var animation in oldModel.Animations)
        {
            newModel.Animations.Add(new Animation
            {
                MountType = animation.MountType,
                PrimarySkeleton = animation.PrimarySkeleton,
                SecondarySkeleton = animation.SecondarySkeleton,
                PriWeaponCount = animation.PriWeaponCount,
                PriWeapons = new List<string>(),
                SecWeaponCount = animation.SecWeaponCount,
                SecWeapons = new List<string>(),
                Name = name
            });
        }
        return newModel;
    }
    #endregion
}

public class Lod : GameType
{
    private string _mesh = "";
    private int _distance = 6400;
        
    public string Name { init => _name = value; }

    public string Mesh
    {
        get => _mesh;
        set
        {
            FileExistsData(value, _name);
            AddChange(nameof(Mesh), _mesh, value);
            _mesh = value;
        }
    }

    public int Distance
    {
        get => _distance;
        set
        {
            AddChange(nameof(Distance), _distance, value);
            _distance = value;
        }
    }
}

public class Texture : GameType
{

    private string _faction = string.Empty;
    private string _texturePath = string.Empty;
    private string _normal = string.Empty;
    private string _sprite = string.Empty;
    public string Name { init => _name = value; }

    public string TexturePath
    {
        get => _texturePath;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _texturePath = value;
                return;
            }

            FileExistsData(value, _name);
            AddChange(nameof(TexturePath), _texturePath, value);
            _texturePath = value;
        }
    }

    public string Normal
    {
        get => _normal;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _normal = value;
                return;
            }
            FileExistsData(value, _name);
            AddChange(nameof(Normal), _normal, value);
            _normal = value;
        }
    }

    public string Sprite
    {
        get => _sprite;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _sprite = value;
                return;
            }
            FileExistsData(value, _name);
            AddChange(nameof(Sprite), _sprite, value);
            _sprite = value;
        }
    }
    public string Faction
    {
        get => _faction;
        set
        {
            if (!FactionDataBase.ContainsKey(value) && value != "merc")
                ErrorDb.AddError("Faction " + value + " does not exist.");
            AddChange(nameof(Faction), _faction, value);
            _faction = value;
        }
    }
}

public class Animation : GameType
{
    private string _mountType = "none";
    private string _primarySkeleton = string.Empty;
    private string _secondarySkeleton = string.Empty;
    private readonly int _priWeaponCount;
    private List<string> _priWeapons = new();
    private int _secWeaponCount;
    private List<string> _secWeapons = new();
    public string Name  { init => _name = value; }

    public string MountType
    {
        get => _mountType;
        set
        {
            if (!BattleModel.MountTypes.Contains(value))
                ErrorDb.AddError("Mount type " + value + " does not exist.");
            AddChange(nameof(MountType), _mountType, value);
            _mountType = value;
        }
    }

    public string PrimarySkeleton
    {
        get => _primarySkeleton;
        set
        {
            AddChange(nameof(PrimarySkeleton), _primarySkeleton, value);
            _primarySkeleton = value;
        }
    }

    public string SecondarySkeleton
    {
        get => _secondarySkeleton;
        set
        {
            AddChange(nameof(SecondarySkeleton), _secondarySkeleton, value);
            _secondarySkeleton = value;
        }
    }

    public int PriWeaponCount
    {
        get =>!IsParsing ? _priWeapons.Count(x => !string.IsNullOrWhiteSpace(x)) : _priWeaponCount;
        init => _priWeaponCount = value;
    }

    public List<string> PriWeapons
    {
        get => _priWeapons; 
        set => _priWeapons = value;
    }

    public string PriWeaponOne
    {
        get => _priWeapons.Count > 0 ? _priWeapons[0] : "";
        set
        { 
            AddChange(nameof(PriWeaponOne), _priWeapons[0], value);
            _priWeapons[0] = value;
        } 
    }

    public string PriWeaponTwo
    {
        get => _priWeapons.Count > 1 ? _priWeapons[1] : "";
        set
        {
            AddChange(nameof(PriWeaponTwo), _priWeapons[1], value);
            _priWeapons[1] = value;
        }
    }

    public string SecWeaponOne
    {
        get => _secWeapons.Count > 0 ? _secWeapons[0] : "";
        set
        {
            AddChange(nameof(SecWeaponOne), _secWeapons[0], value);
            _secWeapons[0] = value;
        }
    }

    public string SecWeaponTwo
    {
        get => _secWeapons.Count > 1 ? _secWeapons[1] : "";
        set
        {
            AddChange(nameof(SecWeaponTwo), _secWeapons[1], value);
            _secWeapons[1] = value;
        }
    }
        
    public int SecWeaponCount
    {
        get =>!IsParsing ? _secWeapons.Count(x => !string.IsNullOrWhiteSpace(x)) : _secWeaponCount;
        set => _secWeaponCount = value;
    }
    public List<string> SecWeapons
    {
        get => _secWeapons; 
        set => _secWeapons = value;
    }
}