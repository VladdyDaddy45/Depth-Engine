using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Utils;


namespace Depth.Graphics.Shaders;


// Class for shader handles
public unsafe class ShaderProgram
{
    private static GL gl = Video.gl;
    private static List<ShaderProgram> Objects = new List<ShaderProgram>();
    private static List<string> warnings = new List<string>();

    public static ShaderProgram ActiveProgram = null;

    public uint handle;
    public bool linked = false;

    public ShaderProgram() 
    {
        handle = gl.CreateProgram();
        Objects.Add(this);
    }

    // Will attach every shader provided in the array
    public ShaderProgram(Shader[] shaders)
    {
        handle = gl.CreateProgram();
        foreach (Shader shader in shaders)
            Attach(shader); 

        Link();
        Objects.Add(this);
    }

    public void Use()
    {
        gl.UseProgram(handle);
        ActiveProgram = this;
    }

    public static void Clear()
    {
        gl.UseProgram(0);
        ActiveProgram = null;
    }
    
    public int GetUniformName(string name)
    {
        gl.UseProgram(handle);
        int location = gl.GetUniformLocation(handle, name);
        if (location == -1 && warnings.IndexOf(name) == -1) {
            warnings.Add(name);
            Console.WriteLine($"Uniform {name} not found in ShaderProgram");
        }

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
        gl.LinkProgram(handle);
        gl.GetProgram(handle, ProgramPropertyARB.LinkStatus, out int status);
        if (status != (int) GLEnum.True)
            throw new Exception("Program failed to link: " + gl.GetProgramInfoLog(handle));
        
        linked = true;
    }

    public void Attach(Shader shader)
    { gl.AttachShader(handle,shader.shader); }

    public void Detach(Shader shader)
    { gl.DetachShader(handle, shader.shader); }

    public void Kill()
    {
        gl.DeleteProgram(handle);
        Objects.Remove(this);
    }

    // minor method overloading for this, don't get rid of this argument.
    private void Kill(bool isGenocide)
    { gl.DeleteProgram(handle); }

    // NO. TOUCH.
    public static void Cleanup()
    {
        foreach (ShaderProgram prog in Objects)
            prog.Kill(true);
    }
}