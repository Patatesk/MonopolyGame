using System;
using UnityEngine;

namespace PK
{
    public class TileCatcher : MonoBehaviour
    {
        public Tile tile;
        private Move playerMove;
        private bool _canlay;
        private void Awake()
        {
            playerMove = transform.root.GetComponent<Move>();
            Invoke("CanPlay", 2);
        }

      
        private void CanPlay()
        {
            _canlay = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Tile"))
            {
                tile = other.GetComponent<Tile>();
                playerMove.currentBoard = other.transform;
                playerMove.nextBoard = tile.nexTile;
                if (_canlay) tile.PlayTouchFeedbacks();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Tile"))
            {
                tile.CloseCanvas();
                tile = null;
            }
        }

    }
    
}
