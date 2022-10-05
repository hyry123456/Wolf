using UnityEngine;


/// <summary> /// �����ڵ��࣬������ʶ�õ���һ������  /// </summary>
public class HookRopeNode : MonoBehaviour
{
    void Start()
    {
        HookRopeManage.Instance.AddNode(transform);
    }

    private void OnDestroy()
    {
        HookRopeManage.Instance.RemoveNode(transform);
    }

    private void OnDisable()
    {
        HookRopeManage.Instance.RemoveNode(transform);
    }
}
