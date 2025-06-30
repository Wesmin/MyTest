Shader "TS/MotionBlur"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _Size("Size", Range(0, 20)) = 1
    }
        Category{

        // We must be transparent, so other objects are drawn before this one.  
        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }


        SubShader {

                // Horizontal blur  
                GrabPass
                {
                    Tags { "LightMode" = "Always" }
                }
                Pass {
                    Tags { "LightMode" = "Always" }

                    Name "MotionBlur"
                    CGPROGRAM
                    #pragma vertex vert  
                    #pragma fragment frag  
                    #pragma fragmentoption ARB_precision_hint_fastest  
                    #include "UnityCG.cginc"  

                    struct appdata_t {
                        float4 vertex : POSITION;
                        float2 texcoord : TEXCOORD0;
                        float4 color    : COLOR;
                    };

                    struct v2f {
                        float4 vertex : POSITION;
                        float4 uvgrab : TEXCOORD0;
                        float4 color    : COLOR;
                    };

                    v2f vert(appdata_t v) {
                        v2f o;
                        o.vertex = UnityObjectToClipPos(v.vertex);
                        #if UNITY_UV_STARTS_AT_TOP  
                        float scale = -1.0;
                        #else  
                        float scale = 1.0;
                        #endif  
                        o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
                        o.uvgrab.zw = o.vertex.zw;

                        o.color = v.color;
                        return o;
                    }

                    sampler2D _GrabTexture;
                    float4 _GrabTexture_TexelSize;
                    float4 _MainTex_TexelSize;
                    float _Size;
                    uniform float4 _Color;

                    half4 GrabPixel(v2f i, float weight, float kernel) {
                        kernel = sign(sign(abs(i.uvgrab.x)) + sign(abs(i.uvgrab.y))) * kernel;
                        return tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x + _GrabTexture_TexelSize.x * kernel * _Size, i.uvgrab.y, i.uvgrab.z, i.uvgrab.w))) * weight;
                    }
                    half4 frag(v2f i) : COLOR {
                        half4 sum = half4(0,0,0,0);
                        sum += GrabPixel(i, 0.05, -4.0);
                        sum += GrabPixel(i, 0.09, -3.0);
                        sum += GrabPixel(i, 0.12, -2.0);
                        sum += GrabPixel(i, 0.15, -1.0);
                        sum += GrabPixel(i, 0.18,  0.0);
                        sum += GrabPixel(i, 0.15, +1.0);
                        sum += GrabPixel(i, 0.12, +2.0);
                        sum += GrabPixel(i, 0.09, +3.0);
                        sum += GrabPixel(i, 0.05, +4.0);

                        float4 col5 = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
                        fixed decayFactor = sign(sign(abs(i.uvgrab.x)) + sign(abs(i.uvgrab.y)));
                        sum = lerp(col5, sum, decayFactor) * i.color * _Color;

                        return sum;
                    }
                    ENDCG
                }

         
        GrabPass {
            Tags { "LightMode" = "Always" }
        }
        Pass {
            Tags { "LightMode" = "Always" }

            Name "MotionBlur"
            CGPROGRAM
            #pragma vertex vert  
            #pragma fragment frag  
            #pragma fragmentoption ARB_precision_hint_fastest  
            #include "UnityCG.cginc"  

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord: TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f {
                float4 vertex : POSITION;
                float4 uvgrab : TEXCOORD0;
                float4 color    : COLOR;
            };

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                #if UNITY_UV_STARTS_AT_TOP  
                float scale = -1.0;
                #else  
                float scale = 1.0;
                #endif  
                o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
                o.uvgrab.zw = o.vertex.zw;

                o.color = v.color;
                return o;
            }

            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            float _Size;
            uniform float4 _Color;

            half4 GrabPixel(v2f i, float weight, float kernel) {
                kernel = sign(sign(abs(i.uvgrab.x)) + sign(abs(i.uvgrab.y))) * kernel;
                return tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x, i.uvgrab.y + _GrabTexture_TexelSize.y * kernel * _Size, i.uvgrab.z, i.uvgrab.w))) * weight;
            }

            half4 frag(v2f i) : COLOR
            {
                half4 sum = half4(0,0,0,0);
                sum += GrabPixel(i, 0.05, -4.0);
                sum += GrabPixel(i, 0.09, -3.0);
                sum += GrabPixel(i, 0.12, -2.0);
                sum += GrabPixel(i, 0.15, -1.0);
                sum += GrabPixel(i, 0.18,  0.0);
                sum += GrabPixel(i, 0.15, +1.0);
                sum += GrabPixel(i, 0.12, +2.0);
                sum += GrabPixel(i, 0.09, +3.0);
                sum += GrabPixel(i, 0.05, +4.0);

                float4 col5 = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
                fixed decayFactor = sign(sign(abs(i.uvgrab.x)) + sign(abs(i.uvgrab.y)));
                sum = lerp(col5, sum, decayFactor) * i.color * _Color;

                return sum;
            }
            ENDCG
        }
    }
    }
}

