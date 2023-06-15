using TMPro;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace PK
{
    public class DiceSumShow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI diceText;
        [SerializeField] private GameObject diceobject;
        [SerializeField] private int delayToClose;
        [SerializeField] private MMF_Player showFeedback;
        [SerializeField] private MMF_Player closeFeedback;
        private Mediator mediator;
        private int diceSum;
        private void Awake()
        {
            mediator = GameObject.FindAnyObjectByType<Mediator>();
        }

        private void OnEnable()
        {
            mediator.Subscribe<ShowDiceCount>(ShowDiceSum);
        }
        private void OnDisable()
        {
            mediator.DeleteSubscriber<ShowDiceCount>(ShowDiceSum);
        }

        private void ShowDiceSum(ShowDiceCount count)
        {
            showFeedback.Initialization();
            diceSum = count.diceSum;
            diceobject.SetActive(true);
            showFeedback.PlayFeedbacks();
            diceText.text = diceSum.ToString();
            Invoke("CloseTextObject", delayToClose);
        }

        private void CloseTextObject()
        {
            //diceText.gameObject.SetActive(false);
            closeFeedback.PlayFeedbacks();
        }

        public void PublishDiceCount()
        {
            CloseDicePanelSignal.Trigger();
            DiceCount count = new DiceCount();
            count.diceSum = diceSum;
            mediator.Publish(count);
        }
    }
}
