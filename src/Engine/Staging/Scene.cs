using System.Numerics;
using Depth.Graphics;

namespace Depth;

public struct Instance
{
    public Transform transform;
    public int MeshID; // you shouldn't have more than 2 billion meshes.
}

public class Scene
{
    public static Scene mainScene;

    private List<Instance> instances;
    private List<Mesh> meshes;

    public Scene()
    { 
        instances = new List<Instance>(); 
        meshes = new List<Mesh>();

        mainScene = this;
    }

    public void Draw()
    {
        foreach (Instance inst in instances) 
            meshes[inst.MeshID].PushInstance(inst.transform);
        foreach (Mesh mesh in meshes)
            mesh.Draw();
    }

    public void AddInstance(Transform transform, Mesh mesh)
    { instances.Add( new Instance { transform = transform, MeshID = meshes.IndexOf(mesh) } ); }

    public void AddInstance(Transform transform, int meshID)
    { instances.Add( new Instance { transform = transform, MeshID = meshID } ); }

    public void RemoveInstance(Instance part)
    { instances.Remove(part); }

    public void AddMesh(Mesh mesh)
    { meshes.Add(mesh); }

    public void RemoveMesh(Mesh mesh)
    { meshes.Remove(mesh); }

    public int FindMesh(Mesh mesh)
    { return meshes.IndexOf(mesh); }
}