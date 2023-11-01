using System.Collections.Generic;

namespace ModdingTool
{
    public class Mount
    {
        public string type { get; set; } = "";
        public string mount_class { get; set; } = "";
        public string model { get; set; } = "";
        public double radius { get; set; } = 0;
        public double x_radius { get; set; } = 0;
        public double y_offset { get; set; } = 0;
        public double height { get; set; } = 0;
        public double mass { get; set; } = 0;
        public double banner_height { get; set; } = 0;
        public double bouyancy_offset { get; set; } = 0;
        public string water_trail_effect { get; set; } = "";
        public double root_node_height { get; set; } = 0;
        public List<RiderOffset> rider_offsets { get; set; } = new();
        public double attack_delay { get; set; } = 0;
        public double dead_radius { get; set; } = 0;
        public double tusk_z { get; set; } = 0;
        public double tusk_radius { get; set; } = 0;
        public int riders { get; set; } = 0;
    }

    public class RiderOffset
    {
        public int id { get; set; } = 0;
        public double x { get; set; } = 0;
        public double y { get; set; } = 0;
        public double z { get; set; } = 0;
    }
}
