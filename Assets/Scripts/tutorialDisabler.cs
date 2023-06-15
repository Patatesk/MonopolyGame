using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class tutorialDisabler : MonoBehaviour
    {
        
        private void Awake()
        {
            OnBoardingProceses.isFirstLoad= true;
        }
    }
}
