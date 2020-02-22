using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat : MonoBehaviour
{
    // Start is called before the first frame update
    
    public bool isAttacking;
    public bool isDeffending;

    public int attackNumber = 0;
    bool incrementDone=false;
    
    public AnimatorStateInfo animatorClip;

    void Start()
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

        if (animatorClip.IsName("AttackStopped") )
        {
            attackNumber = 0;
        }


    }
}
