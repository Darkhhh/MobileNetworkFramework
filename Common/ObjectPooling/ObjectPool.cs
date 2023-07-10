namespace MobileNetworkFramework.Common.ObjectPooling;

public class ObjectPool<T> where T : IPoolObject
{
    private readonly Queue<T> _objects;
    private readonly Func<T> _generator;

    public ObjectPool(Func<T> generator)
    {
        _objects = new Queue<T>();
        _generator = generator;
    }
    
    public ObjectPool(Func<T> generator, int capacity)
    {
        _objects = new Queue<T>(capacity);
        _generator = generator;
        for (var i = 0; i < capacity; i++) _objects.Enqueue(_generator.Invoke());
    }

    public T Get()
    {
        return _objects.TryDequeue(out var result) ? result : _generator.Invoke();
    }

    public void Return(T obj)
    {
        obj.Reset();
        _objects.Enqueue(obj);
    }

    public void Add(T obj)
    {
        _objects.Enqueue(obj);
    }
}