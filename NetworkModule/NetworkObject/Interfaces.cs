namespace MobileNetworkFramework.NetworkModule.NetworkObject;


public interface IInit: INetworkObject
{
    public void Init(NetworkSystem system);
}


public interface ICreateTask : INetworkObject
{
    public ITask CreateTask(NetworkSystem system);
}


public interface ITakeTask: INetworkObject
{
    public void TakeTask(NetworkSystem system, (ITask task, INetworkObject creator)[] tasks);
}


public interface ISolveTask: INetworkObject
{
    public void SolveTask(NetworkSystem system);
}


public interface ISolver: ITakeTask, ISolveTask
{
    
}


public interface IUpdate: INetworkObject
{
    public void Update(NetworkSystem system);
}