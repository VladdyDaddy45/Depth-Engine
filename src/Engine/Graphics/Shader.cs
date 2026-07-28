using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Utils;


namespace Engine.Graphics.Shaders;

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


public enum Uniform
{
    Float1,
    Float2,
    Float3,
    Float4,
    Matrix
}

// Class for shader programs
public class Program
{
    private static GL gl = Video.gl;
    private static List<Program> Objects = new List<Program>();

    public uint program;
    public bool linked = false;

    public Program() 
    {
        program = gl.CreateProgram();
        Objects.Add(this);
    }

    // Will attach every shader provided in the array
    public Program(Shader[] shaders)
    {
        program = gl.CreateProgram();
        foreach (Shader shader in shaders)
        {
            Attach(shader);
        }

        Link();
        Objects.Add(this);
    }

    public void Use()
    {
        gl.UseProgram(program);
    }
    


    public void Uniform(string name, int value)
    {
        int location = gl.GetUniformLocation(program, name);
        if (location == -1)
            throw new Exception($"Uniform {name} not found in ShaderProgram");
        
        gl.Uniform1(location, value);
    }

    public void Uniform(string name, float value)
    {
        int location = gl.GetUniformLocation(program, name);
        if (location == -1)
            throw new Exception($"Uniform {name} not found in ShaderProgram");
        
        gl.Uniform1(location, value);
    }

    public unsafe void Uniform(string name, Matrix4x4 value)
    {
        gl.UseProgram(program);
        int location = gl.GetUniformLocation(program, name);
        if (location == -1)
            throw new Exception ($"Uniform {name} not found in ShaderProgram");

        gl.UniformMatrix4(location, 1, false, (float*) &value);
    }



    public void Attach(Shader shader)
    {
        gl.AttachShader(program,shader.shader);
    }

    public void Detach(Shader shader)
    {
        gl.DetachShader(program, shader.shader);
    }

    public void Link()
    {
        gl.LinkProgram(program);
        gl.GetProgram(program, ProgramPropertyARB.LinkStatus, out int status);
        if (status != (int) GLEnum.True)
            throw new Exception("Program failed to link: " + gl.GetProgramInfoLog(program));
        
        linked = true;
    }

    public void Kill()
    {
        gl.DeleteProgram(program);
        Objects.Remove(this);
    }

    // minor method overloading for this, don't get rid of this argument.
    private void Kill(bool isGenocide)
    {
        gl.DeleteProgram(program);
    }

    public static void Cleanup()
    {
        foreach (Program prog in Objects)
            prog.Kill(true);
    }
}