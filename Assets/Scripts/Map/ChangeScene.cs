using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    private static ChangeScene instance;
    public static ChangeScene Instance => instance;

    [SerializeField]
    string targetScene;

    [SerializeField]
    /// <summary>   /// ��Ҫ��������ĻЧ��  /// </summary>
    public DefferedRender.PostFXSetting fXSetting;
    private float nowRadio, waitTime = 1.0f;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }


    public void BeginChangeMap()
    {
        nowRadio = 0;
        fXSetting.EnableRotate();
        Common.SustainCoroutine.Instance.AddCoroutine(ChangeMap);
    }


    /// <summary>
    /// ���뵽Э�����л���ͼ
    /// </summary>
    bool ChangeMap()
    {
        if (nowRadio < 1.0f)
        {
            fXSetting.SetColorFilter(Color.Lerp(Color.white, Color.black, Mathf.Clamp01(nowRadio)));
            fXSetting.SetRotateRadio(nowRadio);
            nowRadio += Time.deltaTime;
            if (nowRadio >= 1.0f)
            {
                fXSetting.DisableRotate();
                fXSetting.SetRotateRadio(0);
                fXSetting.SetColorFilter(Color.white);
                Control.SceneChangeControl.Instance.ChangeScene(targetScene);
                return true;
            }
            return false;
        }
        return false;
        //else if (nowRadio < 1.0f + waitTime)
        //{
        //    nowRadio += Time.deltaTime;
        //    return false;
        //}
        //return false;
        //else
        //{
        //    fXSetting.SetColorFilter(Color.Lerp(Color.black, Color.white, Mathf.Clamp01(nowRadio - 1.0f - waitTime)));
        //    nowRadio += Time.deltaTime;
        //    if (nowRadio >= 2.0f + waitTime)
        //    {
        //        fXSetting.SetColorFilter(Color.white);
        //        Control.SceneChangeControl.Instance.ChangeScene(targetScene);
        //        return true;
        //    }
        //    return false;
        //}
    }

}
