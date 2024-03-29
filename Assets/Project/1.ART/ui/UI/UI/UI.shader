﻿Shader "Custom/tranSurfaceShader" {
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader
		{
			LOD 200
			// Render normally
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha
			//ColorMask RGB
			//Tags{ "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" }
			Tags{ "Queue" = "Transparent-100" "RenderType" = "Transparent" }
			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows keepalpha
			#pragma target 3.0
			sampler2D _MainTex;
			struct Input {
				float2 uv_MainTex;
			};
			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Standard"
}