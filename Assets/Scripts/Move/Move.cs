using System;
using UnityEngine;
using DG.Tweening;
using Save;
using System.Linq.Expressions;
using System.Collections;

namespace PK
{
    public class Move : MonoBehaviour
    {
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private TileCatcher tileCatcher;
        [SerializeField] private float moveHeight;
        [SerializeField] private float moveTime;
        [SerializeField] private int pathResuliton = 10;
        [SerializeField] private GameObject moveParticle;
        [SerializeField] private float startMoveDelay;
        [SerializeField] private CaeraKontrol cameraControl;

        private Parabol parabol;
        private Mediator mediator;
        private int moveCount;
        private int dice;
        private AnimController animController;


        public Transform nextBoard;
        public Transform currentBoard;

        private void Awake()
        {
            mediator = GameObject.FindObjectOfType<Mediator>();
            animController = GetComponent<AnimController>();
            parabol = new Parabol();
            startMoveDelay += (cameraControl.zoomInDelay + 0.5f);
        }
        private void PublisTile()
        {
            MovemantEnded ended = new MovemantEnded();
            ended.currentTile = tileCatcher.tile.tileIndex;
            mediator.Publish(ended);
        }
        private void OnEnable()
        {
            mediator.Subscribe<DiceCount>(StartMove);
            MoveTheTileSignal.MoveToTheTile += MoveToTheTile;
        }

        private void OnDisable()
        {
            MoveTheTileSignal.MoveToTheTile -= MoveToTheTile;
            mediator.DeleteSubscriber<DiceCount>(StartMove);
        }

        void Start()
        {
            parabol.pointCount = pathResuliton;
            parabol.height = moveHeight;
            Invoke("PublisTile", 1f);
        }

        private void StartMove(DiceCount count)
        {
            StartCoroutine(MovePlayer(count));
            SaveLoadSignals.Signal_Save();
        }

        private IEnumerator MovePlayer(DiceCount count)
        {
            yield return new WaitForSeconds(startMoveDelay);
            if (tileCatcher.tile != null) tileCatcher.tile.CloseCanvas();
            dice = count.diceSum;
            moveParticle.SetActive(true);
            Mover(nextBoard, moveHeight);
        }

        private void Mover(Transform _nextBoard, float _heigh)
        {
            transform.parent = null;
            parabol.height = _heigh;
            animController.Jump();
            transform.DORotateQuaternion(_nextBoard.rotation, moveTime);
            transform.DOPath(parabol.Calculate(currentBoard.position + positionOffset, _nextBoard.position + positionOffset), moveTime, PathType.Linear, PathMode.Full3D).SetEase(Ease.Linear).OnComplete(MoveCounter);
        }

        private void MoveCounter()
        {
            moveCount++;
            if (dice <= moveCount)
            {
                tileCatcher.tile.CloseIndicator();
                moveCount = 0;
                Invoke("PerformTile", 0.2f);
                moveParticle.SetActive(false);
                animController.Idle();
                Invoke("PublisTile", .2f);
                SaveLoadSignals.Signal_Save();
                animController.Idle();
                return;
            }
            Mover(nextBoard, moveHeight);
        }

        private void PerformTile()
        {
            if (tileCatcher.tile == null) return;
            if(tileCatcher.tile is HouseTile)  if (tileCatcher.tile.returnSpawnPoint.returnUpgradeHome != null) animController.Happy();
            tileCatcher.tile.PerformTileAction();
            if (OnBoardingProceses.isFirstLoad) tileCatcher.tile.ShowCanvas();
            else StartCoroutine(Tutorial());
            Invoke("MakeChild", 0.2f);

        }
       IEnumerator Tutorial()
        {
            yield return new WaitForSeconds(.5f);
            tileCatcher.tile.ShowCanvas();
        }
        private void MakeChild()
        {
            transform.parent = tileCatcher.tile.transform.GetChild(0);
        }

        private void MoveToTheTile(Transform nextboard, float heigh)
        {
            dice = 1;
            Mover(nextboard, heigh);
        }


    }
    public class MoveTheTileSignal
    {
        public static event Action<Transform, float> MoveToTheTile;
        public static void Trigger(Transform nextTile, float height)
        {
            MoveToTheTile?.Invoke(nextTile, height);
        }
    }
    public class MovemantEnded : ICommand
    {
        public int currentTile;
        public bool showDice = true;

    }
}
