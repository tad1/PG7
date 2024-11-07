using System.Reflection;

namespace FieldCelluarAutomaton.Core;

public class PersistantBus : IBus
{
    Dictionary<Type, Dictionary<string, List<Delegate>>> eventHandlers = new();
    private Dictionary<(Type, string), object> store = new();

    public Dictionary<(Type, string), object> State => store;

    public void Subscribe<T>(string name, Action<T> handler)
    {
        List<Delegate> handlerList;
        Dictionary<string, List<Delegate>> handlersDictionary;
        
        if (!eventHandlers.TryGetValue(typeof(T), out handlersDictionary))
        {
            handlersDictionary = new Dictionary<string, List<Delegate>>();
            eventHandlers.Add(typeof(T), handlersDictionary);
        }

        if (!handlersDictionary.TryGetValue(name, out handlerList))
        {
            handlerList = new List<Delegate>();
            handlersDictionary.Add(name, handlerList);
        }

        if (store.TryGetValue((typeof(T), name), out var value))
        {
            ((Action<T>)handler)((dynamic)value);
        }
        handlerList.Add(handler);
        
    }

    public void Load(Dictionary<(Type, string), object> state)
    {
        store = state;
        foreach (var entry in state)
        {
            var (type, name) = entry.Key;
            eventHandlers[type][name].ForEach(handler =>
            {
                dynamic action = handler;
                action((dynamic)entry.Value);
            });
        }
    }
    

    public void Publish<T>(string name, T message)
    {
        try
        {
            store[(typeof(T), name)] = message;
            eventHandlers[typeof(T)][name].ForEach((handler => ((Action<T>)handler)(message)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}