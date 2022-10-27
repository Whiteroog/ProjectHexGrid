using System;
using Assets.ProjectHexGrid.Scripts.Memory;
using ProjectHexGrid.ScriptableObjects.Highlight;
using ProjectHexGrid.Scripts.Hex;
using ProjectHexGrid.Scripts.Highlight;
using UnityEngine;

namespace ProjectHexGrid.Scripts.Managers
{
    public class HighlightsManager : MonoBehaviour
    {
        public HighlightTileScriptableObject highlightGroundTile;
        public HighlightTileScriptableObject highlightPath;
        public HighlightTileScriptableObject highlightHeroTile;
        
        private HighlightTileScriptableObject GetHighlightOfType(HighlightType highlightType) =>
            highlightType switch
            {
                HighlightType.GroundSelect => highlightGroundTile,
                HighlightType.PathSelect => highlightPath,
                HighlightType.HeroSelect => highlightHeroTile,
                _ => throw new Exception("the highlight type is not exist")
            };
        
        public HighlightTile AddHighlightTile(HexGrid hexGrid, HighlightType highlightType, Vector3Int position)
        {
            GameObject highlightTileObject = Instantiate(
                GetHighlightOfType(highlightType).highlightObject,
                hexGrid.highlightMap.CellToLocal(position),
                Quaternion.identity,
                transform
            );
            Memory.highlightTiles[position] = highlightTileObject;

            return highlightTileObject.GetComponent<HighlightTile>();;
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

        public void ReplaceHighlightTiles(HexGrid hexGrid, Vector3Int replacedTile, HighlightType highlightType)
        {
            Destroy(Memory.highlightTiles[replacedTile]);
            GameObject newHighlightTileObject = Instantiate(
                GetHighlightOfType(highlightType).highlightObject,
                hexGrid.highlightMap.CellToLocal(replacedTile),
                Quaternion.identity,
                transform
            );
            Memory.highlightTiles[replacedTile] = newHighlightTileObject;
        }
    }
}