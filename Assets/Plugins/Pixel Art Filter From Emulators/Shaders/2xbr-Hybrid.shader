﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Pixel Art Filters/2xBR-Hybrid"
{
	Properties
	{
		decal("_BackgroundTexture (RGB)", 2D) = "white" {} //you should also set this in scripting with material.SetTexture ("decal", yourDecalTexture)
	texture_size("texture_size", Vector) = (256,224,0,0)
		ddt_Scale("ddt_Scale", Float) = 2
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		///////////////////////////////////
		Pass
	{
		CGPROGRAM
#include "UnityCG.cginc"  
#pragma vertex main_vertex
#pragma fragment main_fragment  
#pragma target 3.0

		uniform sampler2D decal : TEXUNIT0;
	uniform float2 texture_size; // set from outside the shader ( THIS NOTE WAS MADE BY SONOSHEE)	

#include "2xbr-Hybrid.txt"

	ENDCG      
	}

	}
		FallBack "Diffuse"
}