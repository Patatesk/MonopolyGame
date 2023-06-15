using TMPro;
using UnityEngine;

namespace PK
{
    public class HousePriceUpdate : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI priceText;
        
        private Mediator mediator;

        private void Awake()
        {
            mediator = GameObject.FindAnyObjectByType<Mediator>();
        }

        private void OnEnable()
        {
            mediator.Subscribe<HousePrice>(UpdatePrice);
        }

        private void OnDisable()
        {
            mediator.DeleteSubscriber<HousePrice>(UpdatePrice);

        }

        private void UpdatePrice(HousePrice price)
        {
            priceText.text = price.price.ToString();
        }

    }
}
