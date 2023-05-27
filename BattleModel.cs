using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace ModdingTool
{
    public class BattleModel
    {
        private string name;
        private float scale;
        private int lodCount;
        private List<LOD> lodTable;
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
        public List<LOD> LodTable { get => lodTable; set => lodTable = value; }
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
                entry += NumString(texture.Normal) + "\n";
                entry += NumString(texture.Sprite) + "\n";
            }
            entry += model.Animations.Count + "\n";
            foreach (var animation in model.Animations)
            {
                if (!animation.PriWeapons.Contains(animation.PriWeaponOne))
                {
                    animation.PriWeapons.Add(animation.PriWeaponOne);
                }

                if (!animation.PriWeapons.Contains(animation.PriWeaponTwo))
                {
                    animation.PriWeapons.Add(animation.PriWeaponTwo);
                }

                if (!animation.SecWeapons.Contains(animation.SecWeaponOne))
                {
                    animation.SecWeapons.Add(animation.SecWeaponOne);
                }

                if (!animation.SecWeapons.Contains(animation.SecWeaponTwo))
                {
                    animation.SecWeapons.Add(animation.SecWeaponTwo);
                }
                entry += NumString(animation.MountType) + "\n";
                entry += NumString(animation.Primary_skeleton) + " ";
                entry += NumString(animation.Secondary_skeleton) + "\n";
                entry += animation.PriWeapons.Count + "\n";
                for (var i = 0; i < animation.PriWeapons.Count; i++)
                {
                    entry += i switch
                    {
                        0 => NumString(animation.PriWeaponOne) + "\n",
                        1 => NumString(animation.PriWeaponTwo) + "\n",
                        _ => NumString(animation.PriWeapons[i]) + "\n"
                    };
                }
                entry += animation.SecWeapons.Count + "\n";
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