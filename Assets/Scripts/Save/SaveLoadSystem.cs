using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PK;
using UnityEngine.SceneManagement;

namespace Save
{
    public class SaveLoadSystem : MonoBehaviour
    {
        public string savePath => $"{Application.persistentDataPath}/save.txt";
        public GameObject startScreen;

        private void Awake()
        {
            AudioListener.volume = 0;
            startScreen.SetActive(true);
            StartCoroutine(StartUp());
            Load();
        }




        IEnumerator StartUp()
        {
            yield return new WaitForSeconds(2);
            AudioListener.volume = 1;
            startScreen.SetActive(false);
            
        }

        private void OnEnable()
        {
            SaveLoadSignals.Save += Save;
            SaveLoadSignals.Load += Load;
            SaveLoadSignals.Delete += delete;
        }

        private void OnDisable()
        {
            SaveLoadSignals.Save -= Save;
            SaveLoadSignals.Load -= Load;
            SaveLoadSignals.Delete -= delete;

        }

        private void OnApplicationPause(bool pause)
        {
            if (pause) Save();
        }
        
        private void OnApplicationQuit()
        {
            Save();
        }

        [ContextMenu("Save")]

        void Save()
        {
            var state = LoadFile();
            SaveState(state);
            SaveFile(state);
            Debug.Log("saved");
        }


        [ContextMenu("Load")]

        void Load()
        {
            var state = LoadFile();
            LoadState(state);
        }

        public void SaveFile(object state)
        {
            using (var stream = File.Open(savePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }
        [ContextMenu("Delete")]
        private void delete()
        {
            File.Delete(savePath);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        

        Dictionary<string, object> LoadFile()
        {
            if (!File.Exists(savePath))
            {
                Debug.Log("File cannot found");
                return new Dictionary<string, object>();
            }


            using (FileStream stream = File.Open(savePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                stream.Position = 0;
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }
        void SaveState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SavebleEntity>())
            {
                state[saveable.Id] = saveable.SaveState();
            }
        }
        void LoadState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SavebleEntity>())
            {
                if (state.TryGetValue(saveable.Id, out object savedState))
                {
                    saveable.LoadState(savedState);
                }
            }
        }


    }
}
