namespace FieldCelluarAutomaton.Core;

public class Bus : IBus
{
    //TODO: later, remove wrapping dictionary
    Dictionary<Type, Dictionary<string, List<Delegate>>> eventHandlers = new();

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
        
        handlerList.Add(handler);
        
    }

    public void Publish<T>(string name, T message)
    {
        try
        {
            eventHandlers[typeof(T)][name].ForEach((handler => ((Action<T>)handler)(message)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}