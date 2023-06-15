using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class StartTile : Tile
    {
        [SerializeField] private int turnPaymentAmount;
        private bool first = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!first) return; 
                TurnPayment();
            }
        }
        private void Start()
        {
            Invoke("Set", 5);
        }
        private void Set()
        {
            first = true;
        }
        private void TurnPayment()
        {
            AddMoneySginal.Trigger(turnPaymentAmount);
        }
        public override void ShowCanvas()
        {
            
        }
    }
}
