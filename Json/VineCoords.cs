using System.Collections.Generic;
using UnityEngine;

namespace YetAnotherRandoConnection {
    public class VineCoords {
        public static Dictionary<string, string> nameToPlacement = new();
        public static Dictionary<string, (string, Vector2)> placementToPosition = new();
        public static Dictionary<string, string> placementToName = new();
    }

    public class JsonVineCoords {
        public string scene;
        public string parent;
        public string name;
        public float x;
        public float y;

        public void translate() {
            VineCoords.nameToPlacement.Add($"{scene}/{parent}", name);
            VineCoords.placementToPosition.Add(name, (scene, new Vector2(x, y)));
            VineCoords.placementToName.Add(name, parent);
        }
    }
}
