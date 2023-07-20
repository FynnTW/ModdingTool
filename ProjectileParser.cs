using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModdingTool;
using static ModdingTool.Globals;
using static ModdingTool.ParseHelpers;

public class ProjectileParser
{
    //Parsing line number and file name used for error logging
    private static int _lineNum;
    private static string _fileName = "";
    private static bool stuck = false;


    public static void ParseProjectiles()
    {
        _fileName = "descr_projectile.txt";
        Console.WriteLine($@"start parse {_fileName}");


        var lines = FileReader("\\data\\descr_projectile.txt", _fileName, Encoding.Default);
        if (lines == null) { return; } //something very wrong if you hit this

        //Reset line counter
        _lineNum = 0;

        //Initialize Global Projectiles Database
        ProjectileDataBase = new Dictionary<string, Projectile>();

        //Create new Projectile for first entry
        var projectile = new Projectile();
        var first = true;

        //Loop through lines
        foreach (var line in lines)
        {
            //Increase line counter
            _lineNum++;

            //Remove Comments and Faulty lines
            var newline = CleanLine(line);
            if (string.IsNullOrWhiteSpace(newline))
            {
                continue;
            }

            //Split line into parts
            var parts = LineSplitterProjectiles(newline);
            if (parts.Length < 1)
            {
                //Should be something wrong with line if you hit this
                ErrorDb.AddError("Unrecognized content", _lineNum.ToString(), _fileName);
                continue;
            }

            //Entry is completed, next entry is starting
            if (parts[0].Equals("projectile"))
            {
                //Add new faction if it is not the first time we hit this
                if (!first)
                {
                    AddProjectile(projectile);
                }
                first = false;
                projectile = new Projectile();
                stuck = false;
            }

            //Fill out the faction field the line is about
            AssignFields(projectile, parts);
        }

        //Add last faction
        AddProjectile(projectile);

        //Reset Line Counter
        Console.WriteLine($@"end parse {_fileName}");
        _lineNum = 0;

        PrintFinal();

    }

    private static void AddProjectile(Projectile projectile)
    {
        if (ProjectileDataBase.ContainsKey(projectile.name))
        {
            ErrorDb.AddError("Projectile name already exists", _lineNum.ToString(), _fileName);
            return;
        }
        ProjectileDataBase.Add(projectile.name, projectile);
        Console.WriteLine(projectile.name);
    }

    private static string[] LineSplitterProjectiles(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            ErrorDb.AddError("Warning empty line send on", _lineNum.ToString(), _fileName);
            return Array.Empty<string>();
        }

        //split by comma
        char[] delimiters = { ',' };
        var lineParts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        //Make sure there are no empty entries
        if (lineParts.Length == 0)
        {
            return lineParts;
        }

        //split first part by white space (no comma between identifier and value)
        char[] delimitersWhite = { ' ', '\t' };
        var firstParts = lineParts[0].Split(delimitersWhite, 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        //concat first part with rest of line
        lineParts = firstParts.Concat(lineParts[1..]).ToArray();
        return lineParts;
    }
    private static void AssignFields(Projectile projectile, string[] parts)
    {
        //name of the field
        var identifier = parts[0];

        var value = "";
        //first value, usually all needed
        if (parts.Length > 1)
        {
            value = parts[1];
        }

        char[] delimitersWhite = { ' ', '\t' };

        try
        {
            switch (identifier)
            {
                case "delay":
                    switch (value)
                    {
                        case "standard":
                            ProjectileDelayStandard = int.Parse(parts[2]);
                            break;
                        case "flaming":
                            ProjectileDelayFlaming = int.Parse(parts[2]);
                            break;
                        case "gunpowder":
                            ProjectileDelayGunpowder = int.Parse(parts[2]);
                            break;
                    };
                    break;
                case "projectile":
                    projectile.name = value;
                    break;
                case "destroy_after_max_range_percent_and_variation":
                    var destroy = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    projectile.destroy_after_max_range_percent = int.Parse(destroy[0]);
                    projectile.destroy_after_max_range_variation = int.Parse(destroy[1]);
                    break;
                case "flaming":
                    projectile.flaming = value;
                    break;
                case "exploding":
                    projectile.exploding = value;
                    break;
                case "effect":
                    projectile.effect = value;
                    break;
                case "end_effect":
                    projectile.end_effect = value;
                    break;
                case "end_man_effect":
                    projectile.end_man_effect = value;
                    break;
                case "end_package_effect":
                    projectile.end_package_effect = value;
                    break;
                case "end_shatter_effect":
                    projectile.end_shatter_effect = value;
                    break;
                case "end_shatter_man_effect":
                    projectile.end_shatter_man_effect = value;
                    break;
                case "end_shatter_package_effect":
                    projectile.end_shatter_package_effect = value;
                    break;
                case "explode_effect":
                    projectile.explode_effect = value;
                    break;
                case "effect_offset":
                    projectile.effect_offset = ParseDouble(value);
                    break;
                case "area_effect":
                    projectile.area_effect = value;
                    break;
                case "damage":
                    projectile.damage = int.Parse(value);
                    break;
                case "damage_to_troops":
                    projectile.damage_to_troops = int.Parse(value);
                    break;
                case "radius":
                    projectile.radius = ParseDouble(value);
                    break;
                case "mass":
                    projectile.mass = ParseDouble(value);
                    break;
                case "area":
                    projectile.area = ParseDouble(value);
                    break;
                case "accuracy_vs_units":
                    projectile.accuracy_vs_units = ParseDouble(value);
                    break;
                case "accuracy_vs_buildings":
                    projectile.accuracy_vs_buildings = ParseDouble(value);
                    break;
                case "accuracy_vs_towers":
                    projectile.accuracy_vs_towers = ParseDouble(value);
                    break;
                case "affected_by_rain":
                    projectile.affected_by_rain = true;
                    break;
                case "no_ae_on_ram":
                    projectile.no_ae_on_ram = true;
                    break;
                case "fiery":
                    projectile.fiery = true;
                    break;
                case "explosive":
                    projectile.explosive = true;
                    break;
                case "min_angle":
                    projectile.min_angle = int.Parse(value);
                    break;
                case "max_angle":
                    projectile.max_angle = int.Parse(value);
                    break;
                case "prefer_high":
                    projectile.prefer_high = true;
                    break;
                case "velocity":
                    var velocity = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    projectile.velocityMin = ParseDouble(velocity[0]);
                    projectile.velocityMax = velocity.Length > 1 ? ParseDouble(velocity[1]) : projectile.velocityMin;
                    break;
                case "ground_shatter":
                    projectile.ground_shatter = true;
                    break;
                case "bounce":
                    var bounce = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    projectile.bounce = new Bounce
                    {
                        bounceVelocityThreshold = ParseDouble(bounce[0]),
                        upVectorThreshold = ParseDouble(bounce[1]),
                        velocityDampener = ParseDouble(bounce[2]),
                        upVectorDampener = ParseDouble(bounce[3])
                    };
                    if (bounce.Length > 4)
                    {
                        if (bounce[4] == "erratic")
                        {
                            projectile.bounce.erratic = true;
                        }
                    };
                    break;
                case "body_piercing":
                    projectile.body_piercing = true;
                    break;
                case "rocket":
                    var rocket = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    projectile.rocket = new Rocket
                    {
                        trailTime = ParseDouble(rocket[0]),
                        spinProb = ParseDouble(rocket[1]),
                        spinTick = ParseDouble(rocket[2]),
                        maxRadiusDelta = ParseDouble(rocket[3]),
                        minAngle = ParseDouble(rocket[4]),
                        maxAngle = ParseDouble(rocket[5]),
                        erraticProb = ParseDouble(rocket[6]),
                        erraticTick = ParseDouble(rocket[7]),
                        erraticMaxAngle = ParseDouble(rocket[8])
                    };
                    break;
                case "elevated_rocket":
                    var elevated_rocket = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    projectile.rocket = new Rocket
                    {
                        trailTime = ParseDouble(elevated_rocket[0]),
                        spinProb = ParseDouble(elevated_rocket[1]),
                        spinTick = ParseDouble(elevated_rocket[2]),
                        maxRadiusDelta = ParseDouble(elevated_rocket[3]),
                        minAngle = ParseDouble(elevated_rocket[4]),
                        maxAngle = ParseDouble(elevated_rocket[5]),
                        erraticProb = ParseDouble(elevated_rocket[6]),
                        erraticTick = ParseDouble(elevated_rocket[7]),
                        erraticMaxAngle = ParseDouble(elevated_rocket[8])
                    };
                    projectile.elevated_rocket = true;
                    break;
                case "cow_carcass":
                    projectile.cow_carcass = true;
                    break;
                case "grapeshot":
                    projectile.grapeshot = true;
                    break;
                case "artillery":
                    projectile.artillery = true;
                    break;
                case "display":
                    var display = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    projectile.display = new Display();
                    if (display.Contains("shatter_dust"))
                    {
                        projectile.display.shatter_dust = true;
                    }
                    if (display.Contains("particle_trail"))
                    {
                        projectile.display.particle_trail = true;
                    }
                    if (display.Contains("vanish_debris"))
                    {
                        projectile.display.vanish_debris = true;
                    }
                    if (display.Contains("vanish_dust"))
                    {
                        projectile.display.vanish_dust = true;
                    }
                    if (display.Contains("shatter_debris"))
                    {
                        projectile.display.shatter_debris = true;
                    }
                    if (display.Contains("aimed"))
                    {
                        projectile.display.aimed = true;
                    }
                    if (display.Contains("invert_model_z"))
                    {
                        projectile.display.invert_model_z = true;
                    }
                    if (display.Contains("spin"))
                    {
                        projectile.display.spin = true;
                        foreach (var item in display)
                        {
                            if (item.Any(char.IsDigit))
                            {
                                projectile.display.spin_amount = ParseDouble(item);
                            }
                        }
                    }
                    break;
                case "self_explode":
                    var selfExplode = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    projectile.self_explode = new SelfExplode
                    {
                        probability = ParseDouble(selfExplode[0]),
                        minSeconds = ParseDouble(selfExplode[1]),
                        maxSeconds = ParseDouble(selfExplode[2]),
                    };
                    if (selfExplode.Length > 3)
                    {
                        projectile.self_explode.area_effect = selfExplode[3];
                    }
                    break;
                case "texture":
                    projectile.triangle = new Triangle
                    {
                        texture = value
                    };
                    break;
                case "tail":
                    projectile.triangle.tail = ParseDouble(value);
                    break;
                case "length":
                    projectile.triangle.length = ParseDouble(value);
                    break;
                case "tail_tex0":
                    var tail_tex0 = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    projectile.triangle.tail_tex0_one = ParseDouble(tail_tex0[0]);
                    projectile.triangle.tail_tex0_two = ParseDouble(tail_tex0[1]);
                    break;
                case "tail_tex1":
                    var tail_tex1 = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    projectile.triangle.tail_tex1_one = ParseDouble(tail_tex1[0]);
                    projectile.triangle.tail_tex1_two = ParseDouble(tail_tex1[1]);
                    break;
                case "head_tex":
                    var head_tex = value.Split(delimitersWhite, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    projectile.triangle.head_tex_one = ParseDouble(head_tex[0]);
                    projectile.triangle.head_tex_two = ParseDouble(head_tex[1]);
                    break;
                case "model":
                    if (stuck)
                    {
                        projectile.stuck_models.Add(new Model
                        {
                            name = value,
                            distance = parts[2]
                        });
                    }
                    else
                    {
                        projectile.models.Add(new Model
                        {
                            name = value,
                            distance = parts[2]
                        });
                    }
                    break;
                case "stuck":
                    stuck = true;
                    break;
                case "effect_only":
                    projectile.effect_only = true;
                    break;
            };

        }
        catch (Exception e)
        {
            ErrorDb.AddError(e.Message, _lineNum.ToString(), _fileName);
            Console.WriteLine(e);
            Console.WriteLine(identifier);
            Console.WriteLine(@"on line: " + _lineNum);
            Console.WriteLine(@"====================================================================================");
        }
    }

}