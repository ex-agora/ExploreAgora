// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DoubleSideCutOut"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 22.7
		_SpecColor("Specular Color",Color)=(1,1,1,1)
		[HDR]_Color0("Color 0", Color) = (1,1,1,0)
		[HDR]_Color1("Color 1", Color) = (1,0.7250076,0,1)
		_Texture0("Texture 0", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.47
		_Float1("Float 1", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf BlinnPhong keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			half2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform float4 _Texture0_ST;
		uniform half4 _Color0;
		uniform half4 _Color1;
		uniform half _Float1;
		uniform float _Cutoff = 0.47;
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
			o.Albedo = ( ( tex2DNode1 * ( temp_output_25_0 * _Color0 ) ) + ( tex2DNode1 * ( ( 1.0 - temp_output_25_0 ) * _Color1 ) ) ).rgb;
			o.Gloss = _Float1;
			o.Alpha = 1;
			clip( tex2DNode1.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15800
0;0;1536;803;1466.118;653.3088;2.182257;False;False
Node;AmplifyShaderEditor.TexturePropertyNode;23;-1229.568,221.8338;Float;True;Property;_Texture0;Texture 0;8;0;Create;True;0;0;False;0;None;2fa88942c91abb346bb767787c63b124;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;1;-803.537,-24.0984;Float;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;25;-177.9366,70.61101;Float;True;False;False;False;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;38;269.3502,184.5788;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;267.5918,490.2685;Float;False;Property;_Color1;Color 1;7;1;[HDR];Create;True;0;0;False;0;1,0.7250076,0,1;1,0.7250076,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;103.6711,65.55685;Float;False;Property;_Color0;Color 0;6;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;555.5448,111.646;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;358.5625,-241.9123;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;797.7188,-53.75101;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;635.3959,-435.0831;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;20;-402.0166,-328.3343;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;1222.383,46.80811;Float;False;Property;_Float1;Float 1;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;1037.741,-166.6029;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1645.543,-186.1361;Half;False;True;6;Half;ASEMaterialInspector;0;0;BlinnPhong;DoubleSideCutOut;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.47;True;True;0;False;TransparentCutout;;AlphaTest;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;22.7;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;10.82;0.1532917,0.8679245,0.01637592,1;VertexOffset;True;False;Cylindrical;False;Relative;0;;9;-1;-1;0;0;False;0;0;False;-1;5;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;23;0
WireConnection;25;0;1;4
WireConnection;38;0;25;0
WireConnection;39;0;38;0
WireConnection;39;1;40;0
WireConnection;29;0;25;0
WireConnection;29;1;30;0
WireConnection;36;0;1;0
WireConnection;36;1;39;0
WireConnection;41;0;1;0
WireConnection;41;1;29;0
WireConnection;31;0;41;0
WireConnection;31;1;36;0
WireConnection;0;0;31;0
WireConnection;0;4;11;0
WireConnection;0;10;1;4
ASEEND*/
//CHKSM=6FAAE75B5CC4851DAD6A13A64560A71E21845EDD