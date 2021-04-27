using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models.Invaders
{
    [Serializable]
    public struct InvaderCatalog
    {
        public List<InvaderDataSet> invaders;
        public List<Color> colors;
        public CellData GetRandom()
        {
            var data = invaders[Random.Range(0, invaders.Count)];
            return new CellData()
            {
                mesh = data.mesh,
                value = data.value,
                color = colors[Random.Range(0, colors.Count)]
            };
        }
    }
}