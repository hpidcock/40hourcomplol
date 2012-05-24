Shader "Custom/Magic" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Multiply ("Multiply Colour", Color) = (1, 1, 1, 1)
		_Add ("Add Colour", Color) = (0, 0, 0, 0)
	}
	SubShader {
		Tags { "Queue" = "Transparent" }
		Blend DstColor Zero
		ZWrite Off

		LOD 200

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _Multiply;
			float4 _Add;

			struct PS_INPUT
			{
				float4 position : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};

			PS_INPUT vert(appdata_base input)
			{
				PS_INPUT output = (PS_INPUT)0;
				output.position = mul(UNITY_MATRIX_MVP, input.vertex);
				output.texcoord = input.texcoord;
				return output;
			}

			half4 frag(PS_INPUT input) : COLOR
			{
				float4 diffuse = tex2D(_MainTex, input.texcoord);

				diffuse *= _Multiply;
				diffuse += _Add;

				return diffuse;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
