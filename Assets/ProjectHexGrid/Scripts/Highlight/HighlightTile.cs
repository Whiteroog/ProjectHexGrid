using ProjectHexGrid.ScriptableObjects.Highlight;
using UnityEngine;

namespace ProjectHexGrid.Scripts.Highlight
{
    public class HighlightTile : MonoBehaviour
    {
        [SerializeField] protected HighlightTileScriptableObject parameters;
        public HighlightTileScriptableObject Parameters => parameters;
    }
}