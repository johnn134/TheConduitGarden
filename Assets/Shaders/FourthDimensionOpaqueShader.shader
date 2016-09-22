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
				float4 posWorld : TEXCOORD2;
				float3 normalDir : TEXCOORD3;
			};
			
			vertexOutput vert (vertexInput input)
			{
				vertexOutput o;

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				o.posWorld = mul(modelMatrix, input.vertex);
				o.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				o.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				o.col = _Color;
				o.tex = input.texcoord;

				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}
			
			float4 frag (vertexOutput input) : COLOR
			{
				float3 lightDirection;
				float attenuation;

				float3 normalDirection = normalize(input.normalDir);
				float4 textureColor = tex2D(_MainTex, input.tex.xy);
				float3 fragmentColor = _Color.rgb;

				if(_WorldSpaceLightPos0.w == 0.0)
				{
					attenuation = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else
				{
					float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
					float distance = length(vertexToLightSource);
					attenuation = 1.0 / distance;
					lightDirection = normalize(vertexToLightSource);
				}

				if(attenuation * dot(normalDirection, lightDirection) <= 0)
				{
					fragmentColor = _Color.rgb * 0.25;
				}

				return textureColor * float4(fragmentColor, 1.0) * LIGHT_ATTENUATION(input);
			}
			ENDCG
		}
	}

	Fallback "Diffuse"
}
