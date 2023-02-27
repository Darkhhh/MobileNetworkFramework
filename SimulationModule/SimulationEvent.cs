namespace MobileNetworkFramework.SimulationModule;

public class SimulationEvent: IComparable
{
    public enum EventType
    {
        DeviceStartComputing,
        ServerStartComputing,
        DeviceFinishComputing,
        ServerFinishComputing,
        ServerFinishDataTransfer
    }
    
    #region Properties
    
    public int NetworkObjectId { get; set; }
    
    public float Time { get; set; }
    public EventType Type { get; set; }

    #endregion

    public SimulationEvent(int networkObjectId, float time, EventType type)
    {
        NetworkObjectId = networkObjectId;
        Time = time;
        Type = type;
    }
    public SimulationEvent() { }

    public int CompareTo(object? obj)
    {
        if ((obj is not SimulationEvent simulationEvent)) throw new Exception("Incomparable types");
        if (this.Time < simulationEvent.Time) return 1;
        if (this.Time > simulationEvent.Time) return -1;
        return 0;

    }
}