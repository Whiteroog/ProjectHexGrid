using System;
using System.Collections.Generic;
using ProjectHexGrid.ScriptableObjects.Highlight;
using ProjectHexGrid.Scripts.Highlight;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace ProjectHexGrid.Scripts.Managers
{
    public class HighlightManager : MonoBehaviour
    {
        [SerializeField] private MainMapManager mainMap;

        [SerializeField] private HighlightTileScriptableObject highlightMainTile;
        [SerializeField] private HighlightTileScriptableObject highlightPath;
        [SerializeField] private HighlightTileScriptableObject highlightHeroTile;
        
        private Dictionary<TypeHighlight, Dictionary<Vector3Int, HighlightTile>> _highlightTiles = new();
        private Dictionary<TypeHighlight, Dictionary<Vector3Int, HighlightTile>> _disabledHighlightTiles = new();

        private void Start()
        {
            _highlightTiles[TypeHighlight.MainSelect] = new();
            _disabledHighlightTiles[TypeHighlight.MainSelect] = new();
            
            _highlightTiles[TypeHighlight.HeroSelect] = new();
            _disabledHighlightTiles[TypeHighlight.HeroSelect] = new();
            
            _highlightTiles[TypeHighlight.PathSelect] = new();
            _disabledHighlightTiles[TypeHighlight.PathSelect] = new();
        }

        private HighlightTileScriptableObject GetHighlightOfType(TypeHighlight typeHighlight) =>
            typeHighlight switch
            {
                TypeHighlight.MainSelect => highlightMainTile,
                TypeHighlight.PathSelect => highlightPath,
                TypeHighlight.HeroSelect => highlightHeroTile,
                _ => throw new Exception("the highlight type is not exist")
            };
        
        public HighlightTile AddHighlightTile(TypeHighlight highlightType, Vector3Int position)
        {
            HighlightTile highlightTile = Instantiate(
                GetHighlightOfType(highlightType).HighlightObject,
                mainMap.Map.CellToLocal(position),
                Quaternion.identity,
                transform
            ).GetComponent<HighlightTile>();

            _highlightTiles[highlightType][position] = highlightTile;

            return highlightTile;
        }

        public void ReplaceHighlightTiles(TypeHighlight oldHighlightType, TypeHighlight newHighlightType, Vector3Int position)
        {
            if (_highlightTiles[newHighlightType].ContainsKey(position) == false)
            {
                DisableHighlightTiles(oldHighlightType, position);
                AddHighlightTile(newHighlightType, position);

                if (_highlightTiles[oldHighlightType][position] is HighlightTileWithCost oldHighlightTileWithCost &&
                    _highlightTiles[newHighlightType][position] is HighlightTileWithCost newHighlightTileWithCost)
                {
                    newHighlightTileWithCost.CostText = oldHighlightTileWithCost.CostText;
                }
            }
            else
            {
                DisableHighlightTiles(oldHighlightType, position);
                EnableHighlightTiles(newHighlightType, position);
            }
        }
        
        public HighlightTile EnableHighlightTiles(TypeHighlight highlightType, Vector3Int position)
        {
            if (_highlightTiles[highlightType].ContainsKey(position) == false)
            {
                return AddHighlightTile(highlightType, position);;
            }
            
            if (_disabledHighlightTiles[highlightType].ContainsKey(position))
            {
                _disabledHighlightTiles[highlightType][position].gameObject.SetActive(true);
                _disabledHighlightTiles[highlightType].Remove(position);
                return _highlightTiles[highlightType][position];
            }

            return null;
        }

        public void EnableHighlightTiles()
        {
            foreach (TypeHighlight highlightType in _disabledHighlightTiles.Keys)
            {
                foreach (var disabledHighlightTile in _disabledHighlightTiles[highlightType])
                {
                    EnableHighlightTiles(highlightType, disabledHighlightTile.Key);
                }
            }
        }

        public void DisableHighlightTiles(TypeHighlight highlightType, Vector3Int position)
        {
            if (_disabledHighlightTiles[highlightType].ContainsKey(position))
                return;

            _highlightTiles[highlightType][position].gameObject.SetActive(false);
            _disabledHighlightTiles[highlightType][position] = _highlightTiles[highlightType][position];
        }
        
        public void DisableHighlightTiles()
        {
            foreach (TypeHighlight highlightType in _highlightTiles.Keys)
            {
                foreach (var highlightTile in _highlightTiles[highlightType])
                {
                    DisableHighlightTiles(highlightType, highlightTile.Key);
                }
            }
        }
        
        public void ClearHighlightTiles()
        {
            foreach (Dictionary<Vector3Int, HighlightTile> allHighlightTiles in _highlightTiles.Values)
            {
                foreach (HighlightTile highlightTile in allHighlightTiles.Values)
                {
                    Destroy(highlightTile.gameObject);
                }
            }

            ClearDictionaryHighlightTiles();
        }

        private void ClearDictionaryHighlightTiles()
        {
            _highlightTiles[TypeHighlight.MainSelect].Clear();
            _disabledHighlightTiles[TypeHighlight.MainSelect].Clear();
            
            _highlightTiles[TypeHighlight.HeroSelect].Clear();
            _disabledHighlightTiles[TypeHighlight.HeroSelect].Clear();
            
            _highlightTiles[TypeHighlight.PathSelect].Clear();
            _disabledHighlightTiles[TypeHighlight.PathSelect].Clear();
        }
    }
}