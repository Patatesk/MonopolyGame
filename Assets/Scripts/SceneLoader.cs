using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PK
{
    public class SceneLoader : MonoBehaviour
    {
       public void LoadNextScene()
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            if (SceneManager.sceneCountInBuildSettings > index + 1)
            {
                SceneManager.LoadScene(index + 1);
            }
            else SceneManager.LoadScene(1);
        }
    }
}
