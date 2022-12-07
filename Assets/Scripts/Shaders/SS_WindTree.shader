Shader "Custom/SS_WindTree"
{
	Properties
	{

		//PBR properties 
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_BumpMap("Bumpmap", 2D) = "bump" {}


		// wind properties 
		_wind_dir("Wind Direction", Vector) = (0.5,0.05,0.5,0)
		_wind_size("Wind Wave Size", range(5,50)) = 15

		_tree_sway_stutter_influence("Tree Sway Stutter Influence", range(0,1)) = 0.2
		_tree_sway_stutter("Tree Sway Stutter", range(0,10)) = 1.5
		_tree_sway_speed("Tree Sway Speed", range(0,10)) = 1
		_tree_sway_disp("Tree Sway Displacement", range(0,1)) = 0.3

		_branches_disp("Branches Displacement", range(0,0.5)) = 0.3

		_leaves_wiggle_disp("Leaves Wiggle Displacement", float) = 0.07
		_leaves_wiggle_speed("Leaves Wiggle Speed", float) = 0.01

		_r_influence("Red Vertex Influence", range(0,1)) = 1
		_b_influence("Blue Vertex Influence", range(0,1)) = 1

	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard vertex:vert addshadow

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			//Declared Variables for WInd 

				float4 _wind_dir;
				float _wind_size;
				float _tree_sway_speed;
				float _tree_sway_disp;
				float _leaves_wiggle_disp;
				float _leaves_wiggle_speed;
				float _branches_disp;
				float _tree_sway_stutter;
				float _tree_sway_stutter_influence;
				float _r_influence;
				float _b_influence;
				float _CustomTime;
				float _windMultiplier = 1;

			sampler2D _MainTex;
			sampler2D _BumpMap;

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_BumpMap;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			// Vertex Manipulation Function

			void vert(inout appdata_full i) {

				//Gets the vertex's World Position 
				float3 worldPos = mul(unity_ObjectToWorld, i.vertex).xyz;

				//Tree Movement and Wiggle
				i.vertex.x += (cos(_CustomTime * _tree_sway_speed + (worldPos.x / _wind_size) + (sin(_CustomTime * _tree_sway_stutter * _tree_sway_speed + (worldPos.x / _wind_size)) * _tree_sway_stutter_influence)) + 1) / 2 * _tree_sway_disp*_windMultiplier * _wind_dir.x * (i.vertex.y / 10) +
					cos(_CustomTime * i.vertex.x * _leaves_wiggle_speed + (worldPos.x / _wind_size)) * _leaves_wiggle_disp * _wind_dir.x * i.color.b * _b_influence*_windMultiplier;

				i.vertex.z += (cos(_CustomTime * _tree_sway_speed + (worldPos.z / _wind_size) + (sin(_CustomTime * _tree_sway_stutter * _tree_sway_speed + (worldPos.z / _wind_size)) * _tree_sway_stutter_influence)) + 1) / 2 * _tree_sway_disp*_windMultiplier * _wind_dir.z * (i.vertex.y / 10) +
					cos(_CustomTime / 2 * i.vertex.z * _leaves_wiggle_speed + (worldPos.x / _wind_size)) * _leaves_wiggle_disp * _wind_dir.z * i.color.b * _b_influence*_windMultiplier;

				i.vertex.y += cos(_CustomTime * _tree_sway_speed + (worldPos.z / _wind_size)) * _tree_sway_disp*_windMultiplier * _wind_dir.y * (i.vertex.y / 10);

				//Branches Movement
				i.vertex.y += sin(_CustomTime * _tree_sway_speed + _wind_dir.x + (worldPos.z / _wind_size)) * _branches_disp * i.color.r * _r_influence*_windMultiplier;


			}

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)



			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;

	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

	// Metallic and smoothness come from slider variables
	o.Metallic = _Metallic;
	o.Smoothness = _Glossiness;
	o.Alpha = c.a;
}
ENDCG
		}
			FallBack "Diffuse"
}
