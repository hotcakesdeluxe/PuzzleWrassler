Shader "Custom/NormalFade"
{
    Properties
    {
        _GroundColor ("Ground Color", Color) = (1,1,1,1)
        _WallColor ("Wall Color", Color) = (1,1,1,1)
        _GroundTex ("Ground Tex", 2D) = "white" {}
		_WallTex ("Wall Tex", 2D) = "white" {}
        _NormalStart ("NormalStart", Range(0,1)) = 0.333
        _NormalEnd ("NormalEnd", Range(0,1)) = 0.666
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf StandardSpecular fullforwardshadows
        #pragma target 3.0

        sampler2D _GroundTex;
		sampler2D _WallTex;

        struct Input
        {
			float3 worldPos;
        };

		float _NormalStart;
		float _NormalEnd;
        fixed4 _GroundColor;
        fixed4 _WallColor;
		float4 _GroundTex_ST;
		float4 _WallTex_ST;

		float map(float x, float inMin, float inMax, float outMin, float outMax)
		{
			return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
		}

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
			fixed2 uvs = fixed2(0, 0);

			float compareVal = 0.6;
			fixed3 normal = WorldNormalVector(IN, o.Normal);

			uvs.x += IN.worldPos.x * step(compareVal, abs(normal.y));
			uvs.x += IN.worldPos.x * step(abs(normal.y), compareVal) * step(compareVal, abs(normal.z));
			uvs.x += IN.worldPos.z * step(abs(normal.y), compareVal) * step(abs(normal.z), compareVal);

			uvs.y += IN.worldPos.z * step(compareVal, abs(normal.y));
			uvs.y += IN.worldPos.y * step(abs(normal.y), compareVal);

			uvs *= 0.333;

            fixed4 ground = tex2D (_GroundTex, uvs * _GroundTex_ST.xy + _GroundTex_ST.zw) * _GroundColor;
			fixed4 walls = tex2D(_WallTex, uvs * _WallTex_ST.xy + _WallTex_ST.zw) * _WallColor;

            o.Albedo = lerp(ground.rgb, walls.rgb, saturate(map(normal.y, _NormalStart, _NormalEnd, 0, 1)));

            o.Specular = 0;
            o.Smoothness = 0;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
