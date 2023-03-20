using System.Collections.Concurrent;

namespace MobileNetworkFramework.Common.ObjectPooling;

public class ObjectPool<T>
{
    private readonly ConcurrentBag<T> _objects;
    private readonly Func<T> _objectGenerator;

    public ObjectPool(Func<T> objectGenerator)
    {
        _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
        _objects = new ConcurrentBag<T>();
    }

    public T Get() => _objects.TryTake(out var item) ? item : _objectGenerator();

    public void Return(T item) => _objects.Add(item);
    
    public void Add(T item) => _objects.Add(item);
}