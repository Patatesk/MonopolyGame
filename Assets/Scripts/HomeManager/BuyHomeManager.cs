using Save;
using UnityEngine;

namespace PK
{
    public class BuyHomeManager : MonoBehaviour,ISaveble
    {
        [SerializeField] private GameObject house;
        [SerializeField] private HouseSpawnPoint[] houseSpawnPoints;
        [SerializeField] private float raisePercentage;
        [SerializeField] private int maxPrice;
        public int housePrice;
        private Mediator mediator;

        private void Awake()
        {
            mediator = GameObject.FindObjectOfType<Mediator>();
        }
        private void Start()
        {
            HousePrice newPrice = new HousePrice();
            newPrice.price = housePrice;
            mediator.Publish(newPrice);
            InýtLevelManager.Trigger();
        }
        public void BuyHouseButton()
        {
            HouseSpawnPoint spawnPoint = CheckEmptySpaces();
            if (spawnPoint == null) return;
            PaymentRequestSignal.Trigger(-housePrice);
            InstantiateHouse(spawnPoint);
            housePrice = Mathf.Clamp(Mathf.RoundToInt(raisePercentage * housePrice),0,maxPrice);
            HousePrice newPrice = new HousePrice();
            newPrice.price = housePrice;
            mediator.Publish(newPrice);
            SaveLoadSignals.Signal_Save();

        }

        private void InstantiateHouse(HouseSpawnPoint spawnPoint)
        {
           
            GameObject _house = Instantiate(house);
            spawnPoint.SnapPosition(_house.transform,false);
            spawnPoint.PlayFeedBack();

        }

        private HouseSpawnPoint CheckEmptySpaces()
        {
            HouseSpawnPoint _house = null;
            foreach (HouseSpawnPoint house in houseSpawnPoints)
            {
                if (house._isFree && house.isSold)
                {
                    _house = house;
                    break;
                }
            }
            return _house;
        }

        public object SaveState()
        {
            return new SaveHousePriceData { price = housePrice };
        }

        public void LoadState(object state)
        {
            var loadedData = (SaveHousePriceData)state;
            housePrice = loadedData.price;
        }
    }
    [System.Serializable]
    public struct SaveHousePriceData
    {
        public int price;
    }

    public class HousePrice : ICommand
    {
        public int price;
    }

    
}
