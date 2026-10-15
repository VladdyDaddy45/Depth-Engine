using Silk.NET.OpenGL;

namespace Depth.Graphics;

public class Mesh
{
    static GL gl = Video.gl;
    static readonly uint TransformPosition = 3;

    private List<float[]> instances = new List<float[]>();
    public uint mbo; // MatrixBufferObject

    public VertexArray<float, uint> vao;

    public Mesh(VertexArray<float, uint> VAO)
    { vao = VAO; }
    
    public void PushInstance(Transform transform)
    { instances.Add(Transform.ToFloatArr(transform)); }

    public unsafe void Draw()
    {
        Process(); 
        vao.Bind();

        gl.DrawElementsInstanced(GLEnum.Triangles, (uint)vao.EBO.data.Length, GLEnum.UnsignedInt, (void*)0, (uint)instances.Count);
        
        gl.DeleteBuffer(mbo);
        instances.Clear();
    }


    public unsafe void Process()
    {
        mbo = gl.GenBuffers(1);
        gl.BindBuffer(GLEnum.ArrayBuffer, mbo);
        var matrices = new List<float>(16*instances.Count);

        for (int i = 0; i < instances.Count; i++)
            matrices.AddRange(instances[i]);
        
        vao.Bind();
        fixed (float* transforms = matrices.ToArray())
            gl.BufferData(GLEnum.ArrayBuffer, (uint)instances.Count * 16u * sizeof(float), &transforms[0], GLEnum.DynamicDraw);
        
        for (uint i = 0; i < 4; i++)
        {
            gl.EnableVertexAttribArray(TransformPosition+i);
            gl.VertexAttribPointer(TransformPosition+i, 4, GLEnum.Float, false, 16 * sizeof(float), (void*)(sizeof(float) * 4 * i));
            gl.VertexAttribDivisor(TransformPosition+i, 1);
        }
    }
}