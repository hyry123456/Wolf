Shader "Unlit/HookRope"
{
    Properties
    {
        _MainTex("Main Tex", 2D) = "White"{}
        _ParticleSize("Particle Size", Float) = 5
        [HDR]_Color("Color", color) = (1,1,1,1)
        _YScale("Y Scale", float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 0
		[Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1

    }
    SubShader
    {

        Pass
        {
            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull Off
			Tags {
                "LightMode" = "FowardShader"
            }

            HLSLPROGRAM

            #pragma target 4.6

            #pragma vertex vert
            #pragma fragment frag


            float3 _TargetPos;
            float3 _BeginPos;
            float _ParticleSize;
            float4 _Color;
            float _YScale;
            
            #include "../../ShaderLibrary/Common.hlsl"
            #include "../../ShaderLibrary/Fragment.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct FragInput
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float color : COLOR;
            };
            struct Attributes {
                float3 positionOS : POSITION;
                float2 baseUV : TEXCOORD0;
            };
            
            FragInput vert(Attributes input)
            {
                FragInput output;
                float3 offset;
                float3 beginToEndOff = _TargetPos - _BeginPos;
                float k = beginToEndOff.y / beginToEndOff.x; k = -1.0/k;    //求斜率
                float3 normal = normalize( float3(1, k, 0) );
                float y;
                if(input.baseUV.x < 0.5){       //左边
                    offset = -normal * _ParticleSize;
                }
                else{
                    offset = normal * _ParticleSize;
                }
                float3 worldPos;
                if(input.baseUV.y < 0.5){       //下边
                    worldPos = _BeginPos + offset;
                    y = 0;
                }
                else{
                    worldPos = _TargetPos + offset;
                    y = distance(_BeginPos, _TargetPos) * _YScale;
                }
                output.pos = TransformWorldToHClip(worldPos);
                output.uv = float2(input.baseUV.x, y);
                output.color = k;
                return output;
            }

            float4 frag(FragInput i) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv.xy) * _Color;

                return color;
            }


            ENDHLSL
        }
    }
}
