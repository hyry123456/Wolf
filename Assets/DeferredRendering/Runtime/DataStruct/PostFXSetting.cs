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

		/// <summary>	/// 体积光计算，这个是真的奢侈	/// </summary>
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

		/// <summary>	/// Bloom参数设置	/// </summary>
		[System.Serializable]
		public struct BloomSettings
		{

			/// <summary>		/// 渐变等级		/// </summary>
			[Range(0f, 16f)]
			public int maxIterations;

			/// <summary>		/// Bloom进行到最小的像素，像素量小于该值就不进行下一步		/// </summary>
			[Min(1f)]
			public int downscaleLimit;

			/// <summary>		/// 是否使用三线性插值		/// </summary>
			public bool bicubicUpsampling;

			/// <summary>		/// Bloom的分割线		/// </summary>
			[Min(0f)]
			public float threshold;

			/// <summary>		/// 分割线下降的剧烈程度，不是直接截断		/// </summary>
			[Range(0f, 1f)]
			public float thresholdKnee;

			/// <summary>		/// Bloom叠加在主纹理的强度		/// </summary>
			[Min(0f)]
			public float intensity;

			/// <summary>		/// 是否使用范围颜色限制，即进行颜色限制时采样了周围的颜色		/// </summary>
			public bool fadeFireflies;

			/// <summary>		/// Bloom的混合模式，是添加还是lerp混合		/// </summary>
			public enum Mode { Additive, Scattering }

			public Mode mode;

			/// <summary>		/// lerp混合的比例		/// </summary>
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

		/// <summary>	/// HDR映射方式	/// </summary>
		[Serializable]
        public struct ToneMappingSettings
        {

            public enum Mode { None, ACES, Neutral, Reinhard }

            public Mode mode;
        }

        [SerializeField]
        ToneMappingSettings toneMapping = default;

		public ToneMappingSettings ToneMapping => toneMapping;


		/// <summary>	/// 颜色值调整	/// </summary>
		[Serializable]
		public struct ColorAdjustmentsSettings
		{

			/// <summary>		/// 曝光度		/// </summary>
			public float postExposure;

			/// <summary>		/// 对比度		/// </summary>
			[Range(-100f, 100f)]
			public float contrast;
			/// <summary>		/// 颜色过滤		/// </summary>
			[ColorUsage(false, true)]
			public Color colorFilter;

			/// <summary>		/// 色相转移		/// </summary>
			[Range(-180f, 180f)]
			public float hueShift;

			/// <summary>		/// 饱和度		/// </summary>
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

		/// <summary>	/// 白平衡阐述控制	/// </summary>
		[Serializable]
		public struct WhiteBalanceSettings
		{

			/// <summary>		/// 色温以及色温强度		/// </summary>
			[Range(-100f, 100f)]
			public float temperature, tint;
		}

		[SerializeField]
		WhiteBalanceSettings whiteBalance = default;

		public WhiteBalanceSettings WhiteBalance => whiteBalance;

		/// <summary>	/// 色调分离	/// </summary>
		[Serializable]
		public struct SplitToningSettings
		{

			/// <summary>		/// 阴影颜色和高亮颜色		/// </summary>
			[ColorUsage(false)]
			public Color shadows, highlights;

			/// <summary>		/// 平衡度		/// </summary>
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

			public Gradient colors;		//颜色，只计算灯光颜色
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

		/// <summary>		/// 三种抗锯齿模式		/// </summary>
		public enum LuminanceMode { None, Green, Calculate }
		/// <summary>		/// 抗锯齿设置		/// </summary>
		[Serializable]
		public struct FXAASetting
        {
			public LuminanceMode luminanceMode;

			/// <summary>	/// 对比度阈值	/// </summary>
			[Range(0.0312f, 0.0833f)]
			public float contrastThreshold;
			[Range(0.063f, 0.333f)]
			public float relativeThreshold;		//对比度高度阈值，舍去高的部分
			[Range(0f, 1f)]
			public float subpixelBlending;      //模糊程度控制，调整细节显示比例
			/// <summary>			/// 高低质量控制			/// </summary>
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

		/// <summary>		/// 抗锯齿设置		/// </summary>
		[Serializable]
		public struct RotateTextureSetting
		{
            [Range(0, 3.14f)]
			public float rotateRadio;
			/// <summary>/// 用来判断是否需要旋转/// </summary>
			public bool isRotate;
		}

		[SerializeField]
		RotateTextureSetting rotateSetting = new RotateTextureSetting
		{
			rotateRadio = 0,
			isRotate = false,
		};
		public RotateTextureSetting RotateSetting => rotateSetting;

		/// <summary>/// 设置图片旋转的比例，在0-1之间的值	/// </summary>
		public void SetRotateRadio(float radio01)
        {
			rotateSetting.rotateRadio = Mathf.Lerp(0f, 3.14f, radio01);
		}
		/// <summary>/// 开启图片旋转/// </summary>
		public void EnableRotate()
        {
			rotateSetting.isRotate = true;
		}
		/// <summary>/// 关闭图片旋转/// </summary>
		public void DisableRotate()
		{
			rotateSetting.isRotate = false;
		}

		/// <summary>		/// 图片波动设置		/// </summary>
		[Serializable]
		public struct WaveTexture
		{
			[Range(0.0001f, 0.1f)]
			/// <summary>/// 波动大小/// </summary>
			public float waveSize;
			[Range(100f, 500f)]
			/// <summary>/// 波动距离/// </summary>
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
		/// <summary>/// 启动波动主纹理/// </summary>
		public void BeginWave()
        {
			waveSettings.isWave = true;
        }
		/// <summary>/// 关闭波动主纹理/// </summary>
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