using System.Collections.Generic;
using static ModdingTool.Globals;

namespace ModdingTool.View.InterfaceData
{
    public partial class ProjectileTab : Tab
    {
        public Projectile selectedProjectile { get; set; }

        public static Dictionary<string, string> ProjectileUiText { get; set; } = new Dictionary<string, string>()
        {
            {"name", "Name"},
            {"destroy_after_max_range_percent", "Destroy after max range percent"},
            {"destroy_after_max_range_variation", "Destroy after max range variation"},
            {"flaming", "Flaming"},
            {"exploding", "Exploding"},
            {"effect", "Effect"},
            {"end_effect", "End Effect"},
            {"end_man_effect", "End Man Effect"},
            {"end_package_effect", "End Package Effect"},
            {"end_shatter_effect", "End Shatter Effect"},
            {"end_shatter_man_effect", "End Shatter Man Effect"},
            {"end_shatter_package_effect", "End Shatter Package Effect"},
            {"explode_effect", "Explode Effect"},
            {"effect_offset", "Effect Offset"},
            {"area_effect", "Area Effect"},
            {"no_ae_on_ram", "No Area Affect On Ram"},
            {"damage", "Damage"},
            {"damage_to_troops", "Damage to Troops"},
            {"radius", "Radius"},
            {"mass", "Mass"},
            {"area", "Area"},
            {"accuracy_vs_units", "Accuracy vs Units"},
            {"accuracy_vs_buildings", "Accuracy vs Buildings"},
            {"accuracy_vs_towers", "Accuracy vs Towers"},
            {"affected_by_rain", "Affected By Rain"},
            {"fiery", "Fiery"},
            {"explosive", "Explosive"},
            {"min_angle", "Minimum Angle"},
            {"max_angle", "Maximum Angle"},
            {"prefer_high", "Prefer High"},
            {"velocityMin", "Minimum Velocity"},
            {"velocityMax", "Maximum Velocity"},
            {"ground_shatter", "Ground Shatter"},
            {"body_piercing", "Body Piercing"},
            {"elevated_rocket", "Elevated Rocket"},
            {"cow_carcass", "Cow Carcass"},
            {"grapeshot", "Grapeshot"},
            {"artillery", "Artillery"},
            {"effect_only", "Effect Only"},
            {"bounceVelocityThreshold", "Bounce Velocity Threshold"},
            {"upVectorThreshold", "Up Vector Threshold"},
            {"velocityDampener", "Velocity Dampener"},
            {"upVectorDampener", "Up Vector Dampener"},
            {"erratic", "Erratic"},
            {"shatter_dust", "Shatter Dust"},
            {"particle_trail", "Particle Trail"},
            {"vanish_debris", "Vanish Debris"},
            {"vanish_dust", "Vanish Dust"},
            {"shatter_debris", "Shatter Debris"},
            {"aimed", "Aimed"},
            {"invert_model_z", "Invert Model Z"},
            {"spin", "Spin"},
            {"spin_amount", "Spin Amount"},
            {"probability", "Probability"},
            {"minSeconds", "Minimum Seconds"},
            {"maxSeconds", "Max Seconds"},
            {"texture", "Texture"},
            {"tail", "Tail"},
            {"length", "Length"},
            {"tail_tex0_one", "tail_tex0_one"},
            {"tail_tex0_two", "tail_tex0_two"},
            {"tail_tex1_one", "tail_tex1_one"},
            {"tail_tex1_two", "tail_tex1_two"},
            {"head_tex_one", "head_tex_one"},
            {"head_tex_two", "head_tex_two"},
            {"trailTime", "Trail Time"},
            {"spinProb", "Spin Probability"},
            {"spinTick", "Spin Tick"},
            {"maxRadiusDelta", "Maximum Radius Delta"},
            {"erraticProb", "Erratic Probability"},
            {"erraticTick", "Erratic Tick"},
            {"erraticMaxAngle", "Erratic Max Angle"},
            {"modelName", "Model Name"},
            {"distance", "Distance"},
        };

        public ProjectileTab(string name)
        {
            Title = name;
            selectedProjectile = ProjectileDataBase[Title];
        }

    }
}
