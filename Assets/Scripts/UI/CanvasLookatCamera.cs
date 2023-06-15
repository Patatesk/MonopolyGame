using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PK
{
    public class CanvasLookatCamera : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
        }
    }
}
