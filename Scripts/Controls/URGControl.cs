using UnityEngine;
using UnityEngine.UI;

public class URGControl : MonoBehaviour
{
    [Space, Header("URG Manager"), SerializeField]
    public URGManager URGManager;

    [Space, Header("URG Control UI"), SerializeField]
    public GameObject URGControlUI;

    [Space, Header("InputFields"), SerializeField]
    private InputField Scale;
    [SerializeField]
    private InputField Limit;
    [SerializeField]
    private InputField NoiseLimit;
    [SerializeField]
    private InputField XMinPos;
    [SerializeField]
    private InputField YMinPos;

    private void Start()
    {
        Scale.text = URGManager.fScale.ToString();
        Limit.text = URGManager.fLimit.ToString();
        NoiseLimit.text = URGManager.iNoiseLimit.ToString();
        XMinPos.text = URGManager.fXMinPos.ToString();
        YMinPos.text = URGManager.fYMinPos.ToString();

        Scale.onValueChanged.AddListener(delegate { CheckfloatValueChange(Scale.text, ref URGManager.fScale); });
        Limit.onValueChanged.AddListener(delegate { CheckfloatValueChange(Limit.text, ref URGManager.fLimit); });
        NoiseLimit.onValueChanged.AddListener(delegate { CheckintValueChange(NoiseLimit.text, ref URGManager.iNoiseLimit); });
        XMinPos.onValueChanged.AddListener(delegate { CheckfloatValueChange(XMinPos.text, ref URGManager.fXMinPos); });
        YMinPos.onValueChanged.AddListener(delegate { CheckfloatValueChange(YMinPos.text, ref URGManager.fYMinPos); });
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlData.bSettings[URGManager.URGID])
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MoveVertical(10.0f);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                MoveVertical(-10.0f);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveHorizontal(10.0f);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                MoveHorizontal(-10.0f);
            }
        }
    }

    void MoveVertical(float fspeed)
    {
        this.transform.position += new Vector3(0, fspeed, 0) * Time.deltaTime;
    }

    void MoveHorizontal(float fspeed)
    {
        this.transform.position += new Vector3(fspeed, 0, 0) * Time.deltaTime;
    }

    void CheckfloatValueChange(string text, ref float value)
    {
        value = float.Parse(text);
    }

    void CheckintValueChange(string text, ref int value)
    {
        value = int.Parse(text);
    }
}