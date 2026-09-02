using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Utils;


namespace Engine.Graphics.Shaders;


// Class for shader programs
public unsafe class ShaderProgram
{
    private static GL gl = Video.gl;
    private static List<ShaderProgram> Objects = new List<ShaderProgram>();

    public uint program;
    public bool linked = false;

    public ShaderProgram() 
    {
        program = gl.CreateProgram();
        Objects.Add(this);
    }

    // Will attach every shader provided in the array
    public ShaderProgram(Shader[] shaders)
    {
        program = gl.CreateProgram();
        foreach (Shader shader in shaders)
            Attach(shader); 

        Link();
        Objects.Add(this);
    }

    public void Use()
    {
        gl.UseProgram(program);
    }
    
    public int GetUniformName(string name)
    {
        gl.UseProgram(program);
        int location = gl.GetUniformLocation(program, name);
        if (location == -1)
            throw new Exception($"Uniform {name} not found in ShaderProgram");

        return location;
    }

    // Int uniform
    public void Uniform(string name, int value)
    { gl.Uniform1(GetUniformName(name), value); }

    // Float Uniform
    public void Uniform(string name, float value)
    { gl.Uniform1(GetUniformName(name), value); }
    
    // Vector2 Uniform
    public void Uniform(string name, Vector2 value)
    { gl.Uniform2(GetUniformName(name), value); }

    // Vector3 Uniform
    public void Uniform(string name, Vector3 value)
    { gl.Uniform3(GetUniformName(name), value); }

    // Vector3 Uniform
    public void Uniform(string name, Vector4 value)
    { gl.Uniform4(GetUniformName(name), value); }

    // Matrix4x4 Uniform
    public void Uniform(string name, Matrix4x4 value)
    { gl.UniformMatrix4(GetUniformName(name), 1, false, (float*) &value); }

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

    public void Attach(Shader shader)
    { gl.AttachShader(program,shader.shader); }

    public void Detach(Shader shader)
    { gl.DetachShader(program, shader.shader); }


    // NO. TOUCH.
    public static void Cleanup()
    {
        foreach (ShaderProgram prog in Objects)
            prog.Kill(true);
    }
}