using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class TaxTile : Tile
    {
        private Mediator _mediator;
        private bool isPlayerHasEnoughMoney;
        private void Awake()
        {
            _mediator = GameObject.FindObjectOfType<Mediator>();
        }

        private void OnEnable()
        {
            _mediator.Subscribe<UpdateMoney>(CheckEnoughMoney);
        }
        private void OnDisable()
        {
            _mediator.DeleteSubscriber<UpdateMoney>(CheckEnoughMoney);
        }
        public override void ShowCanvas()
        {
        }

        public override void PerformTileAction()
        {
            if (isPlayerHasEnoughMoney) AddMoneySginal.Trigger(-baseIncome);
            else TaxToJailSignal.Trigger();
        }
        
        private void CheckEnoughMoney(UpdateMoney moneyData)
        {
            if(baseIncome > moneyData.money)
            {
                isPlayerHasEnoughMoney = false;
            }
            else isPlayerHasEnoughMoney=true;
        }
    }
}
