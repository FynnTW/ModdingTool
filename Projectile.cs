using System.Collections.Generic;

namespace ModdingTool
{
    public class Projectile
    {
        public string name { get; set; } = "";
        public int destroy_after_max_range_percent { get; set; } = 0;
        public int destroy_after_max_range_variation { get; set; } = 0;
        public string flaming { get; set; } = "";
        public string exploding { get; set; } = "";
        public string effect { get; set; } = "default_arrow_trail_set";
        public string end_effect { get; set; } = "";
        public string end_man_effect { get; set; } = "";
        public string end_package_effect { get; set; } = "";
        public string end_shatter_effect { get; set; } = "";
        public string end_shatter_man_effect { get; set; } = "";
        public string end_shatter_package_effect { get; set; } = "";
        public string explode_effect { get; set; } = "";
        public double effect_offset { get; set; } = 0;
        public string area_effect { get; set; } = "";
        public bool no_ae_on_ram { get; set; } = false;
        public int damage { get; set; } = 0;
        public int damage_to_troops { get; set; } = 0;
        public double radius { get; set; } = 0;
        public double mass { get; set; } = 0;
        public double area { get; set; } = 0;
        public double accuracy_vs_units { get; set; } = 0;
        public double accuracy_vs_buildings { get; set; } = 0;
        public double accuracy_vs_towers { get; set; } = 0;
        public bool affected_by_rain { get; set; } = false;
        public bool fiery { get; set; } = false;
        public bool explosive { get; set; } = false;
        public int min_angle { get; set; } = 0;
        public int max_angle { get; set; } = 0;
        public bool prefer_high { get; set; } = false;
        public double velocityMin { get; set; } = 0;
        public double velocityMax { get; set; } = 0;
        public bool ground_shatter { get; set; } = false;
        public Bounce bounce { get; set; } = new();
        public bool body_piercing { get; set; } = false;
        public Rocket rocket { get; set; } = new();
        public bool elevated_rocket { get; set; } = false;
        public bool cow_carcass { get; set; } = false;
        public bool grapeshot { get; set; } = false;
        public bool artillery { get; set; } = false;
        public Display display { get; set; } = new();
        public SelfExplode self_explode { get; set; } = new();
        public Triangle triangle { get; set; } = new();
        public List<Model> models { get; set; } = new();
        public List<Model> stuck_models { get; set; } = new();
        public bool effect_only { get; set; } = true;
    }

    public class Bounce
    {
        public double bounceVelocityThreshold { get; set; } = 0;
        public double upVectorThreshold { get; set; } = 0;
        public double velocityDampener { get; set; } = 0;
        public double upVectorDampener { get; set; } = 0;
        public bool erratic { get; set; } = false;
    }

    public class Display
    {
        public bool shatter_dust { get; set; } = false;
        public bool particle_trail { get; set; } = false;
        public bool vanish_debris { get; set; } = false;
        public bool vanish_dust { get; set; } = false;
        public bool shatter_debris { get; set; } = false;
        public bool aimed { get; set; } = false;
        public bool invert_model_z { get; set; } = false;
        public bool spin { get; set; } = false;
        public double spin_amount { get; set; } = 0;
    }

    public class SelfExplode
    {
        public double probability { get; set; } = 0;
        public double minSeconds { get; set; } = 0;
        public double maxSeconds { get; set; } = 0;
        public string area_effect { get; set; } = "";
    }

    public class Triangle
    {
        public string texture { get; set; } = "";
        public double tail { get; set; } = 0;
        public double length { get; set; } = 0;
        public double tail_tex0_one { get; set; } = 0;
        public double tail_tex0_two { get; set; } = 0;
        public double tail_tex1_one { get; set; } = 0;
        public double tail_tex1_two { get; set; } = 0;
        public double head_tex_one { get; set; } = 0;
        public double head_tex_two { get; set; } = 0;
    }

    public class Rocket
    {
        public double trailTime { get; set; } = 0;
        public double spinProb { get; set; } = 0;
        public double spinTick { get; set; } = 0;
        public double maxRadiusDelta { get; set; } = 0;
        public double minAngle { get; set; } = 0;
        public double maxAngle { get; set; } = 0;
        public double erraticProb { get; set; } = 0;
        public double erraticTick { get; set; } = 0;
        public double erraticMaxAngle { get; set; } = 0;
    }

    public class Model
    {
        public string name { get; set; } = "";
        public string distance { get; set; } = "";
    }
}