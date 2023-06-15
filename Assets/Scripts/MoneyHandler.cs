using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Save;

namespace PK
{
    public class MoneyHandler : MonoBehaviour, ISaveble
    {
        public int money = 1000;
        private Mediator mediator;

        private void Awake()
        {
            mediator = GameObject.FindObjectOfType<Mediator>();
        }
        private void Start()
        {
            UpdateMoney money = new UpdateMoney();
            money.money = this.money;
            mediator.Publish(money);
        }
        private void OnEnable()
        {
            AddMoneySginal.AddMoney += EarnMoney;
            PaymentRequestSignal.PaymentRequest += PaymentProcesses;
        }
        private void OnDisable()
        {
            AddMoneySginal.AddMoney -= EarnMoney;
            PaymentRequestSignal.PaymentRequest -= PaymentProcesses;
        }
        private void EarnMoney(int value)
        {
            money += value;
            UpdateMoney newMoney = new UpdateMoney();
            newMoney.money = money;
            mediator.Publish(newMoney);

        }

        private void PaymentProcesses(int value)
        {
            int moneyBeforeSpent = money;
            money += value;
            PaymentProceseses payment = new PaymentProceseses();
            UpdateMoney newMoney = new UpdateMoney();
            if (money < 0)
            {
                Debug.Log("Para yetmedi");
                money = moneyBeforeSpent;
                payment.money = money;
                newMoney.money = money;
                payment.paymentSucces = false;
                payment.moneyToBeSpend = value;
                mediator.Publish(payment);
                mediator.Publish(newMoney);
                return;

            }
            newMoney.money = money;
            payment.money = money;
            payment.paymentSucces = true;
            payment.moneyToBeSpend = value;
            mediator.Publish(payment);
            mediator.Publish(newMoney);

        }

        public object SaveState()
        {
            return new SaveMoneyData
            {
                money = this.money,
            };
        }


        public void LoadState(object state)
        {

            var loadedData = (SaveMoneyData)state;
            money = loadedData.money;

        }
    }
    #region MediatorAndSignals
    public class AddMoneySginal
    {
        public static event Action<int> AddMoney;
        public static void Trigger(int value)
        {
            AddMoney?.Invoke(value);
        }
    }
    public class PaymentRequestSignal
    {
        public static event Action<int> PaymentRequest;
        public static void Trigger(int value)
        {
            PaymentRequest?.Invoke(value);
        }
    }
    public class PaymentProceseses : ICommand
    {
        public bool paymentSucces;
        public int money;
        public int moneyToBeSpend;
    }
    public class UpdateMoney : ICommand
    {
        public int money;
    }
    #endregion

    [Serializable]
    public struct SaveMoneyData
    {
        public int money;
    }
}
