using UnityEngine;

[System.Serializable]

[CreateAssetMenu(menuName = "Dialogue/SpeakerDialogue")]
public class DialogueInfo : ScriptableObject
{
    [Header("Dialogue Speaker Details")]
    public Texture avatar;
    public string speakerName;
    [TextArea(10,20)]
    public string[] sentences;
    public AudioClip[] clips;
    public GameObject speakerPrefab;
}
