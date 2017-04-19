Shader "Junjie/ff_2" {
	Properties{
		_Color("Color", Color) = (1,0,0,1)
		_Ambient("Ambient", Color) = (0.3,0.3,0.3,0.3)
		_Specular("Specular", Color)= (1,1,1,1)
		_Shininess("Shininess", Range(0,8)) = 4
		_Emission("Emission", Color) = (1,1,1,1)
		_MainTexture("MainTexuture", 2D) ="white"{}
		_SecTexture("SecTexture", 2D) = "white"{}
		_Constant("ConstatntColor", Color) = (1,1,1,0.3)
	}
	
	SubShader {

		Tags{"Queue" = "Transparent"}

		Pass{
				Blend SrcAlpha OneMinusSrcAlpha

				Material
				{
				 	Diffuse[_Color]
				 	Ambient[_Ambient]
				 	Specular[_Specular]
				 	Shininess[_Shininess]
				 	Emission[_Emission]
				}

				Lighting on
				SeparateSpecular on

				SetTexture[_MainTexture]
				{
					combine texture * primary double  //quad  四倍   primary 代表之前的顶点光照信息

				}

				SetTexture[_SecTexture]
				{
					constantcolor[_Constant]
					combine texture * previous double, texture * constant  //只取texture的alpha值

				}
		}
		
	}

	FallBack "Diffuse"
}
