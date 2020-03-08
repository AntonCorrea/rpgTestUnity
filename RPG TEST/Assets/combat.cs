using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    // Start is called before the first frame update
    
    public bool isAttacking;
    public bool isDeffending;

    public int attackNumber = 0;
    bool incrementDone=false;

    public bool animationClipAttackStopped;

    public bool animationHitBoxActive;

    public GameObject hitBox;
    public ParticleSystem SlashParticles;

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            isDeffending=true;
        }
        else
        {
            isDeffending = false;
        }
        
        if(Input.GetButton("Fire1"))
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

        if (animationClipAttackStopped)
        {
            attackNumber = 0;
        }

        if (animationHitBoxActive)
        {
            hitBox.SetActive(true);
            SlashParticles.Play();
        }
        else
        {
            hitBox.SetActive(false);
            SlashParticles.Stop();
        }

    }
}
