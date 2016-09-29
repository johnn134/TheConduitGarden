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
			uniform float4 _MainTex_ST;
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

			vertexOutput vert(vertexInput input)
			{
				vertexOutput o;

				o.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				o.tex = input.texcoord;

				return o;
			}
			
			float4 frag(vertexOutput input) : COLOR
			{
				float4 textureColor = tex2D(_MainTex, _MainTex_ST.xy * input.tex.xy + _MainTex_ST.zw);
				return _Color * textureColor;
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
			uniform float4 _MainTex_ST;
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

			vertexOutput vert(vertexInput input)
			{
				vertexOutput o;

				o.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				o.tex = input.texcoord;

				return o;
			}
			
			float4 frag(vertexOutput input) : COLOR
			{
				float4 textureColor = tex2D(_MainTex, input.tex.xy * _MainTex_ST.xy * _MainTex_ST.zw);
				return _Color * textureColor;
			}

			ENDCG
		}
	}

	Fallback "Legacy Shaders/Transparent/Diffuse"
}
