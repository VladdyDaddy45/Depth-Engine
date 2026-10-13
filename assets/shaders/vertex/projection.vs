#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec3 aColor;
layout (location = 3) in vec2 aTexCoords;
layout (location = 4) in mat4 transform;

//mat4 Transform = mat4(transform);

out vec4 vColor;
out vec2 TexCoords;

uniform mat4 proj;
uniform mat4 view;
//uniform vec3 LightDirection;
uniform float AmbientLight;
uniform float Time;

void main()
{
    TexCoords = aTexCoords;

    vec4 proposedPosition = proj * view * transform * vec4(aPosition, 1.0);
    gl_Position = proposedPosition;

    vec4 normal = vec4(aNormal,1.0) * transform;
    //vec4 light = vec4(LightDirection,1.0);
    vColor = vec4(aColor,1.0);//vec4(aColor,1.0) * (AmbientLight + ( (1.0-AmbientLight) * ( (dot(normalize(light), normal) + 1) / 2 ) ));
}