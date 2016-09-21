Shader "FourthDimension/FourthDimensionTransparentShader"
{
	Properties
	{
		_Color ("Material Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }

		Pass
		{
			Cull Back

			ZWrite Off
			ZTest NotEqual

			Blend Zero OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform float4 _Color;

			float4 vert(float4 vertexPos : POSITION) : SV_POSITION
			{
				return mul(UNITY_MATRIX_MVP, vertexPos);
			}
			
			float4 frag(void) : COLOR
			{
				return _Color;
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

			uniform float4 _Color;

			float4 vert(float4 vertexPos : POSITION) : SV_POSITION
			{
				return mul(UNITY_MATRIX_MVP, vertexPos);
			}
			
			float4 frag(void) : COLOR
			{
				return _Color;
			}

			ENDCG
		}

	}
}
