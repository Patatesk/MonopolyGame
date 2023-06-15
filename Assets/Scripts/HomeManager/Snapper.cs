using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class Snapper : MonoBehaviour
    {
        [SerializeField] private GameObject objectToChange;

        private HouseSpawnPoint point;

        private void Awake()
        {
            point = GetComponent<HouseSpawnPoint>();
        }

        private void OnEnable()
        {
            MoveHouseStartedSignal.MoveHouseStarted += ChangeColor;
        }
        private void OnDisable()
        {
            MoveHouseStartedSignal.MoveHouseStarted -= ChangeColor;
        }

        private void ChangeColor(bool started)
        {
            if (started && point.isSold && point._isFree) objectToChange.SetActive(true);
            else if (!started)
            {
                objectToChange.SetActive(false);
            }
        }
    }
}
