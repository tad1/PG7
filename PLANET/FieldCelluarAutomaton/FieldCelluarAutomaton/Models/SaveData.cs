using System.Numerics;

namespace FieldCelluarAutomaton.Models;

[Serializable]
public struct SaveData
{
    public Complex[,] grid;
    public (string, string)[] rules;
    public (string type, string name, string value)[] state;
}