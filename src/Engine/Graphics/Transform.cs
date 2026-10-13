using System.Numerics;

namespace Depth;

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

    public static float[] ToFloatArr(Transform transform)
    {
        return Mat4ToFloatArr(transform.World);
    }

    public static float[] Mat4ToFloatArr(Matrix4x4 m)
    {
        var result = new float[16];
        // matrices and hotdogs
        result[0]   = m.M11;
        result[1]   = m.M12;
        result[2]   = m.M13;
        result[3]   = m.M14;
    
        result[4]   = m.M21;
        result[5]   = m.M22;
        result[6]   = m.M23;
        result[7]   = m.M24;
    
        result[8]   = m.M31;
        result[9]   = m.M32;
        result[10]  = m.M33;
        result[11]  = m.M34;
    
        result[12]  = m.M41;
        result[13]  = m.M42;
        result[14]  = m.M43;
        result[15]  = m.M44;

        return result;
    }
}