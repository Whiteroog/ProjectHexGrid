using UnityEngine;
using UnityEngine.UI;

namespace ProjectHexGrid.Scripts.Highlight
{
    public class HighlightTileWithCost : HighlightTile
    {
        [SerializeField] private Text costText;
        public string CostText
        {
            set
            {
                if(costText) costText.text = value;
            }
            get => costText is not null ? costText.text : "";
        }
    }
}
