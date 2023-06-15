using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem.EnhancedTouch;

namespace PK
{
    public class DragObject : MonoBehaviour
    {
        [SerializeField] private LayerMask snapLayer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask snapperLayer;
        [SerializeField] private LayerMask closeLayers;
        [SerializeField] private float heightOfsset;
        [SerializeField] private float snapOffset;
        private Camera cam;
        private Transform snappedObject;
        private Vector3 startPos;
        private UpgradeHomeCheck upgradeHomeCheck;


        public bool _isDragging;

        private void Awake()
        {
            cam = Camera.main;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 200, snapLayer))
                {
                    if (snappedObject == null)
                    {
                        snappedObject = hit.transform;
                        upgradeHomeCheck = hit.transform.GetComponent<UpgradeHomeCheck>();
                        startPos = snappedObject.position;
                        if (upgradeHomeCheck.snappedTransform != null) upgradeHomeCheck.snappedTransform.LeaveObject();
                        MoveHouseStartedSignal.Trigger(true);
                        _isDragging = true;
                        //ChoosedSpawnPointSignal.Trigger();
                    }
                }

                if (Input.touchCount > 0)
                {

                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        if (Physics.Raycast(ray, out hit, 550, closeLayers))
                        {
                            if (hit.transform.CompareTag("HouseSnapper"))
                            {
                                hit.transform.GetComponent<HouseSpawnPoint>().OpenCanvas();
                            }
                            else
                            {
                                ChoosedSpawnPointSignal.Trigger();
                            }
                        }
                    }

                }
                //else
                //{
                //    if (!EventSystem.current.IsPointerOverGameObject())
                //    {
                //        if (Physics.Raycast(ray, out hit, 550, closeLayers))
                //        {
                //            if (hit.transform.CompareTag("HouseSnapper"))
                //            {
                //                hit.transform.GetComponent<HouseSpawnPoint>().OpenCanvas();
                //            }
                //            else
                //            {
                //                ChoosedSpawnPointSignal.Trigger();
                //            }
                //        }
                //    }
                //}

            }

            if (Input.GetMouseButton(0))
            {
                if (snappedObject == null) return;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 200, groundLayer))
                {
                    float x = hit.point.x;
                    float z = hit.point.z;
                    Vector3 movePos = new Vector3(x - snapOffset, hit.point.y + heightOfsset, z - snapOffset);
                    snappedObject.position = movePos;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (snappedObject == null) return;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool shouldPalyFeedback = upgradeHomeCheck.CheckUpgrade();
                if (Physics.Raycast(ray, out hit, 200, snapperLayer))
                {
                    if (upgradeHomeCheck.snappedTransform != null) upgradeHomeCheck.snappedTransform.LeaveObject();
                    HouseSpawnPoint spawnPoint = hit.transform.GetComponent<HouseSpawnPoint>();
                    bool isFree = spawnPoint.SnapPosition(snappedObject, shouldPalyFeedback);
                    if (!isFree || !spawnPoint.isSold) upgradeHomeCheck.snappedTransform.SnapPosition(snappedObject, false);

                }
                else
                {
                    if (upgradeHomeCheck.snappedTransform != null) upgradeHomeCheck.snappedTransform.SnapPosition(snappedObject, false);
                    else upgradeHomeCheck.transform.position = startPos;
                }
                if (!OnBoardingProceses.isFirstLoad && shouldPalyFeedback) OnBoardEvet.Triggrt();
                upgradeHomeCheck = null;
                snappedObject = null;
                MoveHouseStartedSignal.Trigger(false);
                _isDragging = false;

            }
        }
        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }


    }
    public class MoveHouseStartedSignal
    {
        public static event Action<bool> MoveHouseStarted;
        public static void Trigger(bool started)
        {
            MoveHouseStarted?.Invoke(started);
        }
    }
}
