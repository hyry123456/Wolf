using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2BeginLoad : MonoBehaviour
{
    void Start()
    {
        UI.DumbShowManage.Instance.ShowDumbText(
            TextLoad.Instance.GetOneDumbText(1),  null,
            new Common.INonReturnAndNonParam[30]);
    }

}
