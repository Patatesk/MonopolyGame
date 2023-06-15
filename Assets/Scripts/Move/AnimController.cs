using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class AnimController : MonoBehaviour
    {
        [SerializeField] private Animator animator;


        public void Idle()
        {
            animator.SetBool("JumpStart",false);
            animator.SetBool("JumpEnd",true);
        }

        public void Jump()
        {
            animator.SetBool("JumpStart",true);
            animator.SetBool("JumpEnd", false);

        }
        public void Happy()
        {
            animator.SetTrigger("Happy");
        }
    }
}
