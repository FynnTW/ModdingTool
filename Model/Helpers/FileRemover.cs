using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ModdingTool.Globals;

namespace ModdingTool
{
    internal class FileRemover
    {

        public static List<string> usedFiles = new();

        public static void CheckFiles()
        {
            var unitModelsPath = ModPath + @"\data\unit_models";

            foreach (var model in BattleModelDataBase)
            {
                foreach (var mesh in model.Value.LodTable)
                {
                    usedFiles.Add((ModPath + @"\data\" + mesh.Mesh.Replace("/", @"\")).ToLower());
                }
                foreach (var texture in model.Value.MainTextures)
                {
                    usedFiles.Add((ModPath + @"\data\" + texture.Normal.Replace("/", @"\")).ToLower());
                    usedFiles.Add((ModPath + @"\data\" + texture.TexturePath.Replace("/", @"\")).ToLower());
                }
                foreach (var texture in model.Value.AttachTextures)
                {
                    usedFiles.Add((ModPath + @"\data\" + texture.Normal.Replace("/", @"\")).ToLower());
                    usedFiles.Add((ModPath + @"\data\" + texture.TexturePath.Replace("/", @"\")).ToLower());
                }
            }

            usedFiles = usedFiles.Distinct().ToList();

            foreach (string filePath in Directory.GetFiles(unitModelsPath, "*", SearchOption.AllDirectories))
            {
                if (usedFiles.Contains(filePath.ToLower())) continue;
                if (Directory.Exists(ModPath + "MODDINGTOOL_BACKUP"))
                {
                    Directory.Delete(ModPath + "MODDINGTOOL_BACKUP", true);
                    Directory.CreateDirectory(ModPath + "MODDINGTOOL_BACKUP");
                }
                var sourceBase = ModPath + @"\data";
                var targetBase = ModPath + @"\MODDINGTOOL_BACKUP";
                var sourceFile = filePath;

                // Get the relative path of the file from the source base path
                string relativePath = Path.GetRelativePath(sourceBase, sourceFile);

                // Combine the relative path with the target base path
                string targetFile = Path.Combine(targetBase, relativePath);

                // Create all directories in the path if they don't exist
                string targetDir = Path.GetDirectoryName(targetFile);
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                // Copy the file to the new location
                File.Copy(sourceFile, targetFile, true);

                File.Delete(filePath);
            }
        }

    }
}
