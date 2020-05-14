using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 movementVector;
    public float HorAxis,VerAxis;
    public Attribute attributes;
    //public float rotationSpeed;
    //public float movementSpeed;
    //public float movementBoost;
    //public Rigidbody rb;


    // Update is called once per frame

    public bool isAttacking;
    public bool isDeffending;

    public int attackNumber = 0;
    bool incrementDone = false;

    

    public bool ActivateHitbox;
    //public bool animationGetHit;
    public Animator animator;
    public GameObject hitBox;
    public ParticleSystem slashParticles;
    public ParticleSystem bloodParticles;
    public ParticleSystem shieldParticles;
    Rigidbody rb;
    private void Start()
    {
        attributes = GetComponent<Attribute>();
        animator = GetComponent<Animator>();
        SetAnimationEvents();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        rb.velocity = Vector3.zero;
        HorAxis = Input.GetAxis("Horizontal");
        VerAxis = Input.GetAxis("Vertical");

        if (Mathf.Abs(HorAxis) > 0 || Mathf.Abs(VerAxis) > 0)
        {
            movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }
        else
        {
            movementVector = Vector3.zero;
        }

        //movementVector *= movementSpeed;
        movementVector *= attributes.speedMov;

        if (Input.GetAxis("Boost") > 0)
        {
            //movementVector += movementVector * movementBoost * Input.GetAxis("Boost");
            movementVector += movementVector * (attributes.speedBoost * Input.GetAxis("Boost"));
        }

        if (Input.GetButton("Fire2"))
        {
            isDeffending = true;
        }
        else
        {
            isDeffending = false;
        }

        if (Input.GetButton("Fire1"))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
            incrementDone = false;
        }
        if (isAttacking && !isDeffending)
        {

            if (incrementDone == false)
            {
                attackNumber += 1;
                incrementDone = true;
            }

            if (attackNumber > 3)
            {
                attackNumber = 0;
            }

           
        }
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("AttackStopped"))
        {
            attackNumber = 0;
        }

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Attack1") ||
            animator.GetCurrentAnimatorStateInfo(1).IsName("Attack2") ||
            animator.GetCurrentAnimatorStateInfo(1).IsName("Attack3") )
        {
            movementVector = Vector3.zero;
        }

        if (ActivateHitbox)
        {
            hitBox.SetActive(true);
            slashParticles.Play();

        }
        else
        {
            hitBox.SetActive(false);
            slashParticles.Stop();

            
        }
        if (attributes.getHit)
        {
            attributes.getHit = false;
            if (isDeffending)
            {
                shieldParticles.Play();
                if (shieldParticles.isPlaying)
                {
                    shieldParticles.gameObject.SetActive(false);
                    shieldParticles.Stop();
                    shieldParticles.gameObject.SetActive(true);
                    shieldParticles.Play();
                }
                else
                {
                    shieldParticles.Play();
                }

            }
            else
            {
                animator.Play("getHit");
                bloodParticles.Play();
            }
            
        }
        else
        {

            bloodParticles.Stop();
            shieldParticles.Stop();
        }

        float maxMovement = Mathf.Max(Mathf.Abs(movementVector.x), Mathf.Abs(movementVector.z));
        animator.SetFloat("BlendMovement", maxMovement);
        animator.SetBool("Defend", isDeffending);
        animator.SetInteger("AttackNumber", attackNumber);
    }
    void FixedUpdate()
    {
        //check time vs ficedtime
        Move();
        Rotate();
    }
    public void Move() //Mueve en la direccion de movimiento que dan las teclas
    {
        movementVector = Vector3.ClampMagnitude(movementVector, attributes.speedMov + attributes.speedBoost); 
        transform.position = transform.position + movementVector;
        
    }
    public void Rotate() //rota en la direccion de movimiento que dan las teclas
    {

        if (Mathf.Abs(HorAxis) > 0 || Mathf.Abs(VerAxis) > 0)
        {
            if(movementVector != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementVector);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, attributes.speedRot);
            }
            
        }
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
                case "attack_01":
                    AddEventToClip(clip, "AttackStarted", 12);
                    AddEventToClip(clip, "AttackStopped", 15);
                    break;
                case "attack_02":
                    AddEventToClip(clip, "AttackStarted", 8);
                    AddEventToClip(clip, "AttackStopped", 12);
                    break;
                case "attack_03":
                    AddEventToClip(clip, "AttackStarted", 13);
                    AddEventToClip(clip, "AttackStopped", 20);
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
    private void AttackStarted()
    {
        ActivateHitbox = true;
        
    }
    private void AttackStopped()
    {
        ActivateHitbox = false;
    }


}
