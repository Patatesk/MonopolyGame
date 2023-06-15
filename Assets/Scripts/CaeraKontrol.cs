using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace PK
{
    public class CaeraKontrol : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera mainCamera;
        [SerializeField] private CinemachineVirtualCamera closeCamera;
        [SerializeField] private float zoomOutDelay;
        public float zoomInDelay;

        private Mediator mediator;

        private void Awake()
        {
            mediator = GameObject.FindAnyObjectByType<Mediator>();
        }

        private void OnEnable()
        {
            mediator.Subscribe<MovemantEnded>(ZoomOut);
            mediator.Subscribe<DiceCount>(ZoomIn);
        }


        private void OnDisable()
        {
            mediator.DeleteSubscriber<MovemantEnded>(ZoomOut);
            mediator.DeleteSubscriber<DiceCount>(ZoomIn);
        }
        private void ZoomIn(DiceCount count)
        {
            Invoke("DelayedZoomIn", zoomInDelay);
        }
        private void DelayedZoomIn()
        {
            closeCamera.Priority = 15;
        }
        private void ZoomOut(MovemantEnded ended)
        {
            Invoke("DelayedZoom",zoomOutDelay);

        }

        private void DelayedZoom()
        {
            closeCamera.Priority = 0;
        }
    }
}
