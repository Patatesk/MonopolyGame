using System;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using Save;

namespace PK
{
    public class OnBoardingProceses : MonoBehaviour,ISaveble
    {
        public static bool isFirstLoad;
        public MMF_Player moveToBuyTile;
        public MMF_Player moveToBuyHouse;
        public MMF_Player moveToMerchHouse;
        public MMF_Player moveToPutDownHouse;
     
        public GameObject clickBlocker,hand,mergeText,putDownText,buyTileImage;
        public Image panelAlpha;
        public Animator animator;

        private bool checker = false;

        private Mediator mediator;
        private int buyedHouse = 0;
        private int engel = 0;
        private void Awake()
        {
            isFirstLoad = checker;
            mediator = GameObject.FindAnyObjectByType<Mediator>();
            if (isFirstLoad)
            {
                clickBlocker.SetActive(false);
                hand.SetActive(false);
            }
            
        }

        private void OnEnable()
        {
            if (isFirstLoad) return;
            AddHotelSignal.AddHotel += HousePlaceit;
            mediator.Subscribe<MovemantEnded>(BytileIndicator);
            mediator.Subscribe<BuyedTileIndex>(BuyHouseIndicator);
            OnBoardEvet.upgraded += Upgraded;
            ChangeColorAlpha(0.8f);

        }
       
        public void ChangeColorAlpha(float alpha)
        {
            if (panelAlpha != null)
            {
                Color color = panelAlpha.color;
                color.a = alpha;
                panelAlpha.color = color;
            }
            
        }
        private void Upgraded()
        {
            if (isFirstLoad) return;
            moveToMerchHouse.gameObject.SetActive(false);
            moveToPutDownHouse.gameObject.SetActive(true);
            mergeText.SetActive(false);
            putDownText.SetActive(true);

        }
        private void OnDisable()
        {
            if (isFirstLoad) return;
            AddHotelSignal.AddHotel -= HousePlaceit;
            mediator.DeleteSubscriber<MovemantEnded>(BytileIndicator);
            mediator.DeleteSubscriber<BuyedTileIndex>(BuyHouseIndicator);
            OnBoardEvet.upgraded -= Upgraded;

        }

        private void HousePlaceit(int value)
        {
            clickBlocker.SetActive(false);
            hand.SetActive(false);
            isFirstLoad= true;
            AddHotelSignal.AddHotel -= HousePlaceit;
            putDownText.SetActive(false);
            mediator.DeleteSubscriber<MovemantEnded>(BytileIndicator);
            mediator.DeleteSubscriber<BuyedTileIndex>(BuyHouseIndicator);
            SaveLoadSignals.Signal_Save();
            Debug.Log(isFirstLoad);
        }

        private void BytileIndicator(MovemantEnded endData)
        {
            if (isFirstLoad) return;
           
            Invoke("LateIndic", .6f);
        }

        private void LateIndic()
        {
            engel++;
            if (engel <= 1) return;
            hand.transform.GetChild(0).gameObject.SetActive(true);
            moveToBuyTile.Initialization();
            moveToBuyTile.PlayFeedbacks();
            buyTileImage.SetActive(true);
            ChangeColorAlpha(0.8f);
        }

        private void BuyHouseIndicator(BuyedTileIndex index)
        {
            if (isFirstLoad) return;
            moveToBuyHouse.Initialization();
            moveToBuyHouse.PlayFeedbacks(); 
        }

        public void HousedByed()
        {
            if (isFirstLoad) return;
            buyedHouse++;
            if(buyedHouse >= 2)
            {
                moveToMerchHouse.Initialization();
                moveToMerchHouse.PlayFeedbacks();
                ChangeColorAlpha(0f);
                mergeText.SetActive(true);
            }

        }

        public void EnableOrDisableAnimator(bool value)
        {
            animator.enabled = value;
        }

        public object SaveState()
        {
            return new SaveOnboarding
            {
                _isFirstLoad = isFirstLoad,
            };
        }

        public void LoadState(object state)
        {
            var loadedData = (SaveOnboarding)state;
            checker = loadedData._isFirstLoad;
        }
    }
    [System.Serializable]
    public struct SaveOnboarding
    {
        public bool _isFirstLoad;
    }
    public class OnBoardEvet
    {
        public static event Action upgraded;
        public static void Triggrt()
        {
            upgraded?.Invoke();
        }
    }
}
