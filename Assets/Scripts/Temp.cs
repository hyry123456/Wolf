using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public int index;
    void Start()
    {
        //string strs = "���\n��������\n�Һ���Ǯ";
        //UI.DumbShowManage.Instance.ShowDumbText(TextLoad.Instance.GetOneDumbText(index), () =>
        //{
        //    //Common.SustainCoroutine.Instance.AddCoroutine(Show, false);
        //});
        //EndUI.Instance.ShowEnd(TextLoad.Instance.GetOneDumbText(18));
    }

    bool Show()
    {
        index++;
        if(index < 100)
        {
            UI.DumbShowManage.Instance.ShowDumbText(TextLoad.Instance.GetOneDumbText(index), () =>
            {
                Common.SustainCoroutine.Instance.AddCoroutine(Show, false);
            }, new Common.INonReturnAndNonParam[30]);
        }
        return true;
    }
}
