using System;
using System.Collections;
using System.Collections.Generic;
using Assets.ProjectHexGrid.Scripts.Memory;
using ProjectHexGrid.Scripts.Hex;
using UnityEngine;

namespace ProjectHexGrid.Scripts.Hex
{
    [SelectionBase]
    public class Unit : MonoBehaviour
    {
        public HexGrid hexGrid;

        [SerializeField] private int movementPoints = 5;
        [SerializeField] private float movementDuration = 1;

        private Queue<Vector3> _pathPositions = new();

        private Vector3Int _coordinate;
        
        public event Action<Unit> MovementFinished;

        public Animator animator;

        public int MovementPoints => movementPoints;
        public Vector3Int Coordinate => _coordinate;

        private void Awake()
        {
            _coordinate = hexGrid.groundMap.WorldToCell(transform.position);

            if (!hexGrid.groundMap.HasTile(_coordinate))
                throw new Exception("the hero is not located on the grid");
            
            transform.position = hexGrid.groundMap.CellToLocal(_coordinate);
            Memory.Units[_coordinate] = gameObject;
        }

        private void MovementAnimation(Vector3 movementDirection, float speedMoving)
        {
            animator.SetFloat("Speed", speedMoving);
            animator.SetFloat("Look X", movementDirection.x);
            animator.SetFloat("Look Y", movementDirection.y);
        }

        public void MoveThroughPath(Vector3[] currentPath)
        {
            _pathPositions = new(currentPath);
            Vector3 nextPosition = _pathPositions.Dequeue();
            StartCoroutine(SmoothedMove(nextPosition));
        }

        private IEnumerator SmoothedMove(Vector3 endPosition)
        {
            Vector3 startPosition = transform.position;

            MovementAnimation(Vector3.Normalize(endPosition - startPosition), 1.0f);

            for (float timeElapsed = 0; timeElapsed < movementDuration; timeElapsed += Time.deltaTime)
            {
                float lerpStep = timeElapsed / movementDuration;
                transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
                yield return null;
            }

            transform.position = endPosition;

            if (_pathPositions.Count > 0)
            {
                Vector3 nextPosition = _pathPositions.Dequeue();
                StartCoroutine(SmoothedMove(nextPosition));
            }
            else
            {
                Memory.Units.Remove(_coordinate);
                _coordinate = hexGrid.groundMap.WorldToCell(transform.position);
                Memory.Units[_coordinate] = gameObject;
                
                MovementAnimation(Vector3.zero, 0.0f);
                MovementFinished?.Invoke(this);
            }
        }
    }
}
