Shader "FourthDimension/FourthDimensionOpaqueShader"
{
	Properties
	{
		_Color ("Material Color", Color) = (1,1,1,1)
		_MainTex ("Material Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			Tags {"LightMode"="ForwardBase"}

			CGPROGRAM
			#pragma multi_compile_fwdbase
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			uniform float4 _LightColor0;

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
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
				float4 posWorld : TEXCOORD2;
				float3 normalDir : TEXCOORD3;
				float3 vertexLighting : TEXCOORD4;
				float4 tex : TEXCOORD5;
			};
			
			vertexOutput vert (vertexInput input)
			{
				vertexOutput o;

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				o.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				o.posWorld = mul(modelMatrix, input.vertex);
				o.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				o.tex = input.texcoord;

				o.vertexLighting = float3(0.0, 0.0, 0.0);

				#ifdef VERTEXLIGHT_ON
				for(int index = 0; index < 4; index++) {
					float4 lightPosition = float4(unity_4LightPosX0[index],
													unity_4LightPosY0[index],
													unity_4LightPosZ0[index], 1.0);
					float3 vertexToLightSource = lightPosition.xyz - o.posWorld.xyz;
					float3 lightDirection = normalize(vertexToLightSource);
					float squaredDistance = dot(vertexToLightSource, vertexToLightSource);
					float attenuation = 1.0 / (1.0 + unity_4LightAtten0[index] * squaredDistance);
					float3 diffuseReflection = attenuation
					 * unity_LightColor[index].rgb * _Color.rgb
					 * max(0.0, dot(output.normalDir, lightDirection));

					 o.vertexLighting = o.vertexLighting + diffuseReflection;
				}
				#endif

				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}
			
			float4 frag (vertexOutput input) : COLOR
			{
				float4 textureColor = tex2D(_MainTex, _MainTex_ST.xy * input.tex.xy + _MainTex_ST.zw);
				float3 normalDirection = normalize(input.normalDir);
				float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
				float3 lightDirection;
				float attenuation;

				if(0.0 == _WorldSpaceLightPos0.w) { //directional light
					attenuation = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else {	//point light
					float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
					float distance = length(vertexToLightSource);
					attenuation = 1.0 / distance;
					lightDirection = normalize(vertexToLightSource);
				}

				float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb
					* max(0.1, dot(normalDirection, lightDirection));

				float4 fragmentColor = float4(input.vertexLighting + diffuseReflection, 1.0);

				return fragmentColor * textureColor * max(0.1, LIGHT_ATTENUATION(input));
			}
			ENDCG
		}

		Pass
		{
			Tags {"LightMode"="ForwardAdd"}

			Blend One One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform float4 _LightColor0;

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
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
				float4 posWorld : TEXCOORD0;
				float3 normalDir : TEXCOORD1;
				float4 tex : TEXCOORD3;
			};
			
			vertexOutput vert (vertexInput input)
			{
				vertexOutput o;

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				o.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				o.posWorld = mul(modelMatrix, input.vertex);
				o.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				o.tex = input.texcoord;

				return o;
			}
			
			float4 frag (vertexOutput input) : COLOR
			{
				float4 textureColor = tex2D(_MainTex, _MainTex_ST.xy * input.tex.xy + _MainTex_ST.zw);
				float3 normalDirection = normalize(input.normalDir);
				float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
				float3 lightDirection;
				float attenuation;

				if(0.0 == _WorldSpaceLightPos0.w) { //directional light
					attenuation = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else {	//point light
					float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
					float distance = length(vertexToLightSource);
					attenuation = 1.0 / distance;
					lightDirection = normalize(vertexToLightSource);
				}

				float3 diffuseReflection = attenuation * _LightColor0.rgb * _Color.rgb
					* max(0.1, dot(normalDirection, lightDirection));

				return textureColor * float4(diffuseReflection, 1.0);
			}
			ENDCG
		}
	}

	Fallback "Diffuse"
}
