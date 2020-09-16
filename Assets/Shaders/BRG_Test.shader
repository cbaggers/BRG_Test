Shader "BRG_Test"
{
	Properties
	{
		_Color("Color 0", Color) = (0, 0, 1, 0) // Default color blue, we are trying to set this green from the script
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 5.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			half filler;
		};

		UNITY_INSTANCING_BUFFER_START(BRG_Test)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
#define _Color_arr BRG_Test
		UNITY_INSTANCING_BUFFER_END(BRG_Test)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 _Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);
			o.Albedo = _Color_Instance.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}