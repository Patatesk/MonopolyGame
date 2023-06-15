using UnityEngine;
using MoreMountains.Feedbacks;
namespace PK
{
    public class House : MonoBehaviour
    {
        private UpgradeHomeCheck upgrade;
        public MMF_Player upgradeFeedBack;

        public void PlayUpgradeFeedBacks()
        {
            if (upgradeFeedBack != null)
            {
                upgradeFeedBack.Initialization();
                upgradeFeedBack.PlayFeedbacks();
            }
            else Debug.Log("FeedBackAtalý Deðil");
        }
    }
}
