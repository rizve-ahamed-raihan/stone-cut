using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Automatically sets up proper hierarchy and configuration for stone cutting scene
/// Attach this to an empty GameObject called "Game Manager"
/// </summary>
public class SceneSetup : MonoBehaviour
{
    [Header("Setup Options")]
    [Tooltip("Click to organize hierarchy")]
    public bool organizeHierarchy = false;
    
    [Tooltip("Click to setup all stones in scene")]
    public bool setupStones = false;
    
    [Tooltip("Click to setup cutting tools")]
    public bool setupCuttingTools = false;

    void OnValidate()
    {
        #if UNITY_EDITOR
        if (organizeHierarchy)
        {
            organizeHierarchy = false;
            OrganizeHierarchy();
        }

        if (setupStones)
        {
            setupStones = false;
            SetupAllStones();
        }

        if (setupCuttingTools)
        {
            setupCuttingTools = false;
            SetupAllCuttingTools();
        }
        #endif
    }

    #if UNITY_EDITOR
    void OrganizeHierarchy()
    {
        Debug.Log("=== Organizing Hierarchy ===");

        // Create container GameObjects
        GameObject stonesContainer = GetOrCreateContainer("Stones");
        GameObject cuttingToolsContainer = GetOrCreateContainer("Cutting Tools");
        GameObject environmentContainer = GetOrCreateContainer("Environment");

        // Find and organize stones
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.parent != null) continue; // Skip children

            string name = obj.name.ToLower();

            // Organize stones
            if (name.Contains("stone") && !name.Contains("cutting") && !name.Contains("tool"))
            {
                if (obj.transform.parent != stonesContainer.transform)
                {
                    obj.transform.SetParent(stonesContainer.transform);
                    Debug.Log($"Moved {obj.name} to Stones container");
                }
            }
            // Organize cutting tools
            else if (name.Contains("cutting") || name.Contains("cutter") || 
                     obj.GetComponent<StoneModelCutter>() != null || 
                     obj.GetComponent<AutoStoneModelCutter>() != null)
            {
                if (obj.transform.parent != cuttingToolsContainer.transform)
                {
                    obj.transform.SetParent(cuttingToolsContainer.transform);
                    Debug.Log($"Moved {obj.name} to Cutting Tools container");
                }
            }
            // Organize environment
            else if (name.Contains("plane") || name.Contains("ground") || name.Contains("floor"))
            {
                if (obj.transform.parent != environmentContainer.transform)
                {
                    obj.transform.SetParent(environmentContainer.transform);
                    Debug.Log($"Moved {obj.name} to Environment container");
                }
            }
        }

        Debug.Log("Hierarchy organization complete!");
    }

    void SetupAllStones()
    {
        Debug.Log("=== Setting up all stones ===");

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int stoneCount = 0;

        foreach (GameObject obj in allObjects)
        {
            string name = obj.name.ToLower();
            if ((name.Contains("stone") || name.Contains("jade") || name.Contains("gem") || name.Contains("crystal")) 
                && !name.Contains("cutting") && !name.Contains("tool") && !name.Contains("cutter"))
            {
                SetupStone(obj);
                stoneCount++;
            }
        }

        Debug.Log($"Setup complete! Configured {stoneCount} stone(s)");
    }

    void SetupStone(GameObject stone)
    {
        // Check if it has a MeshFilter with mesh
        MeshFilter meshFilter = stone.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogWarning($"Skipping {stone.name} - no mesh found");
            return;
        }

        // Set tag (optional - not required for MHCutter)
        // Uncomment if using tags:
        // if (!stone.CompareTag("Stone"))
        // {
        //     stone.tag = "Stone";
        //     Debug.Log($"Set tag 'Stone' on {stone.name}");
        // }

        // Ensure MeshCollider exists (REQUIRED for MHCutter)
        MeshCollider meshCollider = stone.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = stone.AddComponent<MeshCollider>();
            meshCollider.convex = false; // Must be non-convex for complex stones
            Debug.Log($"Added MeshCollider to {stone.name}");
        }
        else if (meshCollider.convex)
        {
            meshCollider.convex = false;
            Debug.Log($"Set MeshCollider to non-convex on {stone.name}");
        }

        // Ensure MeshRenderer exists
        if (stone.GetComponent<MeshRenderer>() == null)
        {
            Debug.LogWarning($"{stone.name} has no MeshRenderer!");
        }

        Debug.Log($"✓ Stone setup complete: {stone.name}");
    }

    void SetupAllCuttingTools()
    {
        Debug.Log("=== Setting up cutting tools ===");

        StoneModelCutter[] manualCutters = FindObjectsOfType<StoneModelCutter>();
        foreach (StoneModelCutter cutter in manualCutters)
        {
            if (cutter.cuttingModel == null)
            {
                cutter.cuttingModel = cutter.gameObject;
                Debug.Log($"Auto-assigned cutting model to {cutter.gameObject.name}");
            }
        }

        AutoStoneModelCutter[] autoCutters = FindObjectsOfType<AutoStoneModelCutter>();
        foreach (AutoStoneModelCutter cutter in autoCutters)
        {
            SetupAutoCutter(cutter.gameObject);
        }

        Debug.Log($"Setup {manualCutters.Length} manual cutters and {autoCutters.Length} auto cutters");
    }

    void SetupAutoCutter(GameObject cutter)
    {
        // Ensure collider exists
        Collider col = cutter.GetComponent<Collider>();
        if (col == null)
        {
            BoxCollider boxCol = cutter.AddComponent<BoxCollider>();
            boxCol.isTrigger = true;
            Debug.Log($"Added trigger collider to {cutter.name}");
        }

        // Ensure rigidbody exists
        Rigidbody rb = cutter.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = cutter.AddComponent<Rigidbody>();
            rb.useGravity = false;
            Debug.Log($"Added Rigidbody to {cutter.name}");
        }
    }

    GameObject GetOrCreateContainer(string containerName)
    {
        GameObject container = GameObject.Find(containerName);
        if (container == null)
        {
            container = new GameObject(containerName);
            Debug.Log($"Created container: {containerName}");
        }
        return container;
    }
    #endif

    // Quick setup menu items
    #if UNITY_EDITOR
    [MenuItem("Tools/Stone Cutting/Setup Scene Hierarchy")]
    static void MenuOrganizeHierarchy()
    {
        SceneSetup setup = FindObjectOfType<SceneSetup>();
        if (setup == null)
        {
            GameObject manager = new GameObject("Game Manager");
            setup = manager.AddComponent<SceneSetup>();
        }
        setup.OrganizeHierarchy();
    }

    [MenuItem("Tools/Stone Cutting/Setup All Stones")]
    static void MenuSetupStones()
    {
        SceneSetup setup = FindObjectOfType<SceneSetup>();
        if (setup == null)
        {
            GameObject manager = new GameObject("Game Manager");
            setup = manager.AddComponent<SceneSetup>();
        }
        setup.SetupAllStones();
    }

    [MenuItem("Tools/Stone Cutting/Setup Cutting Tools")]
    static void MenuSetupCuttingTools()
    {
        SceneSetup setup = FindObjectOfType<SceneSetup>();
        if (setup == null)
        {
            GameObject manager = new GameObject("Game Manager");
            setup = manager.AddComponent<SceneSetup>();
        }
        setup.SetupAllCuttingTools();
    }
    #endif
}
