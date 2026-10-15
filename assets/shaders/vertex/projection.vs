#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;
layout (location = 3) in mat4 aTransform;

out vec4 vColor;
out vec2 TexCoords;

out VS_OUT {
    vec3 vNormal;
    vec2 vTexCoords;
    mat4 vTransform;
} vs_out;

uniform mat4 proj;
uniform mat4 view;
uniform float Time;

void main()
{
    TexCoords = aTexCoords;
    vs_out.vNormal = aNormal;
    vs_out.vTexCoords = aTexCoords;
    
    vec4 proposedPosition = proj * view * aTransform * vec4(aPosition, 1.0);
    gl_Position = proposedPosition;

    vec4 normal = vec4(aNormal,1.0) * aTransform;
    //vec4 light = vec4(LightDirection,1.0);
    vColor = vec4(0.5,0.5,0.5,1.0);//vec4(aColor,1.0) * (AmbientLight + ( (1.0-AmbientLight) * ( (dot(normalize(light), normal) + 1) / 2 ) ));
}