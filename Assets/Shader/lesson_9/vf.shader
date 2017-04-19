Shader "Junjie/vf" {
	
	SubShader {

		pass{
			CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
#pragma exclude_renderers gles

			#pragma vertex vert
			#pragma fragment frag

			typedef float4 FL4;

		    //结构体
		    //struct v2f
		    //{
		    //	float4 pos;
		    //	float2 uv;
		    //};


			void vert(in float2 objPos:POSITION, out float4 col:COLOR, out float4 pos:POSITION)
			{
				pos = float4(objPos,0,1);
				col = pos;
			}

			void frag(inout float4 col:COLOR)
			{
				//col = float4(0,1,0,1);

				//float r = 1;
				//float g = 0;
				//float b = 0;
				//float a = 1;
				//col = float4(r,g,b,a);

				//half r = 1;
				//half g = 0;
				//half b = 0;
				//half a = 1;
				//col = half4(r,g,b,a);


				fixed r = 1;
				fixed g = 0;
				fixed b = 0;
				fixed a = 1;
				col = fixed4(r,g,b,a);

				bool bl = false;
				col = bl?col :float4(0,1,0,1);

				FL4 fl4 = FL4(1,1,0,1);
				col = fl4;

				//向量
				float2x4 M2x4 = {1,1,0,1,  0,1,1,1};
				float3x4 M3x4 = {{1,1,0,1},  {0,1,1,1}, {0,0,1,1}};
				col = M2x4[1];


				//数组
				float arr[4] = {1,0.5,0.5,1};
				col = float4(arr[0], arr[1],arr[3],arr[2]);

				//结构体的使用
				//v2f o;
				//o.pos = fl4;
				//o.uv = float2(1,1);
			}

			ENDCG
		}
		
	}

}
