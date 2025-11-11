using UnityEngine;
using System.Collections.Generic;

namespace MQ3DVirtualHangout.DrawingTools
{
    /// <summary>
    /// 3D drawing tool for VR that allows users to draw in 3D space
    /// Supports multiple brush types, colors, and drawing modes
    /// </summary>
    public class VRDrawingTool : MonoBehaviour
    {
        [Header("Drawing Settings")]
        [SerializeField] private float brushSize = 0.01f;
        [SerializeField] private Color brushColor = Color.white;
        [SerializeField] private Material lineMaterial;
        [SerializeField] private float minDistance = 0.01f; // Minimum distance between points

        [Header("Drawing Modes")]
        [SerializeField] private DrawingMode currentMode = DrawingMode.FreeDraw;

        public enum DrawingMode
        {
            FreeDraw,
            StraightLine,
            Curve,
            Shape3D,
            Spray,
            Erase
        }

        private bool isDrawing = false;
        private LineRenderer currentLine;
        private List<Vector3> currentPoints = new List<Vector3>();
        private List<GameObject> drawnObjects = new List<GameObject>();
        private Vector3 lastPoint;

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

        private void Start()
        {
            InitializeDrawingTool();
        }

        private void InitializeDrawingTool()
        {
            if (lineMaterial == null)
            {
                // Create default material for lines
                lineMaterial = new Material(Shader.Find("Sprites/Default"));
                lineMaterial.color = brushColor;
            }

            Debug.Log("VR Drawing Tool initialized");
        }

        /// <summary>
        /// Starts drawing at the specified position
        /// </summary>
        public void StartDrawing(Vector3 position)
        {
            isDrawing = true;
            currentPoints.Clear();
            lastPoint = position;

            CreateNewLine(position);
            Debug.Log($"Started drawing in {currentMode} mode");
        }

        /// <summary>
        /// Continues drawing to the specified position
        /// </summary>
        public void ContinueDrawing(Vector3 position)
        {
            if (!isDrawing || currentLine == null) return;

            float distance = Vector3.Distance(position, lastPoint);
            
            if (distance >= minDistance)
            {
                AddPoint(position);
                lastPoint = position;
            }
        }

        /// <summary>
        /// Stops the current drawing
        /// </summary>
        public void StopDrawing()
        {
            if (isDrawing)
            {
                isDrawing = false;
                FinalizeDrawing();
                Debug.Log($"Stopped drawing. Created {currentPoints.Count} points");
            }
        }

        /// <summary>
        /// Changes the brush color
        /// </summary>
        public void SetBrushColor(Color color)
        {
            brushColor = color;
            
            if (lineMaterial != null)
            {
                lineMaterial.color = color;
            }
        }

        /// <summary>
        /// Changes the brush size
        /// </summary>
        public void SetBrushSize(float size)
        {
            brushSize = Mathf.Max(0.001f, size);
            
            if (currentLine != null)
            {
                currentLine.startWidth = brushSize;
                currentLine.endWidth = brushSize;
            }
        }

        /// <summary>
        /// Changes the drawing mode
        /// </summary>
        public void SetDrawingMode(DrawingMode mode)
        {
            currentMode = mode;
            Debug.Log($"Drawing mode changed to {mode}");
        }

        /// <summary>
        /// Clears all drawings
        /// </summary>
        public void ClearAllDrawings()
        {
            foreach (GameObject obj in drawnObjects)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
            
            drawnObjects.Clear();
            Debug.Log("Cleared all drawings");
        }

        /// <summary>
        /// Undoes the last drawing
        /// </summary>
        public void UndoLastDrawing()
        {
            if (drawnObjects.Count > 0)
            {
                GameObject lastDrawing = drawnObjects[drawnObjects.Count - 1];
                drawnObjects.RemoveAt(drawnObjects.Count - 1);
                
                if (lastDrawing != null)
                {
                    Destroy(lastDrawing);
                }
                
                Debug.Log("Undid last drawing");
            }
        }

        /// <summary>
        /// Exports drawing data for saving/sharing
        /// </summary>
        public DrawingData ExportDrawing()
        {
            DrawingData data = new DrawingData
            {
                drawingId = System.Guid.NewGuid().ToString(),
                points = currentPoints.ToArray(),
                color = brushColor,
                brushSize = brushSize,
                mode = currentMode,
                timestamp = System.DateTime.Now.ToString()
            };

            return data;
        }

        /// <summary>
        /// Imports and displays drawing data
        /// </summary>
        public void ImportDrawing(DrawingData data)
        {
            brushColor = data.color;
            brushSize = data.brushSize;
            currentMode = data.mode;

            GameObject drawingObject = new GameObject($"ImportedDrawing_{data.drawingId}");
            LineRenderer lineRenderer = drawingObject.AddComponent<LineRenderer>();
            
            SetupLineRenderer(lineRenderer);
            lineRenderer.positionCount = data.points.Length;
            lineRenderer.SetPositions(data.points);
            
            drawnObjects.Add(drawingObject);
            Debug.Log($"Imported drawing with {data.points.Length} points");
        }

        /// <summary>
        /// Creates 3D shapes (sphere, cube, cylinder)
        /// </summary>
        public GameObject CreateShape3D(string shapeType, Vector3 position, float size)
        {
            GameObject shape = null;

            switch (shapeType.ToLower())
            {
                case "sphere":
                    shape = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    break;
                case "cube":
                    shape = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
                case "cylinder":
                    shape = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    break;
            }

            if (shape != null)
            {
                shape.transform.position = position;
                shape.transform.localScale = Vector3.one * size;
                
                Renderer renderer = shape.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = new Material(lineMaterial);
                    renderer.material.color = brushColor;
                }

                drawnObjects.Add(shape);
                Debug.Log($"Created {shapeType} at {position}");
            }

            return shape;
        }

        private void CreateNewLine(Vector3 startPosition)
        {
            GameObject lineObject = new GameObject($"Drawing_{System.DateTime.Now.Ticks}");
            currentLine = lineObject.AddComponent<LineRenderer>();
            
            SetupLineRenderer(currentLine);
            AddPoint(startPosition);
            
            drawnObjects.Add(lineObject);
        }

        private void SetupLineRenderer(LineRenderer lineRenderer)
        {
            lineRenderer.material = lineMaterial;
            lineRenderer.startWidth = brushSize;
            lineRenderer.endWidth = brushSize;
            lineRenderer.positionCount = 0;
            lineRenderer.useWorldSpace = true;
            lineRenderer.numCornerVertices = 5;
            lineRenderer.numCapVertices = 5;
        }

        private void AddPoint(Vector3 point)
        {
            currentPoints.Add(point);
            currentLine.positionCount = currentPoints.Count;
            currentLine.SetPosition(currentPoints.Count - 1, point);
        }

        private void FinalizeDrawing()
        {
            if (currentMode == DrawingMode.StraightLine && currentPoints.Count >= 2)
            {
                // Simplify to straight line
                Vector3 start = currentPoints[0];
                Vector3 end = currentPoints[currentPoints.Count - 1];
                
                currentPoints.Clear();
                currentPoints.Add(start);
                currentPoints.Add(end);
                
                currentLine.positionCount = 2;
                currentLine.SetPositions(currentPoints.ToArray());
            }
        }

        /// <summary>
        /// Gets all drawn objects for manipulation
        /// </summary>
        public List<GameObject> GetDrawnObjects()
        {
            return new List<GameObject>(drawnObjects);
        }
    }
}
