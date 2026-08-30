#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;
layout (location = 2) in mat4 transform;

//mat4 Transform = mat4(transform);

out vec4 vColor;

uniform mat4 proj;
uniform mat4 view;
uniform mat4 uTransform;
uniform float Time;

void main()
{
    vec4 proposedPosition = proj * view * transform * uTransform * vec4(aPosition, 1.0);
    gl_Position = proposedPosition;

    vColor = vec4(aColor,1.0);
}