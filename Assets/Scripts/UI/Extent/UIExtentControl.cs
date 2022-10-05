using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class UIExtentControl : MonoBehaviour
{
    private static UIExtentControl instance;
    public static UIExtentControl Instance
    {
        get
        {
            return instance;
        }
    }

    UIControl control;

    SmallDialogManage smallDialogManage;    //小对话框
    BigDialogManage bigDialogManage;        //大对话框
    NPCDialogManage npcDialogManage;        //人物对话框

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        showStack = new Stack<GameObject>();
        control = GetComponent<UIControl>();
    }
    private void Start()
    {
        smallDialogManage = GetComponentInChildren<SmallDialogManage>();
        bigDialogManage = GetComponentInChildren<BigDialogManage>();
        npcDialogManage = GetComponentInChildren<NPCDialogManage>();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    Stack<GameObject> showStack;


    /// <summary>
    /// 用于显示或者关闭当前的UI，根据栈来判断，之后完成统一的UI管理，现在先放着
    /// </summary>
    public void ShowOrClose()
    {
    }

    /// <summary>
    /// 添加一个显示的物体，放到栈中方便统一管理
    /// </summary>
    public void AddShowObject(GameObject game)
    {
        game.SetActive(true);
        showStack.Peek()?.SetActive(false);
        showStack.Push(game);
    }

    /// <summary>
    /// 显示大对话框对话，对话内容用换行符分割
    /// </summary>
    /// <param name="strs">对话内容</param>
    public void ShowBigDialog(string strs, Common.INonReturnAndNonParam endBehavior)
    {
        bigDialogManage.ShowBigDialog(strs, endBehavior);
    }

    /// <summary>
    /// 显示小对话框对话，对话内容用换行符分割
    /// </summary>
    /// <param name="strs">对话内容</param>
    public void ShowSmallDialog(string strs, Common.INonReturnAndNonParam endBehavior)
    {
        smallDialogManage.ShowSmallDialog(strs, endBehavior);
    }
    /// <summary>
    /// 显示人物对话内容，对话内容用换行符分割
    /// </summary>
    /// <param name="strs">对话内容</param>
    /// <param name="postion">显示位置</param>
    public void ShowNPCDialog(string strs, Vector3 postion, Common.INonReturnAndNonParam endBehavior)
    {
        npcDialogManage.ShowDialog(strs, postion, endBehavior);
    }

    /// <summary>
    /// 显示人物对话内容，对话内容用换行符分割
    /// </summary>
    /// <param name="strs">对话内容</param>
    /// <param name="follow">跟随的目标</param>
    /// <param name="height">偏移目标的高度</param>
    public void ShowNPCDialog(string strs, Transform follow, float height, 
        Common.INonReturnAndNonParam endBehavior)
    {
        npcDialogManage.ShowDialog(strs, follow, height, endBehavior);
    }

}
