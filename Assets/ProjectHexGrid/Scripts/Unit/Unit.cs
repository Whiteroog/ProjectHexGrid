using System;
using System.Collections.Generic;
using ProjectHexGrid.Scripts.Hex;
using UnityEngine;

namespace ProjectHexGrid.Scripts.Unit
{
    [SelectionBase]
    public class Unit : MonoBehaviour
    {
        public HexGrid hexGrid;
        
        [SerializeField] private int movementPoints = 5;
        public int MovementPoints => movementPoints;

        [SerializeField] private float movementDuration = 1;

        private Queue<Vector3> _pathPositions = new();

        public event Action<Unit> movementFinished;

        private void Awake()
        {
            Vector3Int positionRelativeToGrid = hexGrid.groundMap.WorldToCell(transform.position);
            HexTile hexTile = hexGrid.groundMap.GetTile<HexTile>(positionRelativeToGrid);
            
            if(hexTile is null)
                throw new Exception("the hero is not located on the grid");

            hexTile.placedObject = gameObject;
            hexGrid.groundMap.CellToLocal(positionRelativeToGrid);
        }
    }
}
