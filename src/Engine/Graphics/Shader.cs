using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Utils;

namespace Depth.Graphics.Shaders;


struct ShaderTypeData
{
    public string[] names;
    public ShaderType type;
}

// Class for managing singular shaders
public class Shader
{
    private static ShaderTypeData[] shadertypes =
    [
        new ShaderTypeData {names = ["vertex", "vert", "vsh", "vs", "v"],            type = ShaderType.VertexShader},
        new ShaderTypeData {names = ["fragment", "frag", "fsh", "fs", "f"],          type = ShaderType.FragmentShader},
        new ShaderTypeData {names = ["geometry", "geom", "geo", "gsh", "gs", "g"],   type = ShaderType.GeometryShader},
        new ShaderTypeData {names = ["compute", "comp", "csh", "cs", "c"],           type = ShaderType.ComputeShader},
        new ShaderTypeData {names = ["tessctrl", "tesc", "tcsh", "tcs", "tc"],       type = ShaderType.TessControlShader},
        new ShaderTypeData {names = ["tesseval", "tese", "tesh", "tes", "te"],       type = ShaderType.TessEvaluationShader},
    ];

    private static GL gl = Video.gl;
    public static Reader reader = new Reader("assets/shaders");

    public string code;
    public string filepath;
    public uint shader;
    public bool compiled = false;
    public ShaderType type;

    public Shader(string path)
    {
        code = @"" + reader.ReadAllText(path);
        filepath = path;

        ShaderType? pendingType = null;

        foreach (var data in shadertypes)
        {
            pendingType = CheckShaderTypeFromPath(path, data.names)? 
                data.type : null;
            if (pendingType != null) break;
        }

        if (pendingType == null)
            throw new Exception("Error: Could not find type of shader in: \n\t" + path);

        type = (ShaderType)pendingType;
        Compile();
    }
    

    private bool CheckShaderTypeFromPath(string path, string[] extensions)
    {
        foreach (string ext in extensions) 
            if (Reader.GetExtension(path) == "." + ext) return true;
        
        return false;
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