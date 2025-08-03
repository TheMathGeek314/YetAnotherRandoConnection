using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Newtonsoft.Json;

namespace YetAnotherRandoConnection {
    public class DreamOrbCoords {
        public static Dictionary<string, Vector2> data = new();
    }

    public class JsonDreamOrbCoords {
        public string area;
        public int num;
        public float x;
        public float y;

        public void translate() {
            DreamOrbCoords.data.Add($"DreamOrb_{area}_{num}", new Vector2(x, y));
        }
    }

    public class ParseOrbJson {
        private readonly string _jsonFilePath;
        private readonly Stream _jsonStream;
        private bool isPath;

        public ParseOrbJson(string jsonFilePath) {
            _jsonFilePath = jsonFilePath;
            isPath = true;
        }

        public ParseOrbJson(Stream jsonStream) {
            _jsonStream = jsonStream;
            isPath = false;
        }

        public List<JsonDreamOrbCoords> parseCoords() {
            if(isPath) {
                using StreamReader reader = new(_jsonFilePath);
                var json = reader.ReadToEnd();
                List<JsonDreamOrbCoords> orbCoords = JsonConvert.DeserializeObject<List<JsonDreamOrbCoords>>(json);
                return orbCoords;
            }
            else {
                using StreamReader reader = new(_jsonStream);
                var json = reader.ReadToEnd();
                List<JsonDreamOrbCoords> orbCoords = JsonConvert.DeserializeObject<List<JsonDreamOrbCoords>>(json);
                return orbCoords;
            }
        }
    }
}
