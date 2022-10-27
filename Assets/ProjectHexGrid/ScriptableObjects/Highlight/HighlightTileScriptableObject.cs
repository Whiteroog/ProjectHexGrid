using UnityEngine;

namespace ProjectHexGrid.ScriptableObjects.Highlight
{
    [CreateAssetMenu(fileName = "HighlightTileSo", menuName = "Hex/Highlight", order = 1)]
    public class HighlightTileScriptableObject : ScriptableObject
    {
        public GameObject highlightObject;
        public HighlightType highlightType = HighlightType.GroundSelect;
    }
    
    public enum HighlightType
    {
        GroundSelect,
        PathSelect,
        HeroSelect,
    }
}