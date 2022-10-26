using System.Collections.Generic;
using System.Linq;
using ProjectHexGrid.Scripts.Hex;
using UnityEngine;

namespace ProjectHexGrid.Scripts.Pathfinding
{
    public static class GraphSearch
    {
        public static BfsResult BfsGetRange(HexGrid hexGrid, Vector3Int startPoint, int movementPoints)
        {
            Dictionary<Vector3Int, Vector3Int?> visitedNodes = new();
            Dictionary<Vector3Int, int> costSoFar = new();
            Queue<Vector3Int> nodesToVisitQueue = new();

            nodesToVisitQueue.Enqueue(startPoint);
            costSoFar.Add(startPoint, 0);
            visitedNodes.Add(startPoint, null);

            while(nodesToVisitQueue.Count > 0)
            {
                Vector3Int currentNode = nodesToVisitQueue.Dequeue();
                foreach (Vector3Int neighbourPosition in hexGrid.GetNeighboursFor(currentNode))
                {
                    HexTile neighbourHexTile = hexGrid.groundMap.GetTile<HexTile>(neighbourPosition);

                    if (neighbourHexTile.obstacle)
                        continue;

                    int currentCost = costSoFar[currentNode];
                    int nodeCost = neighbourHexTile.cost;
                    int newCost = currentCost + nodeCost;

                    if (newCost <= movementPoints)
                    {
                        if (!visitedNodes.ContainsKey(neighbourPosition))
                        {
                            visitedNodes[neighbourPosition] = currentNode;
                            costSoFar[neighbourPosition] = newCost;
                            nodesToVisitQueue.Enqueue(neighbourPosition);
                        }
                        else if (newCost < costSoFar[neighbourPosition])
                        {
                            costSoFar[neighbourPosition] = newCost;
                            visitedNodes[neighbourPosition] = currentNode;
                        }
                    }
                }
            }

            return new(visitedNodes, costSoFar);
        }

        public static Vector3Int[] GeneratePathBfs(Vector3Int current, Dictionary<Vector3Int, Vector3Int?> visitedNodes)
        {
            Stack<Vector3Int> path = new();

            path.Push(current);
            while (visitedNodes[current] != null)
            {
                path.Push(visitedNodes[current].Value);
                current = visitedNodes[current].Value;
            }
            path.Pop();
            
            return path.ToArray();
        }
    }
    
    public struct BfsResult
    {
        private Dictionary<Vector3Int, Vector3Int?> _visitedNodes;
        
        private Dictionary<Vector3Int, int> _costSoFar;
        public Dictionary<Vector3Int, int> CostSoFar => _costSoFar;
        public BfsResult(Dictionary<Vector3Int, Vector3Int?> visitedNodes, Dictionary<Vector3Int, int> costSoFar)
        {
            _visitedNodes = visitedNodes;
            _costSoFar = costSoFar;
        }

        public Vector3Int[] GetPathTo(Vector3Int destination)
        {
            if (!_visitedNodes.ContainsKey(destination))
                return new Vector3Int[]{};

            return GraphSearch.GeneratePathBfs(destination, _visitedNodes);
        }

        public bool IsHexPositionInRange(Vector3Int position)
            => _visitedNodes.ContainsKey(position);

        public Vector3Int[] GetRangePosition() => _visitedNodes.Keys.Skip(1).ToArray();
    }
}