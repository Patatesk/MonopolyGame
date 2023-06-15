using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PK
{
    public class PlayerHelper : MonoBehaviour
    {
        [SerializeField] private HouseTile[] tiles;

        private HashSet<HouseTile> allHouseTiles = new HashSet<HouseTile>();
        private Mediator mediator;
        private bool checkHelpNeeded;
        public bool isHelpShowed;

        private void Awake()
        {
            mediator = GameObject.FindAnyObjectByType<Mediator>();
        }
        private void OnEnable()
        {
            mediator.Subscribe<BuyedTileIndex>(CheckAndDisacarTiles);
            mediator.Subscribe<MovemantEnded>(CheckPlayerNeedHelp);
        }
        private void OnDisable()
        {
            mediator.DeleteSubscriber<BuyedTileIndex>(CheckAndDisacarTiles);
            mediator.DeleteSubscriber<MovemantEnded>(CheckPlayerNeedHelp);
        }
        [ContextMenu("Gettiles")]
        private void GetHouseTiles()
        {
            tiles = GameObject.FindObjectsOfType<HouseTile>();
        }

        private void CheckPlayerNeedHelp(MovemantEnded moveData)
        {
            if (!checkHelpNeeded || isHelpShowed) return;
            if (allHouseTiles.Count == 0) return;
            else if (allHouseTiles.Count <= 2)
            {
                isHelpShowed = true;
                ShowHelpSignal.Trigger();
            }
        }
        public void MoveToTheTile()
        {
            Transform point = null;
            foreach (HouseTile house in allHouseTiles)
            {
                HouseTile tile = house;
                point = house.transform;
                break;
            }
            MoveTheTileSignal.Trigger(point,3);
        }
        private void ArrayToHashSet()
        {
            foreach (HouseTile tile in tiles)
            {
                allHouseTiles.Add(tile);
            }
        }

        private void CheckAndDisacarTiles(BuyedTileIndex index)
        {

            foreach (HouseTile tile in allHouseTiles)
            {
                if (tile.tileIndex == index.index)
                {
                    allHouseTiles.Remove(tile);
                    break;
                }
            }
            if (allHouseTiles.Count <= 2)
            {
                checkHelpNeeded = true;
            }
            if(allHouseTiles.Count == 0)
            {
                CloseHelpSignal.Trigger();
                checkHelpNeeded = false;
            }
        }

        private void Start()
        {
            ArrayToHashSet();
        }


    }
}
