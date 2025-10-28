using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectManager : MonoBehaviour
{
    [Space]
    [Header("Trigger Unity Event")]
    [SerializeField]
    UnityEvent TriggerUnityEvent;

    [Space]
    [Header("Self Boolean")]
    [SerializeField]
    bool bSelf = false;

    void Triggered()
    {
        if (bSelf)
        {
            this.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            for (int i = 0; i < this.transform.parent.childCount; i++)
            {
                this.transform.parent.GetChild(i).GetComponent<BoxCollider>().enabled = false;
            }
        }

        TriggerUnityEvent.Invoke();
    }

    public void ResetTrigger()
    {
        if (bSelf)
        {
            this.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            for (int i = 0; i < this.transform.parent.childCount; i++)
            {
                this.transform.parent.GetChild(i).GetComponent<BoxCollider>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Pointer")
        {
            Triggered();
        }
    }
}