Shader "Unlit/DistortionEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SmoothRadius("SmoothRadius", float) = (1,1,1,1)
        _Slider ("Slider", Range(0, 0.9)) = 0
        
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" ="Transparent" }
        LOD 100
        AlphaToMask Off
        Zwrite Off
        Blend SrcAlpha DstAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _SmoothRadius;
            float _Slider;

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
                
                float whiteCircle =  1 - smoothstep(_SmoothRadius.x + _Slider -(_SmoothRadius.x * 0.01), _SmoothRadius.y + _Slider + (_SmoothRadius.y * 0.01), dot(uvCenter, uvCenter));
                float blackCircle = smoothstep(_SmoothRadius.z + _Slider -(_SmoothRadius.z * 0.01), _SmoothRadius.w + _Slider + (_SmoothRadius.z * 0.01), dot(uvCenter, uvCenter));
                float circleEffect = whiteCircle * blackCircle;
                circleEffect *= 1 - radialDistance;
                return  circleEffect;
            }
            ENDCG
        }
    }
}
