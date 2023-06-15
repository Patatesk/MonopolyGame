using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using System;
using TMPro;

namespace PK
{
    public class UIContoller : MonoBehaviour
    {
        [SerializeField] private GameObject dicePanel;
        [SerializeField] private GameObject inGameButtons;
        [SerializeField] private GameObject jailPopUp;
        [SerializeField] private GameObject TaxToJailPopUp;
        [SerializeField] private GameObject helpPopup;
        [SerializeField] private GameObject helpButton;
        [SerializeField] private GameObject finishPanel;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI jailCountDowmText;
        private Mediator mediator;

        private void Awake()
        {
            mediator = GameObject.FindObjectOfType<Mediator>();
        }

        private void OnEnable()
        {
            CloseDicePanelSignal.CloseDicePanel += CloseDice;
            mediator.Subscribe<UpdateMoney>(UpdateMoneyText);
            mediator.Subscribe<MovemantEnded>(ShowInGameButtons);
            InGameButtonsOpenCloseSignals.Open += ShowInGameButtons;
            InGameButtonsOpenCloseSignals.Close += CloseInGameButtons;
            JailStartedSignals.JailStart += StartJail;
            TaxToJailSignal.TaxToJail += TaxToJail;
            ShowHelpSignal.ShowHelp += ShowHelpPopUp;
            CloseHelpSignal.CloseHelp += CloseHelp;
            FinishPanelSignal.FinishPanel += OpenFinishPanel;
        }
        private void OnDisable()
        {
            CloseDicePanelSignal.CloseDicePanel -= CloseDice;
            mediator.DeleteSubscriber<UpdateMoney>(UpdateMoneyText);
            InGameButtonsOpenCloseSignals.Open -= ShowInGameButtons;
            InGameButtonsOpenCloseSignals.Close -= CloseInGameButtons;
            JailStartedSignals.JailStart -= StartJail;
            ShowHelpSignal.ShowHelp -= ShowHelpPopUp;
            CloseHelpSignal.CloseHelp -= CloseHelp;
            FinishPanelSignal.FinishPanel -= OpenFinishPanel;
            TaxToJailSignal.TaxToJail -= TaxToJail;
        }

        private void StartJail(bool start)
        {
            jailPopUp.SetActive(start);
            MovemantEnded emptyParameter = new MovemantEnded();
            if (!start)
            {
                this.StopAllCoroutines();

                ShowInGameButtons(emptyParameter);
            }
            else
            {
                StartCoroutine(JailCountDown());
                CloseInGameButtons(emptyParameter);
            }
        }
        private void ShowInGameButtons(MovemantEnded movemantData)
        {
            if(movemantData.showDice)
            inGameButtons.SetActive(true);
        }
        private void CloseInGameButtons(MovemantEnded movemantData = null)
        {
            inGameButtons.SetActive(false);
        }
        private void TaxToJail()
        {
            TaxToJailPopUp.SetActive(true);
            CloseInGameButtons();
        }
        private void UpdateMoneyText(UpdateMoney updateData)
        {
            moneyText.text = updateData.money.ToString();
        }
        private void CloseDice()
        {
            dicePanel.SetActive(false);
        }

        private void OpenFinishPanel()
        {
            finishPanel.SetActive(true);
            finishPanel.transform.GetChild(0).GetComponent<MMF_Player>().PlayFeedbacks();
        }
        private void ShowHelpPopUp()
        {
            helpPopup.SetActive(true);
        }
        
        private void CloseHelp()
        {
            helpPopup.SetActive(false);
            helpButton.SetActive(false);
        }
        IEnumerator JailCountDown()
        {
            int count = 60;
            jailCountDowmText.text = count.ToString();
            while (true)
            {
                yield return new WaitForSecondsRealtime(1);
                count--;
                if (count == 0)
                {
                    StartJail(false);
                    break;
                }
                jailCountDowmText.text = count.ToString();
            }
        }
    }


    public class CloseDicePanelSignal
    {
        public static event Action CloseDicePanel;
        public static void Trigger()
        {
            CloseDicePanel?.Invoke();
        }
    }

    public class InGameButtonsOpenCloseSignals
    {
        public static event Action<MovemantEnded> Open;
        public static event Action<MovemantEnded> Close;

        public static void TriggerOpen(MovemantEnded value)
        {
            Open?.Invoke(value);
        }

        public static void TriggerClose(MovemantEnded value)
        {
            Close?.Invoke(value);
        }
    }

    public class JailStartedSignals
    {
        public static event Action<bool> JailStart;
        public static void Trigger(bool start)
        {
            JailStart?.Invoke(start);
        }
    }

    public class TaxToJailSignal
    {
        public static event Action TaxToJail;
        public static void Trigger()
        {
            TaxToJail?.Invoke();
        }
    }

    public class ShowHelpSignal
    {
        public static event Action ShowHelp;
        public static void Trigger()
        {
            ShowHelp?.Invoke();
        }
    }
    public class FinishPanelSignal
    {
        public static event Action FinishPanel;
        public static void Trigger()
        {
            FinishPanel?.Invoke();
        }
    }
    public class CloseHelpSignal
    {
        public static event Action CloseHelp;
        public static void Trigger()
        {
            CloseHelp?.Invoke();
        }
    }
    
}
