Shader "FourthDimension/FourthDimensionTransparentShader"
{
	Properties
	{
		_Color ("Material Color", Color) = (1,1,1,1)
		_MainTex ("Material Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }

		//Back Passes
		Pass
		{
			Cull Front

			ZWrite Off
			ZTest NotEqual

			Blend Zero OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform sampler2D _MainTex;
			uniform float4 _Color;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD1;
			}; 

			vertexOutput vert(vertexInput i)
			{
				vertexOutput o;

				o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
				o.tex = i.texcoord;

				return o;
			}
			
			float4 frag(vertexOutput i) : COLOR
			{
				return _Color * tex2D(_MainTex, i.tex.xy);
			}

			ENDCG
		}

		Pass
		{
			Cull Front

			ZWrite Off
			ZTest NotEqual

			Blend SrcAlpha One

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform sampler2D _MainTex;
			uniform float4 _Color;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD1;
			}; 

			vertexOutput vert(vertexInput i)
			{
				vertexOutput o;

				o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
				o.tex = i.texcoord;

				return o;
			}
			
			float4 frag(vertexOutput i) : COLOR
			{
				return _Color * tex2D(_MainTex, i.tex.xy);
			}

			ENDCG
		}

		//Front Passes
		Pass
		{
			Cull Back

			ZWrite Off
			ZTest NotEqual

			Blend Zero OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform sampler2D _MainTex;
			uniform float4 _Color;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD1;
			}; 

			vertexOutput vert(vertexInput i)
			{
				vertexOutput o;

				o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
				o.tex = i.texcoord;

				return o;
			}
			
			float4 frag(vertexOutput i) : COLOR
			{
				return _Color * tex2D(_MainTex, i.tex.xy);
			}

			ENDCG
		}

		Pass
		{
			Cull Back

			ZWrite Off
			ZTest NotEqual

			Blend SrcAlpha One

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform sampler2D _MainTex;
			uniform float4 _Color;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD1;
			}; 

			vertexOutput vert(vertexInput i)
			{
				vertexOutput o;

				o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
				o.tex = i.texcoord;

				return o;
			}
			
			float4 frag(vertexOutput i) : COLOR
			{
				return _Color * tex2D(_MainTex, i.tex.xy);
			}

			ENDCG
		}
	}

	Fallback "Legacy Shaders/Transparent/Diffuse"
}
