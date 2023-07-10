using MobileNetworkFramework.Common.ObjectPooling;
using MobileNetworkFramework.NetworkModule.NetworkObject;

namespace MobileNetworkFramework.NetworkModule.SimulationModule;

public class NetworkEvent: IComparable, IPoolObject
{
    public float Time { get; set; }
    
    public NetworkEventType Type { get; set; }
    
    public INetworkObject NetworkObject { get; set; }
    
    public ITask Task { get; set; }


    public NetworkEvent(float time, NetworkEventType type, INetworkObject networkObject, ITask task)
    {
        Time = time;
        Type = type;
        NetworkObject = networkObject;
        Task = task;
    }

    public NetworkEvent() { }


    public int CompareTo(object? obj)
    {
        if ((obj is not NetworkEvent networkEvent)) throw new Exception("Incomparable types");
        if (Time < networkEvent.Time) return 1;
        if (Time > networkEvent.Time) return -1;
        return 0;
    }

    public void Reset()
    {
        Time = -1;
        NetworkObject = null;
        Task = null;
    }
}