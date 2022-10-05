using Common;
using DefferedRender;
using UnityEngine;

/// <summary>
/// һ���ػ������壬�������������壬������ֻ��һ��������������ײ��ᱬը��
/// �Լ��ṩһ��������ײ�����ķ�������ʾ��ײ��ִ�е�ʱ��
/// </summary>
public class Sphere_Pooling : ObjectPoolBase
{
    public delegate void Collision2DEnter(Collision2D collision);
    public Collision2DEnter collsionEnter;
    ParticleDrawData drawData;

    public override void InitializeObject(Vector3 positon, Quaternion quaternion)
    {
        base.InitializeObject(positon, quaternion);
        Vector2 sizeRange = new Vector2(0.1f, 0.2f);
        drawData = new ParticleDrawData
        {
            beginPos = transform.position,
            beginSpeed = Vector3.up,
            speedMode = SpeedMode.JustBeginSpeed,
            useGravity = true,
            followSpeed = true,
            radian = 3.14f,
            radius = 1f,
            cubeOffset = new Vector3(0.1f, 0.1f, 0.1f),
            lifeTime = 1,
            showTime = 1,
            frequency = 1f,
            octave = 8,
            intensity = 20,
            sizeRange = sizeRange,
            colorIndex = (int)ColorIndexMode.HighlightToAlpha,
            sizeIndex = (int)SizeCurveMode.Small_Hight_Small,
            textureIndex = 0,
            groupCount = 1,
        };
    }

    public override void InitializeObject(Vector3 positon, Vector3 lookAt)
    {
        base.InitializeObject(positon, lookAt);
        Vector2 sizeRange = new Vector2(0.1f, 0.2f);
        drawData = new ParticleDrawData
        {
            beginPos = transform.position,
            beginSpeed = Vector3.up,
            speedMode = SpeedMode.JustBeginSpeed,
            useGravity = true,
            followSpeed = true,
            radian = 3.14f,
            radius = 1f,
            cubeOffset = new Vector3(0.1f, 0.1f, 0.1f),
            lifeTime = 1,
            showTime = 1,
            frequency = 1f,
            octave = 4,
            intensity = 20,
            sizeRange = sizeRange,
            colorIndex = (int)ColorIndexMode.HighlightToAlpha,
            sizeIndex = (int)SizeCurveMode.Small_Hight_Small,
            textureIndex = 0,
            groupCount = 1,
        };
    }

    /// <summary> /// ������ƶ��������ƣ����屾��ֻ��һ����ײ�� /// </summary>
    private void FixedUpdate()
    {
        drawData.beginPos = transform.position;
        ParticleNoiseFactory.Instance.DrawPos(drawData);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collsionEnter != null)
            collsionEnter(collision);

        Debug.Log(collision.gameObject.name);

        if (collision.contacts.Length == 0) return;
        drawData.groupCount = 30;
        drawData.beginPos = collision.contacts[0].point;
        drawData.speedMode = SpeedMode.JustBeginSpeed;
        drawData.beginSpeed = collision.contacts[0].normal * 5;
        drawData.lifeTime = 5; drawData.showTime = 5f;

        ParticleNoiseFactory.Instance.DrawPos(drawData);

        drawData.groupCount = 1;
        drawData.speedMode = SpeedMode.JustBeginSpeed;
        CloseObject();

    }

}