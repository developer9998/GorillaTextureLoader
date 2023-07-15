using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using TextureLoader.Models;
using UnityEngine;

namespace TextureLoader
{
    internal static class TextureController
    {
        internal static Package LoadedPackage;
        internal static bool GetTextureLoaded()
        {
            if (GetAllMaterials() == null)
                return false;
            return LoadedPackage != null;
        }
        internal static Package GetPackage(string URL)
        {
            string path = Path.GetFullPath(URL);
            using (Stream stream = File.OpenRead(path))
            using (ZipArchive archive = new ZipArchive(stream))
            {
                var packageEntry = archive.GetEntry("package.json");
                if (packageEntry == null)
                    return null;
                using (StreamReader reader = new StreamReader(packageEntry.Open()))
                {
                    string json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<Package>(json);
                }
            }
        }
        internal static void LoadTexture(string URL, Package package)
        {
            LoadedPackage = package;

            string path = Path.GetFullPath(URL);
            Stream FileStream = File.OpenRead(path); // never nesting;)
            ZipArchive zipArchive = new ZipArchive(FileStream);

            /* Get all textures from the pack */

            List<Texture2D> MaterialEntries = new List<Texture2D>();
            int EntryCount = zipArchive.Entries.Count;
            for (int i = 0; i < EntryCount; i++)
                if (zipArchive.Entries[i].Name != "package.json")
                {
                    ("Adding Texture:" + zipArchive.Entries[i].Name + " to the list").Log();
                    Texture2D texture2D = ConvertToTexture(zipArchive.Entries[i]);
                    texture2D.filterMode = FilterMode.Point;
                    MaterialEntries.Add(texture2D);
                }
                else;
            // If the texturepack does not contain the leaf texture(which is a option) | handler
            if (EntryCount == 6) ;
            else
            {
                "Inserting placeholder leaf texture".Log(BepInEx.Logging.LogLevel.Message);
                // var DefaultLeafTexture = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "cherryblossomssmall").mainTexture as Texture2D;
                var DefaultLeafTexture = Resources.FindObjectsOfTypeAll<Material>().First().mainTexture as Texture2D;
                MaterialEntries.Insert(2, DefaultLeafTexture);
            }

            /* Set all textures + ending */

            SetTextures(MaterialEntries);
            FileStream.Close();
            zipArchive.Dispose();
        }
        /*
        The primary atlas(forestatlas) now only has one element(material),
        So everything is now in the forestatlasobj (exept for the tree stumps)
        */
        private static void SetTextures(List<Texture2D> NewTextures)
        {
            $"Setting textures | List:[{NewTextures.Count}]".Log();
            var AllMaterials = GetAllMaterials();
            //AllMaterials.ForestAtlas.mainTexture = NewTextures[0];
            AllMaterials.ForestAtlasObj.mainTexture = NewTextures[0];
            //AllMaterials.Ground.mainTexture = NewTextures[1];
            //AllMaterials.Leaf.mainTexture = NewTextures[2];
            AllMaterials.TreeStumpAtlas.mainTexture = NewTextures[3];
            AllMaterials.TreeStrumpROOMAtlas.mainTexture = NewTextures[4];
        }
        private static void SetTextures(GameMaterials gameMaterials)
        {
            var AllMaterials = GetAllMaterials();
            //AllMaterials.ForestAtlas.mainTexture = gameMaterials.ForestAtlas.mainTexture;
            AllMaterials.ForestAtlasObj.mainTexture = gameMaterials.ForestAtlasObj.mainTexture;
            //AllMaterials.Ground.mainTexture = gameMaterials.Ground.mainTexture;
            //AllMaterials.Leaf.mainTexture = gameMaterials.Leaf.mainTexture;
            AllMaterials.TreeStumpAtlas.mainTexture = gameMaterials.TreeStumpAtlas.mainTexture;
            AllMaterials.TreeStrumpROOMAtlas.mainTexture = gameMaterials.TreeStrumpROOMAtlas.mainTexture;
        }

        private static Texture2D ConvertToTexture(ZipArchiveEntry Entry)
        {
            $"Starting convertion of {Entry.Name}".Log();
            Stream EntryStream = Entry.Open();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                EntryStream.CopyTo(memoryStream);
                memoryStream.Position = 0;
                Texture2D NewTexture = new Texture2D(2, 2);
                NewTexture.LoadImage(memoryStream.ToArray());
                "Finished converting!".Log();
                return NewTexture;
            }
        }

        internal static void ResetTextures()
        {
            SetTextures(DefaultMaterials);
            LoadedPackage = null;
        }

        /* Material fetching */

        private static GameMaterials DefaultMaterials;
        private static GameMaterials _AllMaterials;
        internal static GameMaterials GetAllMaterials()
        {
            if (_AllMaterials == null)
            {
                GameObject MainForestMesh = GameObject.FindObjectsOfType<GameObject>().First(x => x.name == "Uncover ForestCombined-mesh-mesh"); // This fixes the forest atlas not being changed

                Material ForestAtlasObj = MainForestMesh.GetComponent<MeshRenderer>().materials[0];
                ForestAtlasObj.Log();
                /*Material Ground = MainForestMesh.GetComponent<MeshRenderer>().materials[2];
                Ground.Log();
                Material ForestAtlas = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "forestatlas");
                ForestAtlas.Log();*/
                //Material Leaf = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "cherryblossomssmall");

                Material TreeStump = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "Tree Texture Baker-mat");
                Material TreeRoom = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "treeroomatlas");
                TreeRoom.Log();
                _AllMaterials = new GameMaterials(ForestAtlasObj, /*ForestAtlas, Ground*/ /*Leaf,*/ TreeStump, TreeRoom);
                DefaultMaterials = new GameMaterials(UnityEngine.Object.Instantiate(ForestAtlasObj), /*UnityEngine.Object.Instantiate(ForestAtlas), UnityEngine.Object.Instantiate(Ground),*/ /*UnityEngine.Object.Instantiate(Leaf),*/ UnityEngine.Object.Instantiate(TreeStump), UnityEngine.Object.Instantiate(TreeRoom));
                "Finished retrieving all materials!".Log(BepInEx.Logging.LogLevel.Message);
            }
            return _AllMaterials;
        }
    }
}
