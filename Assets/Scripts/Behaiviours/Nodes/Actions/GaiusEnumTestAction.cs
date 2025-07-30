using System;
using Unity.Behavior;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GaiusEnumTestAction", story: "Testing [state]", category: "Action", id: "958def381d69fc18b1cb867fed3c074a")]
public partial class GaiusEnumTestAction : EnumTestAction<CurrentPhase>
{

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}