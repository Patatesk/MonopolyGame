using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Save;

namespace PK
{
    public class GetAllSaveID : MonoBehaviour
    {
        [ContextMenu("GetIDs")]
        private void GetIdS()
        {
            SavebleEntity[] entitys = GameObject.FindObjectsOfType<SavebleEntity>();

            foreach (SavebleEntity entity in entitys)
            {
                entity.GenerateID();
            }
        }
    }
}
