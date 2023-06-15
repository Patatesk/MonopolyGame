using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PK
{
    public class AsycronLoader : MonoBehaviour
    {
        public void Start()
        {
            StartCoroutine(LoadLevelAsync());
        }

        private IEnumerator LoadLevelAsync()
        {
            var progress = SceneManager.LoadSceneAsync("Game");
            progress.allowSceneActivation = false;
            while (!progress.isDone)
            {

                yield return null;
                if (progress.progress >= 0.8)
                {
                    yield return new WaitForSeconds(1);
                    progress.allowSceneActivation = true;
                }
            }

            Debug.Log("Scene Loaded");

        }


    }
}
