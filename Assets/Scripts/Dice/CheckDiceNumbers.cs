using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class CheckDiceNumbers : MonoBehaviour
    {
        private int _dice1;
        private int _dice2;
        private Mediator _mediator;

        private bool done = false;
        private void Awake()
        {
            _mediator = GameObject.FindObjectOfType<Mediator>();
        }
        private int dice1
        {
            set
            {
                _dice1 = value;
                if(_dice2 != 0)
                {
                    ShowDiceCount diceCount = new ShowDiceCount();
                    diceCount.diceSum = _dice1 + _dice2;
                    _mediator.Publish(diceCount);
                    done = true;
                }
            }
        }
        private int dice2
        {
            set
            {
                _dice2 = value;
                if (_dice1 != 0)
                {
                    ShowDiceCount diceCount = new ShowDiceCount();
                    diceCount.diceSum = _dice1 + _dice2;
                    _mediator.Publish(diceCount);
                    done = true;
                }
            }
        }

        private Rigidbody d1RB;
        private Rigidbody d2RB;

        private void OnEnable()
        {
            _mediator.Subscribe<MovemantEnded>(ResetChecker);
        }

        private void OnDisable()
        {
            _mediator.DeleteSubscriber<MovemantEnded>(ResetChecker);
        }

        private void ResetChecker(MovemantEnded MoveEnded)
        {
            _dice1 = 0;
            _dice2 = 0;
            done = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.parent.name == "DiceBlue")
            {
                d1RB = other.transform.parent.parent.GetComponent<Rigidbody>();
            }
            if (other.transform.parent.parent.name == "DiceRED")
            {
                d2RB = other.transform.parent.parent.GetComponent<Rigidbody>();
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.transform.parent.parent.name == "DiceBlue")
            {
                if (d1RB == null || done) return;
                if (d1RB.velocity == Vector3.zero)
                {
                    dice1 = int.Parse(other.tag);
                }
            }
            if (other.transform.parent.parent.name == "DiceRED")
            {
                if (d2RB == null || done) return;
                if (d2RB.velocity == Vector3.zero)
                {
                    dice2 = int.Parse(other.tag);
                }
            }
        }
    }

    public class DiceCount : ICommand
    {
        public int diceSum;
    }

    public class ShowDiceCount: ICommand
    {
        public int diceSum;
    }
}
