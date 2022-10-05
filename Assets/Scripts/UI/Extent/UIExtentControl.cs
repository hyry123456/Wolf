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

    SmallDialogManage smallDialogManage;    //С�Ի���
    BigDialogManage bigDialogManage;        //��Ի���
    NPCDialogManage npcDialogManage;        //����Ի���

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
    /// ������ʾ���߹رյ�ǰ��UI������ջ���жϣ�֮�����ͳһ��UI���������ȷ���
    /// </summary>
    public void ShowOrClose()
    {
    }

    /// <summary>
    /// ���һ����ʾ�����壬�ŵ�ջ�з���ͳһ����
    /// </summary>
    public void AddShowObject(GameObject game)
    {
        game.SetActive(true);
        showStack.Peek()?.SetActive(false);
        showStack.Push(game);
    }

    /// <summary>
    /// ��ʾ��Ի���Ի����Ի������û��з��ָ�
    /// </summary>
    /// <param name="strs">�Ի�����</param>
    public void ShowBigDialog(string strs, Common.INonReturnAndNonParam endBehavior)
    {
        bigDialogManage.ShowBigDialog(strs, endBehavior);
    }

    /// <summary>
    /// ��ʾС�Ի���Ի����Ի������û��з��ָ�
    /// </summary>
    /// <param name="strs">�Ի�����</param>
    public void ShowSmallDialog(string strs, Common.INonReturnAndNonParam endBehavior)
    {
        smallDialogManage.ShowSmallDialog(strs, endBehavior);
    }
    /// <summary>
    /// ��ʾ����Ի����ݣ��Ի������û��з��ָ�
    /// </summary>
    /// <param name="strs">�Ի�����</param>
    /// <param name="postion">��ʾλ��</param>
    public void ShowNPCDialog(string strs, Vector3 postion, Common.INonReturnAndNonParam endBehavior)
    {
        npcDialogManage.ShowDialog(strs, postion, endBehavior);
    }

    /// <summary>
    /// ��ʾ����Ի����ݣ��Ի������û��з��ָ�
    /// </summary>
    /// <param name="strs">�Ի�����</param>
    /// <param name="follow">�����Ŀ��</param>
    /// <param name="height">ƫ��Ŀ��ĸ߶�</param>
    public void ShowNPCDialog(string strs, Transform follow, float height, 
        Common.INonReturnAndNonParam endBehavior)
    {
        npcDialogManage.ShowDialog(strs, follow, height, endBehavior);
    }

}
