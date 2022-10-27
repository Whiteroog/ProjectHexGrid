using ProjectHexGrid.Scripts.Hex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.ProjectHexGrid.Scripts.Hex
{
    class HighlightTileWithCost : BaseHighlightTile
    {
        [SerializeField] private Text costText;

        public string CostText
        {
            set => costText.text = value;
        }
    }
}
