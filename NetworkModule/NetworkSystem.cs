using MobileNetworkFramework.LandscapeModule;
using MobileNetworkFramework.NetworkModule.NetworkObject;
using MobileNetworkFramework.NetworkModule.SimulationModule;

namespace MobileNetworkFramework.NetworkModule;

public class NetworkSystem
{
    #region Items

    private readonly List<IInit> _initObjects;
    private readonly List<ICreateTask> _createTasksObjects;
    private readonly List<ITakeTask> _takeTasksObjects;
    private readonly List<IUpdate> _updateObjects;
    private readonly List<IDispose> _disposeObjects;

    #endregion


    #region Private Values

    private readonly List<(ITask task, INetworkObject creator)> _tasks = new();
    private readonly List<(ITask task, ISolveTask solver)> _takenTasks = new();
    private readonly EventSystem _eventSystem;

    private float _creatingTasksFrequency = -1;
    private float _updateFrequency = -1;

    #endregion


    #region Public Items

    public Action<INetworkObject, ITask>? TaskWasFinished;
    
    public Terrain Terrain { get; private set; }

    #endregion

    
    #region Constructor

    public NetworkSystem(Terrain terrain)
    {
        _initObjects = new List<IInit>();
        _createTasksObjects = new List<ICreateTask>();
        _takeTasksObjects = new List<ITakeTask>();
        _updateObjects = new List<IUpdate>();
        _disposeObjects = new List<IDispose>();
        
        _eventSystem = new EventSystem();
        Terrain = terrain;
    }

    #endregion

    
    #region LifeCycle

    private void Init()
    {
        foreach (var item in _initObjects) item.Init(this);
    }

    private void CreateTasks()
    {
        foreach (var item in _createTasksObjects)
        {
            var tasks = item.CreateTask(this);
            foreach (var task in tasks) if (!task.None) _tasks.Add((task, item));
        }
    }

    private void Update()
    {
        foreach (var item in _updateObjects) item.Update(this);
    }

    private void TakeTasks()
    {
        var tasks = _tasks.ToArray();
        foreach (var item in _takeTasksObjects) _takenTasks.AddRange(item.TakeTask(this, tasks));
        _tasks.Clear();
        foreach (var item in _takenTasks)
        {
            var e = new NetworkEvent()
            {
                Time = item.task.GetTransferTime(),
                Type = NetworkEventType.DataTransferFinished,
                NetworkObject = item.solver,
                Task = item.task
            };
            _eventSystem.AddEvent(e);
        }
        _takenTasks.Clear();
    }

    private void Dispose()
    {
        foreach (var item in _disposeObjects) item.Dispose(this);
    }

    #endregion


    #region Public Methods

    public void Add(INetworkObject networkObject)
    {
        if (networkObject is IInit init) _initObjects.Add(init);
        if (networkObject is ICreateTask creator) _createTasksObjects.Add(creator);
        if (networkObject is ITakeTask taker) _takeTasksObjects.Add(taker);
        if (networkObject is IUpdate updater) _updateObjects.Add(updater);
        if (networkObject is IDispose disposer) _disposeObjects.Add(disposer);
    }
    
    public void Initialize(float creatingTasksFrequency, float updateFrequency)
    {
        Init();
        _creatingTasksFrequency = creatingTasksFrequency;
        _updateFrequency = updateFrequency;
        _eventSystem.AddEvent(new NetworkEvent{Time = creatingTasksFrequency, Type = NetworkEventType.CreateTasks});
        _eventSystem.AddEvent(new NetworkEvent{Time = updateFrequency, Type = NetworkEventType.Update});
    }
    
    public void Run(Func<bool> stopCondition)
    {
        while (!stopCondition.Invoke() && _eventSystem.EventsAmount > 0)
        {
            var e = _eventSystem.NextEvent();

            if (e.Type == NetworkEventType.CreateTasks)
            {
                CreateTasks();
                TakeTasks();
                e.Time = _creatingTasksFrequency;
                _eventSystem.AddEvent(e);
            }

            if (e.Type == NetworkEventType.Update)
            {
                Update();
                e.Time = _updateFrequency;
                _eventSystem.AddEvent(e);
            }

            if (e.Type == NetworkEventType.DataTransferFinished)
            {
                e.Time = ((ISolveTask) e.NetworkObject).AddTask(this, e.Task);
                e.Type = NetworkEventType.NetworkObjectStartComputing;
                _eventSystem.AddEvent(e);
            }

            if (e.Type == NetworkEventType.NetworkObjectStartComputing)
            {
                e.Time = ((ISolveTask) e.NetworkObject).SolveTask(this, e.Task);
                e.Type = NetworkEventType.NetworkObjectFinishComputing;
                _eventSystem.AddEvent(e);
            }

            if (e.Type == NetworkEventType.NetworkObjectFinishComputing)
            {
                TaskWasFinished?.Invoke(e.NetworkObject, e.Task);
            }
        }
        
        Dispose();
    }

    #endregion
}