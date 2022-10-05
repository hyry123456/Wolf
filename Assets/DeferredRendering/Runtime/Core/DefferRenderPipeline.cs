using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DefferedRender
{
    /// <summary>
    /// Deffer Render invoke class��
    /// �����࣬����ʵ�ʴ�������д���
    /// </summary>
    public partial class DefferRenderPipeline : RenderPipeline
    {
        RenderSetting setting;
        DefferRender renderer;
        ShadowSetting shadow;
        PostFXSetting postFX;

        public DefferRenderPipeline(RenderSetting setting, ShadowSetting shadow,
            PostFXSetting postFXSetting)
        {
            this.setting = setting;
            renderer = new DefferRender(setting.cameraShader);
            this.shadow = shadow;
            postFX = postFXSetting;
        }


        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (Camera camera in cameras)
            {
                //����ʵ����Ⱦ�࣬�Ե��������������Ⱦ
                renderer.Render(setting,
                    context, camera, shadow, postFX);
            }
        }
    }
}