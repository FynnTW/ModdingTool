using System.Collections.Generic;

namespace ModdingTool
{
    public class Engine
    {
        public string type { get; set; } = "";
        public string culture { get; set; } = "";
        public string engine_class { get; set; } = "";
        public string pathfinding_data { get; set; } = "";
        public int surface_occupants { get; set; } = 0;
        public string reference_points { get; set; } = "none";
        public string area_effect { get; set; } = "none";
        public List<EngineModelGroup> engine_model_groups { get; set; } = new();
        public string engine_shadow { get; set; } = "none";
        public double engine_radius { get; set; } = 0;
        public double engine_visual_radius { get; set; } = 0;
        public double engine_length { get; set; } = 0;
        public double engine_width { get; set; } = 0;
        public double engine_height { get; set; } = 0;
        public double engine_mass { get; set; } = 0;
        public double engine_dock_dist { get; set; } = 0;
        public double engine_mob_dist { get; set; } = 0;
        public bool engine_flammable { get; set; } = true;
        public double engine_ignition { get; set; } = 0;
        public string fire_effect { get; set; } = "";
        public bool engine_front_attacked { get; set; } = false;
        public double sa_range { get; set; } = 0;
        public string obstacle_shape { get; set; } = "";
        public double obstacle_x_radius { get; set; } = 0;
        public double obstacle_y_radius { get; set; } = 0;
        public double engine_formation_one { get; set; } = 0;
        public double engine_formation_two { get; set; } = 0;
        public string engine_spo { get; set; } = "physical_obstacle";
        public string engine_spotfx { get; set; } = "";
        public int engine_health { get; set; } = 0;
        public int attack { get; set; } = 0;
        public int charge { get; set; } = 0;
        public string projectile { get; set; } = "no";
        public int range { get; set; } = 0;
        public int ammunition { get; set; } = 0;
        public string weapon_type { get; set; } = "";
        public string weapon_tech_type { get; set; } = "";
        public string weapon_damage_type { get; set; } = "";
        public List<string> attack_stat_attr { get; set; } = new();
        public double missile_pos_x { get; set; } = 0;
        public double missile_pos_y { get; set; } = 0;
        public double missile_pos_z { get; set; } = 0;
        public int normal_shots { get; set; } = 0;
        public int special_shots { get; set; } = 0;
        public int shot_delay { get; set; } = 0;
        public string shot_at { get; set; } = "";
        public string shot_pfx_front { get; set; } = "";
        public string shot_pfx_back { get; set; } = "";
        public string shot_sfx { get; set; } = "";
        public List<CrewAnimation> CrewAnimations { get; set; } = new();
    }

    public class EngineModelGroup
    {
        public string type { get; set; } = "";
        public string engine_skeleton { get; set; } = "";
        public string engine_bone_map { get; set; } = "";
        public string engine_collision { get; set; } = "";
        public List<EngineMesh> engine_meshes { get; set; } = new();

        public List<ArrowGenerator> arrow_generators { get; set; } = new();
    }

    public class EngineMesh
    {
        public string path { get; set; } = "";
        public string distance { get; set; } = "";
    }

    public class CrewAnimation
    {
        public int id { get; set; } = 0;
        public string type { get; set; } = "";
        public List<string> animations { get; set; } = new();
    }

    public class ArrowGenerator
    {
        public int id { get; set; } = 0;
        public double arrow_generator_x { get; set; } = 0;
        public double arrow_generator_y { get; set; } = 0;
        public double arrow_generator_z { get; set; } = 0;
        public double aim_dir_x { get; set; } = 0;
        public double aim_dir_y { get; set; } = 0;
        public double aim_dir_z { get; set; } = 0;
        public double aim_arc { get; set; } = 0;
        public double fire_interval { get; set; } = 0;
    }
}
