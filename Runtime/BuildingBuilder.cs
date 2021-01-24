using UnityEngine;

namespace SimpleBuildingRules {
    public class BuildingBuilder : MonoBehaviour {
        public BuildingRules buildingRules;
        public BuildingResult br;

        public void Build() {
            foreach(Transform child in transform) {
                DestroyImmediate(child.gameObject);
            }
            br = buildingRules.Build(gameObject, new Vector3(0,0,0));
        }

        private void OnDrawGizmosSelected() {
        }
    }
}
