Shader "FourthDimension/CubeMapSkybox"
{
	Properties
	{
		_Cube ("CubeMap", CUBE) = "" {}
	}
	SubShader
	{
		Tags { "Queue"="Background" "RenderType"="Background" }

		LOD 200
		Cull Off
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			samplerCUBE _Cube;

			struct vertexOutput
			{
				float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD0;
			};
			
			vertexOutput vert (appdata_base v)
			{
				vertexOutput o;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.normal = v.normal;

				return o;
			}
			
			half4 frag (vertexOutput i) : COLOR
			{
				return texCUBE(_Cube, i.normal);
			}
			ENDCG
		}
	}

	FallBack "Diffuse"
}
