using UnityEngine;
using EzySlice;

public class StoneModelCutter : MonoBehaviour
{
    [Header("Cutting Tool")]
    public GameObject cuttingModel; // The 3D model used to cut the stone
    public Material stoneInsideMaterial; // Material for the inside of the cut stone
    
    [Header("Settings")]
    public LayerMask stoneLayer; // Layer for stone objects
    public float cuttingForce = 200f; // Force applied to cut pieces
    public bool destroyCuttingModel = true; // Whether to destroy the cutting model after cutting
    
    private bool isDragging = false;
    private Vector3 startPosition;

    void Update()
    {
        // Start dragging the cutting model
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == cuttingModel)
                {
                    isDragging = true;
                    startPosition = cuttingModel.transform.position;
                    Debug.Log("Started dragging cutting model");
                }
            }
        }

        // Drag the cutting model
        if (isDragging && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance = Vector3.Distance(Camera.main.transform.position, cuttingModel.transform.position);
            Vector3 point = ray.GetPoint(distance);
            cuttingModel.transform.position = point;
        }

        // Release and perform cut
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            PerformCut();
        }

        // Alternative: Press Space to cut with current cutting model position
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformCut();
        }
    }

    void PerformCut()
    {
        if (cuttingModel == null)
        {
            Debug.LogError("Cutting model is not assigned!");
            return;
        }

        // Find all stones in the scene
        Collider[] stones = Physics.OverlapSphere(cuttingModel.transform.position, 10f, stoneLayer);
        
        foreach (Collider stoneCollider in stones)
        {
            GameObject stone = stoneCollider.gameObject;
            
            // Check if the cutting model intersects with the stone
            if (IsIntersecting(cuttingModel, stone))
            {
                Debug.Log($"Cutting stone: {stone.name}");
                
                // Use the cutting model's orientation to determine the cutting plane
                Vector3 cutPosition = cuttingModel.transform.position;
                Vector3 cutNormal = cuttingModel.transform.up; // Use the model's up direction as cut plane
                
                SliceStone(stone, cutPosition, cutNormal);
            }
        }

        if (destroyCuttingModel && cuttingModel != null)
        {
            Destroy(cuttingModel);
            Debug.Log("Cutting model destroyed after cutting");
        }
    }

    bool IsIntersecting(GameObject obj1, GameObject obj2)
    {
        Bounds bounds1 = GetBounds(obj1);
        Bounds bounds2 = GetBounds(obj2);
        
        return bounds1.Intersects(bounds2);
    }

    Bounds GetBounds(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds;
        }
        
        // If no renderer, calculate bounds from collider
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            return collider.bounds;
        }
        
        return new Bounds(obj.transform.position, Vector3.one);
    }

    void SliceStone(GameObject stone, Vector3 position, Vector3 normal)
    {
        SlicedHull hull = stone.Slice(position, normal);

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(stone, stoneInsideMaterial);
            GameObject lowerHull = hull.CreateLowerHull(stone, stoneInsideMaterial);

            if (upperHull != null && lowerHull != null)
            {
                ConfigureSlicedPart(upperHull, position);
                ConfigureSlicedPart(lowerHull, position);
                
                Destroy(stone);
                Debug.Log($"Stone {stone.name} successfully sliced!");
            }
        }
        else
        {
            Debug.LogWarning($"Failed to slice stone {stone.name}");
        }
    }

    void ConfigureSlicedPart(GameObject part, Vector3 explosionCenter)
    {
        // Add mesh collider
        MeshCollider meshCollider = part.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        
        // Add rigidbody
        Rigidbody rb = part.AddComponent<Rigidbody>();
        rb.mass = 1f;
        
        // Set layer
        part.layer = LayerMask.NameToLayer("Sliceable");
        if (part.layer == -1) part.layer = 0; // Default layer if Sliceable doesn't exist
        
        // Add explosion force
        rb.AddExplosionForce(cuttingForce, explosionCenter, 2f);
    }

    // Gizmos to visualize cutting plane
    void OnDrawGizmos()
    {
        if (cuttingModel != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(cuttingModel.transform.position, 0.5f);
            
            // Draw cutting plane normal
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(cuttingModel.transform.position, 
                           cuttingModel.transform.position + cuttingModel.transform.up * 2f);
        }
    }
}
