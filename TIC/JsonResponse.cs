using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Json.DataContractJsonSerializer;
using System.IO;


namespace TIC
{
    class JsonResponse
    {
        public JsonWord[,] ParseJson(String json) {
            MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(JsonWord[,]));
            JsonWord[,] response = (JsonWord[,])jsonSerializer.ReadObject(stream);
            return response;
        }
    }
}
