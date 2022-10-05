using UnityEngine;


/// <summary>
/// ����ͼ���ƶ��ٶȲ�һ��ʵ��
/// </summary>
public class MapFollowCamera : MonoBehaviour
{
    /// <summary>
    /// ������ƶ�ʱ����ͼ��ĸ����ٶȣ�Ϊ0���ǲ�����棬Ϊ��̬ͼ��
    /// </summary>
    public float speedX = 0;
    public float speedY = 0;
    private Vector3 preCameraPos = Vector3.zero;

    private void Awake()
    {
        if (Camera.main == null) return;
        preCameraPos = Camera.main.transform.position;
    }


    private void Update()
    {
        if (Camera.main == null) return;
        Vector3 nowPos = Camera.main.transform.position;
        Vector3 offset = nowPos - preCameraPos;
        Vector3 transfer = transform.position;
        transfer.x += offset.x * speedX;
        transfer.y += offset.y * speedY;
        transform.position = transfer;
        preCameraPos = nowPos;
    }

}
