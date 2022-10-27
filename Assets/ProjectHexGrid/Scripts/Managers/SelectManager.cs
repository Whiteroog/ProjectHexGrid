using System.Collections.Generic;
using Assets.ProjectHexGrid.Scripts.Hex;
using Assets.ProjectHexGrid.Scripts.Memory;
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

            if (!hexGrid.groundMap.HasTile(hexGridCoord))
                return;

            highlightsManager.DisableHighlightTiles();

            if (!Memory.Units.ContainsKey(hexGridCoord))
                return;
            
            highlightsManager.AddHighlightTile(hexGrid, highlightHeroTile, hexGridCoord);

            BfsResult bfsResult = GraphSearch.BfsGetRange(hexGrid, hexGridCoord, 5);
            Vector3Int[] possiblePathways = bfsResult.GetRangePosition();

            foreach (Vector3Int pathway in possiblePathways)
            {
                BaseHighlightTile highlightTile = highlightsManager.AddHighlightTile(hexGrid, highlightGroundTile, pathway);

                if(highlightTile is HighlightTileWithCost highlightTileWithCost)
                {
                    highlightTileWithCost.CostText = bfsResult.CostSoFar[pathway].ToString();
                }
            }
        }
    }
}