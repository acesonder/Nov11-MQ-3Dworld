# MQ3D Virtual Hangout - Architecture Documentation

This document describes the technical architecture of the Meta Quest 3 Virtual Hangout Room application.

## System Overview

The application follows a modular architecture with five core systems:

```
┌─────────────────────────────────────────────────┐
│         VirtualHangoutManager (Main)            │
│         Coordinates all subsystems              │
└─────────────────────────────────────────────────┘
                        │
        ┌───────────────┼───────────────┐
        │               │               │
        ▼               ▼               ▼
┌──────────────┐ ┌──────────────┐ ┌──────────────┐
│Room Capture  │ │World Sharing │ │Model Import  │
│  System      │ │   System     │ │   System     │
└──────────────┘ └──────────────┘ └──────────────┘
        │               │               │
        └───────────────┼───────────────┘
                        │
        ┌───────────────┼───────────────┐
        ▼               ▼               ▼
┌──────────────┐ ┌──────────────┐ ┌──────────────┐
│Drawing Tools │ │VR Controller │ │UI/Menu       │
│   System     │ │   Manager    │ │  System      │
└──────────────┘ └──────────────┘ └──────────────┘
```

## Core Systems

### 1. Virtual Hangout Manager

**Purpose:** Central coordinator for all application systems

**Responsibilities:**
- Initialize all subsystems
- Manage application modes (Room Capture, World Exploration, etc.)
- Route controller events to appropriate systems
- Maintain application state
- Handle system transitions

**Key Methods:**
- `Initialize()`: Sets up all systems
- `SetMode(AppMode)`: Changes application mode
- `StartRoomCapture()`: Initiates room scanning
- `BrowseSharedWorlds()`: Opens world browser
- `EnterDrawingMode()`: Activates drawing tools

**Application Modes:**
```csharp
public enum AppMode
{
    RoomCapture,      // Scanning environment
    WorldExploration, // Browsing/exploring worlds
    ModelPlacement,   // Placing 3D models
    Drawing,          // 3D drawing mode
    Menu             // Main menu
}
```

### 2. Spatial Capture System

**Purpose:** Capture and process large-scale spatial environments

**Components:**
- `SpatialCaptureManager.cs`: Main capture logic
- Point cloud generation
- Mesh reconstruction
- Data export/import

**Capture Process:**
```
1. User initiates capture
2. System continuously samples spatial data
3. Raycasting in 360° from camera position
4. Build point cloud from hit data
5. Generate mesh from point cloud
6. Optimize mesh for sharing
7. Export as RoomData format
```

**Data Structure:**
```csharp
public class RoomData
{
    public Vector3[] capturePoints;    // Spatial points
    public MeshData[] meshData;        // Mesh information
    public float captureRadius;        // Capture range
    public string timestamp;           // Capture time
}
```

**Technical Details:**
- Capture radius: Up to 100m (stadium-sized)
- Mesh resolution: Configurable (default 0.1m)
- Real-time processing with background optimization
- Compression for efficient storage

### 3. World Sharing System

**Purpose:** Share and explore user-created worlds

**Components:**
- `WorldSharingManager.cs`: Core sharing logic
- World metadata management
- File system operations
- Search and filtering

**Storage Structure:**
```
Application.persistentDataPath/
└── SharedWorlds/
    ├── {worldId}.world     (Binary world data)
    ├── {worldId}.meta      (JSON metadata)
    ├── {worldId}.thumb     (Thumbnail image)
    └── ...
```

**World Metadata:**
```csharp
public class WorldMetadata
{
    public string worldId;          // Unique identifier
    public string worldName;        // Display name
    public string creatorName;      // Creator info
    public string description;      // World description
    public long fileSize;           // Data size
    public string creationDate;     // Creation timestamp
    public int downloadCount;       // Popularity metric
    public float rating;            // User rating (0-5)
    public string[] tags;           // Search tags
}
```

**Features:**
- Local storage with metadata indexing
- Fast search by name, description, tags
- Rating system
- Download tracking
- Thumbnail generation

### 4. Model Import System

**Purpose:** Import and manipulate 3D models in the environment

**Components:**
- `ModelImportManager.cs`: Import and management logic
- Runtime model loading
- Model library management
- Interaction handling

**Supported Formats:**
- FBX (Filmbox)
- OBJ (Wavefront)
- GLTF/GLB (GL Transmission Format)
- Collada (DAE)

**Import Pipeline:**
```
1. User selects 3D file
2. System validates format
3. Load model data
4. Create Unity GameObject
5. Auto-center if enabled
6. Add physics components
7. Store in model library
8. Generate thumbnail
```

**Model Data Structure:**
```csharp
public class ImportedModel
{
    public string modelId;          // Unique ID
    public string modelName;        // Display name
    public string filePath;         // File location
    public string format;           // File format
    public Vector3 scale;           // Default scale
    public GameObject prefab;       // Unity prefab
    public Texture2D thumbnail;     // Preview image
    public long fileSize;           // Size in bytes
    public string importDate;       // Import timestamp
}
```

**Runtime Features:**
- Dynamic model instantiation
- Transform manipulation (scale, rotate, position)
- Physics integration
- Material management
- Collision detection

### 5. Drawing Tools System

**Purpose:** Enable 3D drawing in VR space

**Components:**
- `VRDrawingTool.cs`: Core drawing logic
- LineRenderer management
- Shape creation
- Drawing data export/import

**Drawing Modes:**
```csharp
public enum DrawingMode
{
    FreeDraw,      // Continuous freehand drawing
    StraightLine,  // Point-to-point lines
    Curve,         // Smooth curves
    Shape3D,       // Primitive 3D shapes
    Spray,         // Particle spray effect
    Erase          // Remove drawings
}
```

**Drawing Pipeline:**
```
1. User presses trigger → StartDrawing()
2. Track controller position each frame
3. Add points at minimum distance intervals
4. Create LineRenderer for visualization
5. User releases trigger → StopDrawing()
6. Finalize drawing (simplify if needed)
7. Add to drawn objects list
```

**Features:**
- Configurable brush size and color
- Minimum distance filtering for smooth lines
- Undo/redo support
- Drawing export/import
- 3D shape primitives (sphere, cube, cylinder)
- Material and color customization

### 6. VR Controller System

**Purpose:** Handle Meta Quest 3 controller input

**Components:**
- `VRControllerManager.cs`: Input management
- Button/trigger event system
- Haptic feedback
- Raycasting utilities

**Input Handling:**
```
Left Controller:
- Trigger: Alternative draw/select
- Grip: Grab/rotate objects
- Thumbstick: Strafe/rotate view
- X Button: Undo action
- Y Button: Toggle UI

Right Controller:
- Trigger: Primary draw/select
- Grip: Grab/scale objects
- Thumbstick: Navigate/locomotion
- A Button: Confirm/menu
- B Button: Cancel/back
```

**Event System:**
```csharp
// Events other systems can subscribe to
public event Action<Vector3> OnLeftTriggerPressed;
public event Action<Vector3> OnRightTriggerPressed;
public event Action OnLeftTriggerReleased;
public event Action OnRightTriggerReleased;
public event Action<float> OnLeftGripPressed;
public event Action<float> OnRightGripPressed;
```

**Utilities:**
- Get controller position/rotation
- Perform raycasts from controller
- Trigger haptic feedback
- Get controller velocity

### 7. UI/Menu System

**Purpose:** VR-optimized user interface

**Components:**
- `VRMenuSystem.cs`: Menu management
- Panel system
- Player-relative positioning

**Menu Structure:**
```
Main Menu
├── Room Capture
│   ├── Start Capture
│   ├── Stop Capture
│   └── Save Room
├── World Browser
│   ├── My Worlds
│   ├── Community Worlds
│   └── Search
├── Model Library
│   ├── Import Model
│   ├── Browse Models
│   └── Place Model
├── Drawing Tools
│   ├── Brush Settings
│   ├── Color Picker
│   └── Drawing Modes
└── Settings
    ├── Quality
    ├── Controls
    └── About
```

**UI Features:**
- Follows player gaze
- Positioned at comfortable viewing distance
- VR pointer interaction
- Smooth transitions
- Context-sensitive panels

## Data Flow

### Room Capture Flow
```
User → VirtualHangoutManager.StartRoomCapture()
     → SpatialCaptureManager.StartCapture()
     → Continuous spatial data collection
     → User stops capture
     → SpatialCaptureManager.ExportRoomData()
     → WorldSharingManager.ShareWorld()
     → Save to persistent storage
```

### World Loading Flow
```
User → VirtualHangoutManager.BrowseSharedWorlds()
     → WorldSharingManager.GetAvailableWorlds()
     → Display world list in UI
     → User selects world
     → WorldSharingManager.LoadSharedWorld()
     → Deserialize world data
     → Instantiate in scene
```

### Drawing Flow
```
User → VirtualHangoutManager.EnterDrawingMode()
     → VRControllerManager detects trigger press
     → Event → VRDrawingTool.StartDrawing()
     → Continuous drawing while trigger held
     → VRControllerManager detects trigger release
     → Event → VRDrawingTool.StopDrawing()
     → Drawing finalized and stored
```

### Model Placement Flow
```
User → VirtualHangoutManager.EnterModelPlacementMode()
     → User selects model from library
     → VRControllerManager raycast detects surface
     → ModelImportManager.PlaceModel()
     → Instantiate model at hit point
     → User can manipulate with controllers
```

## Performance Considerations

### Optimization Strategies

1. **Spatial Capture:**
   - Progressive mesh building
   - Octree spatial partitioning
   - Level-of-detail (LOD) generation
   - Async mesh processing

2. **World Loading:**
   - Streaming world data
   - Chunk-based loading
   - Background deserialization
   - Object pooling

3. **Drawing:**
   - Point culling (minimum distance)
   - Mesh simplification
   - Draw call batching
   - Material instancing

4. **Model Import:**
   - Lazy loading of models
   - Automatic LOD generation
   - Texture compression
   - Mesh combining

### Memory Management

- Object pooling for frequently created objects
- Unload unused assets
- Texture streaming
- Mesh compression
- Progressive garbage collection

### Rendering Optimization

- Forward rendering for VR
- Multiview rendering (Quest 3)
- Occlusion culling
- Frustum culling
- GPU instancing
- Static batching for environment

## Threading Model

```
Main Thread:
- Unity update loop
- Controller input processing
- UI updates
- Physics simulation

Background Threads:
- Spatial data processing
- Mesh generation
- File I/O operations
- World serialization/deserialization
- Model loading

Async Operations:
- Network operations (future)
- Asset loading
- Scene streaming
```

## Extensibility

### Adding New Features

1. **New Drawing Mode:**
   ```csharp
   // Add to DrawingMode enum
   // Implement in VRDrawingTool.ContinueDrawing()
   // Update UI to expose new mode
   ```

2. **New Model Format:**
   ```csharp
   // Add to GetFormatFromExtension()
   // Implement loader in LoadModelByFormat()
   // Update documentation
   ```

3. **New Interaction Mode:**
   ```csharp
   // Add to AppMode enum
   // Implement in VirtualHangoutManager
   // Add UI panel for mode
   ```

### Plugin System (Future)

Planned support for:
- Custom capture algorithms
- External model sources
- Multiplayer backends
- Custom drawing tools
- AI integration

## Security Considerations

### Current Implementation

- Local file system only
- No network communication
- Input validation on file imports
- Sandboxed storage per user

### Future Considerations

- Encryption for shared worlds
- User authentication
- Content moderation
- Rate limiting
- Secure model validation

## Testing Strategy

### Unit Tests
- Individual system functionality
- Data serialization/deserialization
- Utility functions

### Integration Tests
- System interaction
- Data flow between systems
- Event handling

### VR Tests
- Controller input accuracy
- Spatial capture precision
- Drawing smoothness
- Performance benchmarks

### User Tests
- Usability testing
- Feature completeness
- Bug identification
- Performance on device

## Deployment

### Build Pipeline
```
1. Development Build (Debug)
   - Full logging
   - Debug symbols
   - Profiler enabled

2. Release Build
   - Optimizations enabled
   - Minimal logging
   - Code stripping
   - Compressed
```

### Version Management
- Semantic versioning (MAJOR.MINOR.PATCH)
- Changelog maintenance
- Backward compatibility for saved data

## Future Architecture Enhancements

### Planned Improvements

1. **Multiplayer Support:**
   - Netcode integration
   - Real-time collaboration
   - Shared world editing

2. **Cloud Integration:**
   - Cloud world storage
   - Cross-device sync
   - Online model library

3. **AI Features:**
   - Intelligent room recognition
   - Auto-tagging of worlds
   - Smart model suggestions

4. **Advanced Rendering:**
   - Lightmap baking
   - Reflection probes
   - Global illumination

---

**Architecture Version:** 1.0.0  
**Last Updated:** November 11, 2025
