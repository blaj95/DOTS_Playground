using Unity.Entities;

//Used in GameObject coversion workflow to attach components just like you would for MonoBehaviours
[GenerateAuthoringComponent]
public struct MoveDirection : IComponentData
{
    public float value;
}
