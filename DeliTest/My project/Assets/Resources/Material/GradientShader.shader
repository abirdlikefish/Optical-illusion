Shader "Custom/GradientDirection"
{
    Properties
    {
        _ColorBase ("_ColorBase", Color) = (1,0,0,1)

        _ColorUp ("Color Up", Color) = (0,1,0,1)
        _ColorDown ("Color Down", Color) = (0,0,1,1)
        _ColorLeft ("Color Left", Color) = (1,1,0,1)
        _ColorRight ("Color Right", Color) = (1,0,1,1)
        _ColorFront ("Color Front", Color) = (0,1,1,1)
        _ColorBehind ("Color Behind", Color) = (1,1,1,1)

        _CanChangeUp ("CanChange UP", Range(0,1)) = 0
        _CanChangeDown ("CanChange DOWN", Range(0,1)) = 0
        _CanChangeLeft ("CanChange LEFT", Range(0,1)) = 0
        _CanChangeRight ("CanChange RIGHT", Range(0,1)) = 0
        _CanChangeFront ("CanChange FRONT", Range(0,1)) = 0
        _CanChangeBehind ("CanChange BEHIND", Range(0,1)) = 0

        _DirectionUp ("Gradient Direction UP", Vector) = (0,1,0) // 渐变方向
        _DirectionDown ("Gradient Direction DOWN", Vector) = (0,-1,0) // 渐变方向
        _DirectionLeft ("Gradient Direction LEFT", Vector) = (-1,0,0) // 渐变方向
        _DirectionRight ("Gradient Direction RIGHT", Vector) = (1,0,0) // 渐变方向
        _DirectionFront ("Gradient Direction FRONT", Vector) = (0,0,-1) // 渐变方向
        _DirectionBehind ("Gradient Direction BEHIND", Vector) = (0,0,1) // 渐变方向

        _Thickness ("Thickness", Float) = 0.5
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

            fixed4 _ColorBase;
            
            fixed4 _ColorUp;
            fixed4 _ColorDown;
            fixed4 _ColorLeft;
            fixed4 _ColorRight;
            fixed4 _ColorFront;
            fixed4 _ColorBehind;

            float _CanChangeUp;
            float _CanChangeDown;
            float _CanChangeLeft;
            float _CanChangeRight;
            float _CanChangeFront;
            float _CanChangeBehind;

            Vector _DirectionUp;
            Vector _DirectionDown;
            Vector _DirectionLeft;
            Vector _DirectionRight;
            Vector _DirectionFront;
            Vector _DirectionBehind;
            float _Thickness; // 渐变厚度
            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.localPos = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t;
                fixed4 curColor = _ColorBase;
                //6个方向上的渐变
                if(i.localPos.x >= 0 && _CanChangeRight != 0)
                {
                    t = saturate((i.localPos.x - 0.5 + _Thickness)/_Thickness) / 2;
                    curColor = lerp(curColor, _ColorRight, t); // 线性插值
                }
                if(i.localPos.x <= 0 && _CanChangeLeft != 0)
                {
                    t = saturate((-i.localPos.x - 0.5 + _Thickness)/_Thickness) / 2;
                    curColor = lerp(curColor, _ColorLeft, t); // 线性插值
                }
                if(i.localPos.y >= 0 && _CanChangeUp != 0)
				{
					t = saturate((i.localPos.y - 0.5 + _Thickness)/_Thickness) / 2;
					curColor = lerp(curColor, _ColorUp, t); // 线性插值
				}
                if(i.localPos.y <= 0 && _CanChangeDown != 0)
                {
                    t = saturate((-i.localPos.y - 0.5 + _Thickness)/_Thickness) / 2;
					curColor = lerp(curColor, _ColorDown, t); // 线性插值
				}
                if(i.localPos.z >= 0 && _CanChangeBehind != 0)
				{
					t = saturate((i.localPos.z - 0.5 + _Thickness)/_Thickness) / 2;
                    curColor = lerp(curColor, _ColorBehind, t); // 线性插值
                }
                if(i.localPos.z <= 0 && _CanChangeFront != 0)
                {
					t = saturate((-i.localPos.z - 0.5 + _Thickness)/_Thickness) / 2;
					curColor = lerp(curColor, _ColorFront, t); // 线性插值
				}
                
                return curColor;
            }
            ENDCG
        }
    }
     FallBack "Diffuse"
}