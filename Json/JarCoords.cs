using System.Collections.Generic;
using UnityEngine;

namespace YetAnotherRandoConnection {
    public class JarCoords {
        public static Dictionary<string, (string, Vector2)> placementToPosition = new();
        //public static Dictionary<string, string> nameToPlacement = new();
        public static Dictionary<string, string> placementToName = new();
    }

    public class JsonJarCoords {
        public string placement;
        public string scene;
        public string objectName;
        public float x;
        public float y;

        public void translate() {
            JarCoords.placementToPosition.Add(placement, (scene, new Vector2(x, y)));
            //JarCoords.nameToPlacement.Add($"{scene}/{objectName}", placement);
            JarCoords.placementToName.Add(placement, objectName);
        }
    }
}
