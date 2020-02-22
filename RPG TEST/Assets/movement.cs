using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 movementVector;
    public float HorAxis,VerAxis;
    public float rotationSpeed;
    public float movementSpeed;
    public float movementBoost;
    //public Rigidbody rb;
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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

        movementVector = movementVector * movementSpeed;

        if (Input.GetAxis("Boost") > 0)
        {
            movementVector = movementVector + movementVector * movementBoost * Input.GetAxis("Boost");
        }
        
    }
    void FixedUpdate()
    {
        //check time vs ficedtime
        Move();
        Rotate();
    }


    public void Move() //Mueve en la direccion de movimiento que dan las teclas
    {
        transform.position = transform.position + movementVector;
       
    }

    public void Rotate() //rota en la direccion de movimiento que dan las teclas
    {

        if (Mathf.Abs(HorAxis) > 0 || Mathf.Abs(VerAxis) > 0)
        {
            if(movementVector != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementVector);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed);
            }
            
        }
    }
}
