Shader "Unlit/BitmapDataScan0"
{
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;

			fixed4 frag(v2f_img i) : SV_Target
			{
				i.uv.y = 1 - i.uv.y;
				return tex2D(_MainTex, i.uv).bgra;
			}
			ENDCG
		}
	}
	Fallback "Unlit/Texture"
}
