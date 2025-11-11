using UnityEngine;
using MQ3DVirtualHangout.RoomCapture;
using MQ3DVirtualHangout.WorldSharing;
using MQ3DVirtualHangout.ModelImport;
using MQ3DVirtualHangout.DrawingTools;
using MQ3DVirtualHangout.VRInteraction;

namespace MQ3DVirtualHangout
{
    /// <summary>
    /// Main manager that coordinates all systems for the Meta Quest 3 Virtual Hangout Room
    /// Integrates room capture, world sharing, model import, and drawing tools
    /// </summary>
    public class VirtualHangoutManager : MonoBehaviour
    {
        [Header("System References")]
        [SerializeField] private SpatialCaptureManager spatialCapture;
        [SerializeField] private WorldSharingManager worldSharing;
        [SerializeField] private ModelImportManager modelImport;
        [SerializeField] private VRDrawingTool drawingTool;
        [SerializeField] private VRControllerManager controllerManager;

        [Header("Application Settings")]
        [SerializeField] private bool autoInitialize = true;
        [SerializeField] private string applicationVersion = "1.0.0";

        public enum AppMode
        {
            RoomCapture,
            WorldExploration,
            ModelPlacement,
            Drawing,
            Menu
        }

        private AppMode currentMode = AppMode.Menu;
        private bool isInitialized = false;

        private void Awake()
        {
            if (autoInitialize)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Initializes all systems
        /// </summary>
        public void Initialize()
        {
            if (isInitialized) return;

            Debug.Log($"Initializing MQ3D Virtual Hangout v{applicationVersion}");

            // Initialize systems if not already in scene
            if (spatialCapture == null)
            {
                GameObject captureObj = new GameObject("SpatialCaptureManager");
                spatialCapture = captureObj.AddComponent<SpatialCaptureManager>();
                captureObj.transform.SetParent(transform);
            }

            if (worldSharing == null)
            {
                GameObject sharingObj = new GameObject("WorldSharingManager");
                worldSharing = sharingObj.AddComponent<WorldSharingManager>();
                sharingObj.transform.SetParent(transform);
            }

            if (modelImport == null)
            {
                GameObject importObj = new GameObject("ModelImportManager");
                modelImport = importObj.AddComponent<ModelImportManager>();
                importObj.transform.SetParent(transform);
            }

            if (drawingTool == null)
            {
                GameObject drawingObj = new GameObject("VRDrawingTool");
                drawingTool = drawingObj.AddComponent<VRDrawingTool>();
                drawingObj.transform.SetParent(transform);
            }

            if (controllerManager == null)
            {
                GameObject controllerObj = new GameObject("VRControllerManager");
                controllerManager = controllerObj.AddComponent<VRControllerManager>();
                controllerObj.transform.SetParent(transform);
            }

            SetupControllerEvents();
            isInitialized = true;

            Debug.Log("MQ3D Virtual Hangout initialized successfully!");
        }

        private void SetupControllerEvents()
        {
            if (controllerManager != null)
            {
                // Subscribe to controller events
                controllerManager.OnRightTriggerPressed += OnDrawingTrigger;
                controllerManager.OnRightTriggerReleased += OnDrawingRelease;
            }
        }

        /// <summary>
        /// Changes the current application mode
        /// </summary>
        public void SetMode(AppMode mode)
        {
            currentMode = mode;
            Debug.Log($"Switched to {mode} mode");

            // Disable/enable systems based on mode
            UpdateSystemsForMode();
        }

        private void UpdateSystemsForMode()
        {
            // Enable/disable drawing based on mode
            if (drawingTool != null)
            {
                drawingTool.enabled = (currentMode == AppMode.Drawing);
            }
        }

        private void OnDrawingTrigger(Vector3 position)
        {
            if (currentMode == AppMode.Drawing && drawingTool != null)
            {
                drawingTool.StartDrawing(position);
            }
        }

        private void OnDrawingRelease()
        {
            if (drawingTool != null)
            {
                drawingTool.StopDrawing();
            }
        }

        /// <summary>
        /// Starts room capture mode
        /// </summary>
        public void StartRoomCapture()
        {
            SetMode(AppMode.RoomCapture);
            
            if (spatialCapture != null)
            {
                spatialCapture.StartCapture();
                Debug.Log("Room capture started - Walk around to scan the environment");
            }
        }

        /// <summary>
        /// Stops room capture and saves the data
        /// </summary>
        public void StopRoomCapture()
        {
            if (spatialCapture != null)
            {
                spatialCapture.StopCapture();
                var roomData = spatialCapture.ExportRoomData();
                
                // Save to world sharing system
                SaveCapturedRoom(roomData);
            }
        }

        private void SaveCapturedRoom(SpatialCaptureManager.RoomData roomData)
        {
            // Convert room data to shareable format
            byte[] data = SerializeRoomData(roomData);
            
            if (worldSharing != null)
            {
                worldSharing.ShareWorld(
                    $"CapturedRoom_{System.DateTime.Now:yyyyMMdd_HHmmss}",
                    "Room captured using VR spatial scanning",
                    data
                );
            }
        }

        private byte[] SerializeRoomData(SpatialCaptureManager.RoomData roomData)
        {
            // Serialize room data to bytes
            string json = JsonUtility.ToJson(roomData);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        /// <summary>
        /// Browses and loads available shared worlds
        /// </summary>
        public void BrowseSharedWorlds()
        {
            SetMode(AppMode.WorldExploration);
            
            if (worldSharing != null)
            {
                var worlds = worldSharing.GetAvailableWorlds();
                Debug.Log($"Found {worlds.Count} shared worlds available for exploration");
            }
        }

        /// <summary>
        /// Loads a specific world by ID
        /// </summary>
        public void LoadWorld(string worldId)
        {
            if (worldSharing != null)
            {
                worldSharing.LoadSharedWorld(worldId);
                SetMode(AppMode.WorldExploration);
            }
        }

        /// <summary>
        /// Enters model placement mode
        /// </summary>
        public void EnterModelPlacementMode()
        {
            SetMode(AppMode.ModelPlacement);
            Debug.Log("Model placement mode - Select and place 3D models");
        }

        /// <summary>
        /// Places a model at the specified position
        /// </summary>
        public void PlaceModel(string modelId, Vector3 position)
        {
            if (modelImport != null)
            {
                Quaternion rotation = Quaternion.identity;
                modelImport.PlaceModel(modelId, position, rotation);
            }
        }

        /// <summary>
        /// Enters drawing mode
        /// </summary>
        public void EnterDrawingMode()
        {
            SetMode(AppMode.Drawing);
            Debug.Log("Drawing mode - Use controller trigger to draw in 3D space");
        }

        /// <summary>
        /// Shows the main menu
        /// </summary>
        public void ShowMenu()
        {
            SetMode(AppMode.Menu);
            Debug.Log("Main menu");
        }

        /// <summary>
        /// Gets current application mode
        /// </summary>
        public AppMode GetCurrentMode()
        {
            return currentMode;
        }

        /// <summary>
        /// Gets statistics about the application
        /// </summary>
        public string GetStatistics()
        {
            int worldCount = worldSharing != null ? worldSharing.GetAvailableWorlds().Count : 0;
            int modelCount = modelImport != null ? modelImport.GetImportedModels().Count : 0;
            
            return $"Worlds: {worldCount} | Models: {modelCount} | Mode: {currentMode}";
        }

        private void OnDestroy()
        {
            // Cleanup controller events
            if (controllerManager != null)
            {
                controllerManager.OnRightTriggerPressed -= OnDrawingTrigger;
                controllerManager.OnRightTriggerReleased -= OnDrawingRelease;
            }
        }
    }
}
