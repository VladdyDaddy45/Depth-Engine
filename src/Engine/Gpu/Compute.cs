using Depth.Graphics;
using Utils;
using Silk.NET.OpenGL;

namespace Depth.GPU;

public class Compute<ReturnT> where ReturnT : unmanaged
{
    static GL gl = Video.gl;

    uint program, compute;

    public Compute(string shaderPath)
    {
        // create compute shader
        compute = gl.CreateShader(GLEnum.ComputeShader);
        gl.ShaderSource(compute, Reader.ReadAll(shaderPath));
        gl.CompileShader(compute);
        gl.GetShader(compute, ShaderParameterName.CompileStatus, out int status);
        if (status != (int) GLEnum.True)
            throw new Exception("Compute shader failed to compile:" + gl.GetShaderInfoLog(compute));

        // create shaderprogram and compile
        program = gl.CreateProgram();
        gl.AttachShader(program, compute);
        gl.LinkProgram(program);

        gl.GetProgram(program, ProgramPropertyARB.LinkStatus, out int pstatus);
        if (pstatus != (int) GLEnum.True)
            throw new Exception("Program failed to link: " + gl.GetProgramInfoLog(program));
        
    }

    public void Invoke()
    {
        
    }
}