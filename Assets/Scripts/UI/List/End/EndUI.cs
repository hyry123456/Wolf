using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    private static EndUI instance;
    public static EndUI Instance => instance;

    public DefferedRender.PostFXSetting fXSetting;

    Common.INonReturnAndNonParam end;


    public float moveSpeed = 1;
    public float waitTime = 10;
    private float nowTime = 0;
    Text text;
    private Vector3 beginPos;


    private void Awake()
    {
        instance = this;
        text = GetComponent<Text>();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        pos.y += Time.deltaTime * moveSpeed;
        transform.position = pos;
        nowTime += Time.deltaTime;
        if(nowTime > waitTime)
        {
            OnEnd();
            return;
        }
    }

    public void ShowEnd(string str, Common.INonReturnAndNonParam end)
    {
        text.text = str;
        beginPos = Camera.main.transform.position;

        Motor.FollowPlayer2D.Instance?.StopFollow();     //Í£Ö¹ÉãÏñ»ú¸úËæÖ÷½Ç
        beginPos = Camera.main.transform.position;
        Camera.main.transform.position = transform.position + -Vector3.forward * 10
            + Vector3.up * 10;
        Control.PlayerControl.Instance?.DisableInput();     //Í£Ö¹ÊäÈë
        fXSetting.BeginWave();
        nowTime = 0;
        gameObject.SetActive(true);
        this.end = end;
    }

    private void OnEnd()
    {
        if (end != null)
            end();
        gameObject.SetActive(false);
        Control.PlayerControl.Instance?.EnableInput();     //¿ªÆôÊäÈë
        Camera.main.transform.position = beginPos;
        Motor.FollowPlayer2D.Instance?.BeginFollow();     //Í£Ö¹ÉãÏñ»ú¸úËæÖ÷½Ç
        fXSetting.EndWave();
    }
}
