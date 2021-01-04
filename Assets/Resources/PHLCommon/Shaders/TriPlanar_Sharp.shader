Shader "Custom/TriPlanar_Sharp"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf StandardSpecular fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input
		{
			float3 worldPos;
			float3 viewDir;
			float4 color : COLOR;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _MainTex_ST;

		void surf (Input IN, inout SurfaceOutputStandardSpecular o)
		{
			fixed3 normal = WorldNormalVector(IN, o.Normal);
			fixed2 uvs = fixed2(0,0);

			float compareVal = 0.6;

			uvs.x += IN.worldPos.x * step(compareVal, abs(normal.y));
			uvs.x += IN.worldPos.x * step(abs(normal.y), compareVal) * step(compareVal, abs(normal.z));
			uvs.x += IN.worldPos.z * step(abs(normal.y), compareVal) * step(abs(normal.z), compareVal);

			uvs.y += IN.worldPos.z * step(compareVal, abs(normal.y));
			uvs.y += IN.worldPos.y * step(abs(normal.y), compareVal);

			uvs *= 0.333;

			fixed4 c = tex2D(_MainTex, uvs * _MainTex_ST.xy + _MainTex_ST.wz) * _Color;
			o.Albedo = c.rgb * IN.color;
			o.Specular = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
