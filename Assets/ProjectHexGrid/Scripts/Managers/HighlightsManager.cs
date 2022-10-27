using System.Collections.Generic;
using ProjectHexGrid.Scripts.Hex;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectHexGrid.Scripts.Managers
{
    public class HighlightsManager : MonoBehaviour
    {
        private List<HexTile> _highlightedTiles = new();
        
        public HighlightTile AddHighlightTile(HexGrid hexGrid, GameObject highlightType, Vector3Int position)
        {
            GameObject highlightTileObject = Instantiate(
                highlightType,
                hexGrid.highlightMap.CellToLocal(position),
                Quaternion.identity,
                transform
            );
            
            HexTile hexTile = hexGrid.groundMap.GetTile<HexTile>(position);
            hexTile.highlightTile = highlightTileObject;
            
            _highlightedTiles.Add(hexTile);

            return highlightTileObject.GetComponent<HighlightTile>();;
        }
        
        public void DisableHighlightTiles()
        {
            if (_highlightedTiles.Count == 0)
                return;
            
            foreach (HexTile highlightTile in _highlightedTiles)
            {
                Destroy(highlightTile.highlightTile);
            }
            _highlightedTiles.Clear();
        }
    }
}