
using UnityEngine;
using MoreMountains.Feedbacks;

namespace PK
{
    public class PlayerFeedBack : MonoBehaviour
    {
        [SerializeField] private MMF_Player moneyWhiteFeedback;
        [SerializeField] private MMF_Player moneyRedFeedback;
        private MMF_FloatingText textWhite;
        private MMF_FloatingText textRed;

        private void Awake()
        {
            textWhite = moneyWhiteFeedback.GetFeedbackOfType<MMF_FloatingText>();
            textRed = moneyRedFeedback.GetFeedbackOfType<MMF_FloatingText>();
        }
        private void OnEnable()
        {
            AddMoneySginal.AddMoney += MoneyFeedback;
            PaymentRequestSignal.PaymentRequest += MoneyFeedback;
        }
        private void OnDisable()
        {
            AddMoneySginal.AddMoney -= MoneyFeedback;
            PaymentRequestSignal.PaymentRequest -= MoneyFeedback;
        }

        private void MoneyFeedback(int value)
        {
            textWhite.Value = value.ToString();
            textRed.Value = value.ToString();

            if (value < 0)
            {
                moneyRedFeedback.PlayFeedbacks();
            }
            else
            {
                moneyWhiteFeedback.PlayFeedbacks();
            }
        }
    }
}
