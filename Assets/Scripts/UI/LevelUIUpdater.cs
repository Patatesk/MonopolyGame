using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


namespace PK
{
    public class LevelUIUpdater : MonoBehaviour
    {
        [SerializeField] private Slider levelSlider;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI totalHotelText;

        private void Start()
        {
            Set();
        }
        private void Set()
        {
            levelSlider.maxValue = LevelManager.Instance.levelPassRequeirments[LevelManager.Instance.level];
            levelSlider.wholeNumbers = true;
            levelText.text = (LevelManager.Instance.level + 1).ToString();
            totalHotelText.text = LevelManager.Instance.activeHotelCount.ToString() + "/" + levelSlider.maxValue;
            levelSlider.value = LevelManager.Instance.activeHotelCount;
        }

        private void OnEnable()
        {
            UpdateSliderSignal.UpdateSlider += UpdateSlider;
        }
        private void OnDisable()
        {
            UpdateSliderSignal.UpdateSlider -= UpdateSlider;
        }

        private void UpdateSlider(int value)
        {
            levelSlider.value += value;
            totalHotelText.text = LevelManager.Instance.activeHotelCount.ToString() + "/" + levelSlider.maxValue;
        }
        [ContextMenu("AddHotel")]
        private void AddHotelTest()
        {
            AddHotelSignal.Trigger(1);
        }
        [ContextMenu("REmove")]
        private void REmove()
        {
            AddHotelSignal.Trigger(-1);
        }
    }

    public class UpdateSliderSignal
    {
        public static event Action<int> UpdateSlider;
        public  static void Trigger(int value)
        {
            UpdateSlider?.Invoke(value);
        }
    }
}
