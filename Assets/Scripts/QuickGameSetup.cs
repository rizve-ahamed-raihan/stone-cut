using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// One-click stone cutting game setup
/// Run this from the menu to instantly configure your scene
/// </summary>
public class QuickGameSetup : MonoBehaviour
{
    #if UNITY_EDITOR
    [MenuItem("Tools/Stone Cutting/🚀 Quick Game Setup (Recommended)")]
    static void QuickSetup()
    {
        Debug.Log("=== Starting Quick Game Setup ===");
        
        // Step 1: Create/Find Game Manager
        GameObject gameManager = GameObject.Find("Game Manager");
        if (gameManager == null)
        {
            gameManager = new GameObject("Game Manager");
            Debug.Log("✓ Created Game Manager");
        }
        
        // Step 2: Add MHCutter
        var cutter = gameManager.GetComponent<com.marufhow.meshslicer.core.MHCutter>();
        if (cutter == null)
        {
            cutter = gameManager.AddComponent<com.marufhow.meshslicer.core.MHCutter>();
            Debug.Log("✓ Added MHCutter component");
        }
        
        // Step 3: Add StoneCuttingGame script
        var game = gameManager.GetComponent<StoneCuttingGame>();
        if (game == null)
        {
            game = gameManager.AddComponent<StoneCuttingGame>();
            Debug.Log("✓ Added StoneCuttingGame script");
        }
        
        // Step 4: Setup all stones in scene
        SetupAllStonesInScene();
        
        // Step 5: Organize hierarchy
        OrganizeSceneHierarchy();
        
        Debug.Log("=== ✅ Quick Setup Complete! ===");
        Debug.Log("Next steps:");
        Debug.Log("1. Press Play");
        Debug.Log("2. Click and drag on a stone to cut it!");
        Debug.Log("3. (Optional) Assign a Cutting Tool 3D model in Game Manager inspector");
        
        // Select the game manager so user can see it
        Selection.activeGameObject = gameManager;
    }
    
    [MenuItem("Tools/Stone Cutting/🎨 Create Sample Scene")]
    static void CreateSampleScene()
    {
        Debug.Log("=== Creating Sample Stone Cutting Scene ===");
        
        // Create camera if none exists
        if (Camera.main == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            Camera cam = camObj.AddComponent<Camera>();
            camObj.tag = "MainCamera";
            camObj.AddComponent<AudioListener>();
            camObj.transform.position = new Vector3(0, 2, -8);
            camObj.transform.rotation = Quaternion.Euler(15, 0, 0);
            Debug.Log("✓ Created Main Camera");
        }
        
        // Create directional light if none exists
        if (GameObject.FindObjectOfType<Light>() == null)
        {
            GameObject lightObj = new GameObject("Directional Light");
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);
            Debug.Log("✓ Created Directional Light");
        }
        
        // Create ground plane
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = Vector3.one * 2;
        Debug.Log("✓ Created Ground");
        
        // Try to add stone from assets
        TryAddStoneModel();
        
        // Now run quick setup
        QuickSetup();
        
        Debug.Log("=== ✅ Sample Scene Created! ===");
    }
    
    static void SetupAllStonesInScene()
    {
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        int stoneCount = 0;
        
        foreach (GameObject obj in allObjects)
        {
            string name = obj.name.ToLower();
            if ((name.Contains("stone") || name.Contains("jade") || 
                 name.Contains("gem") || name.Contains("crystal")) &&
                !name.Contains("cutting") && !name.Contains("tool") &&
                !name.Contains("cutter") && !name.Contains("manager"))
            {
                MeshFilter mf = obj.GetComponent<MeshFilter>();
                if (mf != null && mf.sharedMesh != null)
                {
                    // Add mesh collider
                    MeshCollider mc = obj.GetComponent<MeshCollider>();
                    if (mc == null)
                    {
                        mc = obj.AddComponent<MeshCollider>();
                        mc.convex = false;
                    }
                    
                    stoneCount++;
                }
            }
        }
        
        if (stoneCount > 0)
        {
            Debug.Log($"✓ Setup {stoneCount} stone(s) in scene");
        }
        else
        {
            Debug.LogWarning("No stones found in scene. Drag a stone model from Assets/3d Models/Stone/ or Assets/Free/Stones/");
        }
    }
    
    static void OrganizeSceneHierarchy()
    {
        GameObject stonesContainer = GetOrCreateObject("Stones");
        GameObject envContainer = GetOrCreateObject("Environment");
        
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.parent != null) continue;
            
            string name = obj.name.ToLower();
            
            if ((name.Contains("stone") || name.Contains("jade") || 
                 name.Contains("gem") || name.Contains("crystal")) &&
                !name.Contains("cutting") && !name.Contains("manager"))
            {
                obj.transform.SetParent(stonesContainer.transform);
            }
            else if (name.Contains("plane") || name.Contains("ground") || name.Contains("floor"))
            {
                obj.transform.SetParent(envContainer.transform);
            }
        }
        
        Debug.Log("✓ Organized scene hierarchy");
    }
    
    static GameObject GetOrCreateObject(string name)
    {
        GameObject obj = GameObject.Find(name);
        if (obj == null)
        {
            obj = new GameObject(name);
        }
        return obj;
    }
    
    static void TryAddStoneModel()
    {
        // Try to find stone assets
        string[] guids = AssetDatabase.FindAssets("t:GameObject stone", new[] { "Assets" });
        
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            GameObject stonePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
            if (stonePrefab != null)
            {
                GameObject stone = PrefabUtility.InstantiatePrefab(stonePrefab) as GameObject;
                stone.name = "Stone";
                stone.transform.position = new Vector3(0, 2, 0);
                Debug.Log($"✓ Added stone model: {stone.name}");
                return;
            }
        }
        
        // Fallback: Create a cube as stone
        GameObject cubeStone = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeStone.name = "Stone";
        cubeStone.transform.position = new Vector3(0, 2, 0);
        cubeStone.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        Debug.Log("✓ Created cube as sample stone (Replace with your 3D model)");
    }
    
    [MenuItem("Tools/Stone Cutting/📚 Open Setup Guide")]
    static void OpenSetupGuide()
    {
        string guidePath = "Assets/GAME_SETUP_GUIDE.md";
        if (System.IO.File.Exists(guidePath))
        {
            EditorUtility.OpenWithDefaultApp(guidePath);
        }
        else
        {
            Debug.LogWarning("Setup guide not found at: " + guidePath);
        }
    }
    
    [MenuItem("Tools/Stone Cutting/🔧 Validate Scene")]
    static void ValidateScene()
    {
        Debug.Log("=== Validating Scene Setup ===");
        
        bool isValid = true;
        
        // Check camera
        if (Camera.main == null)
        {
            Debug.LogError("❌ No Main Camera found!");
            isValid = false;
        }
        else
        {
            Debug.Log("✓ Main Camera found");
        }
        
        // Check game manager
        var gameManager = GameObject.Find("Game Manager");
        if (gameManager == null)
        {
            Debug.LogError("❌ Game Manager not found!");
            isValid = false;
        }
        else
        {
            Debug.Log("✓ Game Manager found");
            
            var cutter = gameManager.GetComponent<com.marufhow.meshslicer.core.MHCutter>();
            if (cutter == null)
            {
                Debug.LogWarning("⚠️ MHCutter not found on Game Manager");
                isValid = false;
            }
            else
            {
                Debug.Log("✓ MHCutter found");
            }
        }
        
        // Check stones
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        int stoneCount = 0;
        int validStones = 0;
        
        foreach (GameObject obj in allObjects)
        {
            string name = obj.name.ToLower();
            if ((name.Contains("stone") || name.Contains("jade")) &&
                !name.Contains("cutting") && !name.Contains("manager"))
            {
                stoneCount++;
                
                MeshFilter mf = obj.GetComponent<MeshFilter>();
                MeshCollider mc = obj.GetComponent<MeshCollider>();
                
                if (mf != null && mc != null && mf.sharedMesh != null)
                {
                    validStones++;
                }
            }
        }
        
        if (stoneCount == 0)
        {
            Debug.LogError("❌ No stones found in scene!");
            isValid = false;
        }
        else if (validStones == 0)
        {
            Debug.LogError($"❌ Found {stoneCount} stone(s) but none have proper setup!");
            isValid = false;
        }
        else if (validStones < stoneCount)
        {
            Debug.LogWarning($"⚠️ Found {stoneCount} stones but only {validStones} are properly setup");
            Debug.Log("Run 'Setup All Stones' to fix");
        }
        else
        {
            Debug.Log($"✓ {validStones} stone(s) properly configured");
        }
        
        // Final result
        if (isValid && validStones > 0)
        {
            Debug.Log("=== ✅ Scene is ready! Press Play to start cutting! ===");
        }
        else
        {
            Debug.Log("=== ❌ Scene needs fixing. Run 'Quick Game Setup' to auto-fix ===");
        }
    }
    #endif
}
