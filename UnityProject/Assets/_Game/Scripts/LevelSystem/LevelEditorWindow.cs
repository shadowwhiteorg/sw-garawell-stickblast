#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;
using _Game.DataStructures;

namespace _Game.LevelSystem
{
    public class LevelEditorWindow : EditorWindow
    {
        private LevelData _levelData;
        private int _levelNr;
        private bool _isHorizontal = true;

        private const string LevelDataPath = "Assets/_Game/Data/";

        [MenuItem("Window/Level Editor")]
        public static void ShowWindow()
        {
            GetWindow<LevelEditorWindow>("Level Editor");
        }

        private void OnGUI()
        {
            GUILayout.Label("Level Editor", EditorStyles.boldLabel);

            _levelNr = EditorGUILayout.IntField("Level Number", _levelNr);

            if (GUILayout.Button("Load Level Data"))
            {
                LoadLevelData();
            }

            if (GUILayout.Button("Create New Level Data"))
            {
                CreateNewLevelData();
            }

            if (_levelData == null)
                return;

            // Grid Size
            _levelData.gridWidth = EditorGUILayout.IntField("Grid Width", _levelData.gridWidth);
            _levelData.gridHeight = EditorGUILayout.IntField("Grid Height", _levelData.gridHeight);
            _levelData.levelNr = _levelNr;

            // Line Orientation Toggle
            _isHorizontal = GUILayout.Toggle(_isHorizontal, "Horizontal", "Button");
            _isHorizontal = !GUILayout.Toggle(!_isHorizontal, "Vertical", "Button");

            if (GUILayout.Button("Reset Grid"))
            {
                ResetGrid();
            }

            if (GUILayout.Button("Save Level Data"))
            {
                SaveLevelData();
            }

            DrawGrid();
        }

        private void LoadLevelData()
        {
            string path = LevelDataPath + _levelNr + "LevelData.asset";
            _levelData = AssetDatabase.LoadAssetAtPath<LevelData>(path);

            if (_levelData == null)
            {
                Debug.LogWarning("No level found. Create a new one.");
            }
        }

        private void CreateNewLevelData()
        {
            string path = LevelDataPath + _levelNr + "LevelData.asset";

            if (File.Exists(path))
            {
                Debug.LogWarning("Level already exists. Load it instead.");
                return;
            }

            _levelData = CreateInstance<LevelData>();
            _levelData.levelNr = _levelNr;
            AssetDatabase.CreateAsset(_levelData, path);
            AssetDatabase.SaveAssets();
        }

        private void SaveLevelData()
        {
            if (_levelData == null) return;
            
            EditorUtility.SetDirty(_levelData);
            AssetDatabase.SaveAssets();
            Debug.Log("Level Data Saved.");
        }

        private void ResetGrid()
        {
            if (_levelData != null)
            {
                _levelData.initialLines.Clear();
                Debug.Log("Grid Reset.");
            }
        }

        private void DrawGrid()
        {
            float cellSize = 20f;
            float cellMargin = 5f;
            Vector2 offset = new Vector2(250, 250);

            for (int x = 0; x < _levelData.gridWidth; x++)
            {
                for (int y = 0; y < _levelData.gridHeight; y++)
                {
                    Rect cellRect = new Rect(
                        offset.x + x * (cellSize + cellMargin),
                        offset.y + y * (cellSize + cellMargin),
                        cellSize,
                        cellSize
                    );

                    bool hasHorizontal = _levelData.initialLines.Any(line => line.gridPosition == new Vector2Int(x, y) && line.isHorizontal);
                    bool hasVertical = _levelData.initialLines.Any(line => line.gridPosition == new Vector2Int(x, y) && !line.isHorizontal);

                    Color cellColor = Color.white;
                    if (hasHorizontal && hasVertical) cellColor = Color.magenta; // Both
                    else if (hasHorizontal) cellColor = Color.blue;  // Horizontal
                    else if (hasVertical) cellColor = Color.red;     // Vertical

                    EditorGUI.DrawRect(cellRect, cellColor);

                    if (Event.current.type == EventType.MouseDown && cellRect.Contains(Event.current.mousePosition))
                    {
                        ToggleLine(x, y);
                        Repaint();
                    }
                }
            }
        }

        private void ToggleLine(int x, int y)
        {
            Vector2Int gridPos = new Vector2Int(x, y);
            var existingLine = _levelData.initialLines.FirstOrDefault(line => line.gridPosition == gridPos && line.isHorizontal == _isHorizontal);

            if (existingLine != null)
            {
                _levelData.initialLines.Remove(existingLine);
            }
            else
            {
                _levelData.initialLines.Add(new LineInfo
                {
                    gridPosition = gridPos,
                    isHorizontal = _isHorizontal
                });
            }
        }
    }
}
#endif
