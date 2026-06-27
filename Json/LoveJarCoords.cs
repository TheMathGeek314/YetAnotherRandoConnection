using System.Collections.Generic;
using UnityEngine;

namespace YetAnotherRandoConnection {
    public class LoveJarCoords {
        public static Dictionary<string, (string, Vector2)> nameToPosition = new();
        public static Dictionary<string, string> nameToObjectName = new();
    }

    public class JsonLoveJarCoords {
        public string name;
        public string scene;
        public string objectName;
        public float x;
        public float y;

        public void translate() {
            LoveJarCoords.nameToPosition.Add(name, (scene, new Vector2(x, y)));
            LoveJarCoords.nameToObjectName.Add(name, objectName);
        }
    }
}