using UnityEngine;

namespace ProjectHexGrid.ScriptableObjects.Highlight
{
    [CreateAssetMenu(fileName = "HighlightTileSo", menuName = "Hex/Highlight", order = 1)]
    public class HighlightTileScriptableObject : ScriptableObject
    {
        [SerializeField] private GameObject highlightObject;
        [SerializeField] private TypeHighlight typeHighlight = TypeHighlight.MainSelect;
        public GameObject HighlightObject => highlightObject;
        public TypeHighlight HighlightType => typeHighlight;
    }
    
    public enum TypeHighlight
    {
        MainSelect,
        PathSelect,
        HeroSelect,
    }
}