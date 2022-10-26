using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectHexGrid.Scripts.Unit
{
    [SelectionBase]
    public class Unit : MonoBehaviour
    {
        [SerializeField] private int movementPoints = 5;
        public int MovementPoints => movementPoints;

        [SerializeField] private float movementDuration = 1;

        private Queue<Vector3> _pathPositions = new();

        public event Action<Unit> movementFinished;
    }
}
