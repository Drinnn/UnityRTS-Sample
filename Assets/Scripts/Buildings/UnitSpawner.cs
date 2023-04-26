using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject bigTankUnity = null;
    [SerializeField] private Transform unitSpawnPoint = null;
    
    #region Server

    [Command]
    public void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(bigTankUnity, unitSpawnPoint.position, unitSpawnPoint.rotation);
        
        NetworkServer.Spawn(unitInstance, connectionToClient);
    }
    
    #endregion
    
    #region Client
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
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
