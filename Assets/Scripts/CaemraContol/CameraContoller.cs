using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace PK
{
    public class CameraContoller : MonoBehaviour
    {
        [SerializeField] private CameraTouchControl cameraControl;
        [SerializeField] private CinemachineVirtualCamera virtualCam;
        [SerializeField] private float speed;
        [SerializeField] private float panSpeed = 20;
        [SerializeField] private float pinchStep;
        [SerializeField] private float zoomMin;
        [SerializeField] private float zoomMax;
        [SerializeField] private BoxCollider bounds;

        private Vector2 touchStart;
        private DragObject dragObject;
        private bool canZoom;
        private bool canPan;

        private void Awake()
        {
            cameraControl = new CameraTouchControl();
            dragObject = GameObject.FindObjectOfType<DragObject>();
        }
        private void OnEnable()
        {
            cameraControl.Enable();
        }
        private void OnDisable()
        {
            cameraControl.Disable();
        }
        private void Start()
        {
            cameraControl.Touch.SecondTouchStart.started += _ => PinchStart();
            cameraControl.Touch.SecondTouchStart.canceled += _ => PinchEnd();
        }
        IEnumerator StartZoom()
        {
            float previusDistance = 0;
            float distance = 0;

            while (true)
            {
                distance = Vector2.Distance(cameraControl.Touch.FirstTouch.ReadValue<Vector2>(), cameraControl.Touch.SecondTouch.ReadValue<Vector2>());

                if (distance > previusDistance)
                {
                    float FOV = virtualCam.m_Lens.FieldOfView;
                    FOV -= pinchStep;
                    FOV = Mathf.Clamp(FOV, zoomMin, zoomMax);
                    virtualCam.m_Lens.FieldOfView = Mathf.Lerp(virtualCam.m_Lens.FieldOfView, FOV, 0.5f);
                }
                else if (distance < previusDistance)
                {
                    float FOV = virtualCam.m_Lens.FieldOfView;
                    FOV += pinchStep;
                    FOV = Mathf.Clamp(FOV, zoomMin, zoomMax);
                    virtualCam.m_Lens.FieldOfView = Mathf.SmoothStep(virtualCam.m_Lens.FieldOfView, FOV, 0.5f);
                }


                previusDistance = distance;
                yield return new WaitForEndOfFrame();

            }
        }
        float previusDistance = 0;
        float distance = 0;

        void Update()
        {
            if (canZoom)
            {
                if (Input.touchCount == 2)
                {
                    distance = Vector2.Distance(cameraControl.Touch.FirstTouch.ReadValue<Vector2>(), cameraControl.Touch.SecondTouch.ReadValue<Vector2>());

                    if (distance > previusDistance)
                    {
                        float FOV = virtualCam.m_Lens.FieldOfView;
                        FOV -= pinchStep;
                        FOV = Mathf.Clamp(FOV, zoomMin, zoomMax);
                        virtualCam.m_Lens.FieldOfView = Mathf.Lerp(virtualCam.m_Lens.FieldOfView, FOV, 0.5f);
                    }
                    else if (distance < previusDistance)
                    {
                        float FOV = virtualCam.m_Lens.FieldOfView;
                        FOV += pinchStep;
                        FOV = Mathf.Clamp(FOV, zoomMin, zoomMax);
                        virtualCam.m_Lens.FieldOfView = Mathf.Lerp(virtualCam.m_Lens.FieldOfView, FOV, 0.5f);
                    }
                    previusDistance = distance;
                }
            }
            else if (!canZoom && !dragObject._isDragging)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    touchStart = touch.position;
                    canPan = true;
                }
                else if (touch.phase == TouchPhase.Moved && canPan)
                {
                    Vector2 touchEnd = touch.position;
                    float x = touchEnd.x - touchStart.x;
                    float y = touchEnd.y - touchStart.y;
                    touchStart = touch.position;
                    Vector3 pos = virtualCam.transform.position;
                    pos.x -= x * panSpeed * Time.deltaTime;
                    pos.z -= y * panSpeed * Time.deltaTime;
                    pos.x = Mathf.Clamp(pos.x, bounds.bounds.min.x, bounds.bounds.max.x);
                    pos.y = Mathf.Clamp(pos.y, bounds.bounds.min.y, bounds.bounds.max.y);
                    pos.z = Mathf.Clamp(pos.z, bounds.bounds.min.z, bounds.bounds.max.z);
                    virtualCam.transform.position = Vector3.Lerp(virtualCam.transform.position,pos,0.1f);
                }
                if(touch.phase == TouchPhase.Ended)
                {
                    canPan = false;
                }
            }

        }

        private void PinchStart()
        {
            canZoom = true;
            previusDistance = 0;
            distance = 0;
            
        }
        private void PinchEnd()
        {
            canZoom = false;
        }
        
    }
}
