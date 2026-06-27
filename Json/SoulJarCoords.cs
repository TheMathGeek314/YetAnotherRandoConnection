using System.Collections.Generic;
using UnityEngine;

namespace YetAnotherRandoConnection {
    public class SoulJarCoords {
        public static Dictionary<string, (string, Vector2)> placementToPosition = new();
        public static Dictionary<string, string> placementToName = new();
    }

    public class JsonSoulJarCoords {
        public string placement;
        public string scene;
        public string objectName;
        public float x;
        public float y;

        public void translate() {
            SoulJarCoords.placementToPosition.Add(placement, (scene, new Vector2(x, y)));
            SoulJarCoords.placementToName.Add(placement, objectName);
        }
    }
}