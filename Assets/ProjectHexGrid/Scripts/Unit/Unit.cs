using System;
using System.Collections.Generic;
using Assets.ProjectHexGrid.Scripts.Memory;
using ProjectHexGrid.Scripts.Hex;
using UnityEngine;

namespace ProjectHexGrid.Scripts.Unit
{
    [SelectionBase]
    public class Unit : MonoBehaviour
    {
        public HexGrid hexGrid;

        [SerializeField] private int movementPoints = 5;

        [SerializeField] private float movementDuration = 1;

        private Queue<Vector3> _pathPositions = new();

        public event Action<Unit> movementFinished;

        public int MovementPoints => movementPoints;

        private void Awake()
        {
            Vector3Int positionRelativeToGrid = hexGrid.groundMap.WorldToCell(transform.position);
            HexTile hexTile = hexGrid.groundMap.GetTile<HexTile>(positionRelativeToGrid);
            
            if(!hexGrid.groundMap.HasTile(positionRelativeToGrid))
                throw new Exception("the hero is not located on the grid");

            Memory.Units[positionRelativeToGrid] = gameObject;
            transform.position = hexGrid.groundMap.CellToLocal(positionRelativeToGrid);
        }
    }
}
