using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehaviour : MonoBehaviour
{
    public GameObject target;
    public float rotationSpeed;
    public float movementSpeed;
    public Vector3 movementVector;
    private Animator animator;
    public float followRadious;
    public float attackRadious;
    private int layerMask;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        layerMask = (1 << 9); //ignora todas las layers,menos la 9
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(Physics.CheckSphere(transform.position, followRadious, layerMask))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, followRadious,layerMask);
            target = hitColliders[0].gameObject;

        }
        else
        {
            target = null;
        }

        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < attackRadious)
            {
                animator.SetBool("Walking", false);
                Quaternion toRotation = Quaternion.LookRotation(movementVector);

                transform.rotation = toRotation;
                animator.SetBool("Attacking", true);

            }
            else 
            {
                animator.SetBool("Attacking", false);
                movementVector = target.transform.position - transform.position;
                movementVector = movementVector.normalized * movementSpeed;
                transform.position = transform.position + movementVector;


                Quaternion toRotation = Quaternion.LookRotation(movementVector);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);

                animator.SetBool("Walking", true);
            }
            


        }
        else
        {
            animator.SetBool("Walking", false);
        }


    }
}
