using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace MQ3DVirtualHangout.ModelImport
{
    /// <summary>
    /// Manages importing and placing 3D models in the virtual environment
    /// Supports various 3D file formats (FBX, OBJ, GLTF)
    /// </summary>
    public class ModelImportManager : MonoBehaviour
    {
        [Header("Import Settings")]
        [SerializeField] private float defaultScale = 1f;
        [SerializeField] private bool autoCenter = true;
        
        [Header("Model Library")]
        [SerializeField] private string modelsDirectory = "ImportedModels";
        
        private List<ImportedModel> importedModels = new List<ImportedModel>();
        private GameObject selectedModel;

        [System.Serializable]
        public class ImportedModel
        {
            public string modelId;
            public string modelName;
            public string filePath;
            public string format; // FBX, OBJ, GLTF, etc.
            public Vector3 scale;
            public GameObject prefab;
            public Texture2D thumbnail;
            public long fileSize;
            public string importDate;
        }

        private void Start()
        {
            InitializeModelImport();
        }

        private void InitializeModelImport()
        {
            string modelsPath = Path.Combine(Application.persistentDataPath, modelsDirectory);
            
            if (!Directory.Exists(modelsPath))
            {
                Directory.CreateDirectory(modelsPath);
            }

            LoadImportedModels();
            Debug.Log($"Model Import Manager initialized. {importedModels.Count} models available");
        }

        /// <summary>
        /// Imports a 3D model from file path
        /// </summary>
        public void ImportModel(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"Model file not found: {filePath}");
                return;
            }

            string extension = Path.GetExtension(filePath).ToLower();
            string format = GetFormatFromExtension(extension);

            if (string.IsNullOrEmpty(format))
            {
                Debug.LogError($"Unsupported file format: {extension}");
                return;
            }

            ImportedModel model = new ImportedModel
            {
                modelId = System.Guid.NewGuid().ToString(),
                modelName = Path.GetFileNameWithoutExtension(filePath),
                filePath = filePath,
                format = format,
                scale = Vector3.one * defaultScale,
                fileSize = new FileInfo(filePath).Length,
                importDate = System.DateTime.Now.ToString()
            };

            // Load the model based on format
            GameObject loadedModel = LoadModelByFormat(filePath, format);
            
            if (loadedModel != null)
            {
                model.prefab = loadedModel;
                
                if (autoCenter)
                {
                    CenterModel(loadedModel);
                }

                importedModels.Add(model);
                Debug.Log($"Successfully imported model: {model.modelName} ({format})");
            }
        }

        /// <summary>
        /// Places an imported model in the scene at specified position
        /// </summary>
        public GameObject PlaceModel(string modelId, Vector3 position, Quaternion rotation)
        {
            ImportedModel model = importedModels.Find(m => m.modelId == modelId);
            
            if (model != null && model.prefab != null)
            {
                GameObject instance = Instantiate(model.prefab, position, rotation);
                instance.name = $"{model.modelName}_Instance";
                instance.transform.localScale = model.scale;
                
                // Add interaction components
                AddModelInteraction(instance);
                
                Debug.Log($"Placed model '{model.modelName}' at {position}");
                return instance;
            }

            return null;
        }

        /// <summary>
        /// Gets list of all imported models
        /// </summary>
        public List<ImportedModel> GetImportedModels()
        {
            return new List<ImportedModel>(importedModels);
        }

        /// <summary>
        /// Deletes an imported model from the library
        /// </summary>
        public void DeleteModel(string modelId)
        {
            ImportedModel model = importedModels.Find(m => m.modelId == modelId);
            
            if (model != null)
            {
                if (model.prefab != null)
                {
                    Destroy(model.prefab);
                }
                
                importedModels.Remove(model);
                Debug.Log($"Deleted model: {model.modelName}");
            }
        }

        /// <summary>
        /// Scales a placed model
        /// </summary>
        public void ScaleModel(GameObject modelInstance, Vector3 scale)
        {
            if (modelInstance != null)
            {
                modelInstance.transform.localScale = scale;
            }
        }

        /// <summary>
        /// Rotates a placed model
        /// </summary>
        public void RotateModel(GameObject modelInstance, Vector3 rotation)
        {
            if (modelInstance != null)
            {
                modelInstance.transform.Rotate(rotation);
            }
        }

        private string GetFormatFromExtension(string extension)
        {
            switch (extension)
            {
                case ".fbx":
                    return "FBX";
                case ".obj":
                    return "OBJ";
                case ".gltf":
                case ".glb":
                    return "GLTF";
                case ".dae":
                    return "Collada";
                default:
                    return null;
            }
        }

        private GameObject LoadModelByFormat(string filePath, string format)
        {
            GameObject model = null;

            switch (format)
            {
                case "FBX":
                    model = LoadFBX(filePath);
                    break;
                case "OBJ":
                    model = LoadOBJ(filePath);
                    break;
                case "GLTF":
                    model = LoadGLTF(filePath);
                    break;
                default:
                    Debug.LogWarning($"Format {format} loading not yet implemented");
                    // Create a placeholder cube for now
                    model = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    model.name = "PlaceholderModel";
                    break;
            }

            return model;
        }

        private GameObject LoadFBX(string filePath)
        {
            // FBX loading would use Unity's FBX importer
            // For now, create a placeholder
            Debug.Log($"Loading FBX from {filePath}");
            return CreatePlaceholderModel("FBX_Model");
        }

        private GameObject LoadOBJ(string filePath)
        {
            // OBJ loading implementation
            Debug.Log($"Loading OBJ from {filePath}");
            return CreatePlaceholderModel("OBJ_Model");
        }

        private GameObject LoadGLTF(string filePath)
        {
            // GLTF loading implementation
            Debug.Log($"Loading GLTF from {filePath}");
            return CreatePlaceholderModel("GLTF_Model");
        }

        private GameObject CreatePlaceholderModel(string name)
        {
            GameObject placeholder = GameObject.CreatePrimitive(PrimitiveType.Cube);
            placeholder.name = name;
            placeholder.SetActive(false); // Keep inactive as prefab
            return placeholder;
        }

        private void CenterModel(GameObject model)
        {
            Renderer renderer = model.GetComponentInChildren<Renderer>();
            
            if (renderer != null)
            {
                Vector3 center = renderer.bounds.center;
                model.transform.position = -center;
            }
        }

        private void AddModelInteraction(GameObject model)
        {
            // Add collider if not present
            if (model.GetComponent<Collider>() == null)
            {
                model.AddComponent<BoxCollider>();
            }

            // Add rigidbody for physics
            if (model.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = model.AddComponent<Rigidbody>();
                rb.useGravity = false;
            }
        }

        private void LoadImportedModels()
        {
            // Load previously imported models from persistent storage
            string modelsPath = Path.Combine(Application.persistentDataPath, modelsDirectory);
            
            if (Directory.Exists(modelsPath))
            {
                // Load model metadata from files
                Debug.Log($"Loading models from {modelsPath}");
            }
        }
    }
}
