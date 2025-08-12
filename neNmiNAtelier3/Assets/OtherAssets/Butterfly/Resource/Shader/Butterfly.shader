Shader "Butterfly/Butterfly"
{
    Properties
    {
        [NoScaleOffset]_Alpha("AlphaMap",2D) = "White"{}
        [NoScaleOffset]_Pattern("PatternMap",2D) = "White"{}
        [NoScaleOffset]_Tip("TipMask",2D) = "White"{}
        [NoScaleOffset]_RootToTip("RootToTipMap",2D) = "White"{}
        [Space(20)]

        _BaseColor("BaseColor",Color) = (1,1,1,1)
        _PatternColor("PatternColor",Color) = (0,0,0,1)
        _TipColor("TipColor",Color) = (1,1,1,1)
        _RootColor("RootColor",Color) = (0.8,0.8,0.8,1)
        [Space(20)]

        [MaterialToggle]_BlendScreen("BlendScreen",int) = 0
        [Space(20)]

        _RootMaskIntensity("RootMaskIntensity",float) = 1
        [Space(20)]

        _FlapScale("FlapScale",float) = 0.1
        _FlapSpeed("FlapSpeed",float) = 500
    }
    SubShader
    {
        Tags { "Queue" = "AlphaTest" "RenderType"="Opaque" }

        Cull Off

        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float2 texcoord : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _Alpha;
            sampler2D _Pattern;
            sampler2D _Tip;
            sampler2D _RootToTip;

            float4 _BaseColor;
            float4 _PatternColor;
            float4 _TipColor;
            float4 _RootColor;

            float _RootMaskIntensity;

            int _BlendScreen;

            float4 _Alpha_ST;

            float _FlapScale;
            float _FlapSpeed;


            float4 ColorBlendScreen(fixed4 color1, fixed4 color2)
            {
                fixed4 c1 = 1 - color1;
                fixed4 c2 = 1 - color2;
                fixed4 col = 1 - (c1 * c2);

                return col;
            }

            fixed4 ColorBlendMultiply(fixed4 color1, fixed4 color2)
            {
                fixed4 col = color1 * color2;
                return col;
            }


            v2f vert (appdata v)
            {
                v2f o;   

                //Flap
                float i = sin(_Time * _FlapSpeed) * tex2Dlod(_Tip, float4(v.texcoord.xy, 0, 0)).r * _FlapScale;
                float3 flapVector = v.normal * i;
                v.vertex.xyz += flapVector;

                o.uv = TRANSFORM_TEX(v.uv, _Alpha);
                o.vertex = UnityObjectToClipPos(v.vertex);

                UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                float rootMask = pow(tex2D(_RootToTip, i.uv).r,_RootMaskIntensity); 
                float patternMask = tex2D(_Pattern, i.uv).r;

                fixed4 baseColor = _BaseColor;
                fixed4 blendColor = lerp(_TipColor, _RootColor, rootMask);
                fixed4 patternColor = _PatternColor;

                fixed4 col = _BlendScreen ? ColorBlendScreen(baseColor, blendColor) : ColorBlendMultiply(baseColor,blendColor);
                col = lerp(patternColor, col, patternMask);


                clip(tex2D(_Alpha, i.uv).r - 0.5);


                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);


                return col;
            }
            ENDCG
        }
    }
}
