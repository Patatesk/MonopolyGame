using UnityEngine;
using Save;
using System.Collections.Generic;
using System.Collections;
using System;
using MoreMountains.Feedbacks;

namespace PK
{
    public class SaveHouse : MonoBehaviour,ISaveble
    {
        private List<LoadHouseData> toLoad = new List<LoadHouseData>();
        private Mediator mediator;
        private int currentTileIndex;
        [SerializeField] private GameObject shack, house, apartmant, hotel;
        [SerializeField] private HashSet<HouseSpawnPoint> spawnPoints = new HashSet<HouseSpawnPoint>();
        [SerializeField] private HashSet<Tile> allTiles = new HashSet<Tile>();
        [SerializeField] private HouseSpawnPoint[] spawnPointsAsArray;
        [SerializeField] private Tile[] tileAsArray;
        [SerializeField] private Vector3 playerSpawnOffset;



        [ContextMenu("GetAllSpawnPoints")]
        private void GetAllSpawnPoints()
        {
            spawnPointsAsArray = GameObject.FindObjectsOfType<HouseSpawnPoint>();
            tileAsArray = GameObject.FindObjectsOfType<Tile>();
        }
        private void Awake()
        {
            mediator = GameObject.FindObjectOfType<Mediator>();
            spawnPoints.Clear();
            allTiles.Clear();
            foreach (HouseSpawnPoint point in spawnPointsAsArray)
            {
                spawnPoints.Add(point);
            } foreach (Tile tile in tileAsArray)
            {
                allTiles.Add(tile);
            }
        }
        private void Start()
        {
            StartCoroutine(WaitInýt());
        }
        private IEnumerator WaitInýt()
        {
            yield return new WaitForSeconds(1.1f);
            Load();
        }

        private void GetCurrentTile(MovemantEnded _currentTile)
        {
            currentTileIndex = _currentTile.currentTile;
        }
        private void OnEnable()
        {
            AddLoadDataSignal.AddData += AddDataTolist;
            mediator.Subscribe<MovemantEnded>(GetCurrentTile);
        }

        private void OnDisable()
        {

            AddLoadDataSignal.AddData -= AddDataTolist;
            mediator.DeleteSubscriber<MovemantEnded>(GetCurrentTile);
        }
        private void AddDataTolist(LoadHouseData data)
        {
            toLoad.Add(data);
        }
        private Tile CheckTileIndex(int index)
        {
            Tile spawnTile = null;
            foreach (Tile tile in allTiles)
            {
                if(index == tile.tileIndex)
                {
                    spawnTile = tile;
                    break;
                }
            }
            return spawnTile;
        }

        private void Load()
        {
            foreach (LoadHouseData data in toLoad)
            {
                GameObject houseToLoad = CheckHouseForInstantiate(data._type);
                GameObject loadedHouse = Instantiate(houseToLoad);
                HouseSpawnPoint point = FindSpawnPoint(data.spawnPoint);
                point.SnapPosition(loadedHouse.transform);
                if (data._type == 0)
                {
                    MMF_Player buyFeedbacks = loadedHouse.GetComponent<UpgradeHomeCheck>().buyFeedbacks;
                    buyFeedbacks.Initialization();
                    buyFeedbacks.PlayFeedbacks();
                }
                else loadedHouse.GetComponent<House>().PlayUpgradeFeedBacks();
            }
        }

        private HouseSpawnPoint FindSpawnPoint(int spawnerIndex)
        {
            HouseSpawnPoint Cheking = null;
            foreach (HouseSpawnPoint point in spawnPoints)
            {
                if(spawnerIndex == point.spawnerIndex)
                {
                    Cheking = point;
                    break;
                }
            }
            return Cheking;
        }

        private GameObject CheckHouseForInstantiate( int type)
        {
            GameObject choosedHouse = null;
            switch (type)
            {
                case 0:
                    choosedHouse = shack;
                    break;
                case 1:
                    choosedHouse = house;
                    break;
                case 2:
                    choosedHouse = apartmant;
                    break;
                case 3:
                    choosedHouse = hotel;
                    break;
            }
            return choosedHouse;
        }

        public object SaveState()
        {
            return new SavePlayerLastPos { lastTileIndex = currentTileIndex };
        }

        public void LoadState(object state)
        {
            var loadedData = (SavePlayerLastPos)state;
            StartCoroutine(LateLoad(loadedData));
        }

        private IEnumerator LateLoad(SavePlayerLastPos loadedData)
        {
            yield return new WaitForSeconds(0.5f);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Tile _tile = CheckTileIndex(loadedData.lastTileIndex);
            Transform spawnTransform = _tile.transform;
            player.transform.position = spawnTransform.position + playerSpawnOffset;
            player.transform.rotation = spawnTransform.rotation;
            _tile.ShowCanvas();
        }
    }

    public class AddLoadDataSignal
    {
        public static event Action<LoadHouseData> AddData;
        public static void Trigger(LoadHouseData  data)
        {
            AddData?.Invoke(data);
        }
    }
    [Serializable]
    public struct SavePlayerLastPos
    {
        public int lastTileIndex;
    }
    [Serializable]
    public class LoadHouseData
    {
        public int spawnPoint;
        public int _type;
    }
}
