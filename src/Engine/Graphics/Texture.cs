using Depth;
using Silk.NET.OpenGL;

namespace Depth.Graphics;

using TexWrapMode = TextureWrapMode;

public class Texture
{
    private static GL gl = Video.gl;

    private float[] coords {get; set;}
    private uint tex;

    public string path;
    public TexWrapMode wrap {get; set;}

    public Texture(string Path, TexWrapMode Wrap)
    {
        path = Path;
        wrap = Wrap;

        gl.TexParameter(TextureTarget.Texture2D, GLEnum.TextureWrapS, (int) GLEnum.ClampToEdge);
        gl.TexParameter(TextureTarget.Texture2D, GLEnum.TextureWrapT, (int) GLEnum.ClampToEdge);
        gl.TexParameter(TextureTarget.Texture2D, GLEnum.TextureWrapR, (int) GLEnum.ClampToEdge);
    }

    public void Apply()
    {
    }
}