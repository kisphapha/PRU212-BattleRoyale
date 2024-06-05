using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrianGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public int roomWidth = 146;
    public int roomHeight = 96;
    public GameObject treePrefab;
    public GameObject housePrefab;
    public List<GameObject> boxesPrefab;
    private int numberOfTree;
    private int numberOfHouse;
    private int numberOfBoxes;
    void Start()
    {
        var r = new System.Random();
        numberOfTree = r.Next(10,20);
        numberOfHouse = 3;// r.Next(3,5);
        numberOfBoxes = r.Next(30,40);
        Generate();
    }

    private void Generate()
    {
        var r = new System.Random();
        for (var i = 0; i < numberOfTree; i++)
        {
            var position = new Vector3 (transform.position.x + r.Next(roomWidth), transform.position.y + r.Next(roomHeight), 0);
            var tree = Instantiate(treePrefab, position , Quaternion.Euler(0,0,r.Next(360)));
            float treeSize = r.Next(200,450) / 100f;
            tree.transform.localScale = new Vector3(treeSize, treeSize, 1);
            tree.name = "Tree " + i;
        }
        for (var i = 0; i < numberOfHouse; i++)
        {
            var position = new Vector3(transform.position.x + r.Next(roomWidth) , transform.position.y + r.Next(roomHeight) , 0);
            var house = Instantiate(housePrefab , position, Quaternion.Euler(0, 0, 0));
            house.name = "House " + i;
        }
    }
}
