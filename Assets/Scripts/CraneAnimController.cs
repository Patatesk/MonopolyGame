using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class CraneAnimController : MonoBehaviour
    {
        
        [SerializeField] private Vector2 AnimRepeatInterval;

        private Animator animator;
        private float animTime;
        private void Awake()
        {
            animator = GetComponent<Animator>();    
        }
        private void Start()
        {
            SetAnimTime();
        }
        private void SetAnimTime()
        {
            animTime = Random.Range(AnimRepeatInterval.x,AnimRepeatInterval.y);
            StartCoroutine(TriggerAnim());
        }
        IEnumerator TriggerAnim()
        {
            yield return new WaitForSeconds(animTime);
            animator.SetTrigger("Play");
            SetAnimTime();
        }
    }
}
