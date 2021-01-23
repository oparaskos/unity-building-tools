using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "Building/Rules", order = 1)]
public class BuildingRules : ScriptableObject
{
    public List<BuildingRules> subRules;

    public GameObject prefab;
    public Mesh mesh;
    public Axis upAxis = Axis.Y;
    public Axis outAxis = Axis.Z;
    public Axis horizontalAxis = Axis.X;
    public Direction buildDirection = Direction.HORIZONTAL_RIGHT;
    public int size = 10;

    public void Check() {
        Check(this);
    }
    public void Check(BuildingRules against) {
        foreach ( BuildingRules rule in subRules) {
            if (rule == against) {
                throw new System.Exception("Recursive building rules not supported!");
            }
            rule.Check(against);
            rule.Check(this);
        }
    }

    public BuildingResult Build(GameObject parent, Vector3 initialPosition) {
        Check();
        GameObject subRulesContainerObject = new GameObject(this.name);
        subRulesContainerObject.transform.parent = parent.transform;
        subRulesContainerObject.transform.localPosition = initialPosition;
        Vector3 position = new Vector3(0, 0, 0);
        Bounds bounds = new Bounds(position, new Vector3(0, 0, 0));
        BuildingResult br = new BuildingResult();
        for(var i = 0; i < size; ++i) {
            if (subRules != null && subRules.Count > 0) {
                foreach (BuildingRules subRule in subRules) {
                    BuildingResult nextResult = subRule.Build(subRulesContainerObject, position);
                    br.childResults.Add(nextResult);
                    bounds = CombineBounds(
                        new Bounds(bounds.center, bounds.size),
                        new Bounds(nextResult.bounds.center + position, nextResult.bounds.size));
                    position = Vector3.Scale(bounds.max - bounds.min, BuildDirectionFilter());
                }
            }

            if (mesh != null) {
                GameObject meshRenderer = new GameObject(
                    mesh.name + " Mesh"
                );
                meshRenderer.AddComponent<MeshFilter>().mesh = mesh;
                meshRenderer.AddComponent<MeshRenderer>();
                meshRenderer.AddComponent<BoxCollider>();
                meshRenderer.transform.parent = subRulesContainerObject.transform;
                meshRenderer.transform.localPosition = new Vector3(0, 0, 0);
                bounds = CombineBounds(bounds, mesh.bounds);
            }

            if (prefab != null) {
                GameObject meshRenderer = Instantiate(prefab);
                Renderer r = meshRenderer.GetComponent<Renderer>();
                meshRenderer.transform.parent = subRulesContainerObject.transform;
                meshRenderer.transform.localPosition = new Vector3(0, 0, 0);
                if (r != null) {
                    bounds = CombineBounds(bounds, r.bounds);
                }
            }
        }

        return new BuildingResult {
            bounds = bounds,
            entity = subRulesContainerObject
        };
    }

    public Vector3 FilterAxis(Vector3 vector, Axis axis) {
        switch(axis) {
            case Axis.X:
                return new Vector3(vector.x, 0, 0);
            case Axis.Y:
                return new Vector3(0, vector.y, 0);
            case Axis.Z:
                return new Vector3(0, 0, vector.z);
            default:
                throw new System.Exception();
        }
    }

    public Vector3 BuildDirectionFilter() {
        Vector3 one = new Vector3(1, 1, 1);
        switch (buildDirection) {
            case Direction.HORIZONTAL_RIGHT:
                return FilterAxis(one, horizontalAxis);
            case Direction.HORIZONTAL_LEFT:
                return FilterAxis(-one, horizontalAxis);
            case Direction.OUTWARD:
                return FilterAxis(one, outAxis);
            case Direction.INWARD:
                return FilterAxis(-one, outAxis);
            case Direction.VERTICAL_UP:
                return FilterAxis(one, upAxis);
            case Direction.VERTICAL_DOWN:
                return FilterAxis(-one, upAxis);
            default:
                throw new System.Exception();
        }
    }

    public Bounds CombineBounds(Bounds a, Bounds b) {
        Vector3 extentsMin = new Vector3(
            Mathf.Min(a.min.x, b.min.x),
            Mathf.Min(a.min.y, b.min.y),
            Mathf.Min(a.min.z, b.min.z)
        );
        Vector3 extentsMax = new Vector3(
            Mathf.Max(a.max.x, b.max.x),
            Mathf.Max(a.max.y, b.max.y),
            Mathf.Max(a.max.z, b.max.z)
        );
        Vector3 center = Vector3.Lerp(extentsMin, extentsMax, 0.5f);
        return new Bounds(center, extentsMax - extentsMin);
    }
}
