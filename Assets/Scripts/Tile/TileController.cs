using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TileMatch.Board;
using UnityEngine;

namespace TileMatch.Tile
{
    public class TileController : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;
        [SerializeField] private TransformData _transformData;

        [SerializeField] private GameObject[] _tilePrefabs;


        private void OnEnable()
        {
            BoardController.BoardCreated += CreateTiles;
        }
        private void OnDisable()
        {
            BoardController.BoardCreated -= CreateTiles;
        }

        private void CreateTiles()
        {
            for (int i = 0; i < _boardSettings.RowCount; i++)
            {
                for (int j = 0; j < _boardSettings.ColCount; j++)
                {
                    Transform tile = SpawnTile(_transformData.SpawnTransforms[i, j]);
                    tile.GetComponent<Tile>().Row = i;
                    tile.GetComponent<Tile>().Col = j;
                    TranslateTile(tile, i, j);
                }
            }
        }

        private Transform SpawnTile(Transform spawnTr)
        {
            int rnd = Random.Range(0, _tilePrefabs.Length);
            GameObject tr = Instantiate(_tilePrefabs[rnd], spawnTr.position, Quaternion.identity);
            return tr.transform;
        }

        public void TranslateTile(Transform tile, int i, int j)
        {
            tile.DOMove(_transformData.BoardTransforms[i, j].position, 0.5f).SetEase(Ease.OutBounce);
        }

    }
}