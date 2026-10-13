#version 330 core

in vec4 vColor;
in vec2 TexCoords;

out vec4 out_color;

uniform sampler2D uTexture;

void main()
{
    out_color = texture(uTexture, vec2(TexCoords.x, 1.0-TexCoords.y));
}