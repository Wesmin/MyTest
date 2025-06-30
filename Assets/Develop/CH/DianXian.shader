// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "T/Flow/DianXian"
{
	Properties
	{
		[Toggle]_FanZheng("FanZheng", Float) = 0
		[Toggle]_UV_FangXiang("UV_FangXiang", Float) = 1
		[Toggle]_WuLiuDong("WuLiuDong", Float) = 1
		[HDR]_Hologramcolor("Hologram color", Color) = (0.3973832,0.7720588,0.7410512,0)
		_Speed("Speed", Range( 0 , 1000)) = 1
		_ScanLines("Scan Lines", Float) = 5.00237
		_Opacity("Opacity", Range( 0 , 1)) = 0.5
		_Intensity("Intensity", Range( 1 , 10)) = 1
		_Fresnel_Val("Fresnel_Val", Range( 0 , 5)) = 5
		[Toggle]_Fresnel_fanXiang("Fresnel_fanXiang", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 viewDir;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float4 _Hologramcolor;
		uniform float _Intensity;
		uniform float _WuLiuDong;
		uniform float _Fresnel_fanXiang;
		uniform float _Fresnel_Val;
		uniform float _ScanLines;
		uniform float _UV_FangXiang;
		uniform float _FanZheng;
		uniform float _Speed;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 HologramColor32 = _Hologramcolor;
			o.Emission = ( ( HologramColor32 * HologramColor32 ) * _Intensity ).rgb;
			float3 ase_worldNormal = i.worldNormal;
			float dotResult180 = dot( i.viewDir , ase_worldNormal );
			float temp_output_184_0 = pow( ( 1.0 - saturate( abs( dotResult180 ) ) ) , _Fresnel_Val );
			float Fresnel186 = (( _Fresnel_fanXiang )?( ( 1.0 - temp_output_184_0 ) ):( temp_output_184_0 ));
			float Speed156 = _Speed;
			float temp_output_13_0 = sin( ( ( ( _ScanLines * (( _UV_FangXiang )?( i.uv_texcoord.y ):( i.uv_texcoord.x )) ) + (( 1.0 - ( (( _FanZheng )?( ( 1.0 - Speed156 ) ):( Speed156 )) * _Time ) )).x ) * UNITY_PI ) );
			float clampResult15 = clamp( (0.0 + (temp_output_13_0 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) , 0.0 , 1.0 );
			float temp_output_198_0 = step( clampResult15 , 0.54 );
			float ScanLines30 = temp_output_198_0;
			float temp_output_177_0 = (( _WuLiuDong )?( Fresnel186 ):( ( Fresnel186 * ScanLines30 * _Opacity ) ));
			o.Alpha = temp_output_177_0;
		}

		ENDCG
		CGPROGRAM
		#pragma exclude_renderers xboxseries playstation switch 
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.CommentaryNode;168;-1625.711,-587.5228;Inherit;False;614.0698;167.2261;Comment;2;6;156;Speed;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1575.711,-535.2966;Float;False;Property;_Speed;Speed;4;0;Create;True;0;0;0;False;0;False;1;672;0;1000;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;170;-3035.439,45.91539;Inherit;False;3047.907;934.652;Comment;28;173;30;155;146;15;149;137;14;13;145;143;150;11;3;107;105;106;8;171;10;27;26;174;157;176;192;198;199;Scan Lines;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;156;-1246.641,-537.5227;Float;False;Speed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;173;-2653.298,688.8535;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;174;-2477.347,628.5665;Inherit;False;Property;_FanZheng;FanZheng;0;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;26;-2525.032,809.2999;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;189;-1830.981,1015.278;Inherit;False;2165.596;496.531;Comment;11;186;191;190;184;185;183;182;181;180;179;178;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-2275.032,725.0067;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldNormalVector;179;-1773.14,1260.015;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;178;-1780.981,1089.79;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;180;-1446.169,1123.705;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2385.629,409.6199;Float;False;Property;_ScanLines;Scan Lines;5;0;Create;True;0;0;0;False;0;False;5.00237;-16.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;8;-2119.871,697.1426;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.AbsOpNode;181;-1158.352,1145.362;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;105;-1943.188,700.5985;Inherit;False;True;False;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;150;-1976.846,146.2888;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-1716.77,565.4415;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;107;-1687.937,703.2096;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;182;-896.3142,1152.482;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;185;-883.1286,1389.798;Inherit;False;Property;_Fresnel_Val;Fresnel_Val;8;0;Create;True;0;0;0;False;0;False;5;0.92;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;183;-711.4338,1080.735;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1551.342,566.8635;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;-1829.079,287.0303;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;184;-488.36,1135.535;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;137;-1656.801,212.5117;Inherit;True;Simplex2D;False;False;2;0;FLOAT2;100,100;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;149;-1332.271,332.3578;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;190;-246.5524,1364.923;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;35;-1599.438,-414.6887;Inherit;False;590.8936;257.7873;Comment;2;32;28;Hologram Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;15;-923.7923,609.802;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;28;-1561.469,-346.6425;Float;False;Property;_Hologramcolor;Hologram color;3;1;[HDR];Create;True;0;0;0;False;0;False;0.3973832,0.7720588,0.7410512,0;0,0.509804,0.7490196,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;186;125.7459,1222.872;Inherit;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;-1283.7,-342.8496;Float;False;HologramColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;127;-645.8871,-480.6362;Inherit;False;32;HologramColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-306.3015,-49.4135;Float;False;Property;_Opacity;Opacity;6;0;Create;True;0;0;0;False;0;False;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;187;-204.2349,-159.4922;Inherit;False;186;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;-334.296,-459.5981;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;132;-533.6958,-193.1992;Float;False;Property;_Intensity;Intensity;7;0;Create;True;0;0;0;False;0;False;1;1.75;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;54.54408,-370.7838;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;177;349.4045,50.04549;Inherit;True;Property;_WuLiuDong;WuLiuDong;2;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;193;107.5322,-165.864;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;191;-182.5637,1198.585;Inherit;False;Property;_Fresnel_fanXiang;Fresnel_fanXiang;9;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;192;-2223.685,87.47789;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;918.0575,-447.1735;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;T/Flow/DianXian;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;9;d3d11;glcore;gles;gles3;metal;vulkan;xboxone;ps4;ps5;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;0;4;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;1;False;;1;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SinOpNode;13;-1413.061,569.9525;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;-1889.245,490.3912;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;6.06;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;171;-2523.337,491.8647;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;157;-2928.464,573.4986;Inherit;True;156;Speed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;176;-2222.287,511.5629;Inherit;False;Property;_UV_FangXiang;UV_FangXiang;1;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;143;-2247.314,243.1854;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;14;-1250.53,591.5672;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;155;-537.4703,81.74572;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;199;-873.4072,484.2308;Inherit;False;Constant;_Float1;Float 1;11;0;Create;True;0;0;0;False;0;False;0.54;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;146;-851.308,63.38977;Float;True;myVarName3;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;198;-682.4072,448.2308;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;-239.5781,431.7931;Float;True;ScanLines;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;197;708.3599,-67.20811;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
WireConnection;156;0;6;0
WireConnection;173;0;157;0
WireConnection;174;0;157;0
WireConnection;174;1;173;0
WireConnection;27;0;174;0
WireConnection;27;1;26;0
WireConnection;180;0;178;0
WireConnection;180;1;179;0
WireConnection;8;0;27;0
WireConnection;181;0;180;0
WireConnection;105;0;8;0
WireConnection;150;0;192;3
WireConnection;3;0;106;0
WireConnection;3;1;105;0
WireConnection;182;0;181;0
WireConnection;183;0;182;0
WireConnection;11;0;3;0
WireConnection;11;1;107;0
WireConnection;145;0;150;0
WireConnection;145;1;143;1
WireConnection;184;0;183;0
WireConnection;184;1;185;0
WireConnection;137;0;145;0
WireConnection;149;0;137;0
WireConnection;149;1;13;0
WireConnection;190;0;184;0
WireConnection;15;0;14;0
WireConnection;186;0;191;0
WireConnection;32;0;28;0
WireConnection;126;0;127;0
WireConnection;126;1;32;0
WireConnection;133;0;126;0
WireConnection;133;1;132;0
WireConnection;177;0;193;0
WireConnection;177;1;186;0
WireConnection;193;0;187;0
WireConnection;193;1;30;0
WireConnection;193;2;49;0
WireConnection;191;0;184;0
WireConnection;191;1;190;0
WireConnection;0;2;133;0
WireConnection;0;9;177;0
WireConnection;13;0;11;0
WireConnection;106;0;10;0
WireConnection;106;1;176;0
WireConnection;176;0;171;1
WireConnection;176;1;171;2
WireConnection;14;0;13;0
WireConnection;155;0;198;0
WireConnection;155;1;146;0
WireConnection;146;0;149;0
WireConnection;198;0;15;0
WireConnection;198;1;199;0
WireConnection;30;0;198;0
WireConnection;197;0;177;0
ASEEND*/
//CHKSM=BBC53CD2FA898177D8D3F985F442F0C669059E96