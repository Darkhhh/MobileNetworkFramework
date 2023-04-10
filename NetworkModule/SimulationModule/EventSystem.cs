namespace MobileNetworkFramework.NetworkModule.SimulationModule;

public class EventSystem
{
    #region Local Variables

    public int EventsAmount => _events.Count;
    
    private readonly List<NetworkEvent> _events;

    #endregion


    #region Constructors

    public EventSystem(int capacity) => _events = new List<NetworkEvent>(capacity);

    public EventSystem() => _events = new List<NetworkEvent>();

    #endregion


    #region Public Methods

    public void AddEvent(NetworkEvent networkEvent)
    {
        if (networkEvent.Time < 0) return;
        
        for (var i = 0; i < _events.Count; i++)
        {
            if (!(_events[i].Time >= networkEvent.Time)) continue;
            _events.Insert(i, networkEvent);
            return;
        }
        _events.Add(networkEvent);
    }

    public NetworkEvent NextEvent()
    {
        var result = _events[0];
        _events.RemoveAt(0);
        foreach (var eEvent in _events) eEvent.Time -= result.Time;
        return result;
    }

    public string GetEventsInfo()
    {
        var s = _events.
            Aggregate("", (current, @event) => 
                current + (" Time: " + @event.Time + " Type: " + @event.Type.ToString() + "\n"));
        s += "\n";
        return s;
    }

    #endregion
    
}