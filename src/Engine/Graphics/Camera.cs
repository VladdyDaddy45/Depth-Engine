using Silk.NET.OpenGL;
using Silk.NET.Maths;
using Utils.CMath;
using System.Numerics;
using Engine.Graphics;

namespace Engine.Graphics;

public class Camera
{
    private static GL gl = Video.gl;
    private static float width => (float)Application.MainApp.width;
    private static float height => (float)Application.MainApp.height;
    public Transform transform;
    public float fov
    { get; set; }

    
    public Matrix4x4 view => Matrix4x4.CreateLookAt(transform.Position, transform.Position + transform.Forward, transform.Up);
    public Matrix4x4 proj
    {
        get
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(CMath.rad(fov), width / height, 0.1f, 1000.0f);
        }
        set;
    }

    public Camera(Transform Trans, float FOV)
    {
        transform = Trans;
        fov = FOV;
    }
}