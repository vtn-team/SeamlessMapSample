Shader "Custom/Evolve" {
	Properties{
		_PlayerPos("Player Position", Vector) = (0,0,0,0)
		_AreaColor("Area Color", Color) = (1,1,1,1)
		_EdgeColor("Edge Color", Color) = (0.3, 0.3, 0.3, 1)
		_AreaSize("Area Size", Float) = 0.02
		_EdgeSize("Edge Size", Float) = 1.0

		[Space]
		[Space]
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
	SubShader{
		Tags { 
			"RenderType" = "Transparent"
			"RenderPipeline" = "UniversalPipeline"
		}
		Pass {
			CGPROGRAM
			//#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			struct Input {
				float2 uv : TEXCOORD0;
				float3 worldPos : SV_POSITION;
			};
		/*
			Input vert(Input v)
			{
				Input o;
				o.worldPos = UnityObjectToClipPos(v.worldPos);
				o.uv = v.uv;
				return o;
			}
		*/
			// 領域展開
			float4 _PlayerPos; //プレイヤー位置
			float4 _AreaColor; //領域内の色
			float4 _EdgeColor; //領域のエッジの色
			float _AreaSize; //領域の大きさ(外部から操作する)
			float _EdgeSize; //エッジの幅
			////

			// other
			half _Glossiness;
			half _Metallic;

			fixed4 frag(Input IN) : SV_Target
			{
				float4 col = float4(0, 0, 0, 0);
				/*
				//領域展開上書き
				//stepなので0か1
				float scanline = step(
					distance(IN.worldPos.xyz, _PlayerPos.xyz),
					_AreaSize
				);

				//smoothstepは補間がはいる
				float edge = smoothstep(
					_AreaSize + _EdgeSize, _AreaSize,
					distance(IN.worldPos.xyz, _PlayerPos.xyz)
				);

				//エッジだけ塗るために雑だけどこうしてる
				if (scanline > 0.5f)
				{
					col.xyz = _AreaColor.xyz * scanline;
					col.a = 1.0f;
				}
				else
				{
					col = _EdgeColor * edge;
				}
				*/
				return col;
			}
			ENDCG
		}
		/*
		void surf(Input IN, inout SurfaceOutputStandard o) {
				//領域展開上書き
				//stepなので0か1
				float scanline = step(
					distance(IN.worldPos.xyz, _PlayerPos.xyz),
					_AreaSize
				);

				//smoothstepは補間がはいる
				float edge = smoothstep(
					_AreaSize + _EdgeSize, _AreaSize,
					distance(IN.worldPos.xyz, _PlayerPos.xyz)
				);

				//エッジだけ塗るために雑だけどこうしてる
				o.Albedo = float3(0, 0, 0);
				if (scanline > 0.5f)
				{
					o.Albedo = _AreaColor.xyz * scanline;
					o.Alpha = 1.0f;
				}
				else
				{
					o.Albedo = _EdgeColor.xyz * edge;
					o.Alpha = edge;
				}
	//			o.Metallic = _Metallic;
	//			o.Smoothness = _Glossiness;
		}
		*/
	}
	FallBack "Diffuse"
}
