using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace PK
{
    public class TileCanvasController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI prizeText;
        [SerializeField] private Button buyButton;
        [SerializeField] private GameObject canvas;
        private Mediator mediator;
        private Tile Tile;


        private void Awake()
        {
            Tile = GetComponent<Tile>();
            mediator = GameObject.FindObjectOfType<Mediator>();
        }
        private void Start()
        {
            SetCanvas();
        }

        private void OnEnable()
        {
            mediator.Subscribe<UpdateMoney>(ControlAndSet);
        }

        private void OnDisable()
        {
            mediator.DeleteSubscriber<UpdateMoney>(ControlAndSet);

        }

        private void SetCanvas()
        {
            prizeText.text = Tile.price.ToString();
            buyButton.interactable = true;
        }

        public void ShowCanvas()
        {
            if (Tile.isSold) return;
            canvas.SetActive(true);
        }
        public void CloseCanvas()
        {
            canvas.SetActive(false);
        }

        private void ControlAndSet(UpdateMoney updateData)
        {
            if(Tile.price< updateData.money)
            {
                prizeText.color = Color.white;
                buyButton.interactable = true;
            }
            else
            {
                prizeText.color = Color.red;
                buyButton.interactable = false;
            }
        }

    }
}
