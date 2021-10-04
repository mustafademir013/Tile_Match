using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TileMatch.Board;
using TileMatch.Inp;
using UnityEngine;

namespace TileMatch.Tile
{
    public class TileController : MonoBehaviour
    {

        const string TILETAG = "Tile";

        [SerializeField] private BoardSettings _boardSettings;
        [SerializeField] private TransformData _transformData;

        [SerializeField] private GameObject[] _tilePrefabs;

        private void OnEnable()
        {
            BoardController.BoardCreated += CreateTiles;
            InputController.TouchDropped += ClickTile;
        }
        private void OnDisable()
        {
            BoardController.BoardCreated -= CreateTiles;
            InputController.TouchDropped -= ClickTile;
        }

        private void CreateTiles()
        {
            for (int i = 0; i < _boardSettings.RowCount; i++)
            {
                for (int j = 0; j < _boardSettings.ColCount; j++)
                {
                    Transform tile = SpawnTile(_transformData.SpawnTransforms[i, j]);
                    tile.GetComponent<TileComponent>().Row = i;
                    tile.GetComponent<TileComponent>().Col = j;
                    tile.parent = transform;
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

        private void TranslateTile(Transform tile, int i, int j)
        {
            tile.DOMove(_transformData.BoardTransforms[i, j].position, 0.5f).SetEase(Ease.OutBounce);
        }

        private void ClickTile(Vector3 touchPos)
        {
            Collider2D hit = Physics2D.OverlapPoint(touchPos);
            if (hit != null && hit.tag == TILETAG)
            {
                Destroy(hit.gameObject);
            }
        }
    }
}