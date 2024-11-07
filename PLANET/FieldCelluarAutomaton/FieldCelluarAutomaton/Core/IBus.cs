namespace FieldCelluarAutomaton.Core;

public interface IBus
{
    public void Subscribe<T>(string name, Action<T> handler);
    public void Publish<T>(string name, T message);
}
