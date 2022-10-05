//实现UI特效用的特殊材质
Shader "Defferer/UIEffectShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        [Toggle(_CLIPPING)] _Clipping ("Alpha Clipping", Float) = 0
        _Cutoff ("Cut Off Value", Range(0, 1)) = 0.2
    }
    SubShader
    {
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            HLSLPROGRAM

            #pragma target 4.6

            #pragma vertex vert
            #pragma fragment frag

            
            #include "../../ShaderLibrary/Common.hlsl"
            #include "../../ShaderLibrary/Fragment.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float _Cutoff;
            float4 _Color;

            struct Attributes2D
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings2D
            {
                float4 positionCS_SS   : SV_POSITION;
                float2 baseUV  : TEXCOORD0;
                float4 color : COLOR;
            };
                        
            Varyings2D vert(Attributes2D input)
            {
                Varyings2D output;
                output.positionCS_SS = TransformObjectToHClip(input.vertex.xyz);
                output.baseUV = input.texcoord;
                output.color = input.color;
                return output;
            }

            float4 frag(Varyings2D i) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.baseUV.xy) * i.color;
                clip(color.a - _Cutoff);
                return color;
            }


            ENDHLSL
        }
    }
}
