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
    
    public ModelUsage ModelUsage { get; set; } = new();

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
                ModData.BattleModelDb.Add(this);
                var oldModel = ModData.BattleModelDb.Get(old);
                if (oldModel != null)
                {
                    oldModel.ModelUsage.Units.Clear();
                    oldModel.ModelUsage.Mounts.Clear();
                    oldModel.ModelUsage.CharacterEntries.Clear();
                }
                ModData.BattleModelDb.Remove(old);
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the number of LODs in the battle model.
    /// </summary>
    /// <remarks>
    /// The LOD count determines the number of levels of detail (LODs) that the battle model has.
    /// An LOD is a version of the model that is less complex and detailed, used for performance optimization at different distances from the camera.
    /// </remarks>
    public int LodCount
    {
        get => !IsParsing ? LodTable.Count : _lodCount;
        set => _lodCount = value;
    }

    /// <summary>
    /// List of meshes and distances for each level of detail (LOD) in the battle model.
    /// </summary>
    public List<Lod> LodTable { get; set; } = new();

    /// <summary>
    /// Adds a new level of detail (LOD) to the BattleModel. If the LOD already exists, updates the mesh and distance.
    /// </summary>
    /// <param name="mesh">The mesh associated with the LOD.</param>
    /// <param name="distance">The distance at which the LOD should be displayed.</param>
    /// <param name="index">The index of the LOD in the LOD table. For example 0 would change the lod0 etc</param>
    /// <param name="name">Name of the model.</param>
    public void AddLod(string mesh, int distance, int index, string name)
    {
        if (LodTable.Count > index)
        {
            LodTable[index].Mesh = mesh;
            LodTable[index].Distance = distance;
            return;
        }
        LodTable.Add(new Lod
        {
            Mesh = mesh,
            Distance = distance,
            Name = name
        });
    }

    /// <summary>
    /// Gets or sets the count of main textures in the battle model.
    /// </summary>
    public int MainTexturesCount
    {
        get => !IsParsing ? MainTextures.Count : _mainTexturesCount;
        set => _mainTexturesCount = value;
    }

    /// <summary>
    /// Represents the main textures of a BattleModel.
    /// </summary>
    public List<Texture> MainTextures { get; private init; } = new();

    /// <summary>
    /// Adds a main texture to the BattleModel. If the texture already exists, updates the texture path, normal map, and sprite.
    /// </summary>
    /// <param name="faction">The faction associated with the texture.</param>
    /// <param name="texturePath">The path of the texture.</param>
    /// <param name="normal">The normal map of the texture.</param>
    /// <param name="sprite">The sprite associated with the texture.</param>
    /// <param name="name">Name of the model.</param>
    /// <returns>The added or updated Texture object.</returns>
    public Texture AddMainTexture(string faction, string texturePath, string normal, string sprite, string name)
    {
        var texture = MainTextures.Find(fac => fac.Faction == faction);
        if (texture != null)
        {
            texture.TexturePath = texturePath;
            texture.Normal = normal;
            texture.Sprite = sprite;
            return texture;
        }
        MainTextures.Add(new Texture
        {
            Faction = faction,
            TexturePath = texturePath,
            Normal = normal,
            Sprite = sprite,
            Name = name,
            IsAttach = false
        });
        return MainTextures.Last();
    }
    
    /// <summary>
    /// Gets or sets the count of attached textures in the battle model.
    /// </summary>
    public int AttachTexturesCount
    {
        get => !IsParsing ? AttachTextures.Count : _attachTexturesCount;
        set => _attachTexturesCount = value;
    }
    
    /// <summary>
    /// Represents the attached textures of a BattleModel.
    /// </summary>
    public List<Texture> AttachTextures { get; private init; } = new ();

    /// <summary>
    /// Adds an attached texture to the BattleModel. If the texture already exists, updates the texture path, normal map, and sprite.
    /// </summary>
    /// <param name="faction">The faction associated with the texture.</param>
    /// <param name="texturePath">The path of the texture.</param>
    /// <param name="normal">The normal map of the texture.</param>
    /// <param name="sprite">The sprite associated with the texture.</param>
    /// <param name="name">Name of the model</param>
    /// <returns>The added or updated Texture object.</returns>
    public Texture AddAttachTexture(string faction, string texturePath, string normal, string sprite, string name)
    {
        var texture = AttachTextures.Find(fac => fac.Faction == faction);
        if (texture != null)
        {
            texture.TexturePath = texturePath;
            texture.Normal = normal;
            texture.Sprite = sprite;
            return texture;
        }
        AttachTextures.Add(new Texture
        {
            Faction = faction,
            TexturePath = texturePath,
            Normal = normal,
            Sprite = sprite,
            Name = Name,
            IsAttach = true
        });
        return AttachTextures.Last();
    }
    
    /// <summary>
    /// Gets or sets the count of mount types in the battle model.
    /// </summary>
    public int MountTypeCount
    {
        get => !IsParsing ? Animations.Count : _mountTypeCount;
        set => _mountTypeCount = value;
    }

    /// <summary>
    /// Represents the animations of a BattleModel.
    /// </summary>
    public List<Animation> Animations { get; set; } = new ();

    /// <summary>
    /// Adds an animation to the BattleModel. If the entry for the provided mount type already exists, updates the primary and secondary skeleton.
    /// </summary>
    /// <param name="mountType">The mount type associated with the animation.</param>
    /// <param name="primarySkeleton">The primary skeleton of the animation.</param>
    /// <param name="secondarySkeleton">The secondary skeleton of the animation.</param>
    /// <param name="name">Name of the model.</param>
    /// <returns>The added or updated Animation object.</returns>
    public Animation AddAnimation(string mountType, string primarySkeleton, string secondarySkeleton, string name)
    {
        var anim = Animations.Find(mount => mount.MountType == mountType);
        if (anim != null)
        {
            anim.PrimarySkeleton = primarySkeleton;
            anim.SecondarySkeleton = secondarySkeleton;
            return anim;
        }
        Animations.Add(new Animation
        {
            MountType = mountType,
            PrimarySkeleton = primarySkeleton,
            SecondarySkeleton = secondarySkeleton,
            Name = Name
        });
        return Animations.Last();
    }
    
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
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the X coordinate of the torch bone in the BattleModel.
    /// </summary>
    /// <remarks>
    /// When setting this property, it logs the change using the AddChange method.
    /// </remarks>
    public float TorchBoneX
    {
        get => _torchBoneX;
        set
        {
            AddChange(nameof(TorchBoneX), _torchBoneX, value);
            _torchBoneX = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the Y coordinate of the torch bone in the BattleModel.
    /// </summary>
    /// <remarks>
    /// When setting this property, it logs the change using the AddChange method.
    /// </remarks>
    public float TorchBoneY
    {
        get => _torchBoneY;
        set
        {
            AddChange(nameof(TorchBoneY), _torchBoneY, value);
            _torchBoneY = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the Z coordinate of the torch bone in the BattleModel.
    /// </summary>
    /// <remarks>
    /// When setting this property, it logs the change using the AddChange method.
    /// </remarks>
    public float TorchBoneZ
    {
        get => _torchBoneZ;
        set
        {
            AddChange(nameof(TorchBoneZ), _torchBoneZ, value);
            _torchBoneZ = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the X coordinate of the torch sprite in the BattleModel.
    /// </summary>
    /// <remarks>
    /// When setting this property, it logs the change using the AddChange method.
    /// </remarks>
    public float TorchSpriteX
    {
        get => _torchSpriteX;
        set
        {
            AddChange(nameof(TorchSpriteX), _torchSpriteX, value);
            _torchSpriteX = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the Y coordinate of the torch sprite in the BattleModel.
    /// </summary>
    /// <remarks>
    /// When setting this property, it logs the change using the AddChange method.
    /// </remarks>
    public float TorchSpriteY
    {
        get => _torchSpriteY;
        set
        {
            AddChange(nameof(TorchSpriteY), _torchSpriteY, value);
            _torchSpriteY = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the Z coordinate of the torch sprite in the BattleModel.
    /// </summary>
    /// <remarks>
    /// When setting this property, it logs the change using the AddChange method.
    /// </remarks>
    public float TorchSpriteZ
    { 
        get => _torchSpriteZ;
        set
        {
            AddChange(nameof(TorchSpriteZ), _torchSpriteZ, value);
            _torchSpriteZ = value;
            NotifyPropertyChanged();
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

/// <summary>
/// Represents a Level of Detail (LOD) in a BattleModel.
/// LODs are used to optimize rendering by displaying less complex versions of the model at greater distances.
/// Every level has a mesh ending with "lod" + index and a distance at which it should be displayed.
/// </summary>
public class Lod : GameType
{
    private string _mesh = "";
    private int _distance = 6400;

    /// <summary>
    /// Gets or sets the of the battle model that uses this LOD.
    /// </summary>
    public string Name { init => _name = value; }

    /// <summary>
    /// Gets or sets the mesh associated with the Level of Detail (LOD).
    /// </summary>
    /// <remarks>
    /// When setting this property, it checks if the file exists, logs the change using the AddChangeList method,
    /// updates the value of the mesh, and notifies any property change listeners.
    /// </remarks>
    /// <value>
    /// The mesh associated with the LOD.
    /// </value>
    public string Mesh
    {
        get => _mesh;
        set
        {
            FileExistsData(value, _name);
            AddChangeList(nameof(Mesh), _mesh, value, Index.ToString(), "Lod");
            _mesh = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the distance associated with the Level of Detail (LOD).
    /// </summary>
    /// <remarks>
    /// When setting this property, it logs the change using the AddChangeList method,
    /// updates the value of the distance, and notifies any property change listeners.
    /// </remarks>
    /// <value>
    /// The distance associated with the LOD.
    /// </value>
    public int Distance
    {
        get => _distance;
        set
        {
            AddChangeList(nameof(Distance), _distance, value, Index.ToString(), "Lod");
            _distance = value;
            NotifyPropertyChanged();
        }
    }
    
    /// <summary>
    /// Gets or sets the index of the Level of Detail (LOD). Mesh names need to end with "lod" + index.
    /// </summary>
    /// <value>
    /// The index of the LOD.
    /// </value>
    public int Index { get; set; }
}

/// <summary>
/// Represents a texture entry in a BattleModel.
/// </summary>
public class Texture : GameType
{

    private string _faction = string.Empty;
    private string _texturePath = string.Empty;
    private string _normal = string.Empty;
    private string _sprite = string.Empty;
    
    /// <summary>
    /// Gets or sets a value indicating whether the texture is an attachment texture or main texture.
    /// </summary>
    public bool IsAttach { get; init; } = false;
    
    /// <summary>
    /// Sets the name of the model entry this texture is attached to.
    /// </summary>
    public string Name { init => _name = value; }

    
    /// <summary>
    /// Gets or sets the path of the texture.
    /// </summary>
    /// <remarks>
    /// When setting this property, it checks if the value is null or whitespace. If it is, it returns without making any changes.
    /// Otherwise, it calls the FileExistsData method with the value and the current texture path, logs the change using the AddChange method,
    /// updates the value of the texture path, and notifies any property change listeners.
    /// </remarks>
    public string TexturePath
    {
        get => _texturePath;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
               return;
            FileExistsData(value, _texturePath);
            AddChange(nameof(TexturePath), _texturePath, value);
            _texturePath = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the normal map of the texture.
    /// </summary>
    /// <remarks>
    /// When setting this property, it checks if the value is null or whitespace. If it is, it returns without making any changes.
    /// Otherwise, it calls the FileExistsData method with the value and the current normal map, logs the change using the AddChange method,
    /// updates the value of the normal map, and notifies any property change listeners.
    /// </remarks>
    public string Normal
    {
        get => _normal;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            FileExistsData(value, _normal);
            AddChange(nameof(Normal), _normal, value);
            _normal = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the sprite associated with the texture.
    /// </summary>
    /// <remarks>
    /// When setting this property, it checks if the value is null or whitespace. If it is, it returns without making any changes.
    /// Otherwise, it calls the FileExistsData method with the value and the current sprite, logs the change using the AddChange method,
    /// updates the value of the sprite, and notifies any property change listeners.
    /// </remarks>
    public string Sprite
    {
        get => _sprite;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                return; 
            FileExistsData(value, _sprite);
            AddChange(nameof(Sprite), _sprite, value);
            _sprite = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the faction associated with the texture.
    /// </summary>
    /// <remarks>
    /// When setting this property, it checks if the value is null or whitespace. If it is, it returns without making any changes.
    /// It also checks if the faction exists in the FactionDataBase and is not "merc". If it doesn't exist, it adds an error to the ErrorDb.
    /// It then logs the change using the AddChange method, updates the value of the faction, and notifies any property change listeners.
    /// </remarks>
    public string Faction
    {
        get => _faction;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                return; 
            if (!FactionDataBase.ContainsKey(value) && value != "merc")
                ErrorDb.AddError("Faction " + value + " does not exist.");
            AddChange(nameof(Faction), _faction, value);
            _faction = value;
            NotifyPropertyChanged();
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
    
    /// <summary>
    /// Gets or sets the name of the model entry this animation is attached to.
    /// </summary>
    public string Name  { init => _name = value; }

    /// <summary>
    /// Gets or sets the mount type associated with the animation.
    /// </summary>
    /// <remarks>
    /// When setting this property, it checks if the value exists in the BattleModel's MountTypes. If it doesn't exist, it adds an error to the ErrorDb and returns without making any changes.
    /// Otherwise, it logs the change using the AddChange method, updates the value of the mount type, and notifies any property change listeners.
    /// </remarks>
    public string MountType
    {
        get => _mountType;
        set
        {
            if (!BattleModel.MountTypes.Contains(value))
            {
                ErrorDb.AddError("Mount type " + value + " does not exist.");
                return;
            }
            AddChange(nameof(MountType), _mountType, value);
            _mountType = value;
            NotifyPropertyChanged();
        }
    }

    public string PrimarySkeleton
    {
        get => _primarySkeleton;
        set
        {
            AddChange(nameof(PrimarySkeleton), _primarySkeleton, value);
            _primarySkeleton = value;
            NotifyPropertyChanged();
        }
    }

    public string SecondarySkeleton
    {
        get => _secondarySkeleton;
        set
        {
            AddChange(nameof(SecondarySkeleton), _secondarySkeleton, value);
            _secondarySkeleton = value;
            NotifyPropertyChanged();
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
            NotifyPropertyChanged();
        } 
    }

    public string PriWeaponTwo
    {
        get => _priWeapons.Count > 1 ? _priWeapons[1] : "";
        set
        {
            AddChange(nameof(PriWeaponTwo), _priWeapons[1], value);
            _priWeapons[1] = value;
            NotifyPropertyChanged();
        }
    }

    public string SecWeaponOne
    {
        get => _secWeapons.Count > 0 ? _secWeapons[0] : "";
        set
        {
            AddChange(nameof(SecWeaponOne), _secWeapons[0], value);
            _secWeapons[0] = value;
            NotifyPropertyChanged();
        }
    }

    public string SecWeaponTwo
    {
        get => _secWeapons.Count > 1 ? _secWeapons[1] : "";
        set
        {
            AddChange(nameof(SecWeaponTwo), _secWeapons[1], value);
            _secWeapons[1] = value;
            NotifyPropertyChanged();
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


/// <summary>
/// Represents the usage of a model in various contexts within the game.
/// </summary>
public class ModelUsage
{
    /// <summary>
    /// List of unit names that use the model.
    /// </summary>
    public List<Unit> Units = new();

    /// <summary>
    /// List of mount names that use the model.
    /// </summary>
    public List<Mount> Mounts = new();

    /// <summary>
    /// List of lines in the descr_strat.txt file where the model is used.
    /// </summary>
    public List<string> DescrStratLines = new();

    /// <summary>
    /// List of lines in the campaign_script.txt file where the model is used.
    /// </summary>
    public List<string> CampaignScriptLines = new();

    /// <summary>
    /// List of character entries that use the model.
    /// </summary>
    public List<string> CharacterEntries = new();
    
    /// <summary>
    /// Checks if the model is used in any context.
    /// </summary>
    /// <returns>
    /// True if the model is used in any context (Units, Mounts, DescrStratLines, CampaignScriptLines, CharacterEntries), otherwise false.
    /// </returns>
    public bool IsUsed()
    {
        return Units.Count > 0 
               || Mounts.Count > 0 
               || DescrStratLines.Count > 0 
               || CampaignScriptLines.Count > 0
               || CharacterEntries.Count > 0;
    }

    /// <summary>
    /// Generates a string that represents the usage of the model.
    /// </summary>
    /// <returns>
    /// A string that contains all the contexts where the model is used. If the model is not used in any context, it returns "No usages found.".
    /// </returns>
    public string GetUsageString()
    {
        if (!IsUsed())
            return "No usages found.";
        return "" + 
               ( Units.Count > 0  ? 
                   Units.Aggregate("Units: ", (current, unit) => current + unit.Type + ", ") : "") +
               ( Mounts.Count > 0  ? 
                   Mounts.Aggregate("\nMounts: ", (current, mount) => current + mount.type + ", ") : "") +
               ( DescrStratLines.Count > 0  ? 
                   DescrStratLines.Aggregate("\nDescrStratLines: ", (current, line) => current + line + "\n") : "") +
               ( CampaignScriptLines.Count > 0  ? 
                   CampaignScriptLines.Aggregate("\nCampaignScriptLines", (current, line) => current + line + "\n") : "") +
               ( CharacterEntries.Count > 0  ? 
                   CharacterEntries.Aggregate("\nCharacterEntries", (current, entry) => current + entry + ", ") : "");
    }
}