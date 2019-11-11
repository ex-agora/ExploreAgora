// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PlantTest"
{
	Properties
	{
		_LifePlantTexture("LifePlantTexture", 2D) = "white" {}
		_DiePlaneTexturer("DiePlaneTexturer", 2D) = "white" {}
		_Alpha("Alpha", Range( 0 , 1)) = 1
		_NormalTexture("NormalTexture", 2D) = "bump" {}
		_MetallicaMap("MetallicaMap", 2D) = "white" {}
		_RMap("RMap", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _NormalTexture;
		uniform float4 _NormalTexture_ST;
		uniform sampler2D _LifePlantTexture;
		uniform float4 _LifePlantTexture_ST;
		uniform sampler2D _DiePlaneTexturer;
		uniform float4 _DiePlaneTexturer_ST;
		uniform float _Alpha;
		uniform sampler2D _MetallicaMap;
		uniform float4 _MetallicaMap_ST;
		uniform sampler2D _RMap;
		uniform float4 _RMap_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalTexture = i.uv_texcoord * _NormalTexture_ST.xy + _NormalTexture_ST.zw;
			o.Normal = UnpackNormal( tex2D( _NormalTexture, uv_NormalTexture ) );
			float2 uv_LifePlantTexture = i.uv_texcoord * _LifePlantTexture_ST.xy + _LifePlantTexture_ST.zw;
			float2 uv_DiePlaneTexturer = i.uv_texcoord * _DiePlaneTexturer_ST.xy + _DiePlaneTexturer_ST.zw;
			float4 lerpResult3 = lerp( tex2D( _LifePlantTexture, uv_LifePlantTexture ) , tex2D( _DiePlaneTexturer, uv_DiePlaneTexturer ) , _Alpha);
			o.Albedo = lerpResult3.rgb;
			float2 uv_MetallicaMap = i.uv_texcoord * _MetallicaMap_ST.xy + _MetallicaMap_ST.zw;
			o.Metallic = tex2D( _MetallicaMap, uv_MetallicaMap ).r;
			float2 uv_RMap = i.uv_texcoord * _RMap_ST.xy + _RMap_ST.zw;
			o.Smoothness = tex2D( _RMap, uv_RMap ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15800
0;0;1536;803;1137.604;564.4074;1.490707;True;False
Node;AmplifyShaderEditor.SamplerNode;1;-639.3359,-308.0058;Float;True;Property;_LifePlantTexture;LifePlantTexture;0;0;Create;True;0;0;False;0;None;887d91225ded1d84482dbb37c575f307;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-636.3549,-39.67857;Float;True;Property;_DiePlaneTexturer;DiePlaneTexturer;1;0;Create;True;0;0;False;0;None;0bef87b4d39500c4ebb6ee57167f8624;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-673.622,203.3066;Float;False;Property;_Alpha;Alpha;2;0;Create;True;0;0;False;0;1;0.01;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-254.7335,349.3958;Float;True;Property;_NormalTexture;NormalTexture;3;0;Create;True;0;0;False;0;None;3e711a82489c5484b8e0a0e8dfdd1856;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;129.0249,407.5334;Float;True;Property;_MetallicaMap;MetallicaMap;4;0;Create;True;0;0;False;0;None;ee6d86eeb9ca7934486cc7705c526937;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;498.7203,458.2174;Float;True;Property;_RMap;RMap;5;0;Create;True;0;0;False;0;None;eeeae0f1dde7d2747974ef258e2bd925;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;3;-163.8005,137.7155;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;866.1003,83.47959;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;PlantTest;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;3;2;4;0
WireConnection;0;0;3;0
WireConnection;0;1;5;0
WireConnection;0;3;7;0
WireConnection;0;4;8;0
ASEEND*/
//CHKSM=D5E7554E143F5BF06EC9088ABE26849CC91B593A