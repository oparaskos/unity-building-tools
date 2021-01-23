using UnityEngine;
using System.Collections.Generic;

public class BuildingResult {
    public Bounds bounds;
    public GameObject entity;

    public List<BuildingResult> childResults = new List<BuildingResult>();
}