using UnityEngine;
using UnityEngine.UI;

namespace ProjectHexGrid.Scripts.Hex
{
    public class HighlightTile : MonoBehaviour
    {
        public Text costText;

        public void SetCostText(string cost) => costText.text = cost;

    }
}
