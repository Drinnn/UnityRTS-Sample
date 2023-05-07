using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject bigTankUnity;
    [SerializeField] private Transform unitSpawnPoint;
    
    #region Server

    [Command]
    public void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(bigTankUnity, unitSpawnPoint.position, unitSpawnPoint.rotation);
        
        NetworkServer.Spawn(unitInstance, connectionToClient);
    }
    
    #endregion
    
    #region Client
    
    public void OnMouseDown()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        if (!isOwned)
        {
            return;
        }
        
        CmdSpawnUnit();
    }
    
    #endregion
}
