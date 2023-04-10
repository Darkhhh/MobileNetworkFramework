namespace MobileNetworkFramework.NetworkModule.NetworkObject;


public interface IInit : INetworkObject
{
    public void Init(NetworkSystem system);
}


public interface ICreateTask : INetworkObject
{
    public ITask[] CreateTask(NetworkSystem system);
}


public interface ITakeTask : INetworkObject
{
    public (ITask, ISolveTask)[] TakeTask(NetworkSystem system, (ITask task, INetworkObject creator)[] tasks);
}


public interface ISolveTask : INetworkObject
{
    public float AddTask(NetworkSystem system, ITask task);
    
    public float SolveTask(NetworkSystem system, ITask task);
}


public interface ISolver : ITakeTask, ISolveTask
{
    
}


public interface IDispose : INetworkObject
{
    public void Dispose(NetworkSystem system);
}


public interface IUpdate : INetworkObject
{
    public void Update(NetworkSystem system);
}