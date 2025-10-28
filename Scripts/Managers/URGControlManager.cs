using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class URGControlManager : MonoBehaviour
{
    [Space, Header("URG Json Manager"), SerializeField]
    URGJsonManager URGJsonManager;

    [Space, Header("URG Controls"), SerializeField]
    List<URGControl> URGControls;

    [Space, Header("URG Control Manager UI"), SerializeField]
    GameObject URGControlManagerUI;

    [Space, Header("URG Control Manager UI Boolean"), SerializeField]
    bool bURGControlManagerUI = false;

    void Start()
    {
        foreach (var (urgmanager, index) in URGJsonManager.URGManagers.Select((urgmanager, index) => (urgmanager, index)))
        {
            urgmanager.URGID = index;

            URGControls.Add(urgmanager.GetComponent<URGControl>());
            ControlData.bSettings.Add(false);
        }
    }

    void Update()
    {
        CheckUIOpen();
        CheckSettingOpen();
    }

    void CheckUIOpen()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            bool bsetting = ControlData.bSettings.Any(setting => setting);

            if (!bsetting)
            {
                bURGControlManagerUI = !bURGControlManagerUI;

                URGControlManagerUI.SetActive(bURGControlManagerUI);
            }
        }
    }

    void CheckSettingOpen()
    {
        if (bURGControlManagerUI)
        {
            foreach (var (urgcontrol, index) in URGControls.Select((urgcontrol, index) => (urgcontrol, index)))
            {
                KeyCode key = KeyCode.Alpha1 + index;

                if (Input.GetKeyDown(key))
                {
                    URGControlManagerUI.SetActive(ControlData.bSettings[index]);

                    ControlData.bSettings[index] = !ControlData.bSettings[index];

                    urgcontrol.URGControlUI.SetActive(ControlData.bSettings[index]);

                    if (!ControlData.bSettings[index])
                    {
                        URGJsonManager.WriteJson(index);
                    }
                }
            }
        }
    }
}

static class ControlData
{
    [Space]
    [Header("Setting Boolean")]
    [SerializeField]
    public static List<bool> bSettings = new List<bool>();
}