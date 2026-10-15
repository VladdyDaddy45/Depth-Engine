#version 330 core
layout (triangles) in;
layout (triangle_strip) out;
layout (max_vertices = 6) out;

in VS_OUT {
    vec3 aNormal;
    vec3 aColor;
    vec2 aTexCoords;
    mat4 transform;
} gs_in[];

out vec4 vColor;
out vec2 TexCoords;

void main() 
{
    gl_Position = gl_in[0].gl_Position;
    vColor = vec4(gs_in[0].aColor,1.0);
    TexCoords = gs_in[0].aTexCoords;
    EmitVertex();

    gl_Position = gl_in[1].gl_Position;
    vColor = vec4(gs_in[1].aColor,1.0);
    TexCoords = gs_in[1].aTexCoords;
    EmitVertex();

    gl_Position = gl_in[2].gl_Position;
    vColor = vec4(gs_in[2].aColor,1.0);
    TexCoords = gs_in[2].aTexCoords;
    EmitVertex();

    EndPrimitive();

    gl_Position = gl_in[0].gl_Position + vec4(0.5,0.0,0.0,0.0);
    vColor = vec4(gs_in[0].aColor,1.0);
    TexCoords = gs_in[0].aTexCoords;
    EmitVertex();

    gl_Position = gl_in[1].gl_Position + vec4(0.5,0.0,0.0,0.0);
    vColor = vec4(gs_in[1].aColor,1.0);
    TexCoords = gs_in[1].aTexCoords;
    EmitVertex();

    gl_Position = gl_in[2].gl_Position + vec4(0.5,0.0,0.0,0.0);
    vColor = vec4(gs_in[2].aColor,1.0);
    TexCoords = gs_in[2].aTexCoords;
    EmitVertex();

    EndPrimitive();
}