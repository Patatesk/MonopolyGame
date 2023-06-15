using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class ChildChecker : MonoBehaviour
    {
        [SerializeField] HouseSpawnPoint check;


        private void OnTransformChildrenChanged()
        {
            if(transform.childCount == 1)
            {
                check._isFree = false;
            }
            else if(transform.childCount == 0)
            {
                check._isFree = true;
            }
        }
    }
}
