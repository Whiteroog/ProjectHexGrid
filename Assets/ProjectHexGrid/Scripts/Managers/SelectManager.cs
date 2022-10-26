using System.Collections.Generic;
using ProjectHexGrid.Scripts.Hex;
using ProjectHexGrid.Scripts.Pathfinding;
using UnityEngine;

namespace ProjectHexGrid.Scripts.Managers
{
    public class SelectManager : MonoBehaviour
    {
        public HexGrid hexGrid;

        public GameObject highlightGroundTile;
        private List<HighlightTile> _highlightedTiles = new();

        public void SelectTile(Vector3 clickPosition)
        {
            clickPosition.z = hexGrid.groundMap.transform.position.z;
            
            Vector3Int hexGridCoord = hexGrid.groundMap.WorldToCell(clickPosition);
            HexTile hexTileSelected = hexGrid.groundMap.GetTile<HexTile>(hexGridCoord);

            if (!hexTileSelected)
                return;

            DisableHighlightTiles();

            BfsResult bfsResult = GraphSearch.BfsGetRange(hexGrid, hexGridCoord, 5);
            Vector3Int[] possiblePathways = bfsResult.GetRangePosition();

            foreach (Vector3Int pathway in possiblePathways)
            {
                GameObject highlightTileObject = Instantiate(
                    highlightGroundTile,
                    hexGrid.highlightMap.CellToLocal(pathway),
                    Quaternion.identity,
                    hexGrid.highlightMap.transform
                );

                HighlightTile highlightTile = highlightTileObject.GetComponent<HighlightTile>();
                highlightTile.CostText = bfsResult.CostSoFar[pathway].ToString();
                
                _highlightedTiles.Add(highlightTile);
            }
        }

        private void DisableHighlightTiles()
        {
            if (_highlightedTiles.Count == 0)
                return;
            
            foreach (HighlightTile highlightTile in _highlightedTiles)
            {
                Destroy(highlightTile.gameObject);
            }
            _highlightedTiles.Clear();
        }
    }
}