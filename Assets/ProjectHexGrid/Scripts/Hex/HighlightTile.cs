using UnityEngine;
using UnityEngine.UI;

namespace ProjectHexGrid.Scripts.Hex
{
    public class HighlightTile : MonoBehaviour
    {
        [SerializeField] private Text costText;

        public string CostText
        {
            set => costText.text = value;
        }
    }
}
