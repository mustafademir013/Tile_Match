using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TileMatch.Board
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Board/Board Settings", order = 1)]
    public class BoardSettings : ScriptableObject
    {
        [Range(5f, 9f)]
        [SerializeField] private int _rowCount = 9;
        public int RowCount { get => _rowCount; }

        [Range(5f, 9f)]
        [SerializeField] private int _colCount = 9;
        public int ColCount { get => _colCount; }

        [SerializeField] private float _tileInterval = 2.3f;
        public float TileInterval { get => _tileInterval;}
    }
}
