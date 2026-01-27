using System.Collections.Generic;
using UnityEngine;

namespace YetAnotherRandoConnection {
    public class StalactiteCoords {
        public static Dictionary<(string, string), string> nameToPlacement = new();
        public static Dictionary<string, (string, Vector2)> placementToPosition = new();
    }

    public class JsonSpikeCoords {
        public string locationName;
        public string scene;
        public string objectName;
        public float x;
        public float y;

        public void translate() {
            if(!(scene == "Tutorial_01" && objectName == "Stalactite Hazard"))
                StalactiteCoords.nameToPlacement.Add((scene, objectName), locationName);
            StalactiteCoords.placementToPosition.Add(locationName, (scene, new Vector2(x, y)));
        }
    }
}
