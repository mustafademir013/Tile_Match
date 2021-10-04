using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TileMatch.Board
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Board/Transform Data", order = 2)]
    public class TransformData : ScriptableObject
    {
        private Transform[,] _spawnTransforms;
        private Transform[,] _boardTransforms;
        public Transform[,] SpawnTransforms { get => _spawnTransforms; set => _spawnTransforms = value; }
        public Transform[,] BoardTransforms { get => _boardTransforms; set => _boardTransforms = value; }
    }
}
