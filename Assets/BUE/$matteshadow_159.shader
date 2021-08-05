// Upgrade NOTE: replaced 'PositionFog()' with transforming position into clip space.
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

Shader "FX/Matte Shadow" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	
}

Category {
	
	Tags {"Queue"="Geometry+500"}
	LOD 200
	Alphatest Greater 0
	ZWrite Off
	AlphaToMask True
	ColorMask RGB
	Fog { Color [_AddFog] }
	Blend DstColor Zero
	
	#warning Upgrade NOTE: SubShader commented out; uses Unity 2.x per-pixel lighting. You should rewrite shader into a Surface Shader.
/*SubShader {
		
		Pass {	
			Tags { "LightMode" = "Pixel" }

CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members uv,normal,lightDir)
#pragma exclude_renderers d3d11
#pragma fragment frag
#pragma vertex vert
#pragma multi_compile_builtin
#pragma fragmentoption ARB_fog_exp2
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"
#include "AutoLight.cginc"

struct v2f {
	float4 pos : SV_POSITION;
	LIGHTING_COORDS
	float2	uv;
	float3	normal;
	float3	lightDir;
};

uniform float4 _MainTex_ST;

v2f vert (appdata_base v)
{
	v2f o;
	o.pos = UnityObjectToClipPos (v.vertex);
	o.normal = v.normal;
	o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
	o.lightDir = ObjSpaceLightDir( v.vertex );
	TRANSFER_VERTEX_TO_FRAGMENT(o);
	return o;
}

uniform sampler2D _MainTex;
uniform float4 _Color;

float4 frag (v2f i) : COLOR
{
	half4 c = _Color * 4 * LIGHT_ATTENUATION(i);
	c.a = (1-LIGHT_ATTENUATION(i));
	return c;
}
ENDCG

		}
	}*/
}

Fallback "Transparent/Cutout/VertexLit", 2

}