using System.Collections.Generic;
using ProjectHexGrid.Scripts.Hex;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectHexGrid.Scripts.Managers
{
    public class SelectManager : MonoBehaviour
    {
        public HexGrid hexGrid;

        public GameObject highlightPrefab;
        private List<GameObject> _highlightedTiles = new();

        public void SelectTile(Vector3 clickPosition)
        {
            clickPosition.z = hexGrid.groundMap.transform.position.z;
            
            Vector3Int hexGridCoord = hexGrid.groundMap.WorldToCell(clickPosition);
            HexTile hexTileSelected = hexGrid.groundMap.GetTile<HexTile>(hexGridCoord);

            if (!hexTileSelected)
                return;

            DisableHighlightTiles();

            foreach (Vector3Int neighbourCoord in hexGrid.GetNeighboursFor(hexGridCoord))
            {
                GameObject highlightTile = Instantiate(
                    highlightPrefab,
                    hexGrid.highlightMap.CellToLocal(neighbourCoord),
                    Quaternion.identity,
                    hexGrid.highlightMap.transform
                );
                
                _highlightedTiles.Add(highlightTile);
            }
        }

        private void DisableHighlightTiles()
        {
            foreach (GameObject highlightTile in _highlightedTiles)
            {
                highlightTile.SetActive(false);
            }
            _highlightedTiles.Clear();
        }
    }
}