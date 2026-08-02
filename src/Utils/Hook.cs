namespace Utils;
// bad code; dont use.

public struct Connection<tAction>
{
    private Hook<tAction> hook;
    public tAction action;
    public bool isOnce;

    public Connection(Hook<tAction> Hook, tAction Action, bool IsOnce)
    {
        hook = Hook;
        action = Action;
        isOnce = IsOnce;
    }

    public void Disconnect()
    {
        hook.Disconnect(this);
    }
};

public class Hook<tAction> 
{
    private List<Connection<tAction>> connections = new List<Connection<tAction>>();

    public void Fire()
    {
        
    }

    public Connection<tAction> Connect(tAction action)
    {
        var conn = new Connection<tAction>(this, action, false);
        connections.Add(conn);
        return conn;
    }

    public Connection<tAction> Once(tAction action)
    {
        var conn = new Connection<tAction>(this, action, true);
        connections.Add(conn);
        return conn;
    }

    public void Disconnect(Connection<tAction> conn)
    {
        connections.Remove(conn);
    }
}