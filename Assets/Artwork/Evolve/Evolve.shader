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
			// �̈�W�J
			float4 _PlayerPos; //�v���C���[�ʒu
			float4 _AreaColor; //�̈���̐F
			float4 _EdgeColor; //�̈�̃G�b�W�̐F
			float _AreaSize; //�̈�̑傫��(�O�����瑀�삷��)
			float _EdgeSize; //�G�b�W�̕�
			////

			// other
			half _Glossiness;
			half _Metallic;

			fixed4 frag(Input IN) : SV_Target
			{
				float4 col = float4(0, 0, 0, 0);
				/*
				//�̈�W�J�㏑��
				//step�Ȃ̂�0��1
				float scanline = step(
					distance(IN.worldPos.xyz, _PlayerPos.xyz),
					_AreaSize
				);

				//smoothstep�͕�Ԃ��͂���
				float edge = smoothstep(
					_AreaSize + _EdgeSize, _AreaSize,
					distance(IN.worldPos.xyz, _PlayerPos.xyz)
				);

				//�G�b�W�����h�邽�߂ɎG�����ǂ������Ă�
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
				//�̈�W�J�㏑��
				//step�Ȃ̂�0��1
				float scanline = step(
					distance(IN.worldPos.xyz, _PlayerPos.xyz),
					_AreaSize
				);

				//smoothstep�͕�Ԃ��͂���
				float edge = smoothstep(
					_AreaSize + _EdgeSize, _AreaSize,
					distance(IN.worldPos.xyz, _PlayerPos.xyz)
				);

				//�G�b�W�����h�邽�߂ɎG�����ǂ������Ă�
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
