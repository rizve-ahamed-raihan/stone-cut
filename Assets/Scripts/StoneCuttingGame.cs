using UnityEngine;
using com.marufhow.meshslicer.core;

/// <summary>
/// Complete Stone Cutting Game Controller
/// Uses 3D models to cut stones with realistic physics
/// </summary>
public class StoneCuttingGame : MonoBehaviour
{
    [Header("Cutting System")]
    [SerializeField] private MHCutter meshCutter;
    [SerializeField] private GameObject cuttingTool; // The 3D model used for cutting (sword, knife, blade, etc.)
    [SerializeField] private LayerMask stoneLayer;
    
    [Header("Cutting Settings")]
    [SerializeField] private float dragSpeed = 10f;
    [SerializeField] private float cutForce = 5f;
    [SerializeField] private bool showCuttingPlaneGizmo = true;
    
    [Header("Game Settings")]
    [SerializeField] private int cutsRemaining = 10;
    [SerializeField] private int score = 0;
    [SerializeField] private bool unlimitedCuts = true;
    
    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip cutSound;
    [SerializeField] private AudioClip missSound;
    
    private bool isDragging = false;
    private Vector3 dragStartPos;
    private Vector3 dragEndPos;
    private Camera mainCamera;
    private AudioSource audioSource;
    
    void Start()
    {
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        
        if (meshCutter == null)
        {
            meshCutter = GetComponent<MHCutter>();
            if (meshCutter == null)
            {
                meshCutter = gameObject.AddComponent<MHCutter>();
            }
        }
        
        if (cuttingTool != null)
        {
            cuttingTool.SetActive(false); // Start hidden
        }
        
        Debug.Log("Stone Cutting Game Initialized!");
        Debug.Log($"Cuts Remaining: {(unlimitedCuts ? "Unlimited" : cutsRemaining.ToString())}");
    }
    
    void Update()
    {
        HandleInput();
    }
    
    void HandleInput()
    {
        // Start dragging
        if (Input.GetMouseButtonDown(0))
        {
            StartCuttingDrag();
        }
        
        // Continue dragging
        if (Input.GetMouseButton(0) && isDragging)
        {
            UpdateCuttingDrag();
        }
        
        // Release and perform cut
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            PerformCut();
        }
        
        // Keyboard shortcuts
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }
    
    void StartCuttingDrag()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100f, stoneLayer))
        {
            isDragging = true;
            dragStartPos = hit.point;
            
            if (cuttingTool != null)
            {
                cuttingTool.SetActive(true);
                cuttingTool.transform.position = hit.point;
            }
            
            Debug.Log("Started cutting drag on: " + hit.collider.gameObject.name);
        }
    }
    
    void UpdateCuttingDrag()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        // Project ray to a plane at the drag start position
        Plane dragPlane = new Plane(mainCamera.transform.forward, dragStartPos);
        float enter;
        
        if (dragPlane.Raycast(ray, out enter))
        {
            dragEndPos = ray.GetPoint(enter);
            
            if (cuttingTool != null)
            {
                // Position and rotate cutting tool along drag path
                Vector3 midPoint = (dragStartPos + dragEndPos) / 2f;
                cuttingTool.transform.position = Vector3.Lerp(
                    cuttingTool.transform.position, 
                    midPoint, 
                    Time.deltaTime * dragSpeed
                );
                
                // Rotate to face drag direction
                Vector3 dragDirection = (dragEndPos - dragStartPos).normalized;
                if (dragDirection != Vector3.zero)
                {
                    cuttingTool.transform.rotation = Quaternion.LookRotation(dragDirection, Vector3.up);
                }
            }
        }
    }
    
    void PerformCut()
    {
        if (!unlimitedCuts && cutsRemaining <= 0)
        {
            Debug.LogWarning("No cuts remaining!");
            PlaySound(missSound);
            isDragging = false;
            if (cuttingTool != null) cuttingTool.SetActive(false);
            return;
        }
        
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100f, stoneLayer))
        {
            GameObject targetStone = hit.collider.gameObject;
            Vector3 cutPoint = hit.point;
            
            // Calculate cut direction from drag
            Vector3 cutDirection = (dragEndPos - dragStartPos).normalized;
            if (cutDirection == Vector3.zero)
            {
                cutDirection = Vector3.right; // Default direction if no drag
            }
            
            // Perform the actual mesh cut
            Debug.Log($"Cutting {targetStone.name} at {cutPoint} with direction {cutDirection}");
            
            try
            {
                meshCutter.Cut(targetStone, cutPoint, cutDirection);
                
                // Success!
                score += 10;
                if (!unlimitedCuts) cutsRemaining--;
                
                PlaySound(cutSound);
                Debug.Log($"Cut successful! Score: {score}, Cuts remaining: {(unlimitedCuts ? "Unlimited" : cutsRemaining.ToString())}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Cut failed: {e.Message}");
                PlaySound(missSound);
            }
        }
        else
        {
            Debug.Log("Cut missed - no stone hit");
            PlaySound(missSound);
        }
        
        isDragging = false;
        if (cuttingTool != null) cuttingTool.SetActive(false);
    }
    
    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    
    void ResetGame()
    {
        score = 0;
        cutsRemaining = 10;
        Debug.Log("Game Reset!");
        
        // Reload scene to reset stones
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
    
    void OnGUI()
    {
        // Simple UI
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 24;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.UpperLeft;
        
        GUI.Label(new Rect(10, 10, 300, 30), $"Score: {score}", style);
        
        if (!unlimitedCuts)
        {
            GUI.Label(new Rect(10, 40, 300, 30), $"Cuts: {cutsRemaining}", style);
        }
        
        GUIStyle helpStyle = new GUIStyle(GUI.skin.label);
        helpStyle.fontSize = 14;
        helpStyle.normal.textColor = Color.yellow;
        helpStyle.alignment = TextAnchor.UpperRight;
        
        GUI.Label(new Rect(Screen.width - 310, 10, 300, 80), 
            "Click & Drag to cut stones\nR - Reset Game", helpStyle);
    }
    
    void OnDrawGizmos()
    {
        if (showCuttingPlaneGizmo && isDragging)
        {
            // Draw cutting line
            Gizmos.color = Color.red;
            Gizmos.DrawLine(dragStartPos, dragEndPos);
            Gizmos.DrawSphere(dragStartPos, 0.1f);
            Gizmos.DrawSphere(dragEndPos, 0.1f);
            
            // Draw cutting plane
            Vector3 cutDirection = (dragEndPos - dragStartPos).normalized;
            Vector3 midPoint = (dragStartPos + dragEndPos) / 2f;
            
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            DrawPlaneGizmo(midPoint, cutDirection);
        }
    }
    
    void DrawPlaneGizmo(Vector3 position, Vector3 normal)
    {
        Vector3 up = Vector3.up;
        if (Mathf.Abs(Vector3.Dot(normal, up)) > 0.99f)
        {
            up = Vector3.right;
        }
        
        Vector3 right = Vector3.Cross(normal, up).normalized;
        up = Vector3.Cross(right, normal).normalized;
        
        float size = 2f;
        Vector3 p1 = position + (right + up) * size;
        Vector3 p2 = position + (right - up) * size;
        Vector3 p3 = position + (-right - up) * size;
        Vector3 p4 = position + (-right + up) * size;
        
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }
}
