using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModdingTool
{
    internal class BattleModel
    {
        private string name;
        private float scale;
        private int lodCount;
        private LOD[] lodTable;
        private int mainTexturesCount;
        private Dictionary<string, Texture> mainTextures = new Dictionary<string, Texture>();
        private int attachTexturesCount;
        private Dictionary<string, Texture> attachTextures = new Dictionary<string, Texture>();
        private int mountTypeCount;
        private Animation[] animations;
        private int torchIndex;
        private float torchBoneX;
        private float torchBoneY;
        private float torchBoneZ;
        private float torchspriteX;
        private float torchspriteY;
        private float torchspriteZ;

        public string Name { get => name; set => name = value; }
        public float Scale { get => scale; set => scale = value; }
        public int LodCount { get => lodCount; set => lodCount = value; }
        public LOD[] LodTable { get => lodTable; set => lodTable = value; }
        public int MainTexturesCount { get => mainTexturesCount; set => mainTexturesCount = value; }
        public Dictionary<string, Texture> MainTextures { get => mainTextures; set => mainTextures = value; }
        public int AttachTexturesCount { get => attachTexturesCount; set => attachTexturesCount = value; }
        public Dictionary<string, Texture> AttachTextures { get => attachTextures; set => attachTextures = value; }
        public int MountTypeCount { get => mountTypeCount; set => mountTypeCount = value; }
        public Animation[] Animations { get => animations; set => animations = value; }
        public int TorchIndex { get => torchIndex; set => torchIndex = value; }
        public float TorchBoneX { get => torchBoneX; set => torchBoneX = value; }
        public float TorchBoneY { get => torchBoneY; set => torchBoneY = value; }
        public float TorchBoneZ { get => torchBoneZ; set => torchBoneZ = value; }
        public float TorchspriteX { get => torchspriteX; set => torchspriteX = value; }
        public float TorchspriteY { get => torchspriteY; set => torchspriteY = value; }
        public float TorchspriteZ { get => torchspriteZ; set => torchspriteZ = value; }
    }

    public class LOD
    {
        private string mesh;
        private int distance;

        public string Mesh { get => mesh; set => mesh = value; }
        public int Distance { get => distance; set => distance = value; }
    }
    public class Texture
    {
        private Faction faction;
        private string texturePath;
        private string normal;
        private string sprite;

        public string TexturePath { get => texturePath; set => texturePath = value; }
        public string Normal { get => normal; set => normal = value; }
        public string Sprite { get => sprite; set => sprite = value; }
        internal Faction Faction { get => faction; set => faction = value; }
    }
    public class Animation
    {
        private string mountType;
        private string primary_skeleton;
        private string secondary_skeleton;
        private int priWeaponCount;
        private string[] priWeapons;
        private int secWeaponCount;
        private string[] secWeapons;

        public string MountType { get => mountType; set => mountType = value; }
        public string Primary_skeleton { get => primary_skeleton; set => primary_skeleton = value; }
        public string Secondary_skeleton { get => secondary_skeleton; set => secondary_skeleton = value; }
        public int PriWeaponCount { get => priWeaponCount; set => priWeaponCount = value; }
        public string[] PriWeapons { get => priWeapons; set => priWeapons = value; }
        public int SecWeaponCount { get => secWeaponCount; set => secWeaponCount = value; }
        public string[] SecWeapons { get => secWeapons; set => secWeapons = value; }
    }
}
