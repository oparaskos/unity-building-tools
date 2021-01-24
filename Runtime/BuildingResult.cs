using UnityEngine;
using System.Collections.Generic;

namespace SimpleBuildingRules {
    public class BuildingResult {
        public Bounds bounds;
        public GameObject entity;

        public List<BuildingResult> childResults = new List<BuildingResult>();
    }
}
