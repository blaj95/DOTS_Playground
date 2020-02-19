using Unity.Entities;

[GenerateAuthoringComponent]
public struct MarkerComponent : IComponentData
{
    public int pathIndex;
}
