using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using System;

namespace PK
{
    public class TileGroupAnimator : MonoBehaviour
    {
        [SerializeField] private int totalGroupLenght;
        [SerializeField] private MMF_Player[] groupFeedbacks;

        private FeedbackChecker[] feedbackCheckerList;
        private bool _canlay;

        private void Awake()
        {
            feedbackCheckerList = new FeedbackChecker[groupFeedbacks.Length];
            for (int i = 0; i < groupFeedbacks.Length; i++)
            {
                FeedbackChecker checker = new FeedbackChecker();
                checker.feedback = groupFeedbacks[i];
                checker.totalGroupLength = totalGroupLenght;
                checker.CurrentGroupLenght = 0;
                checker.feedback = groupFeedbacks[i];
                checker.canAnimate = _canlay;
                feedbackCheckerList[i] = checker;
              
            }
            Invoke("CanPlay", 2f);
        }

     
       
        private void CanPlay()
        {
            _canlay = true;
            for (int i = 0; i < groupFeedbacks.Length; i++)
            {
                feedbackCheckerList[i].canAnimate = _canlay;
            }

        }

        private void OnEnable()
        {
            AddTileToGroupSignal.AddTileToGroup += AddTileToTheGroup;
        }
        private void OnDisable()
        {
            AddTileToGroupSignal.AddTileToGroup -= AddTileToTheGroup;
        }

        private void AddTileToTheGroup(int Index)
        {
          feedbackCheckerList[Index].CurrentGroupLenght++;    
        }
       
    }
    [Serializable]
    public class FeedbackChecker
    {
        public int totalGroupLength;
        public MMF_Player feedback;
        private int currentGroupLenght;
        public bool canAnimate;
        public int CurrentGroupLenght
        {
            get { return currentGroupLenght; }
            set
            {
                currentGroupLenght = value;
                if (currentGroupLenght >= totalGroupLength) {if(canAnimate) feedback.PlayFeedbacks(); }
            }
        }

    }

    public class AddTileToGroupSignal
    {
        public static event Action<int> AddTileToGroup;
        public static void Trigger(int groupIndex)
        {
            AddTileToGroup?.Invoke(groupIndex);
        }
    }
}
