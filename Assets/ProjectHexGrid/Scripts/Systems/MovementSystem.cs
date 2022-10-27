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
        public HighlightsManager highlightsManager;

        private BfsResult _movementRange = new BfsResult();
        private Vector3Int[] _currentPath;

        public void HideRange()
        {
            highlightsManager.DisableHighlightTiles();
            _movementRange = new BfsResult();
        }

        public void ShowRange(Unit selectedUnit, HexGrid hexGrid)
        {
            CalculateRange(selectedUnit, hexGrid);
            
            highlightsManager.AddHighlightTile(hexGrid, HighlightType.HeroSelect, _movementRange.GetRangePosition()[0]);
            foreach (Vector3Int path in _movementRange.GetRangePosition().Skip(1))
            {
                HighlightTile highlightTile = highlightsManager.AddHighlightTile(hexGrid, HighlightType.GroundSelect, path);

                if(highlightTile.highlightTileSo.highlightType is HighlightType.GroundSelect)
                {
                    highlightTile.CostText = _movementRange.CostSoFar[path].ToString();
                }
            }
        }

        private void CalculateRange(Unit selectedUnit, HexGrid hexGrid)
        {
            _movementRange = GraphSearch.BfsGetRange(hexGrid, selectedUnit.Coordinate, selectedUnit.MovementPoints);
        }

        public void ShowPath(Vector3Int selectedHexPosition, HexGrid hexGrid)
        {
            if (_movementRange.GetRangePosition().Contains(selectedHexPosition))
            {
                _currentPath = _movementRange.GetPathTo(selectedHexPosition);
                foreach (Vector3Int hexPosition in _currentPath)
                {
                    highlightsManager.ReplaceHighlightTiles(hexGrid, hexPosition, HighlightType.PathSelect);
                }
            }
        }

        public void MoveUnit(Unit selectedUnit, HexGrid hexGrid)
        {
            selectedUnit.MoveThroughPath(_currentPath.Select(pos => hexGrid.groundMap.CellToLocal(pos)).ToArray());
        }

        public bool IsHexInRange(Vector3Int hexPosition)
            => _movementRange.IsHexPositionInRange(hexPosition);
    }
}