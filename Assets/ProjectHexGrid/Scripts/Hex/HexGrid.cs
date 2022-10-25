using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectHexGrid.Scripts.Hex
{
    public class HexGrid : MonoBehaviour
    {
        public Tilemap groundMap;
        public Tilemap highlightMap;
        
        public List<Vector3Int> GetNeighboursFor(Vector3Int centerHexCoord)
        {
            List<Vector3Int> neighbourCoords = new();

            if (!groundMap.HasTile(centerHexCoord))
                return neighbourCoords;

            foreach (Vector3Int directionCoord in HexDirections.GetDirectionCoords(centerHexCoord.y))
            {
                Vector3Int neighbourCoord = centerHexCoord + directionCoord;
                
                if(!groundMap.HasTile(neighbourCoord))
                    continue;

                neighbourCoords.Add(neighbourCoord);
            }

            return neighbourCoords;
        }
    }

    public static class HexDirections
    {
        private static Vector3Int[] _directionsOffsetEven =
        {
            new (-1, 1, 0), new (0, 1, 0),
            new (-1, 0, 0), new (1, 0, 0),
            new (-1, -1, 0), new (0, -1, 0)
        };
        
        private static Vector3Int[] _directionsOffsetOdd =
        {
            new (0, 1, 0), new (1, 1, 0),
            new (-1, 0, 0), new (1, 0, 0),
            new (0, -1, 0), new (1, -1, 0)
        };

        public static Vector3Int[] GetDirectionCoords(int y)
            => y % 2 == 0 ? _directionsOffsetEven : _directionsOffsetOdd;
    }
}
