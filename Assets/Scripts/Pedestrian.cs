using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace PK
{
    public class Pedestrian : MonoBehaviour
    {
        [SerializeField] private MMF_Player player;
        public Transform targetDesti;
        private MMF_Position position;

        private void Awake()
        {
            position = player.GetFeedbackOfType<MMF_Position>();

        }
        private void Start()
        {
            position.DestinationPositionTransform= targetDesti;
            player.Initialization();
            player.PlayFeedbacks();
        }
    }
}
