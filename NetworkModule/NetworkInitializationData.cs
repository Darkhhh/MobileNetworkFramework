namespace MobileNetworkFramework.NetworkModule;

public abstract class NetworkInitializationData
{
    public float CreateTasksFrequency { get; set; } = -1;

    public float UpdateFrequency { get; set; } = -1;
}