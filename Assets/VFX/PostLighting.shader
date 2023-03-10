Shader "Hidden/PostLighting"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RenderTex("Texture", 2D) = "white" {}
        _MinLight("Float", float) = 0.1 
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _RenderTex;
            fixed _MinLight;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                fixed4 lightPixel = tex2D(_RenderTex, i.uv);
                
                fixed lightContribution = lightPixel.a;
                fixed3 lightColour = lightPixel.rgb;

                col.rgb *= lightColour * (max(_MinLight,lightContribution));
                //col.rgb = lerp(col.rgb, lightColour, lightContribution * 0.5f);
                return col;
            }
            ENDCG
        }
    }
}
