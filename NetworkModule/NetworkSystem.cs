using MobileNetworkFramework.NetworkModule.NetworkObject;

namespace MobileNetworkFramework.NetworkModule;

public class NetworkSystem
{
    #region Items

    private readonly List<INetworkObject> _networkObjects;
    private readonly List<IInit> _initObjects;
    private readonly List<ICreateTask> _createTasksObjects;
    private readonly List<ITakeTask> _takeTasksObjects;
    private readonly List<ISolveTask> _solveTasksObjects;
    private readonly List<IUpdate> _updateObjects;

    #endregion


    #region Constructor

    public NetworkSystem()
    {
        _networkObjects = new List<INetworkObject>();
        _initObjects = new List<IInit>();
        _createTasksObjects = new List<ICreateTask>();
        _takeTasksObjects = new List<ITakeTask>();
        _solveTasksObjects = new List<ISolveTask>();
        _updateObjects = new List<IUpdate>();
    }

    #endregion


    private readonly List<(ITask, INetworkObject)> _tasks = new();

    public void Add(INetworkObject networkObject)
    {
        _networkObjects.Add(networkObject);
        
        if (networkObject is IInit init) _initObjects.Add(init);
        if (networkObject is ICreateTask creator) _createTasksObjects.Add(creator);
        if (networkObject is ITakeTask taker) _takeTasksObjects.Add(taker);
        if (networkObject is ISolveTask solver) _solveTasksObjects.Add(solver);
        if (networkObject is IUpdate updater) _updateObjects.Add(updater);
    }

    #region LifeCycle

    public void Init()
    {
        foreach (var item in _initObjects) item.Init(this);
    }

    public void CreateTasks()
    {
        foreach (var item in _createTasksObjects) _tasks.Add((item.CreateTask(this), item));
    }

    public void Update()
    {
        foreach (var item in _updateObjects) item.Update(this);
    }

    public void TakeTasks()
    {
        var tasks = _tasks.ToArray();
        foreach (var item in _takeTasksObjects) item.TakeTask(this, tasks);
    }

    #endregion
}