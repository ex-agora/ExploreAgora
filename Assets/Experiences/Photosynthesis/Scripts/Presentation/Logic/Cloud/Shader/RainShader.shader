// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RainShader"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 50
		_SpecColor("Specular Color",Color)=(1,1,1,1)
		[HDR]_Color0("Color 0", Color) = (1,1,1,0)
		[HDR]_Color1("Color 1", Color) = (1,0.7250076,0,1)
		_MoveFactor("MoveFactor", Float) = 0
		_Texture0("Texture 0", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.7
		_Float1("Float 1", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf BlinnPhong keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			half2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform half _MoveFactor;
		uniform half4 _Color0;
		uniform half4 _Color1;
		uniform half _Float1;
		uniform float _Cutoff = 0.7;
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
			float4 appendResult43 = (half4(0.0 , ( _Time.y * _MoveFactor ) , 0.0 , 0.0));
			float2 uv_TexCoord42 = i.uv_texcoord + appendResult43.xy;
			half4 tex2DNode1 = tex2D( _Texture0, uv_TexCoord42 );
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
0;0;1536;803;1983.976;821.3426;2.182257;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;45;-1800.669,199.9537;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1791.94,400.7214;Float;False;Property;_MoveFactor;MoveFactor;8;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1492.305,306.8843;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;43;-1308.995,306.8843;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;-1036.213,239.2343;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;23;-1214.292,-266.9918;Float;True;Property;_Texture0;Texture 0;9;0;Create;True;0;0;False;0;None;45a5726539de1b541bd461a0f2fb9afe;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;1;-803.537,-24.0984;Float;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;None;4447ae153512cea45990089bd13516ce;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;25;-245.5866,415.4076;Float;True;False;False;False;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;30;81.84859,617.6678;Float;False;Property;_Color0;Color 0;6;1;[HDR];Create;True;0;0;False;0;1,1,1,0;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;38;481.0291,575.2029;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;317.7837,852.523;Float;False;Property;_Color1;Color 1;7;1;[HDR];Create;True;0;0;False;0;1,0.7250076,0,1;0.9433962,0.9433962,0.9433962,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;362.927,321.11;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;719.2141,611.3828;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;821.7236,249.5827;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;356.067,-155.7541;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;987.5493,-190.6077;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;11;1063.078,40.26134;Float;False;Property;_Float1;Float 1;11;0;Create;True;0;0;False;0;0;0.37;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1455.687,-33.37807;Half;False;True;6;Half;ASEMaterialInspector;0;0;BlinnPhong;RainShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.7;True;True;0;False;TransparentCutout;;AlphaTest;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;50;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;10.82;0.1532917,0.8679245,0.01637592,1;VertexOffset;True;False;Cylindrical;False;Relative;0;;10;-1;-1;0;0;False;0;0;False;-1;5;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;44;0;45;0
WireConnection;44;1;46;0
WireConnection;43;1;44;0
WireConnection;42;1;43;0
WireConnection;1;0;23;0
WireConnection;1;1;42;0
WireConnection;25;0;1;4
WireConnection;38;0;25;0
WireConnection;29;0;25;0
WireConnection;29;1;30;0
WireConnection;39;0;38;0
WireConnection;39;1;40;0
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
//CHKSM=161F095ADCFE147B00DFAA7BA00CD047B1C4C2B1