/*
    I got the idea to convert everything to a ZIP file from MonkeMapLoader. 
*/
using System;

namespace TextureLoader.Models
{
    [Serializable]
    public class Package
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsVerified { get; private set; }

        public Package(string Name, string Description, bool isVerified)
        {
            this.Name = Name;
            this.Description = Description;
            IsVerified = isVerified;
        }
    }
}
