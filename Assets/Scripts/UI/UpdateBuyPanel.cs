using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace PK
{
    public class UpdateBuyPanel : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private Button buyButton;
        [SerializeField] private Tile tile;

        private Mediator Mediator;

        private void Awake()
        {
            Mediator = GameObject.FindObjectOfType<Mediator>();
        }


        private void OnEnable()
        {
            Mediator.Subscribe<UpdateMoney>(UpdatePanel);
        }

        private void OnDisable()
        {
            Mediator.DeleteSubscriber<UpdateMoney>(UpdatePanel);
        }

        private void UpdatePanel(UpdateMoney updateData)
        {
            Debug.Log("Panel Updated");
            if(updateData.money < tile.price)
            {
                moneyText.color = Color.red;
                buyButton.interactable = false;
            }
            else
            {
                moneyText.color= Color.white;
                buyButton.interactable = true;
            }
        }
    }
}
