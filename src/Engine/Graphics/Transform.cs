using System.Numerics;
using Utils.CMath;
using Silk.NET.Maths;

namespace Engine.Graphics;

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
        set { field = value; UpdateVectors(); UpdateWorld(); }
    } = new Vector3(0f,0f,0f);
    
    public Vector3 Rotation {
        get; 
        set {
            // TODO: make this dynamically add or remove pi radians to prevent floating point errors
            field = value;
            UpdateVectors(); 
            UpdateWorld(); 
        }
    } = new Vector3(0f,0f,0f);

    public Vector3 Scale {
        get; 
        set { field = value; UpdateVectors(); UpdateWorld(); }
    }

    public Vector3 Forward, Right, Up;
    public Matrix4x4 World, RotationMatrix;
    public Quaternion Orientation;

    public void Lerp(Transform Target, float Amount)
    {
        Position = Vector3.Lerp(Position, Target.Position, Amount);
        Rotation = Vector3.Lerp(Rotation, Target.Rotation, Amount);
    }



    private void UpdateVectors()
    {
        RotationMatrix = Matrix4x4.CreateFromYawPitchRoll(
            Rotation.X, 
            Rotation.Y, 
            Rotation.Z
        );

        Forward = Vector3.Transform(Vector3.UnitZ, RotationMatrix);
        Right = Vector3.Transform(Vector3.UnitX, RotationMatrix);
        Up = Vector3.Transform(Vector3.UnitY, RotationMatrix);
    }

    private void UpdateWorld()
    {
        Orientation = Quaternion.Identity * 
        Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z);

        World =
        Matrix4x4.Identity * 
        Matrix4x4.CreateScale(Scale) *
        Matrix4x4.CreateFromQuaternion(Orientation) *
        Matrix4x4.CreateTranslation(Position);
    }

}