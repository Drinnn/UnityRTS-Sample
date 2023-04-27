using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    public List<Unit> SelectedUnits => _selectedUnits;

    [SerializeField] private LayerMask layerMask;
    
    private Camera _mainCamera;

    private List<Unit> _selectedUnits;

    private void Awake()
    {
        _mainCamera = Camera.main;

        _selectedUnits = new List<Unit>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ClearSelectionArea();   
        } else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            MakeSelectionArea();
        }
    }

    private void ClearSelectionArea()
    {
        foreach (Unit selectedUnit in _selectedUnits)
        {
            selectedUnit.Deselect();
        }
        
        _selectedUnits.Clear();
    }

    private void MakeSelectionArea()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            return;
        }

        if (!hit.collider.TryGetComponent(out Unit unit))
        {
            return;
        }

        if (!unit.isOwned)
        {
            return;
        }
        
        _selectedUnits.Add(unit);

        foreach (Unit selectedUnit in _selectedUnits)
        {
            selectedUnit.Select();
        }
    }
}
