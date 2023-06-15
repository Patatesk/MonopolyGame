using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PK
{
    public class HouseSnapperCanvasController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Button buyButton;
        [SerializeField] private GameObject canvas;
        public int price;


        private Mediator mediator;
        private void Awake()
        {
            mediator = GameObject.FindObjectOfType<Mediator>();
        }
        private void Start()
        {
                //priceText.text = price.ToString();

        }

        private void OnEnable()
        {
            mediator.Subscribe<UpdateMoney>(CheckEnoughMoney);
        }
        private void OnDisable()
        {
            mediator.DeleteSubscriber<UpdateMoney>(CheckEnoughMoney);
        }

        public void ShowCanvas()
        {
            canvas.SetActive(true);
        }
        public void CloseCanvas()
        {
            canvas.SetActive(false);
        }
        

        private void CheckEnoughMoney(UpdateMoney money)
        {
            if(price > money.money)
            {
                priceText.color = Color.red;
                buyButton.interactable = false;
            }
            else { priceText.color = Color.white; buyButton.interactable = true; }
        }

       
    }
}
