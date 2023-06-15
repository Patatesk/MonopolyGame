using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Save;
using MoreMountains.Feedbacks;

namespace PK
{
    public class Tile : MonoBehaviour, ISaveble

    {
        public Transform nexTile;
        public bool isSold;
        public bool isSpecialTile;
        public int baseIncome;
        public int houseIncome;
        public int price;
        public int tileIndex;
        public int tileGroupIndex;


        [SerializeField] private MMF_Player buyFeedBacks;
        [SerializeField] private MMF_Player touchFeedBacks;
        [SerializeField] protected HouseSpawnPoint spawnPoint;
        [SerializeField] private MaterialFade indcator;

        public HouseSpawnPoint returnSpawnPoint { get { return spawnPoint; } }

        private Mediator mediator;
        


        protected TileCanvasController tileCanvasController;

        private void Awake()
        {
            mediator = GameObject.FindAnyObjectByType<Mediator>();
            tileCanvasController = GetComponent<TileCanvasController>();
        }
        private void Start()
        {
            if (isSold && !isSpecialTile)
            {
                LateLoad();
                AddTileToGroupSignal.Trigger(tileGroupIndex);
            }

        }

        public void PlayTouchFeedbacks()
        {
            touchFeedBacks.Initialization();
            touchFeedBacks.PlayFeedbacks();
        }

        public virtual void PerformTileAction()
        {

        }
        public void ShowIndicator()
        {
            indcator.StartFade();
        }

        public void CloseIndicator()
        {
            indcator.StopFade();
        }
        public void BuyThisTile()
        {
            if (isSold) return;
            spawnPoint.gameObject.SetActive(true);
            PaymentRequestSignal.Trigger(-price);
            isSold = true;
            spawnPoint.isTileSold = true;
            if (buyFeedBacks != null)
            {
                buyFeedBacks.Initialization();
                buyFeedBacks.PlayFeedbacks();
            }
            else Debug.Log("Satýnalma Feedback Ekle", this);
            tileCanvasController.CloseCanvas();
            AddTileToGroupSignal.Trigger(tileGroupIndex);
            PublishBuyedTile();
            SaveLoadSignals.Signal_Save();

        }

        public virtual void ShowCanvas()
        {
            if(OnBoardingProceses.isFirstLoad)
            tileCanvasController.ShowCanvas();
        }
        public void CloseCanvas()
        {
            if (tileCanvasController != null) tileCanvasController.CloseCanvas();
        }
        public void LoadState(object state)
        {
            var loadedData = (SavedTileData)state;
            isSold = loadedData.isSold;

            if (isSold)
            {
                //Invoke("LateLoad",.5f);
                Invoke("PublishBuyedTile", 1);
            }
        }

        private void PublishBuyedTile()
        {
            if (isSpecialTile) return;
            BuyedTileIndex buyedTileIndex = new BuyedTileIndex();
            buyedTileIndex.index = tileIndex;
            mediator.Publish(buyedTileIndex);
        }
        private void LateLoad()
        {
            if (spawnPoint != null)
            {
                spawnPoint.gameObject.SetActive(true);
                spawnPoint.isTileSold = isSold;
            }
            if (buyFeedBacks != null)
            {
                buyFeedBacks.Initialization();
                buyFeedBacks.PlayFeedbacks();
            }

            if (tileCanvasController != null) tileCanvasController.CloseCanvas();
        }

        public object SaveState()
        {
            return new SavedTileData { isSold = this.isSold };
        }
    }
    [System.Serializable]
    public struct SavedTileData
    {
        public bool isSold;
    }

    public class BuyedTileIndex : ICommand
    {
        public int index;
    }
}
