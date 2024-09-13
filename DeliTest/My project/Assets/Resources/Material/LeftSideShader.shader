Shader "Custom/LeftSideShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            fixed4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // 使用物体的世界空间位置计算左侧
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 cameraDir = _WorldSpaceCameraPos - worldPos;

                // 计算左侧方向（-X 轴）
                float dotProduct = dot(cameraDir, float3(-1, 0, 0));
                o.color = dotProduct > 0 ? fixed4(0.2, 0.2, 0.2, 1) : _Color; // 深灰色或原色

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}