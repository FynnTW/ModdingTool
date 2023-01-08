using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModdingTool
{
    internal class Projectile
    {
        private string name = "";
        private string effect = "";
        private string end_effect = "";
        private string end_man_effect = "";
        private string end_package_effect = "";
        private string end_shatter_effect = "";
        private string end_shatter_man_effect = "";
        private string end_shatter_package_effect = "";
        private int effect_offset = 0;
        private string area_effect = "";
        private int damage = 0;
        private int damage_to_troops = 0;
        private float radius = 0;
        private float mass = 0;
        private float area = 0;
        private float accuracy_vs_units = 0;
        private float accuracy_vs_buildings = 0;
        private float accuracy_vs_towers = 0;
        private bool affected_by_rain = false;
        private bool no_ae_on_ram = false;
        private bool fiery = false;
        private Bounce bounce= new Bounce();
        private int min_angle = 0;
        private int max_angle = 0;
        private int velocityMin = 0;
        private int velocityMax = 0;
        private bool ground_shatter = false;
        private bool body_piercing = false;
        private bool effect_only = true;
        private bool grapeshot = false;
        private bool cow_carcass = false;
        private string display = "";
        private SelfExplode self_explode = new SelfExplode();
        private Triangle triangle = new Triangle();
        private Model[] models = Array.Empty<Model>();
    }

    public class Bounce
    {
        private float bounceVelocityThreshold = 0;
        private float upVectorThreshold = 0;
        private float velocityDampener = 0;
        private float upVectorDampener = 0;
    }

    public class SelfExplode
    {
        private float probability = 0;
        private float minSeconds = 0;
        private float maxSeconds = 0;
        private string area_effect = "";
    }

    public class Triangle
    {
        private string texture = "";
        private float tail = 0;
        private float length = 0;
        private float tail_tex0_one = 0;
        private float tail_tex0_two = 0;
        private float tail_tex1_one = 0;
        private float tail_tex1_two = 0;
        private float head_tex_one = 0;
        private float head_tex_two = 0;
    }

    public class Rocket
    {
        private float trailTime = 0;
        private float spinProb = 0;
        private float spinTick = 0;
        private float maxRadiusDelta = 0;
        private float minAngle = 0;
        private float maxAngle = 0;
        private float erraticProb = 0;
        private float erraticTick = 0;
        private float erraticMaxAngle = 0;
    }

    public class Model
    {
        private string name = "";
        private string distance = "";
    }


}
