using MoreMountains.Feedbacks;
using UnityEngine;
using TMPro;

namespace PK
{
    public class JailTile : Tile
    {
        [SerializeField] private int jailTime;
        [SerializeField] private MMF_Player jailAnim;
        [SerializeField] private MMF_Player jailResetAnim;

        public override void ShowCanvas()
        {
        }

        public override void PerformTileAction()
        {
            JailStartedSignals.Trigger(true);
            jailAnim.PlayFeedbacks();
        }

        private void OnEnable()
        {
            JailStartedSignals.JailStart += ResetJail;
        }
        private void OnDisable()
        {
            JailStartedSignals.JailStart -= ResetJail;

        }
        private void ResetJail(bool started)
        {
            if(!started)
            jailResetAnim.PlayFeedbacks();
        }
    }
}
