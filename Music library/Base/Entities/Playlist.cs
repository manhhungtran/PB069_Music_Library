using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    public class Playlist
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public List<string> Songs { get; set; }
    }
}
