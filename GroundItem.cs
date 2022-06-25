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
        EditorUtility.SetDirty(GetComponentInChildren<MeshFilter>());
        GetComponentInChildren<MeshRenderer>().material = item.groundItem.GetComponent<MeshRenderer>().sharedMaterial;
        EditorUtility.SetDirty(GetComponentInChildren<MeshRenderer>());
    }
}