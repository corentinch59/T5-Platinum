Shader "Unlit/CollisionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", float) = 1
        _BlackRadius ("BlackRadius", float) = 1
        _EffectSpeed ("EffectSpeed", float) = 0.001
        _MiddleMask ("MiddleMask", float) = 0.001
        _OutsideMask ("OutsideMask", float) = 0.001
        _Intensity ("Intensity", float) = 1
        _Smoothness ("Smoothness", float) = 1
        _Slider ("Slider", Range(0, 2.5)) = 0
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        AlphaToMask On
        Blend SrcAlpha DstAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"


            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;
            float _EffectSpeed;
            float _MiddleMask;
            float _BlackRadius;
            float _Slider;
            float _Smoothness;
            float _Intensity;
            float _OutsideMask;
            float4 _Color;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };



            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvCenter = i.uv * 2 - 1;
                float radialDistance = length(uvCenter);
                /*
                
                float circleEffect = cos((radialDistance - _Time.y * _EffectSpeed) * 6.28 * 5) * 0.5 + 0.5;
                circleEffect *= 1 - radialDistance;
                circleEffect *= radialDistance - _MiddleMask;
                return circleEffect;
                */
                
                
                float  circle = 1 - smoothstep(_Radius + _Slider -(_Radius * _Smoothness + _Slider ),_Radius + _Slider
                    +(_Radius* _Smoothness + _Slider ),dot(uvCenter,uvCenter) * _Intensity);
                float  blackCircle = 1 - smoothstep(_BlackRadius + _Slider -(_BlackRadius * _Smoothness + _Slider*1.2),_BlackRadius
                    + _Slider*1.2 +(_BlackRadius * _Smoothness + _Slider *1.2),dot(uvCenter,uvCenter) * _Intensity);
                
                //return blackCircle -  circle;
                float circleEffect = blackCircle -  circle ;
                circleEffect *= 1 - radialDistance;
                circleEffect *= radialDistance * 2;
                //return  float4(uvCenter, 0, 1);
                //return 1 - smoothstep(circleEffect, circleEffect, 0.05)  + circleEffect;
                return  circleEffect * 10 * _Color;
                //return circleEffect + circleEffect * 2;
                
            }
            ENDCG
        }
    }
}
