using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class TaxToTileJail : MonoBehaviour
    {
        [SerializeField] private Transform jail;

        public void GoToJail()
        {
            MoveTheTileSignal.Trigger(jail,5);
            gameObject.SetActive(false);
        }
    }
}
