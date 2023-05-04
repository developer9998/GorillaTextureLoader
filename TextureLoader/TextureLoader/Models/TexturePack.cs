using UnityEngine;

namespace TextureLoader.Models
{
    internal class TexturePack
    {
        public Package package;

        public Texture2D ground;
        public Texture2D atlas;
        public Texture2D TreeTexture;
        public Texture2D treeroomatlas;
        public TexturePack(Package package, Texture2D ground, Texture2D atlas, Texture2D TreeTexture, Texture2D treeroomatlas)
        {
            this.package = package;
            this.ground = ground;
            this.atlas = atlas;
            this.TreeTexture = TreeTexture;
            this.treeroomatlas = treeroomatlas;
        }
    }
}
