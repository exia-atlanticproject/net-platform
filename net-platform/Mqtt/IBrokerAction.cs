namespace net_platform.Mqtt
{
    public interface IBrokerAction
    {
        void notify(string brokerResponse);
    }
}