using UnityEngine;


/// <summary>
/// 背景图层移动速度不一致实现
/// </summary>
public class MapFollowCamera : MonoBehaviour
{
    /// <summary>
    /// 摄像机移动时背景图层的跟随速度，为0就是不会跟随，为静态图层
    /// </summary>
    public float speedX = 0;
    public float speedY = 0;
    private Vector3 preCameraPos = Vector3.zero;

    private void Start()
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
