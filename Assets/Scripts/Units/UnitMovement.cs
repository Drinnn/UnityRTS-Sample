using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    private Camera _mainCamera;
    
    #region Server

    [Command]
    public void CmdMove(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    
    #endregion
    
    #region Client

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        _mainCamera = Camera.main;
    }

    #endregion
}
