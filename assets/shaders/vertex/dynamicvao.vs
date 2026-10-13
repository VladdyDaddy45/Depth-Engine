#version 430 core

struct VertexData {
    float pos[3];
    float col[3];
    float nor[3];
}

layout (binding = 0, std430) readonly buffer ssbo1 {
    VertexData data[];
}

out vec4 vColor;

uniform mat4 proj;
uniform mat4 view;
uniform vec3 col0, col1, col2;
uniform vec3 LightDirection;
uniform float AmbientLight;
uniform float Time;

vec3 getPos(int i) {
    return vec3(
        data[i].pos[0],
        data[i].pos[1],
        data[i].pos[2],
    )
}

vec3 getCol(int i) {
    return vec3(
        data[i].col[0],
        data[i].col[1],
        data[i].col[2],
    )
}

vec3 getNor(int i) {
    return vec3(
        data[i].nor[0],
        data[i].nor[1],
        data[i].nor[2],
    )
}

mat3 m3 = mat3(col0, col1, col2);
mat4 uniformTransform = mat4(m3);

void main()
{
    vec4 proposedPosition = proj * view * uniformTransform * transform * vec4(getPos(gl_VertexID), 1.0);
    gl_Position = proposedPosition;

    vColor = vec4(aColor,1.0) * (AmbientLight + ( (1.0-AmbientLight) * ( (dot(normalize(LightDirection), aNormal) + 1) / 2 ) ));
}