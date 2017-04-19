Shader "Junjie/ff_1" {
	Properties{
		_Color("Color", Color) = (1,0,0,1)
		_Ambient("Ambient", Color) = (0.3,0.3,0.3,0.3)
		_Specular("Specular", Color)= (1,1,1,1)
		_Shininess("Shininess", Range(0,8)) = 4
		_Emission("Emission", Color) = (1,1,1,1)
	}
	
	SubShader {
		Pass{
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
		}
		
	}

	FallBack "Diffuse"
}
