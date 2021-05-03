using System;
using System.Collections.Generic;
using System.Linq;
using AirCoder.TJ.Core;
using Models.Grid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models.Invaders
{
    [Serializable]
    public struct InvaderCatalog
    {
        public List<InvaderDataSet> invaders;
        public InvadersLabel[]      indexer;
        public List<Color>          colors;

        public Color GetRandomColor()
            => colors[Random.Range(0, colors.Count)];
        
        /// <returns>random CellData with a random color</returns>
        public CellData GetRandomData()
        {
            var data = invaders[Random.Range(0, invaders.Count)];
            return new CellData()
            {
                meshes = data.meshes,
                value = data.value,
                color = GetRandomColor()
            };
        }

        /// <returns>CellData with a random color by invaders label</returns>
        public CellData GetDataByLabel(InvadersLabel inLabel)
        {
            var data = invaders.FirstOrDefault(d => d.label == inLabel);
            return new CellData()
            {
                meshes = data.meshes,
                value = data.value,
                color = GetRandomColor(),
                health = data.health
            };
        }

        /// <returns>CellData with a random color by invaders' row index</returns>
        public CellData GetDataByInvaderIndex(int index)
        {
            if (index >= indexer.Length) throw new Exception($"Invalid indexer size");
            return GetDataByLabel(indexer[index]);
        }
    }
}