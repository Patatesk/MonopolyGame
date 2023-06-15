using Cinemachine;
using Mono.Cecil.Cil;
using UnityEngine;

namespace PK
{
    class ScrollAndPinch : MonoBehaviour
    {
#if UNITY_IOS || UNITY_ANDROID
        public Camera Camera;
        public GameObject cameraPivot;
        public CinemachineVirtualCamera vCam;
        public bool Rotate;
        protected Plane Plane;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private float zoomMin = 1;
        [SerializeField] private float zoomMax = 8;
        [SerializeField] private Transform midPoint;
        private DragObject dragObject;

        private void Awake()
        {
            dragObject = GameObject.FindObjectOfType<DragObject>();
            if (Camera == null)
                Camera = Camera.main;
        }


        private void Update()
        {
            if (dragObject._isDragging || !OnBoardingProceses.isFirstLoad) return;
            //Update Plane
            if (Input.touchCount >= 1)
                Plane.SetNormalAndPosition(transform.up, transform.position);

            var Delta1 = Vector3.zero;
            var Delta2 = Vector3.zero;

            //Scroll
            //if (Input.touchCount >= 1)
            //{
            //    Delta1 = PlanePositionDelta(Input.GetTouch(0));
            //    if (Input.GetTouch(0).phase == TouchPhase.Moved)
            //        cameraPivot.transform.Translate(Delta1, Space.World);
            //}

            //Pinch
            if (Input.touchCount >= 2)
            {
                var pos1 = PlanePosition(Input.GetTouch(0).position);
                var pos2 = PlanePosition(Input.GetTouch(1).position);
                var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
                var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

                //calc zoom
                //var zoom = Vector3.Distance(pos1, pos2) /
                //           Vector3.Distance(pos1b, pos2b);
                ////edge case
                //if (zoom == 0 || zoom > 2)
                //    return;

                ////Move cam amount the mid ray
                //cameraPivot.transform.position = Vector3.LerpUnclamped(pos1, cameraPivot.transform.position, 1 / zoom);

                if (Rotate && pos2b != pos2)
                    midPoint.transform.RotateAround(midPoint.transform.position, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;
                float difference = currentMagnitude - prevMagnitude;
                zoom(difference * 0.01f);
            }

            Vector3 clampedPos = cameraPivot.transform.position;
            clampedPos.x = Mathf.Clamp(cameraPivot.transform.position.x, _collider.bounds.min.x, _collider.bounds.max.x);
            clampedPos.y = Mathf.Clamp(cameraPivot.transform.position.y, _collider.bounds.min.y, _collider.bounds.max.y);
            clampedPos.z = Mathf.Clamp(cameraPivot.transform.position.z, _collider.bounds.min.z, _collider.bounds.max.z);
            cameraPivot.transform.position = clampedPos;
            
        }
        private void zoom(float increment)
        {
            vCam.m_Lens.OrthographicSize = Mathf.Clamp(vCam.m_Lens.OrthographicSize - increment, zoomMin, zoomMax);
        }



            protected Vector3 PlanePositionDelta(Touch touch)
            {
                //not moved
                if (touch.phase != TouchPhase.Moved)
                    return Vector3.zero;

                //delta
                var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
                var rayNow = Camera.ScreenPointToRay(touch.position);
                if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
                    return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

                //not on plane
                return Vector3.zero;
            }

            protected Vector3 PlanePosition(Vector2 screenPos)
            {
                //position
                var rayNow = Camera.ScreenPointToRay(screenPos);
                if (Plane.Raycast(rayNow, out var enterNow))
                    return rayNow.GetPoint(enterNow);

                return Vector3.zero;
            }

            private void OnDrawGizmos()
            {
                Gizmos.DrawLine(transform.position, transform.position + transform.up);
            }
#endif


        }
    }
