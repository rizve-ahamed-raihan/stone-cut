using UnityEngine;
using com.marufhow.meshslicer.core;

/// <summary>
/// Simple version - Click on stone with cutting tool to slice it
/// Attach to your cutting tool 3D model (sword, knife, blade, etc.)
/// </summary>
public class SimpleStoneCutter : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private MHCutter meshCutter;
    
    [Header("Settings")]
    [SerializeField] private LayerMask cuttableLayer;
    [SerializeField] private bool followMouse = true;
    [SerializeField] private float followSpeed = 15f;
    [SerializeField] private float cutDistance = 2f;
    
    [Header("Cut Direction")]
    [Tooltip("Which local axis of this tool defines the cutting direction")]
    [SerializeField] private Vector3 cutDirectionAxis = Vector3.forward;
    
    private Camera mainCamera;
    private bool canCut = true;
    
    void Start()
    {
        mainCamera = Camera.main;
        
        if (meshCutter == null)
        {
            // Try to find MHCutter in scene
            meshCutter = FindObjectOfType<MHCutter>();
            
            if (meshCutter == null)
            {
                Debug.LogError("MHCutter not found! Please assign it or add to scene.");
            }
        }
        
        // Ensure this tool has a collider
        if (GetComponent<Collider>() == null)
        {
            BoxCollider col = gameObject.AddComponent<BoxCollider>();
            col.isTrigger = true;
            Debug.Log("Added trigger collider to cutting tool");
        }
    }
    
    void Update()
    {
        if (followMouse)
        {
            FollowMousePosition();
        }
        
        if (Input.GetMouseButtonDown(0) && canCut)
        {
            TryCutStone();
        }
        
        // Alternative: Hold Shift and click to cut
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            TryCutNearestStone();
        }
    }
    
    void FollowMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        // Project to a plane at the tool's current depth
        float depth = Vector3.Distance(mainCamera.transform.position, transform.position);
        Vector3 targetPosition = ray.GetPoint(depth);
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }
    
    void TryCutStone()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100f, cuttableLayer))
        {
            CutStone(hit.collider.gameObject, hit.point);
        }
        else
        {
            Debug.Log("No stone found at click position");
        }
    }
    
    void TryCutNearestStone()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, cutDistance, cuttableLayer);
        
        if (nearbyObjects.Length > 0)
        {
            // Cut the nearest stone
            GameObject nearest = nearbyObjects[0].gameObject;
            Vector3 cutPoint = nearbyObjects[0].ClosestPoint(transform.position);
            CutStone(nearest, cutPoint);
        }
        else
        {
            Debug.Log("No stone nearby to cut");
        }
    }
    
    void CutStone(GameObject stone, Vector3 cutPoint)
    {
        if (meshCutter == null)
        {
            Debug.LogError("Cannot cut - MHCutter is not assigned!");
            return;
        }
        
        // Get cut direction from tool's orientation
        Vector3 cutDirection = transform.TransformDirection(cutDirectionAxis);
        
        Debug.Log($"Cutting {stone.name} at {cutPoint} with direction {cutDirection}");
        
        try
        {
            meshCutter.Cut(stone, cutPoint, cutDirection);
            Debug.Log("✓ Stone cut successfully!");
            
            // Optional: Add visual/audio feedback here
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Cut failed: {e.Message}");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Optional: Automatic cutting when tool enters stone
        if (((1 << other.gameObject.layer) & cuttableLayer) != 0)
        {
            Debug.Log($"Cutting tool entered: {other.gameObject.name}");
            // Uncomment to enable auto-cut on collision:
            // CutStone(other.gameObject, other.ClosestPoint(transform.position));
        }
    }
    
    void OnDrawGizmos()
    {
        // Show cut direction
        Gizmos.color = Color.yellow;
        Vector3 cutDir = Application.isPlaying ? 
            transform.TransformDirection(cutDirectionAxis) : 
            transform.forward;
        Gizmos.DrawRay(transform.position, cutDir * 2f);
        
        // Show cut range
        Gizmos.color = new Color(1, 1, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, cutDistance);
    }
}
