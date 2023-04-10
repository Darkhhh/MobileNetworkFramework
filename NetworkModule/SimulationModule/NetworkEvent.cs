using MobileNetworkFramework.NetworkModule.NetworkObject;

namespace MobileNetworkFramework.NetworkModule.SimulationModule;

public class NetworkEvent: IComparable
{
    public float Time { get; set; }
    
    public NetworkEventType Type { get; set; }
    
    public INetworkObject NetworkObject { get; set; }
    
    public ITask Task { get; set; }


    public int CompareTo(object? obj)
    {
        if ((obj is not NetworkEvent networkEvent)) throw new Exception("Incomparable types");
        if (Time < networkEvent.Time) return 1;
        if (Time > networkEvent.Time) return -1;
        return 0;
    }
}