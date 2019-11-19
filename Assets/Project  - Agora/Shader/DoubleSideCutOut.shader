// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DoubleSideCutOut"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 17
		_FademapTex("Fade mapTex", 2D) = "white" {}
		_SpecColor("Specular Color",Color)=(1,1,1,1)
		_Transparency("Transparency", Range( 0 , 1)) = 0
		[HDR]_Color0("Color 0", Color) = (1,1,1,0)
		[HDR]_Color1("Color 1", Color) = (1,0.7250076,0,1)
		_Texture0("Texture 0", 2D) = "white" {}
		_Float1("Float 1", Float) = 0
		_Cutoff( "Mask Clip Value", Float ) = 0.1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			half2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform float4 _Texture0_ST;
		uniform half4 _Color0;
		uniform half4 _Color1;
		uniform half _Transparency;
		uniform sampler2D _FademapTex;
		uniform float4 _FademapTex_ST;
		uniform half _Float1;
		uniform float _Cutoff = 0.1;
		uniform half _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			half4 tex2DNode1 = tex2D( _Texture0, uv_Texture0 );
			float temp_output_25_0 = tex2DNode1.a;
			float4 break50 = ( ( tex2DNode1 * ( temp_output_25_0 * _Color0 ) ) + ( tex2DNode1 * ( ( 1.0 - temp_output_25_0 ) * _Color1 ) ) );
			float2 uv_FademapTex = i.uv_texcoord * _FademapTex_ST.xy + _FademapTex_ST.zw;
			float smoothstepResult42 = smoothstep( 0.0 , 1.0 , ( _Transparency - ( 1.0 - tex2D( _FademapTex, uv_FademapTex ).a ) ));
			float temp_output_47_0 = ( tex2DNode1.a * smoothstepResult42 );
			float4 appendResult52 = (half4(break50.r , break50.g , break50.b , ( break50.a * temp_output_47_0 )));
			o.Albedo = appendResult52.xyz;
			o.Gloss = _Float1;
			o.Alpha = smoothstepResult42;
			clip( temp_output_47_0 - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf BlinnPhong keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
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
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
Version=15800
6;17.2;1524;780;1410.519;1426.917;2.888574;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;23;-1229.568,221.8338;Float;True;Property;_Texture0;Texture 0;10;0;Create;True;0;0;False;0;None;85e3c874ede0c304a8e28f7437975c3c;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;1;-803.537,-24.0984;Float;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;25;-249.9511,201.5464;Float;True;False;False;False;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;46;604.7698,277.9673;Float;True;Property;_FademapTex;Fade mapTex;5;0;Create;True;0;0;False;0;None;1960075bce1b75f42b646a80a5de6202;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;38;269.3502,184.5788;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;267.5918,490.2685;Float;False;Property;_Color1;Color 1;9;1;[HDR];Create;True;0;0;False;0;1,0.7250076,0,1;1,0.7250076,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;-162.5643,-397.0816;Float;False;Property;_Color0;Color 0;8;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0.8301887,0.8301887,0.8301887,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;555.5448,111.646;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;358.5625,-241.9123;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;45;958.3539,342.4915;Float;False;Property;_Transparency;Transparency;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;44;947.4058,478.9121;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;43;1227.689,470.031;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;635.3959,-435.0831;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;749.7093,-167.2284;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;42;1385.332,274.9;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;1009.372,-227.7061;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;1412.279,13.37071;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;50;1189.688,-482.0016;Float;True;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;1540.768,-402.091;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;52;1751.5,-611.8984;Float;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;11;1303.126,-88.49182;Float;False;Property;_Float1;Float 1;11;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;20;-402.0166,-328.3343;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2162.657,-208.9862;Half;False;True;6;Half;ASEMaterialInspector;0;0;BlinnPhong;DoubleSideCutOut;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.1;True;True;0;True;TransparentCutout;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;17;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;10.82;0.1532917,0.8679245,0.01637592,1;VertexOffset;True;False;Cylindrical;False;Relative;0;;12;-1;-1;0;0;False;0;0;False;-1;6;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;23;0
WireConnection;25;0;1;4
WireConnection;38;0;25;0
WireConnection;39;0;38;0
WireConnection;39;1;40;0
WireConnection;29;0;25;0
WireConnection;29;1;30;0
WireConnection;44;0;46;4
WireConnection;43;0;45;0
WireConnection;43;1;44;0
WireConnection;41;0;1;0
WireConnection;41;1;29;0
WireConnection;36;0;1;0
WireConnection;36;1;39;0
WireConnection;42;0;43;0
WireConnection;31;0;41;0
WireConnection;31;1;36;0
WireConnection;47;0;1;4
WireConnection;47;1;42;0
WireConnection;50;0;31;0
WireConnection;51;0;50;3
WireConnection;51;1;47;0
WireConnection;52;0;50;0
WireConnection;52;1;50;1
WireConnection;52;2;50;2
WireConnection;52;3;51;0
WireConnection;0;0;52;0
WireConnection;0;4;11;0
WireConnection;0;9;42;0
WireConnection;0;10;47;0
ASEEND*/
//CHKSM=04DF3699544F5CC76D2BA359A1F92631560D887F