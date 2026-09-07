using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Utils;


namespace Depth.Graphics.Shaders;

// Class for managing singular shaders
public class Shader
{
    private static GL gl = Video.gl;

    public static Reader reader = new Reader("assets/shaders");

    public string code; 
    public uint shader;
    public bool compiled = false;
    public ShaderType type;

    // clean up and fracture this code later
    public Shader(string path)
    {
        code = @"" + reader.ReadAllText(path);
        
        string typetest = Reader.GetExtension(path);
        bool foundType = false;
        
        switch (typetest)
        {
            case ".vert": type = ShaderType.VertexShader; foundType = true; break;
            case ".frag": type = ShaderType.FragmentShader; foundType = true; break;
            case ".comp": type = ShaderType.ComputeShader; foundType = true; break;
            case ".geom": type = ShaderType.GeometryShader; foundType = true; break;
            case ".tesc": type = ShaderType.TessControlShader; foundType = true; break;
            case ".tese": type = ShaderType.TessEvaluationShader; foundType = true; break;
        }

        if (!foundType)
            throw new Exception("Error: Could not find type of shader: " + path);

        Compile();
    }

    public void Compile()
    {
        shader = gl.CreateShader(type);
        gl.ShaderSource(shader, code);
        gl.CompileShader(shader);
        gl.GetShader(shader, ShaderParameterName.CompileStatus, out int status);
        if (status != (int) GLEnum.True)
            throw new Exception(type.ToString() + " failed to compile:" + gl.GetShaderInfoLog(shader));

        compiled = true;        
    }

    public void Delete()
    { 
        gl.DeleteShader(shader);
        compiled = false;
    }
}