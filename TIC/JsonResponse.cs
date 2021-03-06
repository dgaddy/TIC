﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Drawing;


namespace TIC
{
    class JsonResponse
    {


        public JsonWord[][] ParseJson(String json) {

            MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(JsonWord[][]));
            JsonWord[][] response = (JsonWord[][])jsonSerializer.ReadObject(stream);
            return response;
        }

        public List<Point> getLocFromWord(String text, JsonWord[][] response) {
            List<Point> points = new List<Point>();
            for (int i = 0; i < response.Length; i++) {
                for (int j = 0; j < response[i].Length; j++) {
                    JsonWord word = response[i][j];
                    if (word.text.ToLower() == text.ToLower()) {
                        points.Add(new Point(word.x, word.y));
                    }
                }
            }
            return points;
        }

    }
}
