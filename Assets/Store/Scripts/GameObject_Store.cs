using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class GameObject_Store : Store
{
    public GameObject[] items;
    public override void SetPlayer(int i)
    {
        GameObject oldPlayer = player;
        player = Instantiate(items[i],oldPlayer.transform.position,oldPlayer.transform.rotation);
        Destroy(oldPlayer);
    }

}
#if UNITY_EDITOR
[CustomEditor(typeof(GameObject_Store))]
public class GameObject_Store_Editor : Store_Editor
{
    public override void OnInspectorGUI()
    {
        SetSettings();
    }
}
#endif
