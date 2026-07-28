#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;

out vec4 vColor;

uniform mat4 transform;
uniform mat4 proj;
uniform mat4 view;
uniform float Time;

void main()
{
    gl_Position = proj * view * transform * vec4(aPosition, 1.0);
    
    vColor = vec4(aColor,1.0);
}