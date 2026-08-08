using System.Numerics;
using Engine.Graphics;

namespace Engine.Graphics;

public class Mesh3D
{
    public struct Mesh
    {
        public Transform transform;
        public Mesh3D mesh;

        public void Destroy()
        {
            // leave it to be picked up by the garbage collector.
            mesh.Instances.Remove(this); 
        }
    }
    public static List<object> Objects = new List<object>();

    public VertexArray Vao;
    public BufferObject<Matrix4x4> Vbo;
    public List<Mesh> Instances = new List<Mesh>(); 

    public Mesh3D(VertexArray VAO)
    {
        Vao = VAO;
    }

    public void Draw()
    {
        Vao.Bind();
        
    }

    public Mesh NewInstance()
    {
        Mesh mesh;
        mesh.transform = new Transform();
        mesh.mesh = this;
        Instances.Add(mesh);

        return mesh;
    }

    public Mesh NewInstance(Transform transform)
    {
        Mesh mesh = NewInstance();
        mesh.transform = transform;
        return mesh;
    }

    private void CompileVBO()
    {
        Matrix4x4[] data = new Matrix4x4[Instances.Count];

        int index = 0;
        foreach (Mesh mesh in Instances)
        {
            data[index] = mesh.transform.World;
            index++;
        }

        Vbo = new BufferObject<Matrix4x4>(
            data,
            BufferTargetARB.ArrayBuffer
        );
    }
}