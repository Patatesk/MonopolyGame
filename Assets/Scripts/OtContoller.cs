using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace PK
{
    public class OtContoller : MonoBehaviour
    {
        [SerializeField] private GameObject ot;
        [SerializeField] private MMF_Player player;
        [SerializeField] private Vector2 timeInterval;
        [SerializeField] private Transform[] spawnPoints;


        private Transform spawnPoint;
        private float nextTime;
        private void Start()
        {
            SetNexTime();
        }
        public void SetNexTime()
        {
            nextTime = Random.Range(timeInterval.x,timeInterval.y);
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            StartCoroutine(PlayFeedBacks());
        }
        
        IEnumerator PlayFeedBacks()
        {
            yield return new WaitForSeconds(nextTime);
            ot.transform.position = spawnPoint.position;
            ot.transform.rotation = spawnPoint.rotation;
            player.Initialization();
            player.PlayFeedbacks();
        }


    }
}
