using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public GameObject owner;
   // public GameObject objectHit;
    //public GameObject[] objectsBeenHit;
    public List<GameObject> objectsBeingHit = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        owner = transform.parent.gameObject;     
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("enter");
        if(other.gameObject.GetComponent<Attribute>() != null)
        {
            if(other.gameObject.GetComponent<Attribute>().hasBeenHit == false)
            //if(other.gameObject.GetComponent<Attribute>().getHit ==false)
            {
                other.gameObject.GetComponent<Attribute>().getHit = true;
                other.gameObject.GetComponent<Attribute>().gotHitBy = owner;
                other.gameObject.GetComponent<Attribute>().hasBeenHit = true;
               objectsBeingHit.Add(other.gameObject);
            }
           
        }
        
    }


    private void OnDisable()
    {
        for(int i = 0; i < objectsBeingHit.Count; i++)
        {
            objectsBeingHit[i].GetComponent<Attribute>().hasBeenHit = false;
        }
        objectsBeingHit.Clear();
    }



}
