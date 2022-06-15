using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairSit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log(other.name);
            other.GetComponent<Animator>().SetTrigger("Sit");
        }
    }
}
