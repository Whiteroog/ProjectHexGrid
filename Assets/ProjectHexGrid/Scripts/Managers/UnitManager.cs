using System;
using Assets.ProjectHexGrid.Scripts.Memory;
using ProjectHexGrid.Scripts.Hex;
using ProjectHexGrid.Scripts.Systems;
using UnityEngine;

namespace ProjectHexGrid.Scripts.Managers
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private HexGrid hexGrid;

        [SerializeField] private MovementSystem movementSystem;

        private Unit _selectedUnit;
        private Vector3Int? _previouslySelectedHex;

        public bool PlayersTurn { private set; get; } = true;

        private void Awake()
        {
            foreach (Unit unit in GetComponentsInChildren<Unit>())
            {
                unit.unitManager = this;
                SetupUnitPosition(unit);
            }
        }

        public void HandleUnitSelected(Vector3Int unitCoord)
        {
            if (!PlayersTurn)
                return;

            Unit unitReference = Memory.Units[unitCoord].GetComponent<Unit>();

            if (CheckIfTheSameUnitSelected(unitReference))
                return;

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

        public void HandleTerrainSelected(Vector3Int selectedHexCoord)
        {
            if (_selectedUnit is null || !PlayersTurn)
                return;

            if (HandleHexOutOfRange(selectedHexCoord) || HandleSelectedHexIsUnitHex(selectedHexCoord))
            {
                ClearOldSelection();
                return;
            }

            HandleTargetHexSelected(selectedHexCoord);
        }

        private void PrepareUnitForMovement(Unit unitReference)
        {
            if (_selectedUnit is not null)
                ClearOldSelection();

            _selectedUnit = unitReference;
            movementSystem.ShowRange(_selectedUnit, hexGrid);
        }

        private void ClearOldSelection()
        {
            _previouslySelectedHex = null;
            _selectedUnit = null;
            movementSystem.HideRange();
        }

        private void HandleTargetHexSelected(Vector3Int selectedHexCoord)
        {
            if (_previouslySelectedHex is null || _previouslySelectedHex != selectedHexCoord)
            {
                _previouslySelectedHex = selectedHexCoord;
                movementSystem.ShowPath(selectedHexCoord, hexGrid);
            }
            else
            {
                movementSystem.MoveUnit(_selectedUnit, hexGrid);
                PlayersTurn = false;
                _selectedUnit.MovementFinished += ResetTurn;
                ClearOldSelection();
            }
        }

        private bool HandleSelectedHexIsUnitHex(Vector3Int hexCoord) => hexCoord == _selectedUnit.coordinate;
        private bool HandleHexOutOfRange(Vector3Int hexCoord) => !movementSystem.IsHexInRange(hexCoord);

        private void ResetTurn(Unit selectedUnit)
        {
            selectedUnit.MovementFinished -= ResetTurn;
            PlayersTurn = true;
        }

        private void SetupUnitPosition(Unit unit)
        {
            Vector3Int gridPosition = hexGrid.groundMap.WorldToCell(unit.transform.position);

            if (!hexGrid.groundMap.HasTile(gridPosition))
                throw new Exception("the hero is not located on the grid");

            unit.transform.position = hexGrid.groundMap.CellToLocal(gridPosition);
            unit.coordinate = gridPosition;

            Memory.Units[gridPosition] = unit.gameObject;
        }

        public void UpdateUnitCoordinate(Unit unit)
        {
            Memory.Units.Remove(unit.coordinate);
            Vector3Int newCoordinate = hexGrid.groundMap.WorldToCell(unit.transform.position);
            unit.coordinate = newCoordinate;
            Memory.Units[newCoordinate] = unit.gameObject;
        }
    }
}