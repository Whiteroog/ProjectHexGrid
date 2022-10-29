using System;
using System.Collections.Generic;
using ProjectHexGrid.Scripts.Hex;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectHexGrid.Scripts.Managers
{
    public class MainMapManager : MonoBehaviour
    {
        private Tilemap _mainMap;
        
        public Tilemap Map => _mainMap;

        private void Awake()
        {
            _mainMap = GetComponent<Tilemap>();
        }

        public List<Vector3Int> GetNeighboursFor(Vector3Int centerHexCoord)
        {
            List<Vector3Int> neighbourCoords = new();

            if (_mainMap.HasTile(centerHexCoord) == false)
                return neighbourCoords;

            foreach (Vector3Int directionCoord in Direction.GetDirectionCoords(centerHexCoord.y))
            {
                Vector3Int neighbourCoord = centerHexCoord + directionCoord;
                
                if(_mainMap.HasTile(neighbourCoord) == false)
                    continue;

                neighbourCoords.Add(neighbourCoord);
            }

            return neighbourCoords;
        }
    }
    
    public static class Direction
    {
        private static readonly Vector3Int[] DirectionsOffsetEven =
        {
            new (-1, 1, 0), new (0, 1, 0),
            new (-1, 0, 0), new (1, 0, 0),
            new (-1, -1, 0), new (0, -1, 0)
        };
        
        private static readonly Vector3Int[] DirectionsOffsetOdd =
        {
            new (0, 1, 0), new (1, 1, 0),
            new (-1, 0, 0), new (1, 0, 0),
            new (0, -1, 0), new (1, -1, 0)
        };

        public static Vector3Int[] GetDirectionCoords(int y)
            => y % 2 == 0 ? DirectionsOffsetEven : DirectionsOffsetOdd;
    }
}