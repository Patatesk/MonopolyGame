
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Save;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

namespace PK
{
    public class HouseSpawnPoint : MonoBehaviour, ISaveble
    {
        [SerializeField] private int price;
        [SerializeField] private GameObject icon;
        [SerializeField] private Transform snapParent;

        public bool _isFree = true;
        public int houseLevel;
        public int houseBaseIncome;
        public bool connectedToTheTile = false;
        public bool isTileSold;
        public int spawnerIndex;


        private HouseSnapperCanvasController canvasContoller;
        private UpgradeHomeCheck upgradeHome;
        private BoxCollider _collider;
        private SaveHouseSpawnPoint spawnPoint;

        public bool isSold;
        public UpgradeHomeCheck returnUpgradeHome { get { return upgradeHome; } }

        private void OnEnable()
        {
            ChoosedSpawnPointSignal.CloseAllPanels += CheckCanvasClosing;
        }
        private void OnDisable()
        {
            ChoosedSpawnPointSignal.CloseAllPanels -= CheckCanvasClosing;
        }
        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            canvasContoller = GetComponent<HouseSnapperCanvasController>();
            if (isSold)
            {
                isSold = true;
                icon.SetActive(false);
                canvasContoller.CloseCanvas();
            }
        }
        private void Start()
        {
            canvasContoller.price = price;
            if (!isTileSold && connectedToTheTile) gameObject.SetActive(false);
            if(spawnPoint.data != null)
            AddLoadDataSignal.Trigger(spawnPoint.data);

        }

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        public void PlayFeedBack()
        {
            upgradeHome.buyFeedbacks.Initialization();
            upgradeHome.buyFeedbacks.PlayFeedbacks();
        }
        public bool SnapPosition(Transform objectToSnap, bool playFeedbacks = true)
        {
            if (!_isFree || !isSold || snapParent.childCount > 0)
            {
                return _isFree;
            }
            Vector3 pos = transform.position;
            pos.y = _collider.bounds.max.y;
            objectToSnap.position = transform.position;
            objectToSnap.rotation = transform.rotation;
            objectToSnap.parent = snapParent;
            upgradeHome = objectToSnap.GetComponent<UpgradeHomeCheck>();
            if (!playFeedbacks) upgradeHome.PlaySnapFeedbacks();
            if (upgradeHome != null)
            {
                upgradeHome.snappedTransform = this;
                houseBaseIncome = upgradeHome.baseIncome;
                if (upgradeHome.type == HouseType.Hotel && connectedToTheTile)
                {
                    AddHotelSignal.Trigger(1);
                }
                if(upgradeHome.type == HouseType.House && !OnBoardingProceses.isFirstLoad && connectedToTheTile)
                {
                    AddHotelSignal.Trigger(0);
                }
            }

            else { Debug.Log("UpgradeHome Yakalanamadý", this); }
            SaveLoadSignals.Signal_Save();
            return true;

        }
       

        public void OpenCanvas()
        {
            if (!IsPointerOverUIObject())
            {
                if (!isSold)
                {
                    canvasContoller.ShowCanvas();
                    ChoosedSpawnPointSignal.Trigger(this);
                }
            }
        }

        private void CheckCanvasClosing(HouseSpawnPoint spawnPoint)
        {
            if (spawnPoint == this) return;
            canvasContoller.CloseCanvas();
        }
        public void BuyButton()
        {
            PaymentRequestSignal.Trigger(-price);
            isSold = true;
            icon.SetActive(false);
            canvasContoller.CloseCanvas();

        }
        public void LeaveObject()
        {
            if (upgradeHome != null && connectedToTheTile)
            {
                if (upgradeHome.type == HouseType.Hotel)
                {
                    AddHotelSignal.Trigger(-1);
                }
            }
            if (upgradeHome != null) upgradeHome.transform.parent = null;
            _isFree = true;
            upgradeHome = null;
            houseBaseIncome = 0;
        }

        public object SaveState()
        {
            LoadHouseData houseData = new LoadHouseData();
            int number =0;
            if (!_isFree)
            {
                houseData.spawnPoint = spawnerIndex;
                if (upgradeHome != null)
                {
                    switch (upgradeHome.type)
                    {
                        case HouseType.Shack:
                            number = 0;
                            break;
                        case HouseType.House:
                            number = 1;

                            break;

                        case HouseType.Apartmant:
                            number = 2;

                            break;
                        case HouseType.Hotel:
                            number = 3;
                            break;
                    }
                }
                houseData._type = number;
            }
            else houseData = null;
            return new SaveHouseSpawnPoint
            {
                _isSold = isSold,
                _isFree = this._isFree,
                data = houseData,
            };
        }

        public void LoadState(object state)
        {
            var loadedData = (SaveHouseSpawnPoint)state;
            isSold = loadedData._isSold;
            spawnPoint = loadedData;
            
        }
    }

    public class ChoosedSpawnPointSignal
    {
        public static event Action<HouseSpawnPoint> CloseAllPanels;
        public static void Trigger(HouseSpawnPoint spawnPint = null)
        {
            CloseAllPanels?.Invoke(spawnPint);
        }
    }
    [Serializable]
    public struct SaveHouseSpawnPoint
    {
        public bool _isSold;
        public bool _isFree;
        public LoadHouseData data;
    }
}
