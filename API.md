# MQ3D Virtual Hangout - API Reference

This document provides a complete API reference for all public classes and methods in the MQ3D Virtual Hangout application.

## Table of Contents

1. [VirtualHangoutManager](#virtualhangoutmanager)
2. [SpatialCaptureManager](#spatialcapturemanager)
3. [WorldSharingManager](#worldsharingmanager)
4. [ModelImportManager](#modelimportmanager)
5. [VRDrawingTool](#vrdrawingtool)
6. [VRControllerManager](#vrcontrollermanager)
7. [VRMenuSystem](#vrmenusystem)

---

## VirtualHangoutManager

**Namespace:** `MQ3DVirtualHangout`

Main coordinator for the entire application. Manages application modes and system integration.

### Methods

#### Initialize()
```csharp
public void Initialize()
```
Initializes all subsystems. Called automatically on Awake if `autoInitialize` is true.

**Example:**
```csharp
VirtualHangoutManager manager = gameObject.AddComponent<VirtualHangoutManager>();
manager.Initialize();
```

#### SetMode(AppMode mode)
```csharp
public void SetMode(AppMode mode)
```
Changes the current application mode.

**Parameters:**
- `mode`: The mode to switch to (RoomCapture, WorldExploration, ModelPlacement, Drawing, Menu)

**Example:**
```csharp
manager.SetMode(VirtualHangoutManager.AppMode.Drawing);
```

#### StartRoomCapture()
```csharp
public void StartRoomCapture()
```
Initiates room capture mode and begins spatial scanning.

**Example:**
```csharp
manager.StartRoomCapture();
// Walk around to scan the environment
```

#### StopRoomCapture()
```csharp
public void StopRoomCapture()
```
Stops room capture, processes data, and saves to world sharing system.

#### BrowseSharedWorlds()
```csharp
public void BrowseSharedWorlds()
```
Enters world exploration mode and retrieves available worlds.

#### LoadWorld(string worldId)
```csharp
public void LoadWorld(string worldId)
```
Loads a specific world by its unique identifier.

**Parameters:**
- `worldId`: Unique identifier of the world to load

#### EnterModelPlacementMode()
```csharp
public void EnterModelPlacementMode()
```
Switches to model placement mode for adding 3D models to the scene.

#### PlaceModel(string modelId, Vector3 position)
```csharp
public void PlaceModel(string modelId, Vector3 position)
```
Places a 3D model at the specified position.

**Parameters:**
- `modelId`: ID of the model to place
- `position`: World position where model should be placed

#### EnterDrawingMode()
```csharp
public void EnterDrawingMode()
```
Switches to drawing mode for 3D drawing with controllers.

#### ShowMenu()
```csharp
public void ShowMenu()
```
Returns to the main menu.

#### GetCurrentMode()
```csharp
public AppMode GetCurrentMode()
```
Returns the current application mode.

**Returns:** Current `AppMode`

#### GetStatistics()
```csharp
public string GetStatistics()
```
Returns application statistics as a formatted string.

**Returns:** Statistics string with world count, model count, and current mode

---

## SpatialCaptureManager

**Namespace:** `MQ3DVirtualHangout.RoomCapture`

Manages spatial environment capture for large-scale spaces.

### Properties

```csharp
[SerializeField] private float captureRadius = 100f;
[SerializeField] private float meshResolution = 0.1f;
[SerializeField] private bool autoCapture = false;
[SerializeField] private Material spatialMeshMaterial;
```

### Methods

#### StartCapture()
```csharp
public void StartCapture()
```
Begins capturing the spatial environment. User should move around to scan.

**Example:**
```csharp
SpatialCaptureManager capture = GetComponent<SpatialCaptureManager>();
capture.StartCapture();
```

#### StopCapture()
```csharp
public void StopCapture()
```
Stops capturing and processes the collected spatial data.

#### ExportRoomData()
```csharp
public RoomData ExportRoomData()
```
Exports the captured room data for saving or sharing.

**Returns:** `RoomData` object containing all captured information

**Example:**
```csharp
capture.StopCapture();
RoomData data = capture.ExportRoomData();
// Save or share the data
```

#### ImportRoomData(RoomData roomData)
```csharp
public void ImportRoomData(RoomData roomData)
```
Imports and displays previously captured room data.

**Parameters:**
- `roomData`: Previously exported room data to load

### Data Structures

#### RoomData
```csharp
[System.Serializable]
public class RoomData
{
    public Vector3[] capturePoints;
    public MeshData[] meshData;
    public float captureRadius;
    public string timestamp;
}
```

#### MeshData
```csharp
[System.Serializable]
public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector3 position;
    public Quaternion rotation;
}
```

---

## WorldSharingManager

**Namespace:** `MQ3DVirtualHangout.WorldSharing`

Manages sharing and exploring user-created worlds.

### Methods

#### ShareWorld(string worldName, string description, byte[] worldData)
```csharp
public void ShareWorld(string worldName, string description, byte[] worldData)
```
Shares a captured world with metadata.

**Parameters:**
- `worldName`: Display name for the world
- `description`: Description of the world
- `worldData`: Serialized world data

**Example:**
```csharp
WorldSharingManager sharing = GetComponent<WorldSharingManager>();
byte[] data = SerializeWorld();
sharing.ShareWorld("My Stadium", "A captured stadium environment", data);
```

#### LoadSharedWorld(string worldId)
```csharp
public void LoadSharedWorld(string worldId)
```
Loads a shared world into the current scene.

**Parameters:**
- `worldId`: Unique identifier of the world

#### GetAvailableWorlds()
```csharp
public List<WorldMetadata> GetAvailableWorlds()
```
Retrieves list of all available worlds.

**Returns:** List of `WorldMetadata` objects

**Example:**
```csharp
var worlds = sharing.GetAvailableWorlds();
foreach (var world in worlds)
{
    Debug.Log($"World: {world.worldName} by {world.creatorName}");
}
```

#### SearchWorlds(string searchQuery)
```csharp
public List<WorldMetadata> SearchWorlds(string searchQuery)
```
Searches worlds by name, description, or tags.

**Parameters:**
- `searchQuery`: Search term

**Returns:** List of matching `WorldMetadata` objects

#### RateWorld(string worldId, float rating)
```csharp
public void RateWorld(string worldId, float rating)
```
Rates a world (0-5 stars).

**Parameters:**
- `worldId`: World to rate
- `rating`: Rating value (0.0 to 5.0)

### Data Structures

#### WorldMetadata
```csharp
[System.Serializable]
public class WorldMetadata
{
    public string worldId;
    public string worldName;
    public string creatorName;
    public string description;
    public string thumbnailPath;
    public string worldDataPath;
    public long fileSize;
    public string creationDate;
    public int downloadCount;
    public float rating;
    public string[] tags;
}
```

---

## ModelImportManager

**Namespace:** `MQ3DVirtualHangout.ModelImport`

Manages importing and manipulating 3D models.

### Methods

#### ImportModel(string filePath)
```csharp
public void ImportModel(string filePath)
```
Imports a 3D model from the specified file path.

**Parameters:**
- `filePath`: Path to the 3D model file (FBX, OBJ, GLTF, etc.)

**Supported Formats:** FBX, OBJ, GLTF/GLB, Collada (DAE)

**Example:**
```csharp
ModelImportManager importer = GetComponent<ModelImportManager>();
importer.ImportModel("/path/to/model.fbx");
```

#### PlaceModel(string modelId, Vector3 position, Quaternion rotation)
```csharp
public GameObject PlaceModel(string modelId, Vector3 position, Quaternion rotation)
```
Places an imported model in the scene.

**Parameters:**
- `modelId`: ID of the imported model
- `position`: World position
- `rotation`: World rotation

**Returns:** Instantiated `GameObject`

#### GetImportedModels()
```csharp
public List<ImportedModel> GetImportedModels()
```
Retrieves list of all imported models.

**Returns:** List of `ImportedModel` objects

#### DeleteModel(string modelId)
```csharp
public void DeleteModel(string modelId)
```
Removes a model from the library.

**Parameters:**
- `modelId`: Model to delete

#### ScaleModel(GameObject modelInstance, Vector3 scale)
```csharp
public void ScaleModel(GameObject modelInstance, Vector3 scale)
```
Scales a placed model instance.

**Parameters:**
- `modelInstance`: The model GameObject
- `scale`: New scale vector

#### RotateModel(GameObject modelInstance, Vector3 rotation)
```csharp
public void RotateModel(GameObject modelInstance, Vector3 rotation)
```
Rotates a placed model instance.

**Parameters:**
- `modelInstance`: The model GameObject
- `rotation`: Rotation angles in degrees

### Data Structures

#### ImportedModel
```csharp
[System.Serializable]
public class ImportedModel
{
    public string modelId;
    public string modelName;
    public string filePath;
    public string format;
    public Vector3 scale;
    public GameObject prefab;
    public Texture2D thumbnail;
    public long fileSize;
    public string importDate;
}
```

---

## VRDrawingTool

**Namespace:** `MQ3DVirtualHangout.DrawingTools`

Provides 3D drawing capabilities in VR.

### Properties

```csharp
[SerializeField] private float brushSize = 0.01f;
[SerializeField] private Color brushColor = Color.white;
[SerializeField] private Material lineMaterial;
[SerializeField] private float minDistance = 0.01f;
[SerializeField] private DrawingMode currentMode = DrawingMode.FreeDraw;
```

### Methods

#### StartDrawing(Vector3 position)
```csharp
public void StartDrawing(Vector3 position)
```
Begins a new drawing stroke at the specified position.

**Parameters:**
- `position`: Starting position in world space

#### ContinueDrawing(Vector3 position)
```csharp
public void ContinueDrawing(Vector3 position)
```
Continues the current drawing stroke to the new position.

**Parameters:**
- `position`: Next position in the drawing

#### StopDrawing()
```csharp
public void StopDrawing()
```
Completes the current drawing stroke.

**Example:**
```csharp
VRDrawingTool drawing = GetComponent<VRDrawingTool>();

// In update loop when trigger is pressed:
if (triggerPressed && !wasPressed)
{
    drawing.StartDrawing(controllerPosition);
    wasPressed = true;
}
else if (triggerPressed)
{
    drawing.ContinueDrawing(controllerPosition);
}
else if (wasPressed)
{
    drawing.StopDrawing();
    wasPressed = false;
}
```

#### SetBrushColor(Color color)
```csharp
public void SetBrushColor(Color color)
```
Changes the brush color.

**Parameters:**
- `color`: New brush color

#### SetBrushSize(float size)
```csharp
public void SetBrushSize(float size)
```
Changes the brush size.

**Parameters:**
- `size`: New brush size (minimum 0.001)

#### SetDrawingMode(DrawingMode mode)
```csharp
public void SetDrawingMode(DrawingMode mode)
```
Changes the drawing mode.

**Parameters:**
- `mode`: New drawing mode

#### ClearAllDrawings()
```csharp
public void ClearAllDrawings()
```
Removes all drawings from the scene.

#### UndoLastDrawing()
```csharp
public void UndoLastDrawing()
```
Removes the most recently created drawing.

#### ExportDrawing()
```csharp
public DrawingData ExportDrawing()
```
Exports the current drawing data.

**Returns:** `DrawingData` object

#### ImportDrawing(DrawingData data)
```csharp
public void ImportDrawing(DrawingData data)
```
Imports and displays drawing data.

**Parameters:**
- `data`: Previously exported drawing data

#### CreateShape3D(string shapeType, Vector3 position, float size)
```csharp
public GameObject CreateShape3D(string shapeType, Vector3 position, float size)
```
Creates a 3D primitive shape.

**Parameters:**
- `shapeType`: "sphere", "cube", or "cylinder"
- `position`: Position to create the shape
- `size`: Size of the shape

**Returns:** Created shape GameObject

#### GetDrawnObjects()
```csharp
public List<GameObject> GetDrawnObjects()
```
Gets all drawn objects for manipulation.

**Returns:** List of drawn GameObjects

### Enums

#### DrawingMode
```csharp
public enum DrawingMode
{
    FreeDraw,      // Continuous freehand drawing
    StraightLine,  // Point-to-point straight lines
    Curve,         // Smooth curves
    Shape3D,       // Primitive 3D shapes
    Spray,         // Particle spray effect
    Erase          // Remove drawings
}
```

### Data Structures

#### DrawingData
```csharp
[System.Serializable]
public class DrawingData
{
    public string drawingId;
    public Vector3[] points;
    public Color color;
    public float brushSize;
    public DrawingMode mode;
    public string timestamp;
}
```

---

## VRControllerManager

**Namespace:** `MQ3DVirtualHangout.VRInteraction`

Manages Meta Quest 3 controller input and interactions.

### Events

```csharp
public event Action<Vector3> OnLeftTriggerPressed;
public event Action<Vector3> OnRightTriggerPressed;
public event Action OnLeftTriggerReleased;
public event Action OnRightTriggerReleased;
public event Action<float> OnLeftGripPressed;
public event Action<float> OnRightGripPressed;
```

**Example:**
```csharp
VRControllerManager controller = GetComponent<VRControllerManager>();
controller.OnRightTriggerPressed += HandleTriggerPress;

void HandleTriggerPress(Vector3 position)
{
    Debug.Log($"Trigger pressed at {position}");
}
```

### Methods

#### GetControllerPosition(XRNode node)
```csharp
public Vector3 GetControllerPosition(XRNode node)
```
Gets the current position of a controller.

**Parameters:**
- `node`: `XRNode.LeftHand` or `XRNode.RightHand`

**Returns:** Controller position in world space

#### GetControllerRotation(XRNode node)
```csharp
public Quaternion GetControllerRotation(XRNode node)
```
Gets the current rotation of a controller.

**Parameters:**
- `node`: `XRNode.LeftHand` or `XRNode.RightHand`

**Returns:** Controller rotation

#### ControllerRaycast(XRNode node, out RaycastHit hit)
```csharp
public bool ControllerRaycast(XRNode node, out RaycastHit hit)
```
Performs a raycast from the specified controller.

**Parameters:**
- `node`: Controller to raycast from
- `hit`: Output raycast hit information

**Returns:** True if raycast hit something

**Example:**
```csharp
RaycastHit hit;
if (controller.ControllerRaycast(XRNode.RightHand, out hit))
{
    Debug.Log($"Hit object: {hit.collider.name}");
}
```

#### TriggerHaptic(XRNode node, float amplitude = 0.5f, float duration = 0.1f)
```csharp
public void TriggerHaptic(XRNode node, float amplitude = 0.5f, float duration = 0.1f)
```
Triggers haptic feedback on a controller.

**Parameters:**
- `node`: Controller to vibrate
- `amplitude`: Vibration strength (0.0 to 1.0)
- `duration`: Duration in seconds

#### GetControllerVelocity(XRNode node)
```csharp
public Vector3 GetControllerVelocity(XRNode node)
```
Gets the velocity of a controller.

**Parameters:**
- `node`: Controller to query

**Returns:** Velocity vector

---

## VRMenuSystem

**Namespace:** `MQ3DVirtualHangout.UI`

Manages VR-optimized user interface.

### Methods

#### ShowMainMenu()
```csharp
public void ShowMainMenu()
```
Displays the main menu panel.

#### ShowWorldBrowser()
```csharp
public void ShowWorldBrowser()
```
Displays the world browser panel.

#### ShowModelLibrary()
```csharp
public void ShowModelLibrary()
```
Displays the model library panel.

#### ShowSettings()
```csharp
public void ShowSettings()
```
Displays the settings panel.

#### HideAllPanels()
```csharp
public void HideAllPanels()
```
Hides all menu panels.

#### ToggleMenu()
```csharp
public void ToggleMenu()
```
Toggles menu visibility on/off.

---

## Usage Examples

### Complete Room Capture Workflow
```csharp
// Initialize system
VirtualHangoutManager manager = GetComponent<VirtualHangoutManager>();
manager.Initialize();

// Start capturing
manager.StartRoomCapture();

// ... user walks around ...

// Stop and save
manager.StopRoomCapture();
```

### Drawing in 3D Space
```csharp
VRDrawingTool drawing = GetComponent<VRDrawingTool>();
VRControllerManager controller = GetComponent<VRControllerManager>();

// Set up drawing
drawing.SetBrushColor(Color.red);
drawing.SetBrushSize(0.02f);
drawing.SetDrawingMode(VRDrawingTool.DrawingMode.FreeDraw);

// Subscribe to controller events
controller.OnRightTriggerPressed += (pos) => drawing.StartDrawing(pos);
controller.OnRightTriggerReleased += () => drawing.StopDrawing();
```

### Importing and Placing Models
```csharp
ModelImportManager importer = GetComponent<ModelImportManager>();

// Import a model
importer.ImportModel("/path/to/my/model.fbx");

// Get imported models
var models = importer.GetImportedModels();
string modelId = models[0].modelId;

// Place the model
Vector3 position = new Vector3(0, 0, 5);
Quaternion rotation = Quaternion.identity;
GameObject instance = importer.PlaceModel(modelId, position, rotation);

// Manipulate it
importer.ScaleModel(instance, Vector3.one * 2f);
importer.RotateModel(instance, new Vector3(0, 45, 0));
```

### World Sharing
```csharp
WorldSharingManager sharing = GetComponent<WorldSharingManager>();

// Share a world
byte[] worldData = GetWorldData();
sharing.ShareWorld("My World", "A cool VR space", worldData);

// Browse worlds
var worlds = sharing.GetAvailableWorlds();

// Search for specific worlds
var results = sharing.SearchWorlds("stadium");

// Load a world
sharing.LoadSharedWorld(worlds[0].worldId);

// Rate a world
sharing.RateWorld(worlds[0].worldId, 5.0f);
```

---

## Error Handling

All methods use Unity's Debug.Log system for error reporting. Check the console for:

- `Debug.Log`: Informational messages
- `Debug.LogWarning`: Non-critical issues
- `Debug.LogError`: Critical errors

Example error handling:
```csharp
try
{
    importer.ImportModel(filePath);
}
catch (System.Exception e)
{
    Debug.LogError($"Failed to import model: {e.Message}");
}
```

---

**API Version:** 1.0.0  
**Last Updated:** November 11, 2025
