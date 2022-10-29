using System;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectHexGrid.Scripts.Managers
{
    public class SelectManager : MonoBehaviour
    {
        [SerializeField] private UnitManager unitManager;

        public UnityEvent<Vector3Int> onUnitSelected;
        public UnityEvent<Vector3Int> groundSelected;

        public void SelectTile(Vector3 clickPosition)
        {
            clickPosition.z = unitManager.transform.position.z;
            Vector3Int clickPositionOnGrid = unitManager.Map.WorldToCell(clickPosition);

            if (UnitSelected(clickPositionOnGrid))
            {
                onUnitSelected?.Invoke(clickPositionOnGrid);
            }
            else
            {
                groundSelected?.Invoke(clickPositionOnGrid);
            }
        }

        private bool UnitSelected(Vector3Int positionOnGrid) => unitManager.GetUnits().ContainsKey(positionOnGrid);
    }
}