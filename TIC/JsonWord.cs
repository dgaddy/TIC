using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace TIC
{

    [DataContract]
    class JsonWord
    {
        [DataMember(Name = "text")]
        public string text { get; set; }

        [DataMember(Name = "x")]
        public int x { get; set; }

        [DataMember(Name = "y")]
        public int y { get; set; }
    }
}
