using System.Collections.Generic;
using UnityEngine;

namespace YetAnotherRandoConnection {
    public class EggBombCoords {
        public static Dictionary<string, string> nameToPlacement = new();
        public static Dictionary<string, (string, Vector2)> placementToPosition = new();
    }

    public class JsonBombCoords {
        public string name;
        public string scene;
        public string objectName;
        public float x;
        public float y;

        public void translate() {
            EggBombCoords.nameToPlacement.Add($"{scene}/{objectName}", name);
            EggBombCoords.placementToPosition.Add(name, (scene, new Vector2(x, y)));
        }
    }
}
