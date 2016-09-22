Shader "FourthDimension/FourthDimensionOpaqueShader"
{
	Properties
	{
		_Color ("Material Color", Color) = (1,1,1,1)
		_MainTex ("Material Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}

		//Opaque Shader Pass
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _Color;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				LIGHTING_COORDS(0,1)
				float4 tex : TEXCOORD1;
				float3 col : COLOR;
			};
			
			vertexOutput vert (vertexInput input)
			{
				vertexOutput o;

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				float3 normalDirection = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

				float lambertWeight = max(0.1, dot(normalDirection, lightDirection));

				o.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				o.col = _Color * lambertWeight * 1.0;
				o.tex = input.texcoord;

				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}
			
			float4 frag (vertexOutput input) : COLOR
			{
				return tex2D(_MainTex, input.tex.xy) * float4(input.col, 1.0) * LIGHT_ATTENUATION(input);
			}
			ENDCG
		}
	}

	Fallback "Diffuse"
}
