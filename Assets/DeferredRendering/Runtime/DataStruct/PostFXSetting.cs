using System;
using UnityEngine;

namespace DefferedRender
{



	[CreateAssetMenu(menuName = "Rendering/Post FX Settings")]
    public class PostFXSetting : ScriptableObject
	{
		[SerializeField]
		Shader postFXShader = default;
        public enum LUTSize { 
			_16x = 16, _32x = 32, _64x = 64
		}

		[SerializeField]
		LUTSize colorLUTResolution = LUTSize._32x;

		public LUTSize LUTResolution => colorLUTResolution;

		[Serializable]
		public struct SSR
		{
			public bool useSSR;
			public int rayMarchingSetp;
			public float marchSetpSize;
			public float maxMarchDistance;
			public float depthThickness;
		}

			[SerializeField]
		SSR ssrSetting = new SSR
		{
			rayMarchingSetp = 36,
			marchSetpSize = 10,
			maxMarchDistance = 500,
			depthThickness = 1,
		};
		public SSR ssr => ssrSetting;

		/// <summary>	/// �������㣬���������ݳ�	/// </summary>
		[Serializable]
		public struct BulkLight
		{
			public bool useBulkLight;
			[Range(0, 100f)]
			public float shrinkRadio;
			[Range(10, 100)]
			public int circleCount;
			[Range(0, 1)]
			public float scatterRadio;
			public float checkDistance;
		}

		[SerializeField]
		BulkLight bulkLight = new BulkLight
		{
			useBulkLight = false,
			shrinkRadio = 0.00005f,
			checkDistance = 100,
			circleCount = 64,
		};
		public BulkLight BulkLighting => bulkLight;

		/// <summary>	/// Bloom��������	/// </summary>
		[System.Serializable]
		public struct BloomSettings
		{

			/// <summary>		/// ����ȼ�		/// </summary>
			[Range(0f, 16f)]
			public int maxIterations;

			/// <summary>		/// Bloom���е���С�����أ�������С�ڸ�ֵ�Ͳ�������һ��		/// </summary>
			[Min(1f)]
			public int downscaleLimit;

			/// <summary>		/// �Ƿ�ʹ�������Բ�ֵ		/// </summary>
			public bool bicubicUpsampling;

			/// <summary>		/// Bloom�ķָ���		/// </summary>
			[Min(0f)]
			public float threshold;

			/// <summary>		/// �ָ����½��ľ��ҳ̶ȣ�����ֱ�ӽض�		/// </summary>
			[Range(0f, 1f)]
			public float thresholdKnee;

			/// <summary>		/// Bloom�������������ǿ��		/// </summary>
			[Min(0f)]
			public float intensity;

			/// <summary>		/// �Ƿ�ʹ�÷�Χ��ɫ���ƣ���������ɫ����ʱ��������Χ����ɫ		/// </summary>
			public bool fadeFireflies;

			/// <summary>		/// Bloom�Ļ��ģʽ������ӻ���lerp���		/// </summary>
			public enum Mode { Additive, Scattering }

			public Mode mode;

			/// <summary>		/// lerp��ϵı���		/// </summary>
			[Range(0.05f, 0.95f)]
			public float scatter;
		}

		[SerializeField]
		BloomSettings bloom = new BloomSettings
		{
			scatter = 0.7f,
			maxIterations = 1,
			downscaleLimit = 30,
			threshold = 1,
			thresholdKnee = 0.2f,
			intensity = 0.5f,
		};

		public BloomSettings Bloom => bloom;

		/// <summary>	/// HDRӳ�䷽ʽ	/// </summary>
		[Serializable]
        public struct ToneMappingSettings
        {

            public enum Mode { None, ACES, Neutral, Reinhard }

            public Mode mode;
        }

        [SerializeField]
        ToneMappingSettings toneMapping = default;

		public ToneMappingSettings ToneMapping => toneMapping;


		/// <summary>	/// ��ɫֵ����	/// </summary>
		[Serializable]
		public struct ColorAdjustmentsSettings
		{

			/// <summary>		/// �ع��		/// </summary>
			public float postExposure;

			/// <summary>		/// �Աȶ�		/// </summary>
			[Range(-100f, 100f)]
			public float contrast;
			/// <summary>		/// ��ɫ����		/// </summary>
			[ColorUsage(false, true)]
			public Color colorFilter;

			/// <summary>		/// ɫ��ת��		/// </summary>
			[Range(-180f, 180f)]
			public float hueShift;

			/// <summary>		/// ���Ͷ�		/// </summary>
			[Range(-100f, 100f)]
			public float saturation;
		}

		[SerializeField]
		ColorAdjustmentsSettings colorAdjustments = new ColorAdjustmentsSettings
		{
			colorFilter = Color.white
		};

		public ColorAdjustmentsSettings ColorAdjustments => colorAdjustments;

		public void SetColorFilter(Color filter)
        {
			colorAdjustments.colorFilter = filter;
		}

		/// <summary>	/// ��ƽ���������	/// </summary>
		[Serializable]
		public struct WhiteBalanceSettings
		{

			/// <summary>		/// ɫ���Լ�ɫ��ǿ��		/// </summary>
			[Range(-100f, 100f)]
			public float temperature, tint;
		}

		[SerializeField]
		WhiteBalanceSettings whiteBalance = default;

		public WhiteBalanceSettings WhiteBalance => whiteBalance;

		/// <summary>	/// ɫ������	/// </summary>
		[Serializable]
		public struct SplitToningSettings
		{

			/// <summary>		/// ��Ӱ��ɫ�͸�����ɫ		/// </summary>
			[ColorUsage(false)]
			public Color shadows, highlights;

			/// <summary>		/// ƽ���		/// </summary>
			[Range(-100f, 100f)]
			public float balance;
		}

		[SerializeField]
		SplitToningSettings splitToning = new SplitToningSettings
		{
			shadows = Color.gray,
			highlights = Color.gray
		};

		public SplitToningSettings SplitToning => splitToning;


		[Serializable]
		public struct FogSetting
        {
			public Texture fogTex;
			public bool useFog;

			public float fogMaxHeight;
			public float fogMinHeight;

			[Range(0, 1)]
			public float fogMaxDepth;
			[Range(0, 1)]
			public float fogMinDepth;

			[Range(0.001f, 3)]
			public float fogDepthFallOff;
			[Range(0.001f, 3)]
			public float fogPosYFallOff;

			public Gradient colors;		//��ɫ��ֻ����ƹ���ɫ
		}

		[SerializeField]
		FogSetting fog = new FogSetting
		{
			useFog = false,
			fogMaxHeight = 100,
			fogMinHeight = 0,
			fogMaxDepth = 1,
			fogMinDepth = 0.2f,
			fogDepthFallOff = 1,
			fogPosYFallOff = 1,
		};
		public FogSetting Fog => fog;

		/// <summary>		/// ���ֿ����ģʽ		/// </summary>
		public enum LuminanceMode { None, Green, Calculate }
		/// <summary>		/// ���������		/// </summary>
		[Serializable]
		public struct FXAASetting
        {
			public LuminanceMode luminanceMode;

			/// <summary>	/// �Աȶ���ֵ	/// </summary>
			[Range(0.0312f, 0.0833f)]
			public float contrastThreshold;
			[Range(0.063f, 0.333f)]
			public float relativeThreshold;		//�Աȶȸ߶���ֵ����ȥ�ߵĲ���
			[Range(0f, 1f)]
			public float subpixelBlending;      //ģ���̶ȿ��ƣ�����ϸ����ʾ����
			/// <summary>			/// �ߵ���������			/// </summary>
			public bool lowQuality;

		}

		[SerializeField]
		FXAASetting fXAA = new FXAASetting
		{
			contrastThreshold = 0.0312f,
			relativeThreshold = 0.063f,
			subpixelBlending = 1f
		};
		public FXAASetting FXAA => fXAA;

		/// <summary>		/// ���������		/// </summary>
		[Serializable]
		public struct RotateTextureSetting
		{
            [Range(0, 3.14f)]
			public float rotateRadio;
			/// <summary>/// �����ж��Ƿ���Ҫ��ת/// </summary>
			public bool isRotate;
		}

		[SerializeField]
		RotateTextureSetting rotateSetting = new RotateTextureSetting
		{
			rotateRadio = 0,
			isRotate = false,
		};
		public RotateTextureSetting RotateSetting => rotateSetting;

		/// <summary>/// ����ͼƬ��ת�ı�������0-1֮���ֵ	/// </summary>
		public void SetRotateRadio(float radio01)
        {
			rotateSetting.rotateRadio = Mathf.Lerp(0f, 3.14f, radio01);
		}
		/// <summary>/// ����ͼƬ��ת/// </summary>
		public void EnableRotate()
        {
			rotateSetting.isRotate = true;
		}
		/// <summary>/// �ر�ͼƬ��ת/// </summary>
		public void DisableRotate()
		{
			rotateSetting.isRotate = false;
		}

		/// <summary>		/// ͼƬ��������		/// </summary>
		[Serializable]
		public struct WaveTexture
		{
			[Range(0.0001f, 0.1f)]
			/// <summary>/// ������С/// </summary>
			public float waveSize;
			[Range(100f, 500f)]
			/// <summary>/// ��������/// </summary>
			public float waveLength;

			public bool isWave;
		}
		[SerializeField]
		WaveTexture waveSettings = new WaveTexture
		{
			waveSize = 0005f,
			waveLength = 300,
			isWave = false,
		};
		public WaveTexture WaveSetting => waveSettings;
		/// <summary>/// ��������������/// </summary>
		public void BeginWave()
        {
			waveSettings.isWave = true;
        }
		/// <summary>/// �رղ���������/// </summary>
		public void EndWave()
		{
			waveSettings.isWave = false;
		}


		Material material;


		public Material Material
		{
			get
			{
				if (material == null && postFXShader != null)
				{
					material = new Material(postFXShader);
					material.hideFlags = HideFlags.HideAndDontSave;
				}
				return material;
			}
		}
	}
}