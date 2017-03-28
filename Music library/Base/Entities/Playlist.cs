using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Base
{
    [Serializable]
    public class Playlist
    {
        [XmlElement(nameof(Id))]
        public int Id { get; set; }

        [XmlElement(nameof(Name))]
        public string Name { get; set; }

        [XmlElement(nameof(Songs))]
        public List<string> Songs { get; set; }
    }
}
