# Unity Scene Hierarchy Setup for Stone Cutting

## Recommended Hierarchy Structure:

```
Scene: stonecutscenee
│
├── 📷 Main Camera
│   └── (Keep existing camera settings)
│
├── 💡 Lighting
│   └── Directional Light
│
├── 🎮 Game Manager (Empty GameObject)
│   └── SceneSetup.cs (attached)
│
├── 🪨 Stones (Empty GameObject - Container)
│   ├── Stone_1
│   │   ├── Tag: "Stone"
│   │   ├── Layer: "Sliceable"
│   │   ├── MeshFilter
│   │   ├── MeshRenderer
│   │   ├── MeshCollider
│   │   └── Rigidbody (optional)
│   │
│   └── Stone_2 (if multiple stones)
│
├── ✂️ Cutting Tools (Empty GameObject - Container)
│   ├── CuttingModel_1
│   │   ├── StoneModelCutter.cs OR AutoStoneModelCutter.cs
│   │   ├── MeshFilter
│   │   ├── MeshRenderer
│   │   ├── Collider (for AutoStoneModelCutter)
│   │   └── Rigidbody (for AutoStoneModelCutter)
│   │
│   └── CuttingModel_2 (if multiple tools)
│
└── 🌍 Environment (Empty GameObject - Container)
    └── Plane (Ground)
```

## Setup Instructions:

### 1. Stone Objects Setup:
- **Tag**: Set to "Stone"
- **Layer**: Set to "Sliceable" (create if doesn't exist)
- **Components Required**:
  - MeshFilter
  - MeshRenderer
  - MeshCollider (convex = false for original stone)
  - Rigidbody (optional, for physics)

### 2. Cutting Tool Setup:

#### Option A: Manual Cutting (StoneModelCutter.cs)
```
1. Attach StoneModelCutter.cs to your cutting model
2. In Inspector:
   - Assign "Cutting Model" = itself
   - Assign "Stone Inside Material"
   - Set "Stone Layer" = Sliceable
3. No Rigidbody needed
4. Collider optional
```

#### Option B: Auto Cutting (AutoStoneModelCutter.cs)
```
1. Attach AutoStoneModelCutter.cs to your cutting model
2. In Inspector:
   - Assign "Stone Inside Material"
3. Add Collider (can be trigger)
4. Add Rigidbody (Is Kinematic = false)
5. Make sure stone has "Stone" tag
```

### 3. Camera Setup:
- Position to see the stone clearly
- Add orbit/pan controls if needed

### 4. Layers Configuration:
Create these layers in Edit → Project Settings → Tags and Layers:
- Layer 6: "Sliceable" (for stones and cut pieces)

## Common Issues & Fixes:

1. **Stone doesn't cut**: Check tag is "Stone" and layer is correct
2. **Cutting model not dragging**: Ensure StoneModelCutter has cuttingModel assigned
3. **Physics issues**: Make sure Rigidbody is on cut pieces (script adds it)
4. **No collision detection**: Add Collider and Rigidbody to cutting tool
