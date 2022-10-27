#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectHexGrid.Scripts.Hex
{
    public class HexTile : Tile
    {
        [Header("<-- Hex parameters -->")]
        public int cost = 1;
        public bool obstacle = false;

        public GameObject highlightTile;
        public GameObject placedObject;

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Hex/HexTile")]
        public static void CreateHexTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Hex Tile", "New Hex Tile", "Asset", "Save Hex Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<HexTile>(), path);
        }
#endif
    }
}
