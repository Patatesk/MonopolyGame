using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace PK
{
    public class UpgradeHomeCheck : MonoBehaviour
    {
        public HouseType type;
        public HouseSpawnPoint snappedTransform;
        public UpgradeHomeCheck upgradeHome;
        public MMF_Player buyFeedbacks;
        public int baseIncome;
        [SerializeField] private MMF_Player snapFeedBacks;

        private void Start()
        {
            if (OnBoardingProceses.isFirstLoad) return;
            if (type == HouseType.House) OnBoardEvet.Triggrt();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("House")) upgradeHome = other.transform.GetComponent<UpgradeHomeCheck>();
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("House")) upgradeHome = null;
        }

        public void PlaySnapFeedbacks()
        {
            snapFeedBacks.Initialization();
            snapFeedBacks.PlayFeedbacks();
        }
        public bool CheckUpgrade()
        {
            if (upgradeHome == null || type + 1 > HouseType.Hotel) return false;
            if (upgradeHome.type == type)
            {
                upgradeHome.snappedTransform.LeaveObject();
                if (snappedTransform != null) snappedTransform.LeaveObject();
                UpgradeSignal.Trigger(type + 1, upgradeHome.snappedTransform);
                Destroy(upgradeHome.gameObject);
                Destroy(gameObject);
            }
            else Debug.Log("Ayný tip deðil");
            return true;
        }



    }
    public enum HouseType
    {
        Shack = 0,
        House = 1,
        Apartmant = 2,
        Hotel = 3,
    }
}
