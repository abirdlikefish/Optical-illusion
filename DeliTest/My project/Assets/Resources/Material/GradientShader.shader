Shader "Custom/GradientDirection"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1,0,0,1) // 红色
        _Color2 ("Color 2", Color) = (0,1,0,1) // 绿色
        _Direction ("Gradient Direction", Vector) = (0,0,0) // 渐变方向
        _Thickness ("Thickness", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION; // 顶点位置
            };

            struct v2f
            {
                float4 pos : SV_POSITION; // 裁剪空间位置
                float3 localPos : TEXCOORD0; // 局部位置
            };

            fixed4 _Color1; // 第一个颜色
            fixed4 _Color2; // 第二个颜色
            Vector _Direction; // 过渡方向
            float _Thickness; // 渐变厚度
            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // 直接使用顶点的局部位置
                o.localPos = v.vertex.xyz;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t;


                if (_Direction.x == 0.5)
				{
					t = saturate((i.localPos.x - 0.5 + _Thickness)/_Thickness);
                    // t = saturate(i.localPos.x * 2);
				}
				else if (_Direction.y == 0.5)
				{
					t = saturate(i.localPos.y*2);
				}
				else if (_Direction.z == 0.5)
				{
					t = saturate(i.localPos.z*2);
				}

                // t = saturate(i.localPos.x*2);
                // t = saturate(i.localPos.y*2);
                // t = saturate(i.localPos.z*2);

                return lerp(_Color1, _Color2, t); // 线性插值
            }
            ENDCG
        }
    }
}