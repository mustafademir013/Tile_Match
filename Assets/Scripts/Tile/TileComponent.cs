using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TileMatch.Tile
{
    public class TileComponent : MonoBehaviour
    {

        [SerializeField] private int _id;
        public int Id { get => _id; }

        private Index _index;
        public Index Index { get => _index; }

        public void SetIndex(int row, int col)
        {
            _index.Row = row;
            _index.Col = col;
        }
    }
}
