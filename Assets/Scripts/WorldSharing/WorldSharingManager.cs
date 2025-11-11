using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace MQ3DVirtualHangout.WorldSharing
{
    /// <summary>
    /// Manages sharing and exploring worlds created by users
    /// Handles world upload, download, and browsing functionality
    /// </summary>
    public class WorldSharingManager : MonoBehaviour
    {
        [Header("World Storage")]
        [SerializeField] private string worldsDirectory = "SharedWorlds";
        
        private List<WorldMetadata> availableWorlds = new List<WorldMetadata>();
        private WorldMetadata currentWorld;

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

        private void Start()
        {
            InitializeWorldSharing();
        }

        private void InitializeWorldSharing()
        {
            string worldsPath = Path.Combine(Application.persistentDataPath, worldsDirectory);
            
            if (!Directory.Exists(worldsPath))
            {
                Directory.CreateDirectory(worldsPath);
            }

            LoadAvailableWorlds();
            Debug.Log($"World Sharing Manager initialized. {availableWorlds.Count} worlds available");
        }

        /// <summary>
        /// Loads list of available worlds from storage
        /// </summary>
        private void LoadAvailableWorlds()
        {
            availableWorlds.Clear();
            string worldsPath = Path.Combine(Application.persistentDataPath, worldsDirectory);

            if (Directory.Exists(worldsPath))
            {
                string[] worldFiles = Directory.GetFiles(worldsPath, "*.world");
                
                foreach (string worldFile in worldFiles)
                {
                    WorldMetadata metadata = LoadWorldMetadata(worldFile);
                    if (metadata != null)
                    {
                        availableWorlds.Add(metadata);
                    }
                }
            }
        }

        /// <summary>
        /// Shares a captured world with other users
        /// </summary>
        public void ShareWorld(string worldName, string description, byte[] worldData)
        {
            WorldMetadata metadata = new WorldMetadata
            {
                worldId = System.Guid.NewGuid().ToString(),
                worldName = worldName,
                creatorName = SystemInfo.deviceName,
                description = description,
                creationDate = System.DateTime.Now.ToString(),
                fileSize = worldData.Length,
                downloadCount = 0,
                rating = 0f,
                tags = new string[] { "user-created", "vr-space" }
            };

            SaveWorld(metadata, worldData);
            availableWorlds.Add(metadata);
            
            Debug.Log($"World '{worldName}' shared successfully!");
        }

        /// <summary>
        /// Loads a shared world into the current scene
        /// </summary>
        public void LoadSharedWorld(string worldId)
        {
            WorldMetadata world = availableWorlds.Find(w => w.worldId == worldId);
            
            if (world != null)
            {
                byte[] worldData = LoadWorldData(world.worldDataPath);
                
                if (worldData != null)
                {
                    InstantiateWorld(worldData);
                    currentWorld = world;
                    world.downloadCount++;
                    
                    Debug.Log($"Loaded world: {world.worldName} by {world.creatorName}");
                }
            }
        }

        /// <summary>
        /// Gets list of all available worlds for browsing
        /// </summary>
        public List<WorldMetadata> GetAvailableWorlds()
        {
            return new List<WorldMetadata>(availableWorlds);
        }

        /// <summary>
        /// Searches worlds by tags or keywords
        /// </summary>
        public List<WorldMetadata> SearchWorlds(string searchQuery)
        {
            List<WorldMetadata> results = new List<WorldMetadata>();

            foreach (var world in availableWorlds)
            {
                if (world.worldName.ToLower().Contains(searchQuery.ToLower()) ||
                    world.description.ToLower().Contains(searchQuery.ToLower()) ||
                    System.Array.Exists(world.tags, tag => tag.ToLower().Contains(searchQuery.ToLower())))
                {
                    results.Add(world);
                }
            }

            return results;
        }

        /// <summary>
        /// Rates a world (1-5 stars)
        /// </summary>
        public void RateWorld(string worldId, float rating)
        {
            WorldMetadata world = availableWorlds.Find(w => w.worldId == worldId);
            
            if (world != null)
            {
                world.rating = Mathf.Clamp(rating, 0f, 5f);
                SaveWorldMetadata(world);
                Debug.Log($"Rated world '{world.worldName}' with {rating} stars");
            }
        }

        private void SaveWorld(WorldMetadata metadata, byte[] worldData)
        {
            string worldsPath = Path.Combine(Application.persistentDataPath, worldsDirectory);
            string worldFilePath = Path.Combine(worldsPath, $"{metadata.worldId}.world");
            string metadataPath = Path.Combine(worldsPath, $"{metadata.worldId}.meta");

            metadata.worldDataPath = worldFilePath;

            File.WriteAllBytes(worldFilePath, worldData);
            string metadataJson = JsonUtility.ToJson(metadata, true);
            File.WriteAllText(metadataPath, metadataJson);
        }

        private WorldMetadata LoadWorldMetadata(string worldFile)
        {
            string metadataPath = worldFile.Replace(".world", ".meta");
            
            if (File.Exists(metadataPath))
            {
                string metadataJson = File.ReadAllText(metadataPath);
                return JsonUtility.FromJson<WorldMetadata>(metadataJson);
            }

            return null;
        }

        private void SaveWorldMetadata(WorldMetadata metadata)
        {
            string metadataPath = metadata.worldDataPath.Replace(".world", ".meta");
            string metadataJson = JsonUtility.ToJson(metadata, true);
            File.WriteAllText(metadataPath, metadataJson);
        }

        private byte[] LoadWorldData(string worldFilePath)
        {
            if (File.Exists(worldFilePath))
            {
                return File.ReadAllBytes(worldFilePath);
            }

            return null;
        }

        private void InstantiateWorld(byte[] worldData)
        {
            // Deserialize and instantiate the world in the scene
            Debug.Log($"Instantiating world from {worldData.Length} bytes of data");
            // Implementation would deserialize the world mesh and objects
        }
    }
}
