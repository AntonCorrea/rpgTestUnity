using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject target;
    //public float rotationSpeed;
    //public float movementSpeed;
    public Vector3 movementVector;
    private Animator animator;
    public float followRadio;
    public float attackRadio;
    public bool attacking;
    public bool walking;
    //public bool getHit;
    private int layerMask;
    public Attribute attributes;
    public GameObject hitBox;
    public bool animationHitBoxActive;
    public ParticleSystem bloodParticles;
    public ParticleSystem slashParticles;
    // Start is called before the first frame update
    Rigidbody rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        attributes = GetComponent<Attribute>();
        layerMask = (1 << 9); //ignora todas las layers,menos la 9
        SetAnimationEvents();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.zero;
        if (Physics.CheckSphere(transform.position, followRadio, layerMask))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, followRadio, layerMask);
            target = hitColliders[0].gameObject;
        }
        else
        {
            target = null;
        }

        if (target != null)
        {
            walking = true;
            movementVector = target.transform.position - transform.position;
            movementVector = movementVector.normalized * attributes.speedMov;

            if (Vector3.Distance(transform.position, target.transform.position) < attackRadio)
            {
                attacking = true;
                walking = false;
            }
            else
            {
                //attacking = false;
                //sobrecargado por isName(attackstop)
            }
        }
        else
        {
            attacking = false;
            walking = false;
        }





        if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackStop") )
        {
            if (target != null)
            {
                attacking = false;
                movementVector = target.transform.position - transform.position;
                movementVector = movementVector.normalized * attributes.speedMov;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, movementVector, 0.3f, 0f);
                transform.rotation = Quaternion.LookRotation(newDirection);

            }
        }


        if (attributes.getHit)
        {

            animator.Play("GetHit",0,0);

            movementVector = (attributes.gotHitBy.transform.position - transform.position);
            transform.rotation = Quaternion.LookRotation(movementVector);
            //transform.Translate(Vector3.forward * -12f);
            //rb.AddForce(movementVector * -500f);
           
            attributes.getHit = false;
            animationHitBoxActive = false;
            attacking = false;
            bloodParticles.Play();
        }
        else
        {
            bloodParticles.Stop();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
        {
            walking = false;
            attacking = false;         
        }

        if (animationHitBoxActive)
        {
            hitBox.SetActive(true);
            slashParticles.Play();

        }
        else
        {
            hitBox.SetActive(false);
            slashParticles.Stop();

        }

        if (attacking)
        {
            walking = false;
            animator.SetBool("Attacking", true);
        }
        else
        {
            animator.SetBool("Attacking", false);
        }

        if (walking)
        {
            Move();
            Rotate();
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }
    void Move()
    {
        transform.position = transform.position + movementVector;
    }

    void Rotate()
    {
        Quaternion toRotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, attributes.speedRot * Time.fixedDeltaTime);
    }

    void SetAnimationEvents()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            AnimationClip clip = clips[i];

            //Debug.Log(c.name + " - " + c.length);

            switch (clip.name)
            {
                case "Attack01":
                    AddEventToClip(clip, "AttackStarted", 12);
                    AddEventToClip(clip, "AttackStopped", 32);
                    break;
                
            }
        }

    }

    void AddEventToClip(AnimationClip aClip, string aFunctionName, float aFrame)
    {
        AnimationEvent ev = new AnimationEvent();
        ev.functionName = aFunctionName;
        //ev.intParameter = aIntParameter;
        ev.time = aFrame / aClip.frameRate;
        aClip.AddEvent(ev);
    }

    public void AttackStarted()
    {
        animationHitBoxActive = true;
    }
    public void AttackStopped()
    {
        animationHitBoxActive = false;
    }
}
