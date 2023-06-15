using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class DebugAddMoney : MonoBehaviour
    {
        public void EarnMoney()
        {
            AddMoneySginal.Trigger(5000);
        }
        public void DeleteSAve()
        {
            SaveLoadSignals.Signal_Delete();
        }
    }
}
