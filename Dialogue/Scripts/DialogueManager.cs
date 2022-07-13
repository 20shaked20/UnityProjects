// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using TMPro;
// using ReadyPlayerMe;
// using UnityEngine.UI;
// using System;

// public class DialogueManager : MonoBehaviour
// {
//     [Header("Dialogue")]
//     public DialogueInfo[] dialogue;
//     public TextMeshProUGUI dialogueText;

//     private Queue<string> sentences;
//     public int dialogueIndex;
//     public int sentenceIndex;
//     public AudioSource dialogueAudio;
//     private VoiceHandler voiceHandler;
//     public GameObject speakerObj;
//     public Button continueButton;

//     public bool dialogueEnded;
//     public bool playAudio;

//     public int length_of_sentences;

//     public void Awake()
//     {
//         sentences = new Queue<string>();
//         continueButton.gameObject.SetActive(true);

//         voiceHandler = speakerObj.GetComponent<VoiceHandler>();

//         voiceHandler.AudioClip = dialogue[dialogueIndex].clips[0];

//         sentenceIndex = 0;
//         StartDialogue(dialogue[0]);
//         DisplayNextSentence();
//         PlayInitialSpeech();
//     }

//     public void LateUpdate()
//     {

//         if (playAudio)
//         {
//             voiceHandler.PlayCurrentAudioClip();
//             playAudio = false;
//         }

//     }

//     public void QueDialogue()
//     {
//         sentences = new Queue<string>();
//     }

//     public void StartDialogue(DialogueInfo dialogue)
//     {
//         playAudio = true;
//         sentenceIndex = 0;
//         sentences.Clear();

//         foreach (string sentence in dialogue.sentences)
//         {
//             sentences.Enqueue(sentence);
//         }

//         length_of_sentences = 0;
//     }

//     public void DisplayNextSentence()
//     {
//         if (!dialogueAudio.isPlaying)
//         {
//             if (sentences.Count == 0)
//             {
//                 ++dialogueIndex;
//                 Debug.Log(dialogueIndex);
//             }

//             if (sentences.Count == 0 && dialogueIndex != dialogue.Length && !dialogueEnded)
//             {
//                 QueDialogue();
//                 sentenceIndex = 0;
//                 dialogueEnded = false;
//                 StartDialogue(dialogue[dialogueIndex]);
//             }

//             if (sentences.Count == 0 && dialogueIndex == dialogue.Length)
//             {
//                 dialogueEnded = true;
//                 EndDialogue();
//                 // return;
//             }

//             if (length_of_sentences > 0)
//             {
//                 string sentence = sentences.Dequeue();
//                 /*doing this to make sure senteses are not deleted and i can reset dialogue at any time*/
//                 // string sentence = sentences.Dequeue();
//                 // sentences.Enqueue(sentence);
//                 // ++length_of_sentences;

//                 Debug.Log(sentence);

//                 dialogueText.text = sentence;
//                 voiceHandler = speakerObj.GetComponent<VoiceHandler>();
//             }
//         }
//     }

//     public void PlayInitialSpeech()
//     {
//         /*this plays the very first dialogue text and sentence,
//         the reason: the continue button prgoress the dialogue, 
//         we need something to play before we press continue, 
//         so its the first time we encounter the npc.*/
//         if (sentences.Count > 0)
//         {
//             string sentence = sentences.Dequeue();
//             dialogueText.text = sentence;

//             // string sentence = sentences.Dequeue();
//             // sentences.Enqueue(sentence);
//             // ++length_of_sentences;

//             // dialogueText.text = sentence;
//         }
//         ++sentenceIndex;
//     }

//     public void PlaySpeech()
//     {
//         if (!dialogueAudio.isPlaying && !dialogueEnded)
//         {
//             dialogueAudio.PlayOneShot(dialogue[dialogueIndex].clips[sentenceIndex]);
//             voiceHandler.AudioClip = dialogue[dialogueIndex].clips[sentenceIndex];
//             voiceHandler.AudioSource.clip = dialogue[dialogueIndex].clips[sentenceIndex];

//             playAudio = true;
//             ++sentenceIndex;

//         }
//     }

//     private void EndDialogue()
//     {   
//         StartDialogue(dialogue[0]);
//         DisplayNextSentence();
//         PlayInitialSpeech();
//         /*can add here button, change scence, anything */
//         Debug.Log("Start Game");
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using ReadyPlayerMe;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueInfo[] dialogue;
    public TextMeshProUGUI dialogueText;

    private Queue<string> sentences;
    public int dialogueIndex;
    public int sentenceIndex;
    public AudioSource dialogueAudio;
    private VoiceHandler voiceHandler;
    public GameObject speakerObj;
    public Button continueButton;

    public bool dialogueEnded;
    public bool playAudio;


    // public void Awake()
    // {
    //     sentences = new Queue<string>();
    //     continueButton.gameObject.SetActive(true);

    //     voiceHandler = speakerObj.GetComponent<VoiceHandler>();

    //     voiceHandler.AudioClip = dialogue[dialogueIndex].clips[0];

    //     sentenceIndex = 0;
    //     StartDialogue(dialogue[0]);
    //     DisplayNextSentence();
    //     PlayInitialSpeech();
    // }

    public void welcome()
    {
        sentences = new Queue<string>();
        continueButton.gameObject.SetActive(true);

        voiceHandler = speakerObj.GetComponent<VoiceHandler>();

        voiceHandler.AudioClip = dialogue[dialogueIndex].clips[0];

        sentenceIndex = 0;
        StartDialogue(dialogue[0]);
        DisplayNextSentence();
        PlayInitialSpeech();
    }

    public void LateUpdate()
    {

        if (playAudio)
        {
            voiceHandler.PlayCurrentAudioClip();
            playAudio = false;
        }

    }

    public void QueDialogue()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(DialogueInfo dialogue)
    {
        playAudio = true;
        sentenceIndex = 0;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    public void DisplayNextSentence()
    {
        if (!dialogueAudio.isPlaying)
        {
            if (sentences.Count == 0)
            {
                ++dialogueIndex;
            }

            if (sentences.Count == 0 && dialogueIndex != dialogue.Length && !dialogueEnded)
            {
                QueDialogue();
                sentenceIndex = 0;
                dialogueEnded = false;
                StartDialogue(dialogue[dialogueIndex]);
            }

            if (sentences.Count == 0 && dialogueIndex == dialogue.Length)
            {
                dialogueEnded = true;
                EndDialogue();
                return;
            }

            if (sentences.Count > 0)
            {
                string sentence = sentences.Dequeue();
                dialogueText.text = sentence;
                voiceHandler = speakerObj.GetComponent<VoiceHandler>();
            }
        }
    }

    public void PlayInitialSpeech()
    {
        /*this plays the very first dialogue text and sentence,
        the reason: the continue button prgoress the dialogue, 
        we need something to play before we press continue, 
        so its the first time we encounter the npc.*/
        if (sentences.Count > 0)
        {
            string sentence = sentences.Dequeue();
            dialogueText.text = sentence;
        }
        ++sentenceIndex;
    }

    public void PlaySpeech()
    {
        if (!dialogueAudio.isPlaying && !dialogueEnded)
        {
            dialogueAudio.PlayOneShot(dialogue[dialogueIndex].clips[sentenceIndex]);
            voiceHandler.AudioClip = dialogue[dialogueIndex].clips[sentenceIndex];
            voiceHandler.AudioSource.clip = dialogue[dialogueIndex].clips[sentenceIndex];

            playAudio = true;
            ++sentenceIndex;

        }
    }

    private void EndDialogue()
    {
        StartDialogue(dialogue[0]);
        DisplayNextSentence();
        PlayInitialSpeech();
        /*can add here button, change scence, anything */
        Debug.Log("Start Game");
    }
}
