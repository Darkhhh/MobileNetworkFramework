namespace MobileNetworkFramework.SimulationModule;

public class EventSystem
{
    #region Local Variables

    public int EventsAmount => _events.Count;
    
    private readonly List<SimulationEvent> _events;

    #endregion


    #region Constructors

    public EventSystem(int capacity) => _events = new List<SimulationEvent>(capacity);

    #endregion


    #region Public Methods

    public void AddEvent(SimulationEvent @event)
    {
        for (var i = 0; i < _events.Count; i++)
        {
            if (!(_events[i].Time >= @event.Time)) continue;
            _events.Insert(i, @event);
            return;
        }
        _events.Add(@event);
    }

    public void AddTwoEvents(SimulationEvent first, SimulationEvent second)
    {
        AddEvent(first);
        AddEvent(second);
    }

    public SimulationEvent NextEvent()
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