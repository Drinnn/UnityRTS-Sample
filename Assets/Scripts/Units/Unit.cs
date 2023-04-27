using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    public UnitMovement UnitMovement => unitMovement;
    
    [SerializeField] private UnitMovement unitMovement;
    [SerializeField] private UnityEvent onSelected;
    [SerializeField] private UnityEvent onDeselected;

    #region Client
    
    [Client]
    public void Select()
    {
        if (isOwned)
        {
            onSelected?.Invoke();
        }
    }

    [Client]
    public void Deselect()
    {
        if (isOwned)
        {
            onDeselected?.Invoke();
        }
    }
    
    #endregion
}
