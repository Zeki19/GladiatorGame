Shader "Custom/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)

        _OutlineColor("Outline Color", Color) = (1,1,1,1)
        _OutlineSize("Outline Size", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType" = "Sprite"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 uv       : TEXCOORD0;
                float4 color    : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _Color;
            float4 _OutlineColor;
            float  _OutlineSize;

            v2f vert(appdata IN)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(IN.vertex);
                o.uv = IN.uv;
                o.color = IN.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Base sprite
                float4 c = tex2D(_MainTex, i.uv) * i.color;

                // If this pixel is part of the sprite, draw it normally
                if (c.a > 0) 
                    return c;
                
                // If this pixel is transparent AND there is no outline. RETURN FULL TRANSPARENCY
                if (_OutlineSize <= 0)
                    return float4(0, 0, 0, 0);


                // Sample neighbors
                float alpha = 0.0;

                alpha += tex2D(_MainTex, i.uv + float2(_OutlineSize * _MainTex_TexelSize.x, 0)).a;
                alpha += tex2D(_MainTex, i.uv - float2(_OutlineSize * _MainTex_TexelSize.x, 0)).a;
                alpha += tex2D(_MainTex, i.uv + float2(0, _OutlineSize * _MainTex_TexelSize.y)).a;
                alpha += tex2D(_MainTex, i.uv - float2(0, _OutlineSize * _MainTex_TexelSize.y)).a;

                if (alpha > 0)
                {
                    return float4(_OutlineColor.rgb, 1);
                }

                return c;
            }
            ENDCG
        }
    }
}
