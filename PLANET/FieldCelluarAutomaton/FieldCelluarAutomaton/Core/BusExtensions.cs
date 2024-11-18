using System.Reflection;

namespace FieldCelluarAutomaton.Core;

public static class BusExtensions
{
    public static void RegisterSubscriptions(this IBus bus, object target)
    {
        var methods = target.GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(m => m.GetCustomAttributes(typeof(Subscribe), false).Any());
        
        foreach (var method in methods)
        {
            var attribute = method.GetCustomAttribute<Subscribe>();
            var parameter = method.GetParameters().FirstOrDefault();

            if (parameter == null)
            {
                throw new NullReferenceException($"Method {method.Name} requires a single parameter");
            }
            
            var messageType = parameter.ParameterType;
            var name = attribute.Name;
            var dDelegate = Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(messageType), target, method);
            
            typeof(IBus).GetMethod(nameof(bus.Subscribe))?.MakeGenericMethod(messageType).Invoke(bus, new object?[]{name, dDelegate});
        }
}
}