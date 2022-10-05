#ifndef DEFFER_2D_WATER_PASS
#define DEFFER_2D_WATER_PASS

TEXTURE2D(_WaveTex);
SAMPLER(sampler_WaveTex);

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)

	UNITY_DEFINE_INSTANCED_PROP(float4, _WaterColor)
	UNITY_DEFINE_INSTANCED_PROP(float, _WaterLineHeight)
	UNITY_DEFINE_INSTANCED_PROP(float4, _WaterLineColor)

UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

struct Attributes {
	float3 positionOS : POSITION;
	float2 baseUV : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings {
	float4 positionCS_SS : SV_POSITION;
	float3 positionWS : VAR_POSITION;
	float2 baseUV : VAR_BASE_UV;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};


Varyings vert(Attributes input){
    Varyings output = (Varyings)0;
    UNITY_SETUP_INSTANCE_ID(input);
	UNITY_TRANSFER_INSTANCE_ID(input, output);
    output.positionWS = TransformObjectToWorld(input.positionOS);
    output.positionCS_SS = 
}


#endif