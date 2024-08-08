using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject mouseIndicator, cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDataBaseSO database;
    private int selectedObjectindex = -1;
    [SerializeField] private GameObject gridVisualization;

    private void Start()
    {
        StopPlacement();
    }
    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectindex = database.objectData.FindIndex(data => data.ID == ID);
        if(selectedObjectindex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit+= StopPlacement;
        
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        GameObject newObject = Instantiate(database.objectData[selectedObjectindex].Prefab);

        newObject.transform.position = grid.CellToWorld(gridPos);
        //newObject is a Square right now and the orgin point is the midle.
        newObject.transform.position += new Vector3(0.5f, 0, 0.5f);
    }

    private void StopPlacement()
    {
        selectedObjectindex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectedObjectindex <0)
        {
            return;
        }
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        mouseIndicator.transform.position = mousePos;

        cellIndicator.transform.position = grid.CellToWorld(gridPos);
        //CellIndicator is a Square right now and the orgin point is the midle.
        cellIndicator.transform.position += new Vector3(0.5f, 0, 0.5f);
    }


}
