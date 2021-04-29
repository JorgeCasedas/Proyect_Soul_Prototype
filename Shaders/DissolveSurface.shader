Shader "Custom/DissolveSurface" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		//Dissolve properties
		_DissolveTexture("Dissolve Texutre", 2D) = "white" {}
		_Amount("Amount", Range(0,1)) = 0

		//Fresnel properties
		_FresnelColor("Fresnel Color", Color) = (1,1,1,1)
		[PowerSlider(4)] _FresnelExponent("Fresnel Exponent", Range(0.25, 4)) = 1
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200
			Cull Off //Fast way to turn your material double-sided

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows

			#pragma target 3.0

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
				float3 worldNormal;
				float3 viewDir;
				INTERNAL_DATA
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			//Dissolve properties
			sampler2D _DissolveTexture;
			half _Amount;

			//Fresnel properties
			float3 _FresnelColor;
			float _FresnelExponent;

			void surf(Input IN, inout SurfaceOutputStandard o) {

				//Dissolve function
				half dissolve_value = tex2D(_DissolveTexture, IN.uv_MainTex).r;
				clip(dissolve_value - _Amount);
				o.Emission = fixed3(1, 1, 1) * step(dissolve_value - _Amount, 0.005f);
				//Basic shader function
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

				o.Albedo = c.rgb;
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;

				//Fresnel
				//get the dot product between the normal and the view direction
				float fresnel = dot(IN.worldNormal, IN.viewDir);
				//invert the fresnel so the big values are on the outside
				fresnel = saturate(1 - fresnel);
				//raise the fresnel value to the exponents power to be able to adjust it
				fresnel = pow(fresnel, _FresnelExponent);
				//combine the fresnel value with a color
				float3 fresnelColor = fresnel * _FresnelColor;
				//apply the fresnel value to the emission
				o.Emission += fresnelColor;
			}
			ENDCG
			}
				FallBack "Diffuse"
}