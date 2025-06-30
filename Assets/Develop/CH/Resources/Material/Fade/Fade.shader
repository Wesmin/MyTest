Shader "TS/Fade"
{
    Properties
    {
        _Threshold("Threshold", Range(0, 1)) = 0.5
        _MainTex("Texture", 2D) = "white" {}
    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        float _Threshold;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);

            // Use UV's x-coordinate to determine threshold
            if (IN.uv_MainTex.x < _Threshold)
            {
                o.Alpha = 1.0;  // Solid material where UV x < Threshold
            }
            else
            {
                discard;  // Transparent (no material) where UV x >= Threshold
            }

            o.Albedo = c.rgb;
        }
        ENDCG
    }

        FallBack "Diffuse"
}
