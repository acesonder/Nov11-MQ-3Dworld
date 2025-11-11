# MQ3D Virtual Hangout Room

A Meta Quest 3 VR application for creating, sharing, and exploring virtual hangout spaces. Capture large-scale environments like professional stadiums, import 3D models, draw in 3D space, and share your creations with others.

## Features

### 🏟️ Large-Scale Room Capture
- Capture environments as big as professional stadiums using your VR headset
- Real-time spatial mapping and mesh generation
- Export and share captured spaces with others
- Support for complex multi-room environments

### 🌍 World Sharing & Exploration
- Share your captured worlds with the community
- Browse and explore worlds created by other users
- Search worlds by tags and keywords
- Rate and review shared worlds
- Download and explore community-created spaces

### 🎨 3D Model Import
- Import various 3D file formats (FBX, OBJ, GLTF/GLB, Collada)
- Place and manipulate imported models in your space
- Model library for quick access to imported assets
- Scale, rotate, and position models with VR controllers
- Physics-enabled object interaction

### ✏️ 3D Drawing Tools
- Draw freely in 3D space with VR controllers
- Multiple drawing modes:
  - Free Draw: Draw flowing lines in 3D
  - Straight Line: Create precise straight lines
  - Curve: Draw smooth curves
  - 3D Shapes: Create spheres, cubes, cylinders
  - Spray: Particle-like spray painting
  - Erase: Remove unwanted drawings
- Customizable brush colors and sizes
- Undo/redo functionality
- Save and load drawings

### 🎮 VR Interaction
- Native Meta Quest 3 controller support
- Haptic feedback for immersive interactions
- Raycast-based object selection
- Intuitive trigger and grip controls
- Hand tracking support (future enhancement)

## Project Structure

```
Nov11-MQ-3Dworld/
├── Assets/
│   ├── Scenes/           # Unity scene files
│   ├── Scripts/          # C# source code
│   │   ├── RoomCapture/         # Spatial capture system
│   │   ├── WorldSharing/        # World sharing functionality
│   │   ├── ModelImport/         # 3D model import system
│   │   ├── DrawingTools/        # VR drawing tools
│   │   ├── VRInteraction/       # Controller management
│   │   └── UI/                  # User interface
│   ├── Prefabs/          # Reusable game objects
│   ├── Materials/        # Materials and shaders
│   ├── Models/           # 3D model assets
│   └── Textures/         # Texture files
├── Packages/             # Unity package dependencies
├── ProjectSettings/      # Unity project configuration
└── Documentation/        # Additional documentation

```

## Requirements

### Hardware
- **Meta Quest 3** VR headset
- Sufficient storage for captured environments and imported models
- Stable Wi-Fi connection for world sharing features

### Software
- **Unity 2022.3.10f1** or later
- **Meta XR SDK** (included in packages)
- **XR Interaction Toolkit 2.5.2** or later

## Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/acesonder/Nov11-MQ-3Dworld.git
   cd Nov11-MQ-3Dworld
   ```

2. **Open in Unity:**
   - Launch Unity Hub
   - Click "Add" and select the cloned project folder
   - Open with Unity 2022.3.10f1 or compatible version

3. **Install Meta Quest 3 SDK:**
   - The required packages are already defined in `Packages/manifest.json`
   - Unity will automatically download dependencies on first open

4. **Configure Build Settings:**
   - Go to File > Build Settings
   - Select "Android" as the platform
   - Click "Switch Platform"
   - In Player Settings, ensure the package name is set correctly
   - Enable XR in Project Settings > XR Plug-in Management

5. **Build and Deploy:**
   - Connect your Meta Quest 3 via USB
   - Enable Developer Mode on your headset
   - Click "Build and Run" in Unity

## Usage

### Getting Started

1. **Launch the Application:**
   - Put on your Meta Quest 3 headset
   - Launch "MQ3D Virtual Hangout" from your library

2. **Navigate the Menu:**
   - Use the main menu to select your desired mode:
     - Room Capture
     - World Exploration
     - Model Placement
     - Drawing Mode

### Capturing a Room

1. Select "Room Capture" from the main menu
2. Walk around the space you want to capture
3. The system will automatically scan and map the environment
4. When finished, press the menu button to stop capture
5. Name your captured space and save it

### Exploring Shared Worlds

1. Select "World Exploration" from the main menu
2. Browse available worlds created by the community
3. Select a world to load it
4. Explore freely using locomotion controls
5. Rate the world after exploring

### Importing and Placing 3D Models

1. Select "Model Placement" from the main menu
2. Choose "Import Model" to add new 3D assets
3. Select a model from your library
4. Point with your controller and pull trigger to place
5. Use grip button to scale and rotate placed models

### Drawing in 3D Space

1. Select "Drawing Mode" from the main menu
2. Choose your brush color and size
3. Press and hold the trigger to draw
4. Release trigger to finish the stroke
5. Use menu to change drawing modes (straight line, shapes, etc.)

## Controller Mapping

### Meta Quest 3 Controllers

**Right Controller:**
- **Trigger:** Draw / Select / Confirm
- **Grip:** Grab / Scale objects
- **Thumbstick:** Navigate menus / Locomotion
- **A Button:** Primary action / Open menu
- **B Button:** Cancel / Back

**Left Controller:**
- **Trigger:** Alternative draw / Select
- **Grip:** Grab / Rotate objects
- **Thumbstick:** Rotate view / Strafe
- **X Button:** Undo
- **Y Button:** Toggle UI

## Key Scripts

### Core Systems

- **`VirtualHangoutManager.cs`**: Main application coordinator
- **`SpatialCaptureManager.cs`**: Handles room capture and spatial mapping
- **`WorldSharingManager.cs`**: Manages world upload/download and browsing
- **`ModelImportManager.cs`**: 3D model import and placement
- **`VRDrawingTool.cs`**: 3D drawing functionality
- **`VRControllerManager.cs`**: VR input and interaction handling
- **`VRMenuSystem.cs`**: VR-optimized UI system

## Technical Details

### Spatial Capture System
- Uses Unity's XR spatial awareness APIs
- Captures environment geometry through raycasting
- Optimizes mesh data for performance
- Exports data in compressed format for sharing

### World Sharing
- Local storage of world data in persistent directory
- JSON metadata with binary world data
- Search and filtering capabilities
- Rating and download tracking

### Model Import
- Runtime model loading for various formats
- Automatic mesh optimization and LOD generation
- Physics integration for realistic interactions
- Material and texture handling

### Drawing System
- Line renderer-based 3D drawing
- Multiple brush types and effects
- Undo/redo with drawing history
- Export drawings as reusable assets

## Performance Optimization

- **LOD System:** Automatic level-of-detail for imported models
- **Occlusion Culling:** Only render visible objects
- **Mesh Optimization:** Simplified geometry for large spaces
- **Texture Compression:** Optimized textures for mobile VR
- **Draw Call Batching:** Reduced draw calls for better framerate

## Troubleshooting

### Controllers Not Detected
- Ensure headset is in Developer Mode
- Check USB debugging is enabled
- Restart the application
- Re-pair controllers in Quest settings

### Poor Performance
- Reduce spatial capture resolution
- Limit number of imported models in scene
- Simplify drawing complexity
- Close background applications

### World Sharing Issues
- Check storage permissions
- Verify sufficient storage space
- Ensure world files are not corrupted
- Try re-exporting the world

## Future Enhancements

- [ ] Multiplayer support for real-time collaboration
- [ ] Cloud-based world sharing platform
- [ ] Hand tracking integration
- [ ] Advanced physics simulations
- [ ] Audio spatialization and voice chat
- [ ] Procedural environment generation
- [ ] AI-assisted room reconstruction
- [ ] Cross-platform support (PCVR, PSVR2)

## Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Credits

- **Developer:** AceSonder
- **Platform:** Meta Quest 3
- **Engine:** Unity 2022.3
- **XR Framework:** Unity XR Interaction Toolkit

## Support

For issues, questions, or suggestions:
- Open an issue on GitHub
- Contact: [GitHub Issues](https://github.com/acesonder/Nov11-MQ-3Dworld/issues)

## Acknowledgments

- Meta for the Quest 3 platform and SDK
- Unity Technologies for the XR Interaction Toolkit
- The VR development community for inspiration and support

---

**Made with ❤️ for the VR Community**