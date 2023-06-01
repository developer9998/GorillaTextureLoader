using UnityEngine;

namespace TextureLoader.Models
{
    internal class GameMaterials
    {
        internal Material ForestAtlasObj { get; set; }
        //internal Material ForestAtlas { get; set; }
        //internal Material Ground { get; set; }
        //internal Material Leaf { get; set; }
        internal Material TreeStumpAtlas { get; set; }
        internal Material TreeStrumpROOMAtlas { get; set; }
        internal GameMaterials(Material forestAtlasObj, /*Material forestAtlas, Material ground, Material leaf,*/ Material treeStumpAtlas, Material treeStrumpROOMAtlas)
        {
            ForestAtlasObj = forestAtlasObj;
            //ForestAtlas = forestAtlas;
            //Ground = ground;
            //Leaf = leaf;
            TreeStumpAtlas = treeStumpAtlas;
            TreeStrumpROOMAtlas = treeStrumpROOMAtlas;
        }
    }
}
