using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ProjectHexGrid.Scripts.Memory
{
    static class Memory
    {
        public static Dictionary<Vector3Int, GameObject> highlightTiles = new();
        public static Dictionary<Vector3Int, GameObject> Units = new();
    }
}
