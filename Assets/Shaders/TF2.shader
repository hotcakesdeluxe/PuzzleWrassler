 
Shader "Custom/Team Fortress 2" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _ShadowTint ("Shadow Color", Color) = (0, 0, 0, 1)
        _RimColor ("Rim Color", Color) = (0.97,0.88,1,1)
        _RimPower ("Rim Power", Float) = 2.5
        _MainTex ("Diffuse (RGB) Alpha (A)", 2D) = "white" {}
        _BumpMap ("Normal (Normal)", 2D) = "bump" {}
        _SpecularTex ("Specular Level (R) Gloss (G) Rim Mask (B)", 2D) = "gray" {}
        _RampTex ("Toon Ramp (RGB)", 2D) = "white" {}
        _Cutoff ("Alphatest Cutoff", Range(0, 1)) = 1
    }
 
    SubShader{
        Tags { "RenderType" = "Opaque" }
       
        CGPROGRAM
            #pragma surface surf TF2 alphatest:_Cutoff
            #pragma target 4.0
 
            struct Input
            {
                float2 uv_MainTex;
                float3 worldNormal;
                float3 viewDir;
                INTERNAL_DATA
            };
           
            sampler2D _MainTex, _SpecularTex, _BumpMap, _RampTex;
            float4 _RimColor, _Color, _ShadowTint;
            float _RimPower;
 
            inline fixed4 LightingTF2 (SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
            {
                fixed3 h = normalize (lightDir + viewDir);
               
                fixed NdotL = dot(s.Normal, lightDir) * 0.5 + 0.5;
                fixed3 ramp = tex2D(_RampTex, float2(NdotL * atten, NdotL)).rgb;
                float3 shadowColor = s.Albedo * _ShadowTint;
                float nh = max (0, dot (s.Normal, h));
                float spec = pow (nh, s.Gloss * 128) * s.Specular;
               
                fixed4 c;
                c.rgb = ((s.Albedo * _Color.rgb * ramp * _LightColor0.rgb + _LightColor0.rgb * spec) * (atten * 2));
                c.rgb += shadowColor * max(0.0,(1.0-(nh*atten*2))) * _Color;
                c.a = s.Alpha * _Color.a;
                return c;
            }
 
            void surf (Input IN, inout SurfaceOutput o)
            {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
                o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;
                o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
                float3 specGloss = tex2D(_SpecularTex, IN.uv_MainTex).rgb;
                o.Specular = specGloss.r;
                o.Gloss = specGloss.g;
               
                half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
                o.Emission = _RimColor.rgb * pow (rim, _RimPower);
            }
        ENDCG
    }
    Fallback "Transparent/Cutout/Bumped Specular"
}