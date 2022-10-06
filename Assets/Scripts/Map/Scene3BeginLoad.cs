using System.Collections.Generic;
using UnityEngine;

public class Scene3BeginLoad : MonoBehaviour
{
    void Start()
    {
        UI.DumbShowManage.Instance.ShowDumbText(
            TextLoad.Instance.GetOneDumbText(2), null, 
            new Common.INonReturnAndNonParam[30]);


    }

}
