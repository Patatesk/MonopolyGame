using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Save;
using System;

namespace PK
{
    public class LevelManager : MonoBehaviour, ISaveble
    {
        public static LevelManager Instance { get; private set; }
        public int level;
        public int activeHotelCount;
        public int[] levelPassRequeirments;
        private void Awake()
        {

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            activeHotelCount= 0;

        }

      

        private void OnEnable()
        {
            AddHotelSignal.AddHotel += AddHotel;
        }
        private void OnDisable()
        {
            AddHotelSignal.AddHotel -= AddHotel;
        }
        private void AddHotel(int value)
        {
            activeHotelCount += value;
            UpdateSliderSignal.Trigger(value);
            if(activeHotelCount >= levelPassRequeirments[level])
            {
                level++;
                if (level > levelPassRequeirments.Length) level = 0;
                activeHotelCount= 0;
                UpdateSliderSignal.Trigger(value);
                FinishPanelSignal.Trigger();
                SaveLoadSignals.Signal_Save();
            }
        }

        public object SaveState()
        {
            return new SaveLevelData { _level = this.level };
        }

        public void LoadState(object state)
        {
            var loadedData = (SaveLevelData)state;
            level = loadedData._level;
            if (level == SceneManager.GetActiveScene().buildIndex) return;
            SceneManager.LoadScene(level+1);
        }
    }
    [Serializable]
    public struct SaveLevelData
    {
        public int _level;
    }

    public class AddHotelSignal
    {
        public static event Action<int> AddHotel;
        public static void Trigger(int count)
        {
            AddHotel?.Invoke(count);
        }
    }

    public class InýtLevelManager
    {
        public static event Action Inýt;
        public static void Trigger()
        {
            Inýt?.Invoke();
        }
    }
}
