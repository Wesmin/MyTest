Shader "Custom/GalaxyGlow"
{
    Properties
    {
        _MainTex ("Galaxy Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (0.5, 1, 2, 1)
        _ScrollSpeed ("Scroll Speed", Vector) = (0.1, 0.1, 0, 0)
        _EmissionStrength ("Emission Strength", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ScrollSpeed;
            float4 _Color;
            float4 _GlowColor;
            float _EmissionStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            float _TimeValue;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv += (_Time.y * _ScrollSpeed.xy); // 滚动贴图
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texCol = tex2D(_MainTex, i.uv) * _Color;

                // 自发光模拟
                fixed3 emission = texCol.rgb * _GlowColor.rgb * _EmissionStrength;

                // 简单边缘发光：基于视角和法线
                float edge = 1.0 - saturate(dot(normalize(i.worldNormal), normalize(_WorldSpaceCameraPos - i.worldPos)));
                emission += _GlowColor.rgb * edge * 0.5;

                return fixed4(emission, texCol.a);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
}
