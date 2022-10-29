using System;
using System.Collections.Generic;
using ProjectHexGrid.Scripts.Hex;
using ProjectHexGrid.Scripts.Systems;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectHexGrid.Scripts.Managers
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private MainMapManager mainMap;
        [SerializeField] private MovementSystem movementSystem;

        private Unit _selectedUnit;
        private Vector3Int? _previouslySelectedHex;
        
        private Dictionary<Vector3Int, Unit> _units = new();

        public Tilemap Map => mainMap.Map;
        
        public bool PlayersTurn { private set; get; } = true;

        private void Awake()
        {
            foreach (Unit unit in GetComponentsInChildren<Unit>())
            {
                SetupUnit(unit);
            }
        }
        
        private void SetupUnit(Unit unit)
        {
            unit.SetUnitManager(this);
            
            Vector3Int unitCoordinate = mainMap.Map.WorldToCell(unit.transform.position);

            if (mainMap.Map.HasTile(unitCoordinate) == false)
                throw new Exception("the hero is not located on the grid");

            unit.transform.position = mainMap.Map.CellToLocal(unitCoordinate);
            unit.Coordinate = unitCoordinate;
            
            _units[unitCoordinate] = unit;
        }

        public void UpdateUnitCoordinate(Unit unit)
        {
            _units.Remove(unit.Coordinate);
            
            Vector3Int newCoordinate = mainMap.Map.WorldToCell(unit.transform.position);
            unit.Coordinate = newCoordinate;
            
            _units[newCoordinate] = unit;
        }

        public Dictionary<Vector3Int, Unit> GetUnits() => _units;

        private void ClearOldSelection()
        {
            _previouslySelectedHex = null;
            _selectedUnit = null;
            movementSystem.HideRange();
        }
        
        private void ResetTurn(Unit selectedUnit)
        {
            selectedUnit.MovementFinished -= ResetTurn;
            PlayersTurn = true;
        }

        public void HandleUnitSelected(Vector3Int unitCoord)
        {
            if (PlayersTurn == false)
                return;

            Unit unitReference = _units[unitCoord].GetComponent<Unit>();

            // disable highlight
            if (CheckIfTheSameUnitSelected(unitReference))
                return;

            // enable highlight
            PrepareUnitForMovement(unitReference);
        }

        private bool CheckIfTheSameUnitSelected(Unit unitReference)
        {
            if (_selectedUnit == unitReference)
            {
                ClearOldSelection();
                return true;
            }

            return false;
        }
        
        private void PrepareUnitForMovement(Unit unitReference)
        {
            if (_selectedUnit is not null)
                ClearOldSelection();

            _selectedUnit = unitReference;
            movementSystem.ShowRange(_selectedUnit, mainMap);
        }

        public void HandleTerrainSelected(Vector3Int selectedHexCoord)
        {
            if (_selectedUnit is null || PlayersTurn == false)
                return;

            if (HandleHexOutOfRange(selectedHexCoord) || HandleSelectedHexIsUnitHex(selectedHexCoord))
            {
                ClearOldSelection();
                return;
            }

            HandleTargetHexSelected(selectedHexCoord);
        }
        
        private bool HandleHexOutOfRange(Vector3Int hexCoord) => movementSystem.IsHexInRange(hexCoord) == false;
        
        // select other unit
        private bool HandleSelectedHexIsUnitHex(Vector3Int hexCoord) => _units.ContainsKey(hexCoord);

        private void HandleTargetHexSelected(Vector3Int selectedHexCoord)
        {
            if (_previouslySelectedHex is null || _previouslySelectedHex != selectedHexCoord)
            {
                _previouslySelectedHex = selectedHexCoord;
                movementSystem.ShowPath(selectedHexCoord);
            }
            else
            {
                PlayersTurn = false;
                _selectedUnit.MovementFinished += ResetTurn;
                movementSystem.MoveUnit(_selectedUnit, mainMap);
                
                // clear unit, selected, highlight tile
                ClearOldSelection();
            }
        }
    }
}