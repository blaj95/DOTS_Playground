using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct UserInputData : IComponentData
{
    public KeyCode spaceKey;
    public KeyCode enterKey;
}
