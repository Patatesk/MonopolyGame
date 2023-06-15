using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PK
{
    public class BuyGomeButtonAndTextUpdater : MonoBehaviour
    {
        private Mediator Mediator;
        private BuyHomeManager manager;

        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Button buyHouseButton;

        private void Awake()
        {
            Mediator = GameObject.FindObjectOfType<Mediator>(); 
            manager = GetComponent<BuyHomeManager>();
        }
        private void OnEnable()
        {
            Mediator.Subscribe<UpdateMoney>(UpdateButtonAndText);
        }
        private void OnDisable()
        {
            Mediator.DeleteSubscriber<UpdateMoney>(UpdateButtonAndText);
        }

        private void UpdateButtonAndText(UpdateMoney updateData)
        {
            if (priceText == null || buyHouseButton == null)
            {
                Debug.Log("Text ve Button Ata", this);
                return;
            }
            if (updateData.money < manager.housePrice)
            {
                priceText.color = Color.red;
                buyHouseButton.interactable = false;
            }
            else
            {
                priceText.color = Color.white;
                buyHouseButton.interactable = true;
            }
        }
    }
}
