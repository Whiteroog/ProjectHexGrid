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
        public GameObject highlightHeroTile;

        public HighlightsManager highlightsManager;

        public void SelectTile(Vector3 clickPosition)
        {
            clickPosition.z = hexGrid.groundMap.transform.position.z;

            Vector3Int hexGridCoord = hexGrid.groundMap.WorldToCell(clickPosition);
            HexTile hexTileSelected = hexGrid.groundMap.GetTile<HexTile>(hexGridCoord);

            if (!hexTileSelected)
                return;

            highlightsManager.DisableHighlightTiles();

            if (!hexTileSelected.placedObject)
                return;
            
            highlightsManager.AddHighlightTile(hexGrid, highlightHeroTile, hexGridCoord);

            BfsResult bfsResult = GraphSearch.BfsGetRange(hexGrid, hexGridCoord, 5);
            Vector3Int[] possiblePathways = bfsResult.GetRangePosition();

            foreach (Vector3Int pathway in possiblePathways)
            {
                HighlightTile highlightTile = highlightsManager.AddHighlightTile(hexGrid, highlightGroundTile, pathway);
                
                highlightTile.CostText = bfsResult.CostSoFar[pathway].ToString();
            }
        }
    }
}