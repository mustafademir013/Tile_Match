using System;
using System.Collections;
using System.Collections.Generic;
using TileMatch.Tile;
using UnityEngine;

namespace TileMatch.UI
{
    public class GameOverPanelController : MonoBehaviour
    {
        public static event Action PanelClosed;

        [SerializeField] private RectTransform _backgroundTr;
        [SerializeField] private RectTransform _panelTr;


        private void OnEnable()
        {
            TileController.NoFoundMatch += ShowPanel;
        }
        private void OnDisable()
        {
            TileController.NoFoundMatch -= ShowPanel;
        }

        public void ClosePanel()
        {
            _panelTr.gameObject.SetActive(false);
            _backgroundTr.gameObject.SetActive(false);
            PanelClosed?.Invoke();
        }
        private void ShowPanel()
        {
            _backgroundTr.gameObject.SetActive(true);
            _panelTr.gameObject.SetActive(true);
        }
    }
}
