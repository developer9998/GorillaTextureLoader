using System;

namespace TextureLoader.Models
{
    [Serializable]
    public class Package
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVerified { get; set; } // TODO: Implement this, any verified texture packs will be able to work in EVERY LOBBY. 

        public Package(string Name, string Description, bool isVerified)
        {
            this.Name = Name;
            this.Description = Description;
            IsVerified = isVerified;
        }
    }
}
