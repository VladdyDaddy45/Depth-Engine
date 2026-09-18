using System.Numerics;
using Utils.CMath;
using Silk.NET.Maths;
using Depth.ECS;

namespace Depth.Graphics;

public struct Transform
{
    public Transform()
    {
        Position = new Vector3(0f,0f,0f);
        Rotation = new Vector3(0f,0f,0f);
        Scale = new Vector3(1f,1f,1f);
    }

    public Vector3 Position {
        get;
        set { field = value; UpdateAll();
        }
    } = new Vector3(0f,0f,0f);
    
    public Vector3 Rotation {
        get; 
        set { field = value; UpdateAll(); }
    } = new Vector3(0f,0f,0f);

    public Vector3 Scale {
        get; 
        set { field = value; UpdateAll(); }
    } = new Vector3(1f,1f,1f);

    public Vector3 Forward, Right, Up;
    public Matrix4x4 World, RotationMatrix;
    public Quaternion Orientation;

    public static Vector3[] ToMatrix3x3(Transform T)
    {
        Matrix4x4 wrld = T.World;
        Vector3[] mat =
        [
            new Vector3(wrld.M11, wrld.M12, wrld.M13),
            new Vector3(wrld.M21, wrld.M22, wrld.M23),
            new Vector3(wrld.M31, wrld.M32, wrld.M33),
        ];
        return mat;
    }



    private unsafe void UpdateAll()
    {
        fixed (Transform* T = &this)
        {
            UpdateWorld(T);
            UpdateVectors(T);
        }
    }

    private unsafe void UpdateVectors(Transform* T)
    {
        RotationMatrix = Matrix4x4.CreateFromYawPitchRoll(
            T->Rotation.X, 
            T->Rotation.Y, 
            T->Rotation.Z
        );

        T->Forward = Vector3.Transform(Vector3.UnitZ, T->RotationMatrix);
        T->Right = Vector3.Transform(Vector3.UnitX, T->RotationMatrix);
        T->Up = Vector3.Transform(Vector3.UnitY, T->RotationMatrix);
    }

    private unsafe static void UpdateWorld(Transform* T)
    {
        T->Orientation = Quaternion.Identity * 
        Quaternion.CreateFromYawPitchRoll(T->Rotation.X, T->Rotation.Y, T->Rotation.Z);

        T->World =
        Matrix4x4.Identity * 
        Matrix4x4.CreateScale(T->Scale) *
        Matrix4x4.CreateFromQuaternion(T->Orientation) *
        Matrix4x4.CreateTranslation(T->Position);
    }
}