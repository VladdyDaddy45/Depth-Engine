global using TexUnit = Silk.NET.OpenGL.TextureUnit;

using Depth.Graphics.Shaders;
using Silk.NET.OpenGL;
using SkiaSharp;

namespace Depth.Graphics;

public enum TextureType
{
    Standard,
    Diffuse,
}

public class Texture : IDisposable
{
    private static GL gl = Video.gl;

    private uint handle;
    public TextureUnit texNumber;
    public string path, name;

    public unsafe Texture(string Path, string Name, TextureUnit TextureNumber)
    {
        path = Path;
        name = Name;
        
        var bitmap = SKBitmap.Decode(Path) 
            ?? throw new Exception("Failed to load texture image:\n\t" + Path);
        var image = bitmap.Copy(SKColorType.Rgba8888);
        //byte[] flipped = FlipVertical(image);

        handle = gl.GenTexture();
        Bind();

        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);

        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.LinearMipmapLinear);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
        
        
        gl.TexImage2D(GLEnum.Texture2D, 0, (int)GLEnum.Rgba8, (uint)image.Width, (uint)image.Height, 0, GLEnum.Rgba, GLEnum.UnsignedByte, image.Bytes);

        gl.GenerateMipmap(GLEnum.Texture2D);
    }

    public void Bind()
    {
        gl.BindTexture(GLEnum.Texture2D, handle);
    }

    public void PassToShader(ShaderProgram program)
    {
        gl.BindTexture(GLEnum.Texture2D, handle);
        gl.ActiveTexture(TexUnit.Texture0);
        gl.Uniform1(gl.GetUniformLocation(program.handle, name), 0);
    }

    public string InspectPixel(SKBitmap map, int x, int y)
    {
        return ""+ 
            "(R: " + map.Bytes[(y*map.RowBytes) + x*4] +
            ",G: " + map.Bytes[(y*map.RowBytes) + x*4+1] + 
            ",B: " + map.Bytes[(y*map.RowBytes) + x*4+2] + 
            ",A: " + map.Bytes[(y*map.RowBytes) + x*4+3] + ")";
    }

    public byte[] FlipVertical(SKBitmap image)
    {
        byte[] result = new byte[image.RowBytes * image.Height];

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var ry = (y*image.RowBytes) + (x * image.BytesPerPixel);
                var iy = ((image.Height-y-1) * image.RowBytes) + (x * image.BytesPerPixel);
                Console.Write("•");
                result[ry] = image.Bytes[iy];
                result[ry+1] = image.Bytes[iy+1];
                result[ry+2] = image.Bytes[iy+2];
                result[ry+3] = image.Bytes[iy+3];
            }
            Console.WriteLine();
        }

        return result;
    }

    public void Dispose()
    { gl.DeleteTexture(handle); }
}