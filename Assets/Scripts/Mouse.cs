using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

namespace PK
{
    public class Mouse : MonoBehaviour
    {
        public Vector3 Offset;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform mousePosImg;
        [SerializeField] private MMF_Player player;
        [SerializeField] private MMF_Player Reset;
        Vector3 startPos;
        
        private void Update()
        {
            startPos = Input.mousePosition / canvas.scaleFactor;
            startPos -= Offset;
            mousePosImg.anchoredPosition = Vector3.Lerp(mousePosImg.anchoredPosition, startPos,0.5f);
            if (Input.GetMouseButtonDown(0)) player.PlayFeedbacks();
            if (Input.GetMouseButtonUp(0)) { Reset.PlayFeedbacks(); }
        }
    }
}
