using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public GameObject origin;

    void Start()
    {
        string strs = "你好\n我是马云\n我很有钱";
        UI.DumbShowManage.Instance.ShowDumbText(strs, null);
    }
}
