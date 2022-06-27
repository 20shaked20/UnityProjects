using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {   
        /*2d*/
        // GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        // EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());

        /*3d*/
        GetComponentInChildren<MeshFilter>().mesh = item.groundItem.GetComponent<MeshFilter>().sharedMesh;
#if UNITY_EDITOR
        EditorUtility.SetDirty(GetComponentInChildren<MeshFilter>());
#endif
        GetComponentInChildren<MeshRenderer>().material = item.groundItem.GetComponent<MeshRenderer>().sharedMaterial;
#if UNITY_EDITOR
        EditorUtility.SetDirty(GetComponentInChildren<MeshRenderer>());
#endif
    }
}