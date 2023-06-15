using Save;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class FindWichTileWeareGong : MonoBehaviour,ISaveble
    {
        [SerializeField] private Tile[] tileArray;
        private HashSet<Tile> allTiles = new HashSet<Tile>();
        private Mediator mediator;
        private int lastTileIndex = 0;
        private int nextTile;

        private void Awake()
        {
            mediator = GameObject.FindObjectOfType<Mediator>();

            foreach (var tile in tileArray)
            {
                allTiles.Add(tile);
            }
        }
        private void OnEnable()
        {
            mediator.Subscribe<ShowDiceCount>(CheckDices);
            mediator.Subscribe<MovemantEnded>(UpdateLastTile);
        }
        private void OnDisable()
        {
            mediator.DeleteSubscriber<ShowDiceCount>(CheckDices);
            mediator.DeleteSubscriber<MovemantEnded>(UpdateLastTile);
        }

        private void UpdateLastTile(MovemantEnded endData)
        {
            lastTileIndex = endData.currentTile;
        }

        private void CheckDices(ShowDiceCount count)
        {
            FindTile(count.diceSum);
        }
        private void FindTile(int diceSum)
        {
            nextTile = lastTileIndex + diceSum;
            if(nextTile > allTiles.Count - 1)
            {
                nextTile -= (allTiles.Count);
            }
            foreach (Tile tile in allTiles)
            {
                if(tile.tileIndex == nextTile)
                {
                    tile.ShowIndicator();
                    lastTileIndex= nextTile;
                    break;
                }
            }
            lastTileIndex= nextTile;

        }

        public object SaveState()
        {
            return new SaveLastTileData
            {
                lastTile = lastTileIndex,
            };
        }

        public void LoadState(object state)
        {
            var loadedData = (SaveLastTileData)state;
            //lastTileIndex = loadedData.lastTile;
        }
    }
    [System.Serializable]
    public struct SaveLastTileData
    {
        public int lastTile;
    }
}
