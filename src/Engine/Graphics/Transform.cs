using System.Numerics;
namespace Engine.Graphics;

public struct Transform
{
    public Transform() {}

    public Vector3 Position {get; set;} = new Vector3(0,0,0);
    public Vector3 Rotation {get; set;} = new Vector3(0,0,0);

    public float Scale {get; set;} = 1f;

    // Allows for idk man im tired and dont wanna write this comment
    public Quaternion Orientation => Quaternion.Identity * 
        Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z);

    public Matrix4x4 world => 
        Matrix4x4.Identity * 
        Matrix4x4.CreateFromQuaternion(Orientation) *
        Matrix4x4.CreateScale(Scale) *
        Matrix4x4.CreateTranslation(Position);
}