using System.Collections.Generic;
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

            return new BfsResult() { visitedNodesDict = visitedNodes };
        }

        public static Vector3Int[] GeneratePathBfs(Vector3Int current, Dictionary<Vector3Int, Vector3Int?> visitedNodesDict)
        {
            Stack<Vector3Int> path = new();

            path.Push(current);
            while (visitedNodesDict[current] != null)
            {
                path.Push(visitedNodesDict[current].Value);
                current = visitedNodesDict[current].Value;
            }
            path.Pop();
            
            return path.ToArray();
        }
    }
    
    public struct BfsResult
    {
        public Dictionary<Vector3Int, Vector3Int?> visitedNodesDict;

        public Vector3Int[] GetPathTo(Vector3Int destination)
        {
            if (!visitedNodesDict.ContainsKey(destination))
                return new Vector3Int[]{};

            return GraphSearch.GeneratePathBfs(destination, visitedNodesDict);
        }

        public bool IsHexPositionInRange(Vector3Int position)
            => visitedNodesDict.ContainsKey(position);

        public Vector3Int[] GetRangePosition()
        {
            Vector3Int[] rangePosition = new Vector3Int[visitedNodesDict.Keys.Count];
            visitedNodesDict.Keys.CopyTo(rangePosition, 0);
            return rangePosition;
        }
    }
}