using System.Linq;
using ProjectHexGrid.ScriptableObjects.Highlight;
using ProjectHexGrid.Scripts.Hex;
using ProjectHexGrid.Scripts.Highlight;
using ProjectHexGrid.Scripts.Managers;
using ProjectHexGrid.Scripts.Pathfinding;
using UnityEngine;

namespace ProjectHexGrid.Scripts.Systems
{
    public class MovementSystem : MonoBehaviour
    {
        public HighlightManager highlightManager;

        private BfsResult _movementRange;
        private Vector3Int[] _currentPath = {};

        public bool IsHexInRange(Vector3Int hexCoord) => _movementRange.IsHexPositionInRange(hexCoord);
        
        private void CalculateRange(Unit selectedUnit, MainMapManager mainMap)
        {
            _movementRange = GraphSearch.BfsGetRange(mainMap, selectedUnit.Coordinate, selectedUnit.MovementPoints);
        }

        public void ShowRange(Unit selectedUnit, MainMapManager mainMap)
        {
            CalculateRange(selectedUnit, mainMap);

            highlightManager.EnableHighlightTiles(TypeHighlight.HeroSelect, _movementRange.GetRangePosition()[0]);

            foreach (Vector3Int path in _movementRange.GetRangePosition().Skip(1))
            {
                HighlightTile highlightTile = highlightManager.EnableHighlightTiles(TypeHighlight.MainSelect, path);

                if (highlightTile is HighlightTileWithCost highlightTileWithCost)
                {
                    highlightTileWithCost.CostText = _movementRange.CostSoFar[path].ToString();
                }
            }
        }

        public void HideRange()
        {
            highlightManager.DisableHighlightTiles();
            _movementRange = new BfsResult();
        }

        public void ShowPath(Vector3Int selectedHexCoord)
        {
            if (IsHexInRange(selectedHexCoord))
            {
                foreach (Vector3Int hexPosition in _currentPath)
                {
                    highlightManager.ReplaceHighlightTiles(TypeHighlight.PathSelect, TypeHighlight.MainSelect, hexPosition);
                }
                
                _currentPath = _movementRange.GetPathTo(selectedHexCoord);
                
                foreach (Vector3Int hexPosition in _currentPath)
                {
                    highlightManager.ReplaceHighlightTiles(TypeHighlight.MainSelect, TypeHighlight.PathSelect, hexPosition);
                }
            }
        }

        public void MoveUnit(Unit selectedUnit, MainMapManager mainMap)
        {
            Vector3[] pathCoordConvertedToPathPositions = _currentPath.Select(pos => mainMap.Map.CellToLocal(pos)).ToArray();
            selectedUnit.MoveThroughPath(pathCoordConvertedToPathPositions);
            
            highlightManager.ClearHighlightTiles();
            _currentPath = new Vector3Int[]{};
        }
    }
}