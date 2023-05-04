/*
I got the idea to use the   ZIP file architecture from MonkeMapLoader so credits I guess.
*/
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using TextureLoader.Models;
using UnityEngine;

namespace TextureLoader.Core
{
    internal class TextureLoader
    {
        private static Texture MainAtlasDefault;
        private static Texture TreeStumpDefault;
        private static Texture treeroomatlasDefault; // now I am pissed
        internal static void SetTexture(TexturePack texturePack)
        {
            Material AtlasMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "forestatlas");
            Material TreeStumpMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "Tree Texture Baker-mat");
            Material TreeRoomMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "treeroomatlas");
            if (MainAtlasDefault == null)
                MainAtlasDefault = AtlasMaterial.mainTexture;
            if (TreeStumpDefault == null)
                TreeStumpDefault = TreeStumpMaterial.mainTexture;
            if (treeroomatlasDefault == null)
                treeroomatlasDefault = TreeRoomMaterial.mainTexture;
            AtlasMaterial.mainTexture = texturePack.atlas;
            TreeStumpMaterial.mainTexture = texturePack.TreeTexture;
            TreeRoomMaterial.mainTexture = texturePack.treeroomatlas;
            // Set ground texture
            GameObject.Find("Level/forest/ForestObjects/Uncover ForestCombined/CombinedMesh-Uncover ForestCombined-mesh").GetComponentInChildren<MeshRenderer>().materials[2].mainTexture = texturePack.ground;
        }
        internal static void ResetTexture()
        {
            Material AtlasMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "forestatlas");
            Material TreeStumpMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "Tree Texture Baker-mat-_MainTex-atlas-0");
            Material TreeRoomMaterial = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "treeroomatlas");
            AtlasMaterial.mainTexture = MainAtlasDefault;
            TreeStumpMaterial.mainTexture = TreeStumpDefault;
            TreeRoomMaterial.mainTexture = treeroomatlasDefault;
            // Set ground texture
            GameObject.Find("Level/forest/ForestObjects/Uncover ForestCombined/CombinedMesh-Uncover ForestCombined-mesh").GetComponentInChildren<MeshRenderer>().materials[2].mainTexture = MainAtlasDefault;
        }

        internal static TexturePack[] GetAllTexture()
        {
            string[] Paths = Directory.GetFiles(Path.GetDirectoryName(typeof(Main).Assembly.Location) + "/addons", "*.texture");
            List<TexturePack> texturePack = new List<TexturePack>();
            foreach (string path in Paths)
                texturePack.Add(LoadTextureByPath(Path.GetFullPath(path)));
            return texturePack.ToArray();
        }
        internal static TexturePack LoadTextureByPath(string path)
        {
            if (!File.Exists(path))
            {
                $"Invalid path : {path}".Log(LogType.Error);
                return null;
            }

            ZipArchive zipArchive = ZipFile.OpenRead(path);

            Stream PackageStreeam = zipArchive.Entries.First(x => x.Name == "package.json").Open();
            MemoryStream memoryStream = new MemoryStream();
            PackageStreeam.CopyTo(memoryStream);
            PackageStreeam.Close();
            byte[] bytes = memoryStream.ToArray();

            string PackageJson = BytesToString(bytes);
            Package Package = JsonConvert.DeserializeObject<Package>(PackageJson);

            ("Successfully loaded the texture packs info! Texture name:[" + Package.Name + "]").Log();
            "Moving on too loading the textures!".Log();

            Texture2D Ground = GetImage(zipArchive, Image.Ground);
            Texture2D PrimaryAtlas = GetImage(zipArchive, Image.Atlas);
            Texture2D TreeStump = GetImage(zipArchive, Image.TreeStump);
            Texture2D treeroomatlas = GetImage(zipArchive, Image.TreeRoomAtlas);
            TexturePack texturePack = new TexturePack(Package, Ground, PrimaryAtlas, TreeStump, treeroomatlas);
            zipArchive.Dispose();
            return texturePack;
        }

        private static string BytesToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        private static Texture2D GetImage(ZipArchive zipArchive, Image image)
        {
            string Name = image == 0 ? "ground.png" : "atlas.png";
            if ((int)image == 2)
                Name = "treestump.png";
            else if ((int) image == 3)
                Name = "treestumproom.png";

            Stream stream = zipArchive.GetEntry(Name).Open();
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            stream.Close();
            byte[] bytes = memoryStream.ToArray();

            (int, int) Rect = image == 0 ? (250, 2048) : (167, 2048);
            if ((int)image == 2 || (int)image == 3)
                Rect = (2048, 2048);
            Texture2D texture = new Texture2D(Rect.Item1, Rect.Item2);
            texture.LoadImage(bytes);
            return texture;
        }

        private enum Image
        {
            Ground,
            Atlas,
            TreeStump,
            TreeRoomAtlas
        }
    }
}