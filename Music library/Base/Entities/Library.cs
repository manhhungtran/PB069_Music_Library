using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Base
{
    [Serializable]
    public class Library
    {
        [XmlElement(nameof(Songs))]
        public HashSet<string> Songs { get; set; }

        [XmlElement(nameof(RootPath))]
        public HashSet<string> RootPath { get; set; }


        public Library()
        {
            Songs = new HashSet<string>();
            RootPath = new HashSet<string>();
        }
    }
}
