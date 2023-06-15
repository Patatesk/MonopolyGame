using Save;
using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections;

namespace PK
{
    public class SoundAndHaticSttings : MonoBehaviour, ISaveble
    {
        [SerializeField] private GameObject soundOn;
        [SerializeField] private GameObject soundOff;
        [SerializeField] private GameObject hapticOn;
        [SerializeField] private GameObject hapticOff;
        [SerializeField] private MMF_Player hapticOnSet;
        [SerializeField] private MMF_Player hapticOffSet;

        private int sound = 0;
        private int haptic = 0;


        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (sound == 0)
            {
                soundOff.SetActive(true);
                soundOn.SetActive(false);
            }
            else if (sound == 1)
            {
                soundOff.SetActive(false);
                soundOn.SetActive(true);
            }

            if (haptic == 0)
            {
                hapticOff.SetActive(true);
                hapticOn.SetActive(false);
            }
            else if (haptic == 1)
            {
                hapticOff.SetActive(false);
                hapticOn.SetActive(true);
            }
        }
        public void SoundOnButton()
        {
            if (sound == 0)
            {
                sound = 1;
                soundOff.SetActive(false);
                soundOn.SetActive(true);
                SoundChange();
            }
            else if (sound == 1)
            {
                sound = 0;
                soundOff.SetActive(true);
                soundOn.SetActive(false);
                SoundChange();
            }
        }


        public void HapticOnButton()
        {

            if (haptic == 0)
            {
                haptic = 1;
                hapticOff.SetActive(false);
                hapticOn.SetActive(true);
                HapticChange();
            }
            else if (haptic == 1)
            {
                haptic = 0; HapticChange();

                hapticOff.SetActive(true);
                hapticOn.SetActive(false);
            }
        }

        private void HapticChange()
        {
            if (haptic == 1) hapticOnSet.PlayFeedbacks();
            else hapticOffSet.PlayFeedbacks();
        }

        private void SoundChange()
        {
            AudioListener.volume = sound;
        }

        public object SaveState()
        {
            return new SaveSettingsData { sound = sound, _haptic = haptic };
        }

        public void LoadState(object state)
        {
            var loadedData = (SaveSettingsData)state;
            StartCoroutine(LateLoad(loadedData));

        }

        private IEnumerator LateLoad(SaveSettingsData loadedData)
        {
            yield return new WaitForSeconds(2);
            sound = loadedData.sound;
            haptic = loadedData._haptic;
            HapticChange();
            SoundChange();
            Initialize();
        }
    }
    [System.Serializable]
    public struct SaveSettingsData
    {
        public int sound;
        public int _haptic;
    }
}
