using UnityEngine;

namespace DefferedRender
{
    [System.Serializable]
    public struct NoiseParticleData
    {
        public Vector4 random;          //xyz���������w��Ŀǰ���ʱ��
        public Vector2Int index;             //״̬��ǣ�x�ǵ�ǰ��ţ�y���Ƿ���
        public Vector3 worldPos;        //��ǰλ��
        public Vector4 uvTransData;     //uv������Ҫ������
        public float interpolation;    //��ֵ��Ҫ������
        public Vector4 color;           //��ɫֵ������͸����
        public float size;             //���Ӵ�С
        public Vector3 nowSpeed;        //xyz�ǵ�ǰ�ٶȣ�w�Ǵ��ʱ��
        public float liveTime;         //�����ʱ��
    };


    public struct ParticleInitializeData
    {
        public Vector3 beginPos;        //�����������г�ʼλ��
        public Vector3 velocityBeg;     //��ʼ�ٶȷ�Χ
        public Vector3 velocityEnd;     //��ʼ�ٶȷ�Χ
        public Vector3Int InitEnum;     //��ʼ���ĸ��ݱ��
        public Vector2 sphereData;      //��ʼ����������Ҫ������
        public Vector3 cubeRange;       //��ʼ����������ķ�Χ
        public Matrix4x4 transfer_M;    //��ʼ����������ķ�Χ
        public Vector2 lifeTimeRange;   //�������ڵķ�Χ

        public Vector3 noiseData;       //���������ٶ�ʱ��Ҫ������

        public Vector3Int outEnum;      //ȷ�����ʱ�㷨��ö��
        public Vector2 smoothRange;       //���ӵĴ�С��Χ

        public uint arriveIndex;
    };

    public enum SizeBySpeedMode
    {
        TIME = 0,
        X = 1,
        Y = 2,
        Z = 3,
    }

    public enum InitialShapeMode
    {
        Pos = 0,
        Sphere = 1,
        Cube = 2
    }

    /// <summary>   /// �������Ƶ����������ӵ����ݽṹ��    /// </summary>
    [System.Serializable]
    public class NoiseData 
    {
        //��ʼ��
        public InitialShapeMode shapeMode = InitialShapeMode.Pos;
        public Transform position;
        [Range(0.01f, 6.18f)]
        public float arc = 0.1f;              //�������ɷ�Χ
        public float radius = 1;           //Բ��С
        public Vector3 cubeRange;          //���δ�С
        public Vector3 velocityBegin;      //�ٶȷ�Χ
        public Vector3 velocityEnd;
        public Vector2 lifeTime = new Vector2(0.01f, 1);

        //����
        [Range(1, 8)]
        public int octave = 1;
        public float frequency = 1;
        [Min(0.1f)]
        public float intensity = 0.5f;

        //�������
        public bool isSizeBySpeed;
        public SizeBySpeedMode sizeBySpeedMode = SizeBySpeedMode.TIME;
        public Vector2 smoothRange = Vector2.up;

        //����
        public bool isPhysical;     //��������
    }



    #region ���ӹ�����Ҫ������


    [System.Serializable]
    /// <summary>    /// ���ӵĸ�������    /// </summary>
    public struct ParticleNodeData
    {
        public Vector3 beginPos;        //�����������г�ʼλ��
        public Vector3 beginSpeed;      //��ʼ�ٶ�
        public Vector3Int initEnum;     //x:��ʼ������״,y:�Ƿ�ʹ��������z:ͼƬ���
        public Vector2 sphereData;      //��ʼ����������Ҫ������, x=�Ƕ�, y=�뾶
        public Vector3 cubeRange;       //��ʼ����������ķ�Χ
        public Vector3 lifeTimeRange;   //�������ڵķ�Χ,x:����ͷ�ʱ��,Y:���ʱ��,Z:������浽��ʱ��
        public Vector3 noiseData;       //���������ٶ�ʱ��Ҫ������
        public Vector3Int outEnum;      //ȷ�����ʱ�㷨��ö��,x:followSpeed?
        public Vector2 smoothRange;     //���ӵĴ�С��Χ
        public Vector2Int uvCount;        //x:row��y:column,
        public Vector2Int drawData;     //x:��ɫ�����,y�Ǵ�С�ı��
    };

    /// <summary>    /// ����ͼƬ�е�ͼ������    /// </summary>
    [System.Serializable]
    public struct TextureUVCount
    {
        public int rowCount;
        public int columnCount;
    }

    /// <summary>    /// �ٶ�ģʽ�����Ƴ�ʼ���ٶȵķ�ʽ    /// </summary>
    public enum SpeedMode
    {
        /// <summary>        /// �����ٶȾ��ǳ�ʼ�����ٶ�        /// </summary>
        JustBeginSpeed = 0,
        /// <summary>   /// ��ֱ���ٶȣ���ʱ������ٶȱ�ʾ���ߣ�
        /// ���ȱ�ʾ�ٶȴ�С���ҷ����Ǵ�ֱ��������   /// </summary>
        VerticalVelocityOutside = 1,
        /// <summary>   /// ��ֱ���ٶȣ���ʱ������ٶȱ�ʾ���ߣ�
        /// ���ȱ�ʾ�ٶȴ�С���ҷ����Ǵ�ֱ��������  /// </summary>
        VerticalVelocityInside = 2,
        /// <summary>   /// ���ݵ�ǰλ��������ƫ�ƣ��ٶȳ���Ϊ�ٶȴ�С   /// </summary>
        PositionInside = 3,
        /// <summary>   /// ���ݵ�ǰλ������ƫ�ƣ��ٶȳ���Ϊ�ٶȴ�С   /// </summary>
        PositionOutside = 4,
    }

    /// <summary>
    /// ��Ⱦ����ʱ�ĸ������ݣ�����һ���ṹ�巽��ȷ���β����ͣ���Ȼ����̫����,���ü��
    /// </summary>
    public struct ParticleDrawData
    {
        /// <summary>        /// ��ʼλ�ã����ڸ�λ�ü�����Χ��������        /// </summary>
        public Vector3 beginPos;
        /// <summary>        /// ��ʼ�ٶȣ���������Ĭ���ٶȳ�ʼ��        /// </summary>
        public Vector3 beginSpeed;
        /// <summary>        /// �ٶ�ģʽ�����������ͷŵ�ģʽ        /// </summary>
        public SpeedMode speedMode;
        /// <summary>        /// �Ƿ������ʹ������        /// </summary>
        public bool useGravity;
        /// <summary>        /// ������Ⱦ�Ƿ�Ҫ�����ٶȣ������ǳ��������        /// </summary>
        public bool followSpeed;
        /// <summary>        /// radius�ǽǶȣ�radian�ǻ���(0-3.14)�����������������Ⱦ�ķ�Χ        /// </summary>
        public float radius, radian;
        /// <summary>        /// ������������ʱȷ��������εĴ�С���ֱ��ʾxyz��ƫ��ֵ        /// </summary>
        public Vector3 cubeOffset;
        /// <summary>        /// ������ӵ����������        /// </summary>
        public float lifeTime;
        /// <summary>        /// �������ӵ���ʾʱ�䣬ע�⣬��ʾʱ���벻Ҫ��������ʱ��        /// </summary>
        public float showTime;
        /// <summary>        /// ����������Ƶ��        /// </summary>
        public float frequency;
        /// <summary>        /// ����������ѭ������������Խ��Խ���ң����������ĸо�����Ҫ����8��        /// </summary>
        public int octave;
        /// <summary>        /// ������ǿ�ȣ�Խǿ�����ƶ��仯Խ��        /// </summary>
        public float intensity;
        /// <summary>        /// ���ӵĴ�С��Χ��size���ߵĽ����ӳ�䵽��������        /// </summary>
        public Vector2 sizeRange;
        /// <summary>        /// ��ɫ��ţ�����ȷ�����ӵ���ɫ�Լ�͸����        /// </summary>
        public int colorIndex;
        /// <summary>        /// ѡ��Ĵ�С���߱��        /// </summary>
        public int sizeIndex;
        /// <summary>        /// ѡ���ͼƬ���        /// </summary>
        public int textureIndex;
        /// <summary>        /// ������������Ҳ����Ҫ���ɵ�����������һ����64������        /// </summary>
        public int groupCount;
    }

    /// <summary>    /// ��ɫ��Ŀ�Ŀ�ѡģʽ    /// </summary>
    public enum ColorIndexMode
    {
        /// <summary>   /// ȫ�ף���͸����͸�����м�һ����Ϊ��ɫ    /// </summary>
        AlphaToAlpha = 0,
        /// <summary>        /// ͸����͸�����м��������Ļƹ�        /// </summary>
        HighlightAlphaToAlpha = 1,
        /// <summary>        /// ǿ�⣬ǰ�治͸��������͸��        /// </summary>
        HighlightToAlpha = 2,
        /// <summary>        /// ��͸��        /// </summary>
        ToAlpha = 3,
    }

    public enum SizeCurveMode
    {
        /// <summary>        /// ��С��������͹        /// </summary>
        SmallToBig_Epirelief = 0,
        /// <summary>        /// ��С�����ٵ�С��������̬�ֲ�����        /// </summary>
        Small_Hight_Small = 1,
        /// <summary>        /// ��С������͹        /// </summary>
        SmallToBig_Subken = 2,
    }

    #endregion
}