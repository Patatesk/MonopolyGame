using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class PedestrianManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] prefaps;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private Vector2 spawnInterval;

        private GameObject nextSpawnObject;
        private float nextSpawnTime;
        private Transform nextSpawnPoint;

        private void Start()
        {
            SetNext();
        }



        private void SetNext()
        {
            float randomTime = Random.Range(spawnInterval.x, spawnInterval.y);
            int randomPrefab = Random.Range(0,2);
            int randomSpawn = Random.Range(0,2);
            nextSpawnObject = prefaps[randomPrefab];
            nextSpawnTime = randomTime;
            nextSpawnPoint = spawnPoints[randomSpawn];
            StartCoroutine(SpawnPedestrian());
        }



        IEnumerator SpawnPedestrian()
        {
            yield return new WaitForSeconds(nextSpawnTime);
            GameObject obj = Instantiate(nextSpawnObject,nextSpawnPoint.GetChild(0).position,nextSpawnPoint.GetChild(0).rotation);
            Pedestrian pedestrian= obj.GetComponent<Pedestrian>();
            pedestrian.targetDesti = nextSpawnPoint.GetChild(1);
            SetNext();

        }
    }
}
