using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footmanAnim : MonoBehaviour
{
    private Animator animator;
    private movement movement;
    private combat combat;
    float maxMovement;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<movement>();
        combat = GetComponent<combat>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        combat.animatorClip = animator.GetCurrentAnimatorStateInfo(2);

        maxMovement = Mathf.Max(Mathf.Abs(movement.movementVector.x) , Mathf.Abs(movement.movementVector.z)) * 10f ;
        animator.SetFloat("BlendMovement", maxMovement);
        animator.SetBool("Defend", combat.isDeffending);
        animator.SetInteger("AttackNumber", combat.attackNumber);

        
    }
}
