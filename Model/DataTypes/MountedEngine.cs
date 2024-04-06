using System.Collections.Generic;

namespace ModdingTool;

public class MountedEngine : GameType
{

	public List<string> EngineVariants { get; } = new()
	{
		"small",
		"medium",
		"large"
	};
	public List<string> ShotAtTypes { get; } = new()
	{
		"commence",
		"recover"
	};
	public List<string> EngineClasses { get; } = new()
	{
		"catapult",
		"onager",
		"trebuchet",
		"heavy_onager",
		"ballista",
		"scorpion",
		"bombard",
		"grand_bombard",
		"huge_bombard",
		"culverin",
		"basilisk",
		"cannon",
		"mortar",
		"serpentine",
		"rocket_launcher",
		"ribault",
		"monster_ribault",
		"tower",
		"ram",
		"ladder",
		"holy_cart",
		"mangonel"
	};

	private string _class = "catapult";
	private string _variant = string.Empty;
	private string _referencePoints = "none";
	private float _missilePositionX;
	private float _missilePositionY;
	private float _missilePositionZ;
	private int _normalShots;
	private int _specialShots;
	private int _shotDelay;
	private string _shotAt = "";
	private string _shotPfxFront = "";
	private string _shotPfxBack = "";
	private string _shotSfx = "";

	public string Type
    {
        get => _name;
        set
        {
            AddChange(nameof(Type), _name, value);
            _name = value;
        }
    }

    public List<string> Cultures { get; set; } = new();
    /// <summary>
    /// Gets or sets the class of the Mounted Engine.
    /// </summary>
    /// <value>
    /// The Class of the MountedEngine.
    /// </value>
    /// <remarks>
    /// When setting this property, it checks if the value is contained in the EngineClasses list.
    /// If not, it logs an error. Regardless, it logs the change using the AddChange method.
    /// </remarks>
    public string ClassType
    {
        get => _class;
        set
        {
	        if (!EngineClasses.Contains(value.ToLower()))
		        Globals.ErrorDb.AddError($"Engine class {value} is not a valid engine class.");
            AddChange(nameof(ClassType), _class, value);
            _class = value;
        }
    }

    public string Variant
    {
        get => _variant;
        set
        {
	        if (!EngineVariants.Contains(value.ToLower()))
		        Globals.ErrorDb.AddError($"Engine variant {value} is not a valid engine variant.");
            AddChange(nameof(Variant), _variant, value);
            _variant = value;
        }
    }

    public string ReferencePoints
    {
	    get => _referencePoints;
	    set
	    {
		    AddChange(nameof(ReferencePoints), _referencePoints, value);
		    _referencePoints = value;
	    }
    }

    public List<EnginePoint> PushPoints { get; set; } = new();
    public List<EnginePoint> StationPoints { get; set; } = new();

    public float MissilePositionX
    {
	    get => _missilePositionX;
	    set
	    {
		    AddChange(nameof(MissilePositionX), _missilePositionX, value);
		    _missilePositionX = value;
	    }
    }

    public float MissilePositionY
    {
	    get => _missilePositionY;
	    set
	    {
		    AddChange(nameof(MissilePositionY), _missilePositionY, value);
		    _missilePositionY = value;
	    }
    }

    public float MissilePositionZ
    {
	    get => _missilePositionZ;
	    set
	    {
		    AddChange(nameof(MissilePositionZ), _missilePositionZ, value);
		    _missilePositionZ = value;
	    }
    }

    public int NormalShots
    {
	    get => _normalShots;
	    set
	    {
		    AddChange(nameof(NormalShots), _normalShots, value);
		    _normalShots = value;
	    }
    }
    
    public int SpecialShots
	{
	    get => _specialShots;
	    set
	    {
		    AddChange(nameof(SpecialShots), _specialShots, value);
		    _specialShots = value;
	    }
	}
    
    public int ShotDelay
	{
	    get => _shotDelay;
	    set
	    {
		    AddChange(nameof(ShotDelay), _shotDelay, value);
		    _shotDelay = value;
	    }
	}

	public string ShotAt
	{
		get => _shotAt;
		set
		{
			if (!ShotAtTypes.Contains(value.ToLower()))
				Globals.ErrorDb.AddError($"Shot At Type {value} is not valid.");
			AddChange(nameof(ShotAt), _shotAt, value);
			_shotAt = value;
		}
	}
	
	public string ShotPfxFront
	{
		get => _shotPfxFront;
		set
		{
			AddChange(nameof(ShotPfxFront), _shotPfxFront, value);
			_shotPfxFront = value;
		}
	}
	
	public string ShotPfxBack
	{
		get => _shotPfxBack;
		set
		{
			AddChange(nameof(ShotPfxBack), _shotPfxBack, value);
			_shotPfxBack = value;
		}
	}
	
	public string ShotSfx
	{
		get => _shotSfx;
		set
		{
			AddChange(nameof(ShotSfx), _shotSfx, value);
			_shotSfx = value;
		}
	}
	
	public List<CrewAnimation> CrewAnimations { get; set; } = new();
    
    
}