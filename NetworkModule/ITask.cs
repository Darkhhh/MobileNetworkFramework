namespace MobileNetworkFramework.NetworkModule;

public interface ITask
{
    public bool None { get; set; }

    public float GetTransferTime();

    public float GetComputingTime();
}