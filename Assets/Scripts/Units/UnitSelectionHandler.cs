using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    public List<Unit> SelectedUnits => _selectedUnits;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private RectTransform unitSelectionArea;
    
    private Camera _mainCamera;
    private RTSPlayer _player;

    private Vector2 _dragStartingPosition;

    private List<Unit> _selectedUnits;

    private void Awake()
    {
        _mainCamera = Camera.main;

        _selectedUnits = new List<Unit>();
    }

    private void Update()
    {
        // temporary fix
        if (_player == null)
        {
            _player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();   
        } else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            BuildSelectionArea();
        } else if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }

    private void StartSelectionArea()
    {
        foreach (Unit selectedUnit in _selectedUnits)
        {
            selectedUnit.Deselect();
        }
        
        _selectedUnits.Clear();
        
        unitSelectionArea.gameObject.SetActive(true);
        _dragStartingPosition = Mouse.current.position.ReadValue();
        UpdateSelectionArea();
    }

    private void BuildSelectionArea()
    {
        unitSelectionArea.gameObject.SetActive(false);

        if (unitSelectionArea.sizeDelta.magnitude == 0)
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
            unit.Select();

            return;
        }

        Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
        Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2);

        foreach (Unit unit in _player.Units)
        {
            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPosition.x > min.x && screenPosition.x < max.x
                                         && screenPosition.y > min.y && screenPosition.y < max.y)
            {
                _selectedUnits.Add(unit);
                unit.Select();
            }
        }

    }
    
    private void UpdateSelectionArea()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - _dragStartingPosition.x;
        float areaHeight = mousePosition.y - _dragStartingPosition.y;

        unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));
        unitSelectionArea.anchoredPosition = _dragStartingPosition +
                                             new Vector2(areaWidth / 2, areaHeight / 2);
    }
}
