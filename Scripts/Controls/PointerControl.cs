using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerControl : MonoBehaviour
{
    [Space]
    [Header("Mesh Renderer")]
    [SerializeField]
    MeshRenderer MeshRenderer;

    [Space]
    [Header("URG ID")]
    [SerializeField]
    public int URGID;

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer.enabled =false;
    }

    // Update is called once per frame
    void Update()
    {
        SettingCheck();
    }

    void SettingCheck()
    {
        MeshRenderer.enabled = ControlData.bSettings[URGID];
    }
}
