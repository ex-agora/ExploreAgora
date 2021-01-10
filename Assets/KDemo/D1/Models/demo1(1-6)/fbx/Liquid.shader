Shader "Toon/Lit Glass" {
    Properties{
        [Header(Main)]
        _Color("Main Color", Color) = (1,1,1,0.2)
        _SColor("Specular Color", Color) = (1,1,1,1)       
        [Header(Light Direction Specular)]
        _SpecSize("Light Direction Specular Size", Range(0.65,0.999)) = 0.9 // specular size
        _SpecOffset("Light Direction Specular Offset", Range(0.5,1)) = 0.6 // specular offset of the spec Ramp
        _Offset2("LightDir Spec Smoothness", Range(0,1)) = 0.05
        [Header(View Direction Specular)]
        _SpecSize2("View Specular", Range(0.65,0.999)) = 0.9 // specular size
        _Offset("View Spec Smoothness", Range(0,1)) = 0.1
        [Header(Outer Rim)]
        _RimPower2("Rim Offset Out Rim", Range(0,4)) = 0.7
        _RimColor2("Outer Rim Color", Color) = (0.49,0.94,0.64,1)
        _OutRimCutoff("Out Rim Cutoff Inner", Range(0,1)) = 0
        [Header(Inner Fresnel)]
        _RimPower("Rim Offset Inner Fresnel", Range(0,4)) = 1.2
        _RimColor("Inner Fresnel Rim Color", Color) = (0.49,0.94,0.64,1)       
        _FresnelInner("Fresnell Rim Cutoff", Range(0,2)) = 0.7      
    }
 
        SubShader{
        Tags{ "Queue" = "Transparent"}
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha 
 
        CGPROGRAM
#pragma surface surf ToonRamp keepalpha
       
    // custom lighting function that uses a texture ramp based
    // on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
    inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
    {
#ifndef USING_DIRECTIONAL_LIGHT
        lightDir = normalize(lightDir);
#endif
 
        float d = dot(s.Normal, lightDir);
        float dfwidth = fwidth(d);
        float ramp = smoothstep(0, dfwidth, d);     
        half4 c;
        c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
        c.a = s.Alpha;
        return c;
    }
 
   
    float4 _Color;
    float4 _SColor; // specular color
    float _SpecSize, _SpecSize2; // specular size
    float _SpecOffset; // offset specular ramp
    float _Offset, _Offset2; // specular fade offset
    float4 _RimColor, _RimColor2; // fresnel rim color
    float _RimPower, _RimPower2; // rim offsets
    float _FresnelInner, _OutRimCutoff; // cutoffs
    
 
    struct Input {     
        float3 viewDir; // view direction from camera
    };
 
    void surf(Input IN, inout SurfaceOutput o) {
 
        // lights
        float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);            
        half lightDot = dot(o.Normal, lightDirection -IN.viewDir)*0.5 + _SpecOffset; // basing on normal and light direction and view direction
       
        // inner glow and outer rim
        float outerrim = _RimPower - saturate(dot(IN.viewDir, o.Normal)); // calculate fresnel rim
        float innerglow = _RimPower2 - saturate(dot(IN.viewDir, o.Normal)); // calculate fresnel rim
        innerglow =  smoothstep(0.5, 0.5 + _OutRimCutoff, innerglow) * _RimColor2.a;
        outerrim = (1-smoothstep(0.5 - _FresnelInner, 0.5, outerrim)) * _RimColor.a;
      
        // make it glow
        o.Emission = _RimColor.rgb * pow(outerrim, 1.5); // fresnel rim
        o.Emission += _RimColor2.rgb * pow(innerglow, 1.5); // inner glow fresnel
 
        // view specular
        half viewSpec =  saturate(dot(o.Normal, (IN.viewDir))); // basing on normal and light direction
        
        float viewSpecLine = (smoothstep(_SpecSize2, _SpecSize2 + _Offset, viewSpec)) * 10;
        
        // light dir specular
        float specular= (smoothstep(_SpecSize, _SpecSize + _Offset2, lightDot)) * 10;
       
        o.Emission += saturate(viewSpecLine + specular) * _SColor * 2;
        
        o.Alpha = saturate(_Color.a + (viewSpecLine + specular + outerrim + innerglow));
        o.Albedo = saturate(o.Albedo + _Color);
      
    }
    ENDCG
    }
        Fallback "Diffuse"
}