using System.Collections.Generic;
using UnityEngine;

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
}