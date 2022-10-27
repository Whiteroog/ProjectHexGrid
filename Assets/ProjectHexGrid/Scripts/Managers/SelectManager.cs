using System;
using Assets.ProjectHexGrid.Scripts.Memory;
using ProjectHexGrid.Scripts.Hex;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectHexGrid.Scripts.Managers
{
    public class SelectManager : MonoBehaviour
    {
        public HexGrid hexGrid;

        public UnityEvent<Vector3Int> onUnitSelected;
        public UnityEvent<Vector3Int> groundSelected;

        public void SelectTile(Vector3 clickPosition)
        {
            clickPosition.z = hexGrid.groundMap.transform.position.z;
            Vector3Int positionOnGrid = hexGrid.groundMap.WorldToCell(clickPosition);

            if (UnitSelected(positionOnGrid))
            {
                onUnitSelected?.Invoke(positionOnGrid);
            }
            else
            {
                groundSelected?.Invoke(positionOnGrid);
            }
        }

        private bool UnitSelected(Vector3Int positionOnGrid) => Memory.Units.ContainsKey(positionOnGrid);
    }
}