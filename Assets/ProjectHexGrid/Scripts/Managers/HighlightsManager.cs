using System.Collections.Generic;
using Assets.ProjectHexGrid.Scripts.Memory;
using ProjectHexGrid.Scripts.Hex;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectHexGrid.Scripts.Managers
{
    public class HighlightsManager : MonoBehaviour
    {       
        public BaseHighlightTile AddHighlightTile(HexGrid hexGrid, GameObject highlightType, Vector3Int position)
        {
            GameObject highlightTileObject = Instantiate(
                highlightType,
                hexGrid.highlightMap.CellToLocal(position),
                Quaternion.identity,
                transform
            );
            Memory.highlightTiles[position] = highlightTileObject;

            return highlightTileObject.GetComponent<BaseHighlightTile>();;
        }
        
        public void DisableHighlightTiles()
        {
            if (Memory.highlightTiles.Count == 0)
                return;
            
            foreach (GameObject highlightTile in Memory.highlightTiles.Values)
            {
                Destroy(highlightTile);
            }
            Memory.highlightTiles.Clear();
        }
    }
}