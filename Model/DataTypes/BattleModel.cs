using System;
using System.Collections.Generic;
using System.Linq;

namespace ModdingTool
{
    public class BattleModel
    {
        private string name;
        private float scale;
        private int lodCount;
        private List<Lod> lodTable;
        private int mainTexturesCount;
        private List<Texture> mainTextures = new List<Texture>();
        private int attachTexturesCount;
        private List<Texture> attachTextures = new List<Texture>();
        private int mountTypeCount;
        private List<Animation> animations;
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
        public List<Lod> LodTable { get => lodTable; set => lodTable = value; }
        public int MainTexturesCount { get => mainTexturesCount; set => mainTexturesCount = value; }
        public List<Texture> MainTextures { get => mainTextures; set => mainTextures = value; }
        public int AttachTexturesCount { get => attachTexturesCount; set => attachTexturesCount = value; }
        public List<Texture> AttachTextures { get => attachTextures; set => attachTextures = value; }
        public int MountTypeCount { get => mountTypeCount; set => mountTypeCount = value; }
        public List<Animation> Animations { get => animations; set => animations = value; }
        public int TorchIndex { get => torchIndex; set => torchIndex = value; }
        public float TorchBoneX { get => torchBoneX; set => torchBoneX = value; }
        public float TorchBoneY { get => torchBoneY; set => torchBoneY = value; }
        public float TorchBoneZ { get => torchBoneZ; set => torchBoneZ = value; }
        public float TorchspriteX { get => torchspriteX; set => torchspriteX = value; }
        public float TorchspriteY { get => torchspriteY; set => torchspriteY = value; }
        public float TorchspriteZ { get => torchspriteZ; set => torchspriteZ = value; }

        public string WriteEntry(BattleModel model)
        {
            var entry = "";
            entry += NumString(model.Name) + "\n";
            entry += FormatFloat(model.Scale) + " " + model.LodTable.Count + "\n";
            for (int i = 0; i < model.LodTable.Count; i++)
            {
                entry += NumString(model.LodTable[i].Mesh) + " " + model.LodTable[i].Distance + "\n";
            }
            entry += model.MainTextures.Count + "\n";
            foreach (var texture in model.MainTextures)
            {
                entry += NumString(texture.Faction) + "\n";
                entry += NumString(texture.TexturePath) + "\n";
                entry += NumString(texture.Normal) + "\n";
                entry += NumString(texture.Sprite) + "\n";
            }
            entry += model.AttachTextures.Count + "\n";
            foreach (var texture in model.AttachTextures)
            {
                entry += NumString(texture.Faction) + "\n";
                entry += NumString(texture.TexturePath) + "\n";
                entry += NumString(texture.Normal);
                if (texture.Sprite != null && texture.Sprite != "0" && texture.Sprite != "")
                {
                    entry += "\n";
                }
                else
                {
                    entry += " ";
                }
                entry += NumString(texture.Sprite) + "\n";
            }
            entry += model.Animations.Count + "\n";
            foreach (var animation in model.Animations)
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
                entry += NumString(animation.Primary_skeleton) + " ";
                entry += NumString(animation.Secondary_skeleton) + "\n";
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
            entry += model.TorchIndex + " ";
            entry += FormatFloat(model.TorchBoneX) + " " + FormatFloat(model.TorchBoneY) + " " + FormatFloat(model.TorchBoneZ) + " ";
            entry += FormatFloat(model.TorchspriteX) + " " + FormatFloat(model.TorchspriteY) + " " + FormatFloat(model.TorchspriteZ) + "\n";

            return entry;
        }

        private static string NumString(string input)
        {
            if (input == null) return "0";
            if (input.Length > 0)
            {
                return input.Length + " " + input;
            }
            return input.Length + "";
        }

        private static string FormatFloat(float input)
        {
            if (Math.Abs(input - 1) < 0.001)
            {
                return "1";
            }
            return input == 0 ? "0" : input.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }
        
        public static BattleModel? CloneModel(string name, BattleModel oldModel)
        {
            if (Globals.BattleModelDataBase.Any(model => model.Key == name))
            {
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
                TorchspriteX = oldModel.TorchspriteX,
                TorchspriteY = oldModel.TorchspriteY,
                TorchspriteZ = oldModel.TorchspriteZ
            };
            foreach (var lod in oldModel.LodTable)
            {
                newModel.LodTable.Add(new Lod
                {
                    Mesh = lod.Mesh,
                    Distance = lod.Distance
                });
            }
            foreach (var texture in oldModel.MainTextures)
            {
                newModel.MainTextures.Add(new Texture
                {
                    Faction = texture.Faction,
                    TexturePath = texture.TexturePath,
                    Normal = texture.Normal,
                    Sprite = texture.Sprite
                });
            }
            foreach (var texture in oldModel.AttachTextures)
            {
                newModel.AttachTextures.Add(new Texture
                {
                    Faction = texture.Faction,
                    TexturePath = texture.TexturePath,
                    Normal = texture.Normal,
                    Sprite = texture.Sprite
                });
            }
            foreach (var animation in oldModel.Animations)
            {
                newModel.Animations.Add(new Animation
                {
                    MountType = animation.MountType,
                    Primary_skeleton = animation.Primary_skeleton,
                    Secondary_skeleton = animation.Secondary_skeleton,
                    PriWeaponCount = animation.PriWeaponCount,
                    PriWeapons = new List<string>(),
                    SecWeaponCount = animation.SecWeaponCount,
                    SecWeapons = new List<string>()
                });
            }
            return newModel;
        }
    }

    public class Lod
    {
        public string Mesh { get; set; } = "";
        public int Distance { get; set; } = 6400;
    }

    public class Texture
    {
        private string faction;
        private string texturePath;
        private string normal;
        private string sprite;

        public string TexturePath { get => texturePath; set => texturePath = value; }
        public string Normal { get => normal; set => normal = value; }
        public string Sprite { get => sprite; set => sprite = value; }
        public string Faction { get => faction; set => faction = value; }
    }

    public class Animation
    {
        private string mountType;
        private string primary_skeleton;
        private string secondary_skeleton;
        private int priWeaponCount;
        private List<string> priWeapons;
        private int secWeaponCount;
        private List<string> secWeapons;

        public string MountType { get => mountType; set => mountType = value; }
        public string Primary_skeleton { get => primary_skeleton; set => primary_skeleton = value; }
        public string Secondary_skeleton { get => secondary_skeleton; set => secondary_skeleton = value; }
        public int PriWeaponCount { get => priWeaponCount; set => priWeaponCount = value; }
        public List<string> PriWeapons { get => priWeapons; set => priWeapons = value; }
        public string PriWeaponOne { get; set; }
        public string PriWeaponTwo { get; set; }
        public string SecWeaponOne { get; set; }
        public string SecWeaponTwo { get; set; }
        public int SecWeaponCount { get => secWeaponCount; set => secWeaponCount = value; }
        public List<string> SecWeapons { get => secWeapons; set => secWeapons = value; }
    }
    
}