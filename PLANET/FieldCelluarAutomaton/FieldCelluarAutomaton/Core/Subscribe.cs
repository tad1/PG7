namespace FieldCelluarAutomaton.Core;

[AttributeUsage(AttributeTargets.Method)]
public class Subscribe(string name) : Attribute
{
    public string Name { get; init; } = name;
}