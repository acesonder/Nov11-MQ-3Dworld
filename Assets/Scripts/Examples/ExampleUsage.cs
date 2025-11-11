using UnityEngine;
using MQ3DVirtualHangout;
using MQ3DVirtualHangout.RoomCapture;
using MQ3DVirtualHangout.WorldSharing;
using MQ3DVirtualHangout.ModelImport;
using MQ3DVirtualHangout.DrawingTools;
using MQ3DVirtualHangout.VRInteraction;

namespace MQ3DVirtualHangout.Examples
{
    /// <summary>
    /// Example script demonstrating how to use all the systems together
    /// This script shows common use cases and patterns for the MQ3D Virtual Hangout application
    /// </summary>
    public class ExampleUsage : MonoBehaviour
    {
        [Header("System References")]
        [SerializeField] private VirtualHangoutManager mainManager;
        [SerializeField] private SpatialCaptureManager captureManager;
        [SerializeField] private WorldSharingManager sharingManager;
        [SerializeField] private ModelImportManager modelManager;
        [SerializeField] private VRDrawingTool drawingTool;
        [SerializeField] private VRControllerManager controllerManager;

        private void Start()
        {
            // Initialize all systems
            InitializeSystems();
            
            // Set up event listeners
            SetupEventHandlers();
            
            // Show some example usage
            ExampleWorkflows();
        }

        /// <summary>
        /// Initialize all systems if not already initialized
        /// </summary>
        private void InitializeSystems()
        {
            Debug.Log("=== Initializing MQ3D Virtual Hangout Systems ===");

            // Get references if not set
            if (mainManager == null)
            {
                mainManager = FindObjectOfType<VirtualHangoutManager>();
            }

            if (captureManager == null)
            {
                captureManager = FindObjectOfType<SpatialCaptureManager>();
            }

            if (sharingManager == null)
            {
                sharingManager = FindObjectOfType<WorldSharingManager>();
            }

            if (modelManager == null)
            {
                modelManager = FindObjectOfType<ModelImportManager>();
            }

            if (drawingTool == null)
            {
                drawingTool = FindObjectOfType<VRDrawingTool>();
            }

            if (controllerManager == null)
            {
                controllerManager = FindObjectOfType<VRControllerManager>();
            }

            // Initialize main manager (which initializes all subsystems)
            if (mainManager != null)
            {
                mainManager.Initialize();
            }

            Debug.Log("All systems initialized successfully!");
        }

        /// <summary>
        /// Set up event handlers for controller inputs
        /// </summary>
        private void SetupEventHandlers()
        {
            if (controllerManager != null)
            {
                // Subscribe to trigger events for drawing
                controllerManager.OnRightTriggerPressed += HandleDrawingStart;
                controllerManager.OnRightTriggerReleased += HandleDrawingStop;

                // Subscribe to grip events for grabbing objects
                controllerManager.OnRightGripPressed += HandleGripPressed;

                Debug.Log("Event handlers set up successfully!");
            }
        }

        /// <summary>
        /// Example workflows showing common use cases
        /// </summary>
        private void ExampleWorkflows()
        {
            Debug.Log("=== Example Workflows ===");
            
            // Example 1: Room Capture Workflow
            ExampleRoomCaptureWorkflow();
            
            // Example 2: World Sharing Workflow
            ExampleWorldSharingWorkflow();
            
            // Example 3: Drawing Workflow
            ExampleDrawingWorkflow();
            
            // Example 4: Model Import Workflow
            ExampleModelImportWorkflow();
        }

        /// <summary>
        /// Example 1: Complete room capture workflow
        /// </summary>
        private void ExampleRoomCaptureWorkflow()
        {
            Debug.Log("--- Example 1: Room Capture Workflow ---");
            
            // Step 1: Enter room capture mode
            if (mainManager != null)
            {
                mainManager.SetMode(VirtualHangoutManager.AppMode.RoomCapture);
            }

            // Step 2: Start capturing (would be triggered by user button press)
            // captureManager.StartCapture();
            // User walks around to scan the environment
            
            // Step 3: Stop capture and save (after user is done scanning)
            // captureManager.StopCapture();
            // var roomData = captureManager.ExportRoomData();
            
            Debug.Log("Room capture workflow example completed");
        }

        /// <summary>
        /// Example 2: World sharing and exploration workflow
        /// </summary>
        private void ExampleWorldSharingWorkflow()
        {
            Debug.Log("--- Example 2: World Sharing Workflow ---");

            if (sharingManager == null) return;

            // Browse available worlds
            var availableWorlds = sharingManager.GetAvailableWorlds();
            Debug.Log($"Found {availableWorlds.Count} available worlds");

            // Search for specific worlds
            var searchResults = sharingManager.SearchWorlds("stadium");
            Debug.Log($"Found {searchResults.Count} stadium worlds");

            // Example: Load first available world (if any)
            if (availableWorlds.Count > 0)
            {
                string worldId = availableWorlds[0].worldId;
                Debug.Log($"Loading world: {availableWorlds[0].worldName}");
                // sharingManager.LoadSharedWorld(worldId);
                
                // Rate the world
                // sharingManager.RateWorld(worldId, 4.5f);
            }

            Debug.Log("World sharing workflow example completed");
        }

        /// <summary>
        /// Example 3: 3D drawing workflow
        /// </summary>
        private void ExampleDrawingWorkflow()
        {
            Debug.Log("--- Example 3: Drawing Workflow ---");

            if (drawingTool == null) return;

            // Set drawing mode to free draw
            drawingTool.SetDrawingMode(VRDrawingTool.DrawingMode.FreeDraw);

            // Customize brush
            drawingTool.SetBrushColor(Color.red);
            drawingTool.SetBrushSize(0.02f);

            // Drawing is handled by controller events (see HandleDrawingStart/Stop)

            // Create a 3D shape
            Vector3 shapePosition = Camera.main.transform.position + Camera.main.transform.forward * 2f;
            // drawingTool.CreateShape3D("sphere", shapePosition, 0.3f);

            Debug.Log("Drawing workflow example completed");
        }

        /// <summary>
        /// Example 4: Model import and placement workflow
        /// </summary>
        private void ExampleModelImportWorkflow()
        {
            Debug.Log("--- Example 4: Model Import Workflow ---");

            if (modelManager == null) return;

            // Get list of imported models
            var importedModels = modelManager.GetImportedModels();
            Debug.Log($"Currently have {importedModels.Count} imported models");

            // Example: Import a model (would be triggered by file picker)
            // string modelPath = "/path/to/model.fbx";
            // modelManager.ImportModel(modelPath);

            // Example: Place a model (if any are available)
            if (importedModels.Count > 0)
            {
                string modelId = importedModels[0].modelId;
                Vector3 placePosition = Camera.main.transform.position + Camera.main.transform.forward * 3f;
                Quaternion placeRotation = Quaternion.identity;

                // GameObject placedModel = modelManager.PlaceModel(modelId, placePosition, placeRotation);
                
                // Manipulate the placed model
                // modelManager.ScaleModel(placedModel, Vector3.one * 2f);
                // modelManager.RotateModel(placedModel, new Vector3(0, 45, 0));

                Debug.Log($"Placed model: {importedModels[0].modelName}");
            }

            Debug.Log("Model import workflow example completed");
        }

        /// <summary>
        /// Handle drawing start when right trigger is pressed
        /// </summary>
        private void HandleDrawingStart(Vector3 position)
        {
            if (mainManager != null && 
                mainManager.GetCurrentMode() == VirtualHangoutManager.AppMode.Drawing &&
                drawingTool != null)
            {
                drawingTool.StartDrawing(position);
                Debug.Log("Started drawing at " + position);

                // Provide haptic feedback
                if (controllerManager != null)
                {
                    controllerManager.TriggerHaptic(UnityEngine.XR.XRNode.RightHand, 0.3f, 0.1f);
                }
            }
        }

        /// <summary>
        /// Handle drawing stop when right trigger is released
        /// </summary>
        private void HandleDrawingStop()
        {
            if (drawingTool != null)
            {
                drawingTool.StopDrawing();
                Debug.Log("Stopped drawing");
            }
        }

        /// <summary>
        /// Handle grip press for object manipulation
        /// </summary>
        private void HandleGripPressed(float gripValue)
        {
            if (gripValue > 0.8f && controllerManager != null)
            {
                // Perform raycast to detect objects
                RaycastHit hit;
                if (controllerManager.ControllerRaycast(UnityEngine.XR.XRNode.RightHand, out hit))
                {
                    Debug.Log($"Gripping object: {hit.collider.gameObject.name}");
                    // Could implement object grabbing here
                }
            }
        }

        /// <summary>
        /// Example of switching between different modes
        /// </summary>
        public void DemonstrateModeSwitching()
        {
            if (mainManager == null) return;

            Debug.Log("=== Demonstrating Mode Switching ===");

            // Switch to room capture
            mainManager.SetMode(VirtualHangoutManager.AppMode.RoomCapture);
            Debug.Log("Switched to Room Capture mode");

            // Switch to drawing
            mainManager.SetMode(VirtualHangoutManager.AppMode.Drawing);
            Debug.Log("Switched to Drawing mode");

            // Switch to model placement
            mainManager.SetMode(VirtualHangoutManager.AppMode.ModelPlacement);
            Debug.Log("Switched to Model Placement mode");

            // Switch to world exploration
            mainManager.SetMode(VirtualHangoutManager.AppMode.WorldExploration);
            Debug.Log("Switched to World Exploration mode");

            // Return to menu
            mainManager.SetMode(VirtualHangoutManager.AppMode.Menu);
            Debug.Log("Switched to Menu mode");
        }

        /// <summary>
        /// Example of complete workflow: Capture room, share it, and load it back
        /// </summary>
        public void DemonstrateCompleteWorkflow()
        {
            Debug.Log("=== Demonstrating Complete Workflow ===");

            if (captureManager == null || sharingManager == null) return;

            // 1. Capture a room
            Debug.Log("Step 1: Starting room capture...");
            captureManager.StartCapture();
            
            // (User would walk around here)
            
            // 2. Stop capture and get data
            Debug.Log("Step 2: Stopping capture and exporting data...");
            captureManager.StopCapture();
            var roomData = captureManager.ExportRoomData();
            
            // 3. Share the captured room
            Debug.Log("Step 3: Sharing the captured room...");
            string worldName = "My Captured Space";
            string description = "A demonstration of room capture";
            byte[] worldData = SerializeRoomData(roomData);
            sharingManager.ShareWorld(worldName, description, worldData);
            
            // 4. Browse and load worlds
            Debug.Log("Step 4: Browsing available worlds...");
            var worlds = sharingManager.GetAvailableWorlds();
            
            if (worlds.Count > 0)
            {
                Debug.Log("Step 5: Loading the most recent world...");
                sharingManager.LoadSharedWorld(worlds[worlds.Count - 1].worldId);
            }

            Debug.Log("Complete workflow demonstration finished!");
        }

        /// <summary>
        /// Helper method to serialize room data
        /// </summary>
        private byte[] SerializeRoomData(SpatialCaptureManager.RoomData roomData)
        {
            string json = JsonUtility.ToJson(roomData);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        private void OnDestroy()
        {
            // Clean up event handlers
            if (controllerManager != null)
            {
                controllerManager.OnRightTriggerPressed -= HandleDrawingStart;
                controllerManager.OnRightTriggerReleased -= HandleDrawingStop;
                controllerManager.OnRightGripPressed -= HandleGripPressed;
            }
        }

        /// <summary>
        /// Display current system statistics
        /// </summary>
        private void Update()
        {
            // Press Space key in editor to demonstrate workflows
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DemonstrateModeSwitching();
            }

            // Press C key to demonstrate complete workflow
            if (Input.GetKeyDown(KeyCode.C))
            {
                DemonstrateCompleteWorkflow();
            }
        }
    }
}
