using UnityEngine;
using UnityEngine.Rendering;

namespace DefferedRender
{
    enum CameraRenderMode
    {
        _CopyBilt = 0,
        _CopyDepth = 1,
    }

    public partial class DefferRender
    {
        const string bufferName = "Render Camera";

        static ShaderTagId
            litShaderTagId = new ShaderTagId("FowardShader"),       //͸�����壬ʹ��ǰ����Ⱦ
            gBufferShaderTagId = new ShaderTagId("OutGBuffer");     //���GBuffer

        //�洢GBuffer������
        static int[] gBufferIds = 
        {
            Shader.PropertyToID("_GBufferColorTex"),    //��ɫ����
            Shader.PropertyToID("_GBufferNormalTex"),   //��������
            Shader.PropertyToID("_GBufferSpecularTex"), //�߹�����
            Shader.PropertyToID("_GBufferBakeTex"),     //�決���Է�������
        };

        /// <summary>        /// ��һ֡����������������SSS����        /// </summary>
        RenderTexture preFrameFinalTex;

        /// <summary>	/// һ��ȫ��ͼ����û�з�������ʱ�Ͳ����������� 	/// </summary>
        Texture2D missingTexture;

        //�洢һ���id
        static int
            colorAttachmentId = Shader.PropertyToID("_CameraColorAttachment"),
            cameraColorTexId = Shader.PropertyToID("_CameraColorTexture"),
            cameraDepthTexId = Shader.PropertyToID("_CameraDepthTexture"),
            cameraNormalTexId = Shader.PropertyToID("_CameraNormalTexture"),
            gBufferDepthId = Shader.PropertyToID("_GBufferDepthTex"),
            frustumCornersRayId = Shader.PropertyToID("_FrustumCornersRay"),
            inverseVPMatrixId = Shader.PropertyToID("_InverseVPMatrix"),
            tempRenderTexId = Shader.PropertyToID("_TempRenderTexture"),
            sourceTextureId = Shader.PropertyToID("_SourceTexture"),
            inverseProjectionMatrix = Shader.PropertyToID("_InverseProjectionMatrix"),
            viewToScreenMatrixId = Shader.PropertyToID("_ViewToScreenMatrix"),
            cameraProjectMatrixId = Shader.PropertyToID("_CameraProjectionMatrix"),
            worldToCameraMatrixId = Shader.PropertyToID("_WorldToCamera"),
            screenSizeId = Shader.PropertyToID("_ScreenSize");


        CommandBuffer buffer = new CommandBuffer
        {
            name = bufferName
        };

        Camera camera;
        ScriptableRenderContext context;
        CullingResults cullingResults;
        /// <summary>        /// �ƹ⴦����        /// </summary>
        Lighting lighting = new Lighting();
        PostFXSetting defaultPostSetting = default;

        bool useHDR;
        RenderSetting renderSetting;
        /// <summary>        /// ��Ⱦʱ���к����õĲ���        /// </summary>
        Material material;

        //int sssPyramidId;
        PostFXStack postFXStack = new PostFXStack();
        RenderTargetIdentifier[] gBuffers;
        public DefferRender(Shader shader)
        {
            //����һ������������Ⱦbuffer
            material = CoreUtils.CreateEngineMaterial(shader);

            missingTexture = new Texture2D(1, 1)
            {
                hideFlags = HideFlags.HideAndDontSave,
                name = "Missing"
            };
            //��ֵ���أ���Ϊ��ֻ��1������
            missingTexture.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.1f));
            //���б仯������ô���ú����û��Ӱ��
            missingTexture.Apply(true, true);
        }

        public void Render(RenderSetting render,
            ScriptableRenderContext context, Camera camera, 
            ShadowSetting shadowSetting, PostFXSetting postFXSetting)
        {
            this.camera = camera;
            this.context = context;
            renderSetting = render;

            PrepareBuffer();
            PrepareForSceneWindow();        //׼��UI����

            PostFXSetting thisCameraSetting = camera.GetComponent<DefferPipelineCamera>()?.Settings;
            if (thisCameraSetting != null)
                postFXSetting = thisCameraSetting;
            if (postFXSetting == null)
                postFXSetting = defaultPostSetting;


            //׼���޳�����
            if (!Cull(shadowSetting.maxDistance))
            {   //������޳�׼��
                return;     //׼��ʧ�ܾ��˳�
            }

            useHDR = render.allowHDR && camera.allowHDR;

            buffer.BeginSample(SampleName);
            ExecuteBuffer();
            //֮��Ӻ����ƹ������׼��

            //׼���ƹ����ݣ��ڵƹ������л������Ӱ����׼��
            lighting.Setup(
                context, renderSetting.maskLight ? renderSetting.renderingLayerMask : -1,
                cullingResults, shadowSetting, camera, renderSetting.clusterLightSetting
            );

            postFXStack.Setup(context, camera, postFXSetting, useHDR);

            buffer.EndSample(SampleName);

            //���������Ⱦ׼��
            Setup();

            DrawGBuffer();
            //DrawUnsupportedShaders();   //���Ʋ�֧�ֵ�����

            DrawGizmosBeforeFX();       //�ں���ǰ׼��һ��Gizmos��Ҫ������

            if (postFXStack.IsActive)
            {
                SavePreFrameTex(postFXSetting);
                postFXStack.Render(colorAttachmentId);
            }
            else
            {
                DrawFinal();
            }


            DrawGizmosAfterFX();    //�������յ�GizmosЧ��

            Cleanup();              //�������
            Submit();
        }

        /// <summary>	/// ִ����������ݽ�����Ӱ�޳���������Ҫ�Ĳ��ֽ����޳�	/// </summary>
        /// <param name="maxShadowDistance">�޳�����</param>
        /// <returns>�Ƿ�ɹ��޳�</returns>
        bool Cull(float maxShadowDistance)
        {
            GPUDravinDrawStack.Instance.SetUp(context, buffer, camera);
            //ִ���޳����п����޳�ʧ�ܣ���ΪҪ��������
            if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
            {
                //������Ӱ����
                p.shadowDistance = Mathf.Min(maxShadowDistance, camera.farClipPlane);
                //��ֵ�ü����
                cullingResults = context.Cull(ref p);
                return true;
            }
            return false;
        }

        /// <summary>	/// ��װһ��Bufferд�뺯�����������	/// </summary>
        void ExecuteBuffer()
        {
            context.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }

        /// <summary>	/// ��Ⱦ��ʼ��׼������  	/// </summary>
        void Setup()
        {
            //׼����������ݣ��޳�֮���
            context.SetupCameraProperties(camera);

            //GBuffer������Ŀ������
            buffer.GetTemporaryRT(
                colorAttachmentId, camera.pixelWidth, camera.pixelHeight,
                0, FilterMode.Bilinear, useHDR ?
                    RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default
            );

            //��ȡ���ͼ
            buffer.GetTemporaryRT(
                gBufferDepthId, camera.pixelWidth, camera.pixelHeight,
                32, FilterMode.Point, RenderTextureFormat.Depth
            );

            buffer.GetTemporaryRT(
                tempRenderTexId, camera.pixelWidth, camera.pixelHeight,
                0, FilterMode.Bilinear, useHDR ?
                RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default
            );

            gBuffers = new RenderTargetIdentifier[gBufferIds.Length];

            //׼����GBuffer�������Է��⣬��Ϊ�Է�����Ҫ���HDR
            for (int i=0; i<gBufferIds.Length - 1; i++)
            {
                buffer.GetTemporaryRT(
                    gBufferIds[i], camera.pixelWidth, camera.pixelHeight,
                    0, FilterMode.Bilinear, RenderTextureFormat.Default
                );
                gBuffers[i] = gBufferIds[i];
            }
            buffer.GetTemporaryRT(
                gBufferIds[gBufferIds.Length - 1], camera.pixelWidth, camera.pixelHeight,
                0, FilterMode.Bilinear, useHDR ?
                RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default
            );
            gBuffers[gBufferIds.Length - 1] = gBufferIds[gBufferIds.Length - 1];

            //������ȾĿ�꣬�������е���ȾĿ��
            buffer.SetRenderTarget(
                gBuffers,
                gBufferDepthId
            );

            buffer.BeginSample(SampleName);

            //�����һ֡������
            buffer.ClearRenderTarget(true, true, Color.clear);

            Matrix4x4 frustum = GetFrustumMatrix();
            buffer.SetGlobalMatrix(frustumCornersRayId, frustum);

            Matrix4x4 matrix4X4 = camera.projectionMatrix * camera.worldToCameraMatrix;
            buffer.SetGlobalMatrix(inverseVPMatrixId, matrix4X4.inverse);
            buffer.SetGlobalMatrix(inverseProjectionMatrix, camera.projectionMatrix.inverse);

            Matrix4x4 clipToScreenMatrix = Matrix4x4.identity;
            float width = camera.pixelWidth, height = camera.pixelHeight;
            clipToScreenMatrix.SetRow(0, new Vector4(width * 0.5f, 0, 0, width * 0.5f));
            clipToScreenMatrix.SetRow(1, new Vector4(0, height * 0.5f, 0, height * 0.5f));
            clipToScreenMatrix.SetRow(2, new Vector4(0, 0, 1.0f, 0));
            clipToScreenMatrix.SetRow(3, new Vector4(0, 0, 0, 1.0f));
            var viewToScreenMatrix = clipToScreenMatrix * camera.projectionMatrix;
            buffer.SetGlobalMatrix(viewToScreenMatrixId, viewToScreenMatrix);

            buffer.SetGlobalVector(screenSizeId, new Vector4(1.0f / width, 1.0f / height, width, height));
            buffer.SetGlobalMatrix(cameraProjectMatrixId, camera.projectionMatrix);
            buffer.SetGlobalMatrix(worldToCameraMatrixId, camera.worldToCameraMatrix);
            ExecuteBuffer();
        }

        void DrawGBuffer()
        {
            PerObjectData lightsPerObjectFlags = PerObjectData.None;
            //���ø����������������ģʽ��Ŀǰ����Ⱦ��ͨ���壬�����һ������ʽ
            var sortingSettings = new SortingSettings(camera)
            {
                criteria = SortingCriteria.CommonOpaque
            };

            //��һ����Ⱦֻ����GBuffer����GBuffer����Ⱦ��͸��
            var drawingSettings = new DrawingSettings(
                gBufferShaderTagId, sortingSettings
            )
            {
                enableDynamicBatching = renderSetting.useDynamicBatching,
                enableInstancing = renderSetting.useGPUInstancing,
                perObjectData =
                PerObjectData.ReflectionProbes |
                PerObjectData.Lightmaps | PerObjectData.ShadowMask |
                PerObjectData.LightProbe | PerObjectData.OcclusionProbe |
                PerObjectData.LightProbeProxyVolume |
                PerObjectData.OcclusionProbeProxyVolume |
                lightsPerObjectFlags
            };
            var filteringSettings = new FilteringSettings(
                RenderQueueRange.opaque, renderingLayerMask: (uint)renderSetting.renderingLayerMask
            );
            //������Ⱦ��ִ�з���
		    context.DrawRenderers(
			    cullingResults, ref drawingSettings, ref filteringSettings
		    );

            //��ȾGPU�����ı�׼PBR����
            GPUDravinDrawStack.Instance.DrawPreSSS(context, buffer, camera);
            ExecuteBuffer();

            //������պ�
            context.DrawSkybox(camera);

            buffer.GetTemporaryRT(cameraDepthTexId, camera.pixelWidth, camera.pixelHeight,
                32, FilterMode.Point, RenderTextureFormat.Depth);
            buffer.GetTemporaryRT(cameraNormalTexId, camera.pixelWidth, camera.pixelHeight,
                0, FilterMode.Bilinear, RenderTextureFormat.Default);
            Draw(gBufferDepthId, cameraDepthTexId, CameraRenderMode._CopyDepth);    //Save Depth
            Draw(gBufferIds[1], cameraNormalTexId, CameraRenderMode._CopyBilt);     //Save Normal

            //������ȾĿ�꣬�������е���ȾĿ��
            buffer.SetRenderTarget(
                gBuffers,
                gBufferDepthId
            );
            ExecuteBuffer();

            //׼�������SSS����
            GPUDravinDrawStack.Instance.DrawOtherSSS(context, buffer, camera);

            DrawGBufferFinal();         //BRDF

            buffer.GetTemporaryRT(cameraColorTexId, camera.pixelWidth, camera.pixelHeight,
                0, FilterMode.Bilinear, useHDR ?
                RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default);

            Draw(colorAttachmentId, cameraColorTexId, CameraRenderMode._CopyBilt);



            sortingSettings.criteria = SortingCriteria.CommonTransparent;
            drawingSettings.sortingSettings = sortingSettings;
            filteringSettings.renderQueueRange = RenderQueueRange.transparent;
            drawingSettings.SetShaderPassName(0, litShaderTagId);

            buffer.SetRenderTarget(
                colorAttachmentId, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store,
                gBufferDepthId, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);

            //ExecuteBuffer();

            context.DrawRenderers(
                cullingResults, ref drawingSettings, ref filteringSettings
            );

            //����ComputeShaderʵ�ֵ�����
            GPUDravinDrawStack.Instance.BeginDraw(context, buffer, 
                ClustDrawType.Simple, camera);
            ExecuteBuffer();

        }

        /// <summary>        /// ȷ��GBuffer������ɫ        /// </summary>
        void DrawGBufferFinal()
        {
            postFXStack.DrawSSS(preFrameFinalTex);
            postFXStack.DrawGBufferFinal(colorAttachmentId, missingTexture);
            ExecuteBuffer();
        }

        /// <summary>        /// �����������        /// </summary>
        /// <param name="from">��������</param>
        /// <param name="to">Ŀ������</param>
        /// <param name="isDepth">�Ƿ�Ϊ���</param>
        void Draw(
            RenderTargetIdentifier from, RenderTargetIdentifier to, CameraRenderMode mode
        )
        {
            buffer.SetGlobalTexture(sourceTextureId, from);
            buffer.Blit(null, to, material, (int)mode);
        }

        void DrawFinal()
        {
            buffer.SetGlobalTexture(sourceTextureId, colorAttachmentId);

            buffer.Blit(null, BuiltinRenderTextureType.CameraTarget,
                material, (int)CameraRenderMode._CopyBilt);

            ExecuteBuffer();
        }

        void SavePreFrameTex(PostFXSetting postFXSetting)
        {
            if (camera.cameraType != CameraType.Game)
                return;

            //��ǰ�洢����Ҫ������ٴ洢
            if (preFrameFinalTex != null)
                RenderTexture.ReleaseTemporary(preFrameFinalTex);
            if (postFXSetting.ssr.useSSR)
            {
                preFrameFinalTex = RenderTexture.GetTemporary(camera.pixelWidth, camera.pixelHeight,
                    0, RenderTextureFormat.Default);
                preFrameFinalTex.name = "PerFrameFinalTexture";
                Draw(colorAttachmentId, preFrameFinalTex, CameraRenderMode._CopyBilt);
            }
            ExecuteBuffer();
        }

        /// <summary>	/// ���ʹ�ù������ݣ���Ϊ����ͼ������������ڴ��еģ������Ҫ�����ֶ��ͷ�	/// </summary>
        void Cleanup()
        {
            buffer.ReleaseTemporaryRT(colorAttachmentId);
            buffer.ReleaseTemporaryRT(gBufferDepthId);
            buffer.ReleaseTemporaryRT(tempRenderTexId);
            buffer.ReleaseTemporaryRT(cameraColorTexId);
            buffer.ReleaseTemporaryRT(cameraDepthTexId);
            buffer.ReleaseTemporaryRT(cameraNormalTexId);
            for(int i=0; i<gBufferIds.Length; i++)
            {
                buffer.ReleaseTemporaryRT(gBufferIds[i]);
            }

            lighting.Cleanup();		//�ƹ��������
        }

        /// <summary>        /// �ύ�����������������ϴ�        /// </summary>
        void Submit()
        {
            buffer.EndSample(SampleName);
            ExecuteBuffer();
            context.Submit();
        }

        Matrix4x4 GetFrustumMatrix()
        {
            Matrix4x4 frustumCorners = Matrix4x4.identity;
            Transform cameraTransform = camera.transform;
            float fov = camera.fieldOfView;
            float near = camera.nearClipPlane;
            //aspect = width / height
            float aspect = camera.aspect;

            //�����ƽ��ĸ߶ȣ�fov*0.5*Mathf.Deg2Red������������һ��Ƕ�ֵ��ʹ��tan��ֵ���Ǹ߶��ˣ����廭��ͼ
            float halfHeight = near * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
            //halfHeight * aspect���width��С�������䷽����ȥ���ұ�Ե�ķ���
            Vector3 toRight = cameraTransform.right * halfHeight * aspect;
            //ͬ����õ�������ķ���
            Vector3 toTop = cameraTransform.up * halfHeight;

            //���Ϸ�
            Vector3 topLeft = cameraTransform.forward * near + toTop - toRight;
            //��õ�nearΪ1ʱ���������Ϸ��ĳ���
            float scale = topLeft.magnitude / near;
            //��׼���÷���
            topLeft.Normalize();
            //���Ŵ�С��ʹ���Ϊ��nearΪ1ʱ�Ĵ�С
            topLeft *= scale;

            //���Ϸ�
            Vector3 topRight = cameraTransform.forward * near + toRight + toTop;
            topRight.Normalize();
            topRight *= scale;

            //���·�
            Vector3 bottomLeft = cameraTransform.forward * near - toTop - toRight;
            bottomLeft.Normalize();
            bottomLeft *= scale;

            //���·�
            Vector3 bottomRight = cameraTransform.forward * near + toRight - toTop;
            bottomRight.Normalize();
            bottomRight *= scale;

            //����ȷ���ˣ���nearΪ1ʱ��ԭ�㵽���ĸ�����Ķ���ķ������ǰ������ȵķ���ֵ
            //���潫��Щ���ݴ��ݸ�����
            frustumCorners.SetRow(0, bottomLeft);
            frustumCorners.SetRow(1, bottomRight);
            frustumCorners.SetRow(2, topRight);
            frustumCorners.SetRow(3, topLeft);

            return frustumCorners;
        }


    }
}