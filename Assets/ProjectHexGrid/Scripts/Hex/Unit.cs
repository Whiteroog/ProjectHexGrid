using System;
using System.Collections;
using System.Collections.Generic;
using ProjectHexGrid.Scripts.Managers;
using UnityEngine;

namespace ProjectHexGrid.Scripts.Hex
{
    [SelectionBase]
    public class Unit : MonoBehaviour
    {
        public  UnitManager unitManager;
        [SerializeField] private int movementPoints = 5;
        [SerializeField] private float movementDuration = 1;

        private Queue<Vector3> _pathPositions = new();

        public event Action<Unit> MovementFinished;

        public Animator animator;

        public Vector3Int coordinate;
        
        public int MovementPoints => movementPoints;
        

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
                MovementAnimation(Vector3.zero, 0.0f);
                unitManager.UpdateUnitCoordinate(this);
                MovementFinished?.Invoke(this);
            }
        }
    }
}
