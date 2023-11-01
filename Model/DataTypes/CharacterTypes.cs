using System.Collections.Generic;

namespace ModdingTool;

public class CharacterType
{
    public int starting_action_points { get; set; } = 0;
    public string type { get; set; } = "";
    public List<string>? actions { get; set; } = new();
    public int wage_base { get; set; } = 0;
}

