using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class StopJail : MonoBehaviour
    {
        public void StopJailNow()
        {
            JailStartedSignals.Trigger(false);
        }
    }
}
