using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PK
{
    public class DiceThrow : MonoBehaviour
    {
        [SerializeField] private Transform diceParent;
        [SerializeField] private Transform diceStartPoint;
        [SerializeField] private int maxTorq;
        public UnityEvent feedback;
        private Mediator mediator;

        private Rigidbody dice1RB;
        private Rigidbody dice2RB;

        private Vector3 startPos1;
        private Vector3 startPos2;

        private Quaternion startRot1;
        private Quaternion startRot2;

        private void Awake()
        {
            dice1RB = diceParent.GetChild(0).GetComponent<Rigidbody>();
            dice2RB = diceParent.GetChild(1).GetComponent<Rigidbody>();
            startPos1 = dice1RB.transform.localPosition;
            startPos2 = dice2RB.transform.localPosition;
            startRot1 = dice1RB.transform.localRotation;
            startRot2 = dice2RB.transform.localRotation;
            mediator = GameObject.FindObjectOfType<Mediator>();
        }
        private void OnEnable()
        {
            mediator.Subscribe<MovemantEnded>(ResetPos);
        }
        private void OnDisable()
        {
            mediator.DeleteSubscriber<MovemantEnded>(ResetPos);
        }
        public void PushDices()
        {
            int multiplayer = 1;
            Vector3 hack = Vector3.zero;
            if (!OnBoardingProceses.isFirstLoad)
            {
                multiplayer = 0;
                hack = Vector3.one*100;
            }
            dice1RB.isKinematic = false;
            dice2RB.isKinematic = false;
            dice1RB.AddTorque((GetRandomVector()*multiplayer) + hack);
            dice1RB.AddForce(300 * Vector3.up);
            dice2RB.AddTorque((GetRandomVector() * multiplayer) + hack);
            dice2RB.AddForce(300 * Vector3.up);
            ChoosedSpawnPointSignal.Trigger();
            feedback.Invoke();
            SaveLoadSignals.Signal_Save();

        }

        public void ResetPos(MovemantEnded movemant)
        {
            dice1RB.isKinematic = true;
            dice2RB.isKinematic = true;
            dice1RB.transform.localPosition = startPos1;
            dice2RB.transform.localPosition = startPos2;
            if (!OnBoardingProceses.isFirstLoad)
            {
                dice1RB.transform.rotation = startRot1;
                dice2RB.transform.rotation = startRot2;
            }
            else
            {
                dice1RB.transform.rotation = Quaternion.Euler(GetRandomVector());
                dice2RB.transform.rotation = Quaternion.Euler(GetRandomVector());
            }
        }

        private Vector3 GetRandomVector()
        {
            float x = Random.Range(0, maxTorq);
            float y = Random.Range(0, maxTorq);
            float z = Random.Range(0, maxTorq);

            return new Vector3(x, y, z);
        }

    }
}
