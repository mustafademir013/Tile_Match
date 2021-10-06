using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TileMatch.Base;
using TileMatch.Board;
using TileMatch.Inp;
using TileMatch.UI;
using UnityEngine;

namespace TileMatch.Tile
{
    public struct Index
    {
        public int Row;
        public int Col;

        public Index(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }

    public class TileController : MonoBehaviour
    {

        const string TILETAG = "Tile";

        public static event Action NoFoundMatch;

        [SerializeField] private BoardSettings _boardSettings;
        [SerializeField] private TransformData _transformData;

        [SerializeField] private GameObject[] _tilePrefabs;
        [SerializeField] private IntValue _minumumMatchCount;

        private Transform[,] _tileTransforms;

        private void OnEnable()
        {
            BoardController.BoardCreated += CreateTilesHandler;
            InputController.TouchDropped += ClickTileHandler;
            GameOverPanelController.PanelClosed += RenewTilesHandler;
        }
        private void OnDisable()
        {
            BoardController.BoardCreated -= CreateTilesHandler;
            InputController.TouchDropped -= ClickTileHandler;
            GameOverPanelController.PanelClosed -= RenewTilesHandler;
        }

        private void ClickTileHandler(Vector3 touchPos)
        {
            Collider2D hit = Physics2D.OverlapPoint(touchPos);
            if (hit != null && hit.tag == TILETAG)
            {
                List<Index> matchedList = FindMatches(hit.transform);
                if (matchedList.Count >= _minumumMatchCount.Value)
                {
                    int id = _tileTransforms[matchedList[0].Row, matchedList[0].Col].
                        GetComponent<TileComponent>().Id;
                    Debug.Log("--Tile Id =>" + id + "-- Match Count =>" + matchedList.Count);
                    DeSpawnTile(matchedList);
                    StartCoroutine(AlignTile());
                }
            }
        }
        private void CreateTilesHandler()
        {
            if (_tileTransforms == null)
                _tileTransforms = new Transform[_boardSettings.RowCount, _boardSettings.ColCount];

            for (int i = 0; i < _boardSettings.RowCount; i++)
            {
                for (int j = 0; j < _boardSettings.ColCount; j++)
                {
                    Transform tile = SpawnTile(_transformData.SpawnTransforms[i, j]);
                    tile.GetComponent<TileComponent>().SetIndex(i, j);
                    tile.parent = transform;
                    _tileTransforms[i, j] = tile;
                    TranslateTile(tile, new Index(i, j), 0.5f);
                }
            }
        }

        private void RenewTilesHandler()
        {
            foreach (var item in _tileTransforms)
                DeSpawnTile(item);
            CreateTilesHandler();
        }
        private void SpawnTile(int[] emptyCounts)
        {
            for (int col = 0; col < emptyCounts.Length; col++)
            {
                if (emptyCounts[col] > 0)
                {
                    for (int row = 0; row < emptyCounts[col]; row++)
                    {
                        Transform tile = SpawnTile(_transformData.SpawnTransforms[row, col]);
                        tile.GetComponent<TileComponent>().SetIndex(row, col);
                        tile.parent = transform;
                        _tileTransforms[row, col] = tile;
                        TranslateTile(tile, new Index(row, col), 0.1f);
                    }
                }
            }
        }
        private Transform SpawnTile(Transform spawnTr)
        {
            int rnd = UnityEngine.Random.Range(0, _tilePrefabs.Length);
            GameObject tr = SimplePool.Spawn(_tilePrefabs[rnd], spawnTr.position, Quaternion.identity);
            return tr.transform;
        }
        private void DeSpawnTile(List<Index> list)
        {
            foreach (var item in list)
            {
                DeSpawnTile(_tileTransforms[item.Row, item.Col]);
                _tileTransforms[item.Row, item.Col] = null;
            }
        }
        private void DeSpawnTile(Transform transform)
        {
            SimplePool.Despawn(transform.gameObject);
        }
        private void TranslateTile(Transform tile, Index index, float time)
        {
            tile.DOMove(_transformData.BoardTransforms[index.Row, index.Col].position, time).SetEase(Ease.OutBounce);
        }
        private void TranslateTile(Transform sourceTr, Index target)
        {
            _tileTransforms[target.Row, target.Col] = sourceTr;
            sourceTr.GetComponent<TileComponent>().SetIndex(target.Row, target.Col);
            TranslateTile(sourceTr, target, 0.1f);
        }
        IEnumerator AlignTile()
        {
            yield return new WaitForSeconds(0.2f);
            int[] emptyCounts = ReAlignTile();
            SpawnTile(emptyCounts);
            yield return new WaitForSeconds(0.2f);
            List<Index> longestMatchList = FindLongestMatch();

            Debug.Log("Long Match List Count=>" + longestMatchList.Count);
            if (longestMatchList.Count < _minumumMatchCount.Value)
            {
                NoFoundMatch?.Invoke();
            }
        }

        private int[] ReAlignTile()
        {
            int[] emptyTransformCount = new int[_boardSettings.ColCount];

            for (int col = 0; col < _boardSettings.ColCount; col++)
            {
                int emptyCount = 0;
                for (int row = _boardSettings.RowCount - 1; row >= 0; row--)
                {
                    if (_tileTransforms[row, col] == null)
                    {
                        Index source = FindFirstUpNeighbor(new Index(row, col));

                        if (source.Row == -1)
                        {
                            emptyCount = row + 1;
                            row = -1;
                        }
                        else
                        {
                            Index target = new Index(row, col);
                            Transform sourceTr = _tileTransforms[source.Row, source.Col];
                            _tileTransforms[source.Row, source.Col] = null;
                            TranslateTile(sourceTr, target);
                        }
                    }
                }
                emptyTransformCount[col] = emptyCount;
            }
            return emptyTransformCount;
        }
        private Index FindFirstUpNeighbor(Index pivot)
        {
            Index index = new Index(-1, -1);

            for (int row = pivot.Row - 1; row >= 0; row--)
            {
                if (_tileTransforms[row, pivot.Col] != null)
                    return new Index(row, pivot.Col);
            }
            return index;
        }
        private List<Index> FindLongestMatch()
        {
            List<Index> longestMatchList = new List<Index>();

            foreach (var item in _tileTransforms)
            {
                List<Index> list = FindMatches(item);
                if (longestMatchList.Count < list.Count)
                    longestMatchList = list;
            }
            return longestMatchList;
        }
        private List<Index> FindMatches(Transform pivotTr)
        {
            Index pivotIndex = pivotTr.GetComponent<TileComponent>().Index;
            List<Index> matchedList = new List<Index>();
            matchedList.Add(pivotIndex);

            int pivotId = _tileTransforms[pivotIndex.Row, pivotIndex.Col].
                GetComponent<TileComponent>().Id;

            int counter = 0;

            while (matchedList.Count > counter)
            {
                Index pivot = matchedList[counter];
                List<Index> indexList = GetNeigborIndexList(pivot);
                foreach (var item in indexList)
                {
                    try
                    {
                        Transform tile = _tileTransforms[item.Row, item.Col];
                        int id = tile.GetComponent<TileComponent>().Id;
                        if (id == pivotId && !matchedList.Contains(item))
                            matchedList.Add(item);
                    }
                    catch
                    {

                    }
                }
                counter++;
            }
            return matchedList;
        }

        private List<Index> GetNeigborIndexList(Index pivot)
        {
            List<Index> list = new List<Index>();
            list.Add(new Index(pivot.Row, pivot.Col + 1));
            list.Add(new Index(pivot.Row + 1, pivot.Col));
            list.Add(new Index(pivot.Row, pivot.Col - 1));
            list.Add(new Index(pivot.Row - 1, pivot.Col));
            return list;
        }
    }
}