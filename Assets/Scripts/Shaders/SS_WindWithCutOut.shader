﻿// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Transparent/Cutout/Diffuse Shake" {

    Properties{
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
        _Cutoff("Alpha cutoff", Range(0,1)) = 0.5
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

        SubShader{
            Tags {"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
            LOD 200

        CGPROGRAM
        #pragma target 3.0
        #pragma surface surf Lambert alphatest:_Cutoff vertex:vert addshadow

        sampler2D _MainTex;
        fixed4 _Color;
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

        struct Input {
            float2 uv_MainTex;
           
        };

        void vert(inout appdata_full i) {

            //Gets the vertex's World Position 
            float3 worldPos = mul(unity_ObjectToWorld, i.vertex).xyz;

            //Tree Movement and Wiggle
            i.vertex.x += (cos(_CustomTime * _tree_sway_speed + (worldPos.x / _wind_size) + (sin(_CustomTime * _tree_sway_stutter * _tree_sway_speed + (worldPos.x / _wind_size)) * _tree_sway_stutter_influence)) + 1) / 2 * _tree_sway_disp*_windMultiplier * _wind_dir.x * (i.vertex.y / 10) +
                cos(_CustomTime * i.vertex.x * _leaves_wiggle_speed + (worldPos.x / _wind_size)) * _leaves_wiggle_disp * _wind_dir.x * i.color.b * _b_influence*_windMultiplier;

            i.vertex.z += (cos(_CustomTime * _tree_sway_speed + (worldPos.z / _wind_size) + (sin(_CustomTime * _tree_sway_stutter * _tree_sway_speed + (worldPos.z / _wind_size)) * _tree_sway_stutter_influence)) + 1) / 2 * _tree_sway_disp*_windMultiplier * _wind_dir.z * (i.vertex.y / 10) +
                cos(_CustomTime/2 * i.vertex.z * _leaves_wiggle_speed + (worldPos.x / _wind_size)) * _leaves_wiggle_disp * _wind_dir.z * i.color.b * _b_influence*_windMultiplier;

            i.vertex.y += cos(_CustomTime * _tree_sway_speed + (worldPos.z / _wind_size)) * _tree_sway_disp*_windMultiplier * _wind_dir.y * (i.vertex.y / 10);

            //Branches Movement
            i.vertex.y += sin(_CustomTime * _tree_sway_speed + _wind_dir.x + (worldPos.z / _wind_size)) * _branches_disp * i.color.r * _r_influence*_windMultiplier;


        }

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
        }

            Fallback "Transparent/Cutout/VertexLit"
}