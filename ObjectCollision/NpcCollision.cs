using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCollision : MonoBehaviour
{
    private bool npc_trigger;
    public GameObject npc_chat_area;
    public GameObject npc_interact;

    public DialogueManager dialogue;

    public bool awake_dialogue;


    // Start is called before the first frame update
    void Start()
    {
        npc_chat_area.SetActive(false);/*at first*/
        npc_interact.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (npc_trigger)
        {   
            if(Input.GetKeyDown(KeyCode.E)){
                dialogue.welcome(); /*start dialogue sequence*/
                npc_chat_area.SetActive(true);
                npc_interact.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*npc interaction*/
        if (other.tag == "Player")
        {
            npc_interact.SetActive(true);
            npc_trigger = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        /*if we done with npc*/
        if (other.tag == "Player")
        {
            npc_trigger = false;
            npc_chat_area.SetActive(false);
            dialogue.dialogueIndex = 0;
            dialogue.sentenceIndex = 0;
        }

    }


}
