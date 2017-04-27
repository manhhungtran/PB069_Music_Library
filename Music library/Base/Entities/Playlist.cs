using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Base
{
    [Serializable]
    public class Playlist
    {
        [XmlElement(nameof(Name))]
        public string Name { get; set; }

        [XmlElement(nameof(Songs))]
        public List<string> Songs { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
