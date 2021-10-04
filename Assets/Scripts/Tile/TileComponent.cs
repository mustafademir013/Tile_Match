using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TileMatch.Tile
{
    public class TileComponent : MonoBehaviour
    {
        private int _row;
        private int _col;

        public int Row { get => _row; set => _row = value; }
        public int Col { get => _col; set => _col = value; }
    }
}
