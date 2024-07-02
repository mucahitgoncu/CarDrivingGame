using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Mesh_Store : Store
{
    MeshRenderer p;
    public GameObject[] items; // Store 3D GameObjects instead of Sprites

    public override void SettingPlayer()
    {
        p = player.GetComponent<MeshRenderer>();
    }

    public override void SetPlayer(int i)
    {
        GameObject item = items[i];
        player = Instantiate(item, player.transform.position, player.transform.rotation);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Mesh_Store))]
public class MeshStore_Editor : Store_Editor
{
    public override void OnInspectorGUI()
    {
        SetSettings();
    }
}
#endif
