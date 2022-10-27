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

        public Animator animator;

        public int MovementPoints => movementPoints;

        private void Awake()
        {
            Vector3Int positionRelativeToGrid = hexGrid.groundMap.WorldToCell(transform.position);
            HexTile hexTile = hexGrid.groundMap.GetTile<HexTile>(positionRelativeToGrid);

            if (!hexGrid.groundMap.HasTile(positionRelativeToGrid))
                throw new Exception("the hero is not located on the grid");

            Memory.Units[positionRelativeToGrid] = gameObject;
            transform.position = hexGrid.groundMap.CellToLocal(positionRelativeToGrid);
        }

        public void SelectDirection(Vector3 clickPosition)
        {
            clickPosition.z = transform.position.z;
            Vector3 direction = (clickPosition - transform.position).normalized;

            animator.SetFloat("Speed", 1.0f);
            animator.SetFloat("Look X", direction.x);
            animator.SetFloat("Look Y", direction.y);
        }

        public void MoveThroughPath(Vector3[] currentPath)
        {
            _pathPositions = new(currentPath);
            Vector3 firstTarget = _pathPositions.Dequeue();
        }
    }
}
