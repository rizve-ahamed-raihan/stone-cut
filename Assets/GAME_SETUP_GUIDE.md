# Complete Stone Cutting Game Setup Guide

## 📋 What's Included

Your stone cutting game now has:
- ✅ Mesh Slicer Library (com.marufhow.meshslicer) - Already installed
- ✅ 3D Stone Models (Free Stones, Jade Stone, Gems & Crystals)
- ✅ Cutting Tool Models (Can use any 3D model)
- ✅ Complete Game Scripts

## 🎮 Quick Setup (5 Minutes)

### Step 1: Open Scene
1. Open `Assets/Scenes/stonecutscenee.unity`

### Step 2: Setup Game Manager
1. Create Empty GameObject → Name it "Game Manager"
2. Add Component → `StoneCuttingGame.cs`
3. In Inspector:
   - **Mesh Cutter**: Will auto-create if empty
   - **Cutting Tool**: Drag a 3D model (sword/knife/blade)
   - **Stone Layer**: Set to "Everything" or create "Stone" layer
   - **Unlimited Cuts**: ✓ (Check for unlimited)

### Step 3: Setup Stones
1. Find your stone objects in hierarchy
2. For each stone:
   - Set **Layer** to match Game Manager's "Stone Layer"
   - Make sure it has **MeshFilter** + **MeshRenderer**
   - Add **MeshCollider** (convex = false)
   - Optional: Add **Rigidbody** for gravity

**Quick Way:**
- Use `SceneSetup.cs` → Check "Setup Stones"

### Step 4: Setup Cutting Tool (Optional)
If you want a visual cutting tool:
1. Drag a 3D model into scene (sword, knife, etc.)
2. Position it in front of camera
3. Assign it to Game Manager's "Cutting Tool" field

### Step 5: Test
1. Press Play ▶️
2. Click and drag across a stone
3. Release to cut!

## 🎯 Alternative: Simple Setup

If the main game is too complex, use **SimpleStoneCutter.cs**:

1. Create Empty GameObject → "Game Manager"
2. Add `MHCutter` component
3. Drag your cutting tool model into scene
4. Add `SimpleStoneCutter.cs` to cutting tool
5. In Inspector:
   - Assign "Mesh Cutter" = Game Manager
   - Set "Cuttable Layer"
6. Play and click on stones!

## 🗂️ Scene Hierarchy Structure

```
stonecutscenee
├── 📷 Main Camera
├── 💡 Directional Light
├── 🎮 Game Manager
│   ├── StoneCuttingGame.cs (or MHCutter)
│   └── AudioSource (optional)
├── 🪨 Stones Container
│   ├── stone_3d
│   ├── JadeStone09
│   └── Stone (any stone models)
├── ✂️ Cutting Tool (optional)
│   └── SimpleStoneCutter.cs (if using)
└── 🌍 Ground
    └── Plane
```

## 🎨 Available 3D Assets

### Stones (Use any):
- `Assets/3d Models/Stone/stone_3d.fbx`
- `Assets/StoneModel/JadeStone09 (1).fbx`
- `Assets/Free/Stones/Mesh/Stone_1.fbx` to `Stone_5.fbx`
- `Assets/Aztech Games/GemsAndCrystals/Meshes/` (10 gems + 10 crystals)

### Cutting Tools (Examples):
- Use meshes from `Assets/Aztech Games/GemsAndCrystals/Meshes/Crystal_*.fbx`
- Import your own sword/knife/blade models
- Use primitive shapes (Cube, Cylinder, etc.)

## ⚙️ Configuration

### StoneCuttingGame Settings:
- **Drag Speed**: How fast tool follows mouse (10 = good)
- **Cut Force**: Physics force applied to pieces (5 = good)
- **Unlimited Cuts**: Enable for endless gameplay
- **Cuts Remaining**: Number of cuts if not unlimited (10)
- **Show Cutting Plane Gizmo**: Visual debug line

### SimpleStoneCutter Settings:
- **Follow Mouse**: Tool follows cursor
- **Follow Speed**: Mouse tracking speed (15 = smooth)
- **Cut Distance**: Auto-cut range (2.0)
- **Cut Direction Axis**: Which way the tool cuts (Forward/Up/Right)

## 🎮 Controls

### StoneCuttingGame:
- **Left Click + Drag**: Slice stone along drag path
- **R Key**: Reset game and reload scene

### SimpleStoneCutter:
- **Left Click**: Cut stone at cursor
- **Shift + Click**: Cut nearest stone in range

## 🔧 Troubleshooting

### Stone doesn't cut:
1. Check stone has **MeshCollider** (not trigger)
2. Check stone layer matches game's "Stone Layer"
3. Check stone has **MeshFilter** with mesh assigned
4. Try disabling/re-enabling the stone's collider

### Cutting tool not visible:
1. Assign 3D model to "Cutting Tool" field
2. Check model has MeshRenderer enabled
3. Position tool in front of camera

### Cut pieces fly away too fast:
1. Reduce "Cut Force" in game settings
2. Add drag to Rigidbody on cut pieces

### Error "MHCutter not found":
1. Make sure Game Manager has `MHCutter` component
2. Assign it manually in SimpleStoneCutter's inspector

## 🚀 Advanced Features

### Add Score System:
- Already built into StoneCuttingGame
- Score shown on screen (top-left)

### Add Sound Effects:
1. Import audio clips (.wav/.mp3)
2. Add **AudioSource** to Game Manager
3. Assign clips to "Cut Sound" and "Miss Sound"

### Multiple Stones:
- Just add more stone models to scene
- They'll all be cuttable automatically

### Custom Materials for Cut Interior:
- In MHCutter, you can assign materials for the inside of cuts
- Check mesh slicer documentation for details

## 📝 Quick Command Reference

### Unity Menu Commands:
- `Tools → Stone Cutting → Setup Scene Hierarchy`
- `Tools → Stone Cutting → Setup All Stones`
- `Tools → Stone Cutting → Setup Cutting Tools`

### Layer Setup:
1. Edit → Project Settings → Tags and Layers
2. Add layer "Stone" or "Sliceable"
3. Assign to stone objects

## ✅ Final Checklist

- [ ] Scene opened (stonecutscenee.unity)
- [ ] Game Manager created with StoneCuttingGame.cs
- [ ] Stones have MeshCollider + correct layer
- [ ] Cutting Tool assigned (optional)
- [ ] Tested by clicking and dragging on stone
- [ ] Stone cuts successfully!

## 🎓 Next Steps

1. **Try different stones**: Swap stone models to test
2. **Customize cutting tool**: Use different 3D models
3. **Add materials**: Create materials for cut interior
4. **Build game**: File → Build Settings → Build
5. **Share your game**: Export and share!

---

## 💡 Tips

- Drag **slowly** for precise cuts
- Cut at **different angles** for cool effects
- Use **multiple cuts** to make small pieces
- Add **physics materials** for bouncy/slippery stones
- Try **gems and crystals** for shiny cuts!

**Your stone cutting game is ready! Press Play and start slicing! 🎮✂️🪨**
