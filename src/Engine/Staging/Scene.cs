using System.Diagnostics;
using System.Timers;
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

    public List<Instance> instances;
    public List<Mesh> meshes;

    static Stopwatch CpuStopwatch = new();
    static Stopwatch GpuStopwatch = new();
    static double gpuTime = 0.0;
    static double cpuTime = 0.0;
    static int frames = 0;


    public Scene()
    { 
        instances = new List<Instance>(); 
        meshes = new List<Mesh>();

        mainScene = this;

        System.Timers.Timer fpstimer = new System.Timers.Timer();
        fpstimer.Elapsed += new ElapsedEventHandler(CalcFrameTime);
        fpstimer.Interval = 1000;
        fpstimer.Enabled = true;
    }

    public void Draw()
    {
        foreach (Instance inst in instances) 
            meshes[inst.MeshID].PushInstance(inst.transform);

        CpuStopwatch.Stop();
        cpuTime += CpuStopwatch.ElapsedMilliseconds;
        CpuStopwatch.Reset();

        GpuStopwatch.Start();

        foreach (Mesh mesh in meshes)
            mesh.Draw();
        
        GpuStopwatch.Stop();
        gpuTime += GpuStopwatch.ElapsedMilliseconds;
        GpuStopwatch.Reset();

        CpuStopwatch.Start();
        frames++;
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


    private void CalcFrameTime(object? source, ElapsedEventArgs? e)
    {
        Console.WriteLine($"CPU avg: {Math.Round(cpuTime/frames,5)}ms \t\tGPU avg: {Math.Round(gpuTime/frames,5)}ms");
        frames = 0;
        cpuTime = 0;
        gpuTime = 0;
    }
}