using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectHexGrid.Scripts.Hex
{
    public class HexGrid : MonoBehaviour
    {
        public Tilemap groundMap;

        public Tilemap highlightMap;
        public Vector3Int[] GetNeighboursFor(Vector3Int centerHexCoord)
        {
            Vector3Int[] neighbourCoords = new Vector3Int[6];
            
            if (!groundMap.HasTile(centerHexCoord))
                return neighbourCoords;

            Vector3Int[] directionCoords = HexDirections.GetDirectionCoords(centerHexCoord.y);
            
            for (int i = 0; i < 6; i++)
            {
                Vector3Int neighbourCoord = centerHexCoord + directionCoords[i];
                
                if(!groundMap.HasTile(neighbourCoord))
                    continue;

                neighbourCoords[i] = neighbourCoord;
            }

            return neighbourCoords;
        }
    }

    public static class HexDirections
    {
        private static Vector3Int[] _directionsOffsetOdd =
        {
            new (0, 1, 0), new (1, 1, 0),
            new (-1, 0, 0), new (1, 0, 0),
            new (0, -1, 0), new (1, -1, 0)
        };
        
        private static Vector3Int[] _directionsOffsetEven =
        {
            new (-1, 1, 0), new (0, 1, 0),
            new (-1, 0, 0), new (1, 0, 0),
            new (-1, -1, 0), new (0, -1, 0)
        };

        public static Vector3Int[] GetDirectionCoords(int y)
            => y % 2 == 0 ? _directionsOffsetEven : _directionsOffsetOdd;
    }
}
