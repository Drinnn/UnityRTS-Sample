using Mirror;
using UnityEngine;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField] private GameObject unitSpawner;
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        GameObject unitSpawnerInstance = Instantiate(unitSpawner, conn.identity.transform.position,
            conn.identity.transform.rotation);
        
        NetworkServer.Spawn(unitSpawnerInstance, conn);
    }
}
