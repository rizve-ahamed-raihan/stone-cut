# 🪨✂️ Stone Cutting Game

A complete Unity 3D stone cutting game where you slice stones with realistic mesh cutting!

## ✨ Features

- **Real-time mesh slicing** using advanced algorithms
- **Physics-based cutting** with realistic piece separation
- **Multiple stone models** (rocks, gems, crystals, jade)
- **Drag-to-cut interface** for intuitive gameplay
- **Score system** with unlimited or limited cuts
- **Visual feedback** with cutting plane visualization
- **Easy setup** with one-click configuration tools

## 🚀 Quick Start (30 Seconds!)

1. Open Unity project
2. Go to menu: `Tools → Stone Cutting → 🚀 Quick Game Setup`
3. Press Play ▶️
4. Click and drag on a stone to cut it!

That's it! Your game is ready! 🎉

## 📖 Detailed Setup

### Option 1: Use Existing Scene
1. Open `Assets/Scenes/stonecutscenee.unity`
2. Run `Tools → Stone Cutting → Quick Game Setup`
3. Play!

### Option 2: Create New Scene
1. Create new scene in Unity
2. Run `Tools → Stone Cutting → 🎨 Create Sample Scene`
3. Play!

### Option 3: Manual Setup
See `Assets/GAME_SETUP_GUIDE.md` for detailed instructions

## 🎮 How to Play

### Default Controls:
- **Left Click + Drag**: Draw cutting line on stone
- **Release**: Perform the cut
- **R Key**: Reset game

### Alternative Mode (SimpleStoneCutter):
- **Left Click**: Cut stone at cursor position
- **Shift + Click**: Cut nearest stone

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── StoneCuttingGame.cs          # Main game controller
│   ├── SimpleStoneCutter.cs         # Simple cutting mode
│   ├── SceneSetup.cs                # Automatic scene setup
│   ├── QuickGameSetup.cs            # One-click setup tools
│   ├── StoneCutter.cs               # Legacy hole effect
│   └── StoneSlicer.cs               # Legacy EzySlice example
│
├── Scenes/
│   ├── stonecutscenee.unity         # Main game scene
│   └── SampleScene.unity            # Original sample
│
├── 3d Models/
│   └── Stone/
│       └── stone_3d.fbx             # Custom stone model
│
├── StoneModel/
│   └── JadeStone09 (1).fbx          # Jade stone
│
├── Free/Stones/                     # Free stone pack
│   ├── Mesh/                        # 5 stone models
│   ├── Materials/
│   └── Textures/
│
├── Aztech Games/GemsAndCrystals/    # Gems & crystals pack
│   ├── Meshes/                      # 20+ gem/crystal models
│   ├── Materials/
│   └── Textures/
│
├── Packages/
│   └── com.marufhow.meshslicer/     # Mesh cutting library
│
└── Documentation/
    ├── GAME_SETUP_GUIDE.md          # Complete setup guide
    └── HIERARCHY_SETUP.md           # Scene structure guide
```

## 🔧 Unity Menu Tools

All tools available under `Tools → Stone Cutting`:

- **🚀 Quick Game Setup** - One-click complete setup (Recommended!)
- **🎨 Create Sample Scene** - Generate a test scene with stones
- **📚 Open Setup Guide** - View detailed documentation
- **🔧 Validate Scene** - Check if scene is configured correctly
- **Setup Scene Hierarchy** - Organize objects into folders
- **Setup All Stones** - Configure stone colliders and components
- **Setup Cutting Tools** - Configure cutting tool models

## 🎨 Assets Included

### Stone Models (20+):
- Custom stone models
- Jade stones
- Free rock pack (5 stones)
- Gems pack (10 gems)
- Crystals pack (10 crystals)

### Libraries:
- **MHCutter (com.marufhow.meshslicer)** - Advanced mesh slicing
- Complete with demo scenes and examples

## ⚙️ Configuration

### Game Settings (StoneCuttingGame):
- **Unlimited Cuts**: Enable/disable cut limit
- **Cuts Remaining**: Number of cuts (if limited)
- **Drag Speed**: Mouse follow speed
- **Cut Force**: Physics impulse on pieces
- **Score System**: Built-in scoring

### Cutting Tool Settings (SimpleStoneCutter):
- **Follow Mouse**: Tool tracks cursor
- **Cut Distance**: Auto-cut range
- **Cut Direction**: Blade orientation

## 🐛 Troubleshooting

### "Stone doesn't cut!"
1. Run `Tools → Stone Cutting → Validate Scene`
2. Check stone has **MeshCollider** (non-convex)
3. Check stone has **MeshFilter** with mesh

### "No Game Manager found!"
1. Run `Tools → Stone Cutting → Quick Game Setup`
2. Or manually create GameObject + add MHCutter component

### "Cut pieces disappear!"
- Pieces fall due to gravity - add ground plane
- Or disable Rigidbody.useGravity on pieces

### "Errors in console!"
- Check you're using Unity 2020.3 or later
- Ensure all packages are imported

## 📝 Scripts Overview

### Main Scripts:

**StoneCuttingGame.cs**
- Complete game controller
- Drag-to-cut mechanics
- Score system
- Visual feedback

**SimpleStoneCutter.cs**
- Simplified cutting mode
- Click-to-cut mechanics
- Good for beginners

**SceneSetup.cs**
- Automatic stone configuration
- Hierarchy organization
- Component setup

**QuickGameSetup.cs**
- One-click game setup
- Scene validation
- Sample scene creation

### Legacy Scripts (Reference):
- `StoneCutter.cs` - Old hole effect system
- `StoneSlicer.cs` - Old EzySlice example

## 🎯 Game Modes

### 1. Unlimited Mode (Default)
- Cut as many times as you want
- Focus on precision and creativity
- Great for sandbox play

### 2. Challenge Mode
- Limited number of cuts
- Score-based gameplay
- Try to make the most cuts efficiently

### 3. Custom Mode
- Modify scripts for your own game type
- Add timers, objectives, power-ups, etc.

## 🚀 Next Steps

1. **Customize stones**: Import your own 3D models
2. **Add materials**: Create custom textures for cut interiors
3. **Enhance gameplay**: Add power-ups, obstacles, targets
4. **Polish audio**: Import cutting sounds and music
5. **Build game**: File → Build Settings → Build!

## 📚 Learn More

- Read `GAME_SETUP_GUIDE.md` for detailed setup
- Check `HIERARCHY_SETUP.md` for scene organization
- Explore mesh slicer documentation in `Packages/com.marufhow.meshslicer/`

## 🎓 Tips & Tricks

- **Slow drags** = precise cuts
- **Fast drags** = dynamic cuts
- Try cutting at **different angles**
- Use **small pieces** for challenges
- Combine with **gems** for shiny effects
- Add **particle effects** for juice

## 🤝 Credits

- **Mesh Slicer**: com.marufhow.meshslicer
- **Stone Models**: Free Stones Pack, Custom Models
- **Gems & Crystals**: Aztech Games
- **Game Scripts**: Custom implementation

## 📄 License

Check individual asset licenses in their respective folders.

---

**Ready to cut some stones? Press Play! 🎮✂️🪨**

For help, run: `Tools → Stone Cutting → 🔧 Validate Scene`
