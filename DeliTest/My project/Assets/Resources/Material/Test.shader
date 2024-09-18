Shader "Custom/Test"
{
    Properties
    {

    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : POSITION;
            };

            // 传入的旋转矩阵
            float4x4 _RotationMatrix;

            v2f vert (appdata_t v)
            {
                v2f o;

                // 反向旋转顶点位置
                float4 reversedPosition = mul(_RotationMatrix, v.vertex);
                
                // 将旋转后的顶点位置传递给片段着色器
                o.pos = UnityObjectToClipPos(reversedPosition);

                return o;
            }

            half4 frag (v2f i) : COLOR
            {
                return (1,1,1,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
