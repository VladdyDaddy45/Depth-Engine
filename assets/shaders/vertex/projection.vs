#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec3 aColor;
layout (location = 3) in mat4 transform;

//mat4 Transform = mat4(transform);

out vec4 vColor;

uniform mat4 proj;
uniform mat4 view;
uniform vec3 col0, col1, col2;
uniform vec3 LightDirection;
uniform float AmbientLight;
uniform float Time;

mat3 m3 = mat3(col0, col1, col2);
mat4 uniformTransform = mat4(m3);

void main()
{
    vec4 proposedPosition = proj * view * uniformTransform * transform * vec4(aPosition, 1.0);
    gl_Position = proposedPosition;

    vColor = vec4(aColor,1.0) * (AmbientLight + ( (1.0-AmbientLight) * ( (dot(normalize(LightDirection), aNormal) + 1) / 2 ) ));
}