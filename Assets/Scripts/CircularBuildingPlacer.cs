using UnityEngine;

public class CircularBuildingPlacer : MonoBehaviour
{
    public GameObject buildingPrefab;
    public LayerMask gridLayerMask;

    private Camera mainCamera;
    private GameObject currentBuilding;
    private CircularGrid grid;

    void Start()
    {
        mainCamera = Camera.main;
        grid = FindObjectOfType<CircularGrid>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBuilding();
        }

        if (currentBuilding != null)
        {
            MoveBuildingToMouse();
        }
    }

    public void PlaceBuilding()
    {
        if (currentBuilding == null)
        {
            currentBuilding = Instantiate(buildingPrefab);
        }
        else
        {
            currentBuilding = null;
        }
    }

    void MoveBuildingToMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayerMask))
        {
            Vector3 point = hit.point;
            currentBuilding.transform.position = grid.SnapToGrid(point);
        }
    }
}
