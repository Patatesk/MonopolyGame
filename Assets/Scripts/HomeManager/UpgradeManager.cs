using System;
using UnityEngine;

namespace PK
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private GameObject shack, house, apartmant, hotel;
        private void Upgrade(HouseType type, HouseSpawnPoint snapTransform)
        {
            switch (type)
            {
                case HouseType.Shack:
                    InstantiateHouseAndPlayFeedBacks(shack, snapTransform);
                    break;
                case HouseType.House:
                    InstantiateHouseAndPlayFeedBacks(house, snapTransform);
                    break;
                case HouseType.Apartmant:
                    InstantiateHouseAndPlayFeedBacks(apartmant, snapTransform);
                    break;
                case HouseType.Hotel:
                    InstantiateHouseAndPlayFeedBacks(hotel, snapTransform);
                    break;

            }

        }
        private void OnEnable()
        {
            UpgradeSignal.Upgrade += Upgrade;
        }

        private void OnDisable()
        {
            UpgradeSignal.Upgrade -= Upgrade;
        }

        private void InstantiateHouseAndPlayFeedBacks(GameObject house, HouseSpawnPoint _snapTransform)
        {
            GameObject newHouse = Instantiate(house);
            _snapTransform.SnapPosition(newHouse.transform);
            House houseScript = newHouse.GetComponent<House>();
            houseScript.PlayUpgradeFeedBacks();

        }
    }
    public class UpgradeSignal
    {
        public static event Action<HouseType, HouseSpawnPoint> Upgrade;
        public static void Trigger(HouseType type, HouseSpawnPoint spawnPoint)
        {
            Upgrade?.Invoke(type, spawnPoint);
        }
    } 
}
