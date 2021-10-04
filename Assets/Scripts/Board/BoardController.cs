using System;
using DG.Tweening;
using TileMatch.Base;
using UnityEngine;

namespace TileMatch.Board
{
    public class BoardController : AbtstractBaseInitalizable
    {

        public static event Action BoardCreated;

        [SerializeField] private BoardSettings _boardSettings;
        [SerializeField] private TransformData _transformData;

        [SerializeField] private Transform _boardParent;
        [SerializeField] private Transform _spawnParent;

        protected override void Initalize()
        {
            CreateBoard();
        }

        private void CreateBoard()
        {
            _transformData.SpawnTransforms = new Transform[_boardSettings.RowCount, _boardSettings.ColCount];
            _transformData.BoardTransforms = new Transform[_boardSettings.RowCount, _boardSettings.ColCount];

            float startPointX = -(_boardSettings.ColCount / 2) * _boardSettings.TileInterval;
            float startPointY = (_boardSettings.RowCount / 2) * _boardSettings.TileInterval;
            Vector3 startPoint = new Vector3(startPointX, startPointY, 0);

            for (int i = 0; i < _boardSettings.RowCount; i++)
            {
                for (int j = 0; j < _boardSettings.ColCount; j++)
                {
                    string name = "Tr(" + i.ToString() + "," + j.ToString() + ")";

                    Transform spawnTr = new GameObject(name).transform;
                    spawnTr.parent = _spawnParent;
                    spawnTr.localPosition = startPoint;
                    _transformData.SpawnTransforms[i, j] = spawnTr;

                    Transform boardTr = new GameObject(name).transform;
                    boardTr.parent = _boardParent;
                    boardTr.localPosition = startPoint;
                    _transformData.BoardTransforms[i, j] = boardTr;

                    startPoint.x += _boardSettings.TileInterval;
                }
                startPoint.x = startPointX;
                startPoint.y -= _boardSettings.TileInterval;
            }
            BoardCreated?.Invoke();
        }
    }
}
