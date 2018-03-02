// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Pixel Art Filters/5XBR3.7"
{
	Properties
	{

	decal("decal (RGB)", 2D) = "white" {} //you should also set this in scripting with material.SetTexture ("decal", yourDecalTexture)
	texture_size("texture_size", Vector) = (256,224,0,0)
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		///////////////////////////////////
		Pass
	{
		CGPROGRAM
		// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices ( THIS NOTE WAS MADE BY UNITY)
#include "UnityCG.cginc"
#pragma vertex main_vertex
#pragma fragment main_fragment
#pragma target 3.0
		/*
		Hyllian's 5xBR v3.7c (squared) Shader

		Copyright (C) 2011/2012 Hyllian/Jararaca - sergiogdb@gmail.com

		This program is free software; you can redistribute it and/or
		modify it under the terms of the GNU General Public License
		as published by the Free Software Foundation; either version 2
		of the License, or (at your option) any later version.

		This program is distributed in the hope that it will be useful,
		but WITHOUT ANY WARRANTY; without even the implied warranty of
		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
		GNU General Public License for more details.

		You should have received a copy of the GNU General Public License
		along with this program; if not, write to the Free Software
		Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

		*/
		/*
		Leonardo da Luz Pinto's 3xBRZ shader Adaptation to Unity ShaderLab

		Copyright (C) 2018 Leo Luz - leodluz@yahoo.com/leoluzprog@gmail.com

		Permission is hereby granted, free of charge, to any person obtaining a copy
		of this software and associated documentation files (the "Software"), to deal
		in the Software without restriction, including without limitation the rights
		to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
		copies of the Software, and to permit persons to whom the Software is
		furnished to do so, subject to the following conditions:

		The above copyright notice and this permission notice shall be included in
		all copies or substantial portions of the Software.

		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
		IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
		FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
		AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
		LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
		OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
		THE SOFTWARE.

		*/
		uniform half4 _Color;
	sampler2D _MainTex;

	float2 texture_size; // set from outside the shader ( THIS NOTE WAS MADE BY SONOSHEE)


	const static float coef = 2.0;
	const static float3 dtt = float3(65536, 255, 1);
	const static half y_weight = 48.0;
	const static half u_weight = 7.0;
	const static half v_weight = 6.0;
	const static half3x3 yuv = half3x3(0.299, 0.587, 0.114, -0.169, -0.331, 0.499, 0.499, -0.418, -0.0813);
	const static half3x3 yuv_weighted = half3x3(y_weight*yuv[0], u_weight*yuv[1], v_weight*yuv[2]);


	float4 df(float4 A, float4 B)
	{
		return float4(abs(A - B));
	}

	bool4 eq(float4 A, float4 B)
	{
		return (df(A, B) < float4(15.0, 0,0, 0));
	}

	float4 weighted_distance(float4 a, float4 b, float4 c, float4 d, float4 e, float4 f, float4 g, float4 h)
	{
		return (df(a, b) + df(a, c) + df(d, e) + df(d, f) + 4.0*df(g, h));
	}

	struct out_vertex
	{
		float4 position : POSITION;
		float4 color : COLOR;
		float2 texCoord : TEXCOORD0;
		half4 t1       : TEXCOORD1;

	};

	//VERTEX_SHADER
	out_vertex main_vertex(appdata_base v)
	{
		out_vertex OUT;

		OUT.position = UnityObjectToClipPos(v.vertex);
		OUT.color = _Color;

		float2 ps = float2(1.0 / texture_size.x, 1.0 / texture_size.y);
		float dx = ps.x;
		float dy = ps.y;

		// A1 B1 C1
		// A0 A B C C4
		// D0 D E F F4
		// G0 G H I I4
		// G5 H5 I5

		// This line fix a bug in ATI cards. ( THIS NOTE WAS MADE BY XBR AUTHOR)
		//float2 texCoord = texCoord1 + float2(0.0000001, 0.0000001);

		OUT.texCoord = v.texcoord;
		OUT.t1.xy = half2(dx, 0); // F
		OUT.t1.zw = half2(0, dy); // H

		return OUT;
	}

	//FRAGMENT SHADER
	half4 main_fragment(out_vertex VAR) : COLOR
	{
		bool4 edr, edr_left, edr_up, px; // px = pixel, edr = edge detection rule
	bool4 interp_restriction_lv1, interp_restriction_lv2_left, interp_restriction_lv2_up;
	bool4 nc; // new_color
	bool4 fx, fx_left, fx_up; // inequations of straight lines.

	float2 fp = frac(VAR.texCoord*texture_size);

	half2 dx = VAR.t1.xy;
	half2 dy = VAR.t1.zw;

	half3 A = tex2D(_MainTex, VAR.texCoord - dx - dy).xyz;
	half3 B = tex2D(_MainTex, VAR.texCoord - dy).xyz;
	half3 C = tex2D(_MainTex, VAR.texCoord + dx - dy).xyz;
	half3 D = tex2D(_MainTex, VAR.texCoord - dx).xyz;
	half3 E = tex2D(_MainTex, VAR.texCoord).xyz;
	half3 F = tex2D(_MainTex, VAR.texCoord + dx).xyz;
	half3 G = tex2D(_MainTex, VAR.texCoord - dx + dy).xyz;
	half3 H = tex2D(_MainTex, VAR.texCoord + dy).xyz;
	half3 I = tex2D(_MainTex, VAR.texCoord + dx + dy).xyz;

	half3  A1 = tex2D(_MainTex, VAR.texCoord - dx - 2.0*dy).xyz;
	half3  C1 = tex2D(_MainTex, VAR.texCoord + dx - 2.0*dy).xyz;
	half3  A0 = tex2D(_MainTex, VAR.texCoord - 2.0*dx - dy).xyz;
	half3  G0 = tex2D(_MainTex, VAR.texCoord - 2.0*dx + dy).xyz;
	half3  C4 = tex2D(_MainTex, VAR.texCoord + 2.0*dx - dy).xyz;
	half3  I4 = tex2D(_MainTex, VAR.texCoord + 2.0*dx + dy).xyz;
	half3  G5 = tex2D(_MainTex, VAR.texCoord - dx + 2.0*dy).xyz;
	half3  I5 = tex2D(_MainTex, VAR.texCoord + dx + 2.0*dy).xyz;
	half3  B1 = tex2D(_MainTex, VAR.texCoord - 2.0*dy).xyz;
	half3  D0 = tex2D(_MainTex, VAR.texCoord - 2.0*dx).xyz;
	half3  H5 = tex2D(_MainTex, VAR.texCoord + 2.0*dy).xyz;
	half3  F4 = tex2D(_MainTex, VAR.texCoord + 2.0*dx).xyz;

	float4 b = mul(half4x3(B, D, H, F), yuv_weighted[0]);
	float4 c = mul(half4x3(C, A, G, I), yuv_weighted[0]);
	float4 e = mul(half4x3(E, E, E, E), yuv_weighted[0]);
	float4 d = b.yzwx;
	float4 f = b.wxyz;
	float4 g = c.zwxy;
	float4 h = b.zwxy;
	float4 i = c.wxyz;

	float4 i4 = mul(half4x3(I4, C1, A0, G5), yuv_weighted[0]);
	float4 i5 = mul(half4x3(I5, C4, A1, G0), yuv_weighted[0]);
	float4 h5 = mul(half4x3(H5, F4, B1, D0), yuv_weighted[0]);
	float4 f4 = h5.yzwx;

	float4 Ao = float4(1.0, -1.0, -1.0, 1.0);
	float4 Bo = float4(1.0,  1.0, -1.0,-1.0);
	float4 Co = float4(1.5,  0.5, -0.5, 0.5);
	float4 Ax = float4(1.0, -1.0, -1.0, 1.0);
	float4 Bx = float4(0.5,  2.0, -0.5,-2.0);
	float4 Cx = float4(1.0,  1.0, -0.5, 0.0);
	float4 Ay = float4(1.0, -1.0, -1.0, 1.0);
	float4 By = float4(2.0,  0.5, -2.0,-0.5);
	float4 Cy = float4(2.0,  0.0, -1.0, 0.5);

	// These inequations define the line below which interpolation occurs.
	fx = (Ao*fp.y + Bo*fp.x > Co);
	fx_left = (Ax*fp.y + Bx*fp.x > Cx);
	fx_up = (Ay*fp.y + By*fp.x > Cy);

	interp_restriction_lv1 = ((e != f) && (e != h) && (!eq(f,b) && !eq(f,c) || !eq(h,d) && !eq(h,g) || eq(e,i) && (!eq(f,f4) && !eq(f,i4) || !eq(h,h5) && !eq(h,i5)) || eq(e,g) || eq(e,c)));
	interp_restriction_lv2_left = ((e != g) && (d != g));
	interp_restriction_lv2_up = ((e != c) && (b != c));



	edr = (weighted_distance(e, c, g, i, h5, f4, h, f) < weighted_distance(h, d, i5, f, i4, b, e, i)) && interp_restriction_lv1;
	edr_left = ((coef*df(f,g)) <= df(h,c)) && interp_restriction_lv2_left;
	edr_up = (df(f,g) >= (coef*df(h,c))) && interp_restriction_lv2_up;

	nc = (edr && (fx || edr_left && fx_left || edr_up && fx_up));

	px = (df(e,f) <= df(e,h));

	half3 res = nc.x ? px.x ? F : H : nc.y ? px.y ? B : F : nc.z ? px.z ? D : B : nc.w ? px.w ? H : D : E;

	return half4(res, 1.0);
	}

		ENDCG
	}
	}
		FallBack "Diffuse"
}