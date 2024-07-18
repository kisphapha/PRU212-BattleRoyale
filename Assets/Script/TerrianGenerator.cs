using NavMeshPlus.Components;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class TerrianGenerator : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public int roomWidth = 182;
    public int roomHeight = 233;
    public GameObject treePrefab;
    public GameObject housePrefab;
    public GameObject playerAI;
    public List<GameObject> boxesPrefab;
    public List<GameObject> gunsPrefab;
    public List<GameObject> utilitesPrefab;
    public GameObject mapBorder;
    private int numberOfTree;
    private int numberOfAIPlayer;
    private int numberOfHouse;
    private int numberOfBoxes;
    private int numberOfGuns;
    private int numberOfUtilities;

    void Start()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            // Check if the terrain has already been generated
            if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("TerrainGenerated") ||
                !(bool)PhotonNetwork.CurrentRoom.CustomProperties["TerrainGenerated"])
            {
                // Terrain has not been generated yet, generate it
                Generate();

                // Set the custom property to indicate the terrain has been generated
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "TerrainGenerated", true } });
            }
        }
    }

    private void Generate()
    {
        var r = new System.Random();
        numberOfTree = r.Next(20, 30);
        numberOfAIPlayer = r.Next(5,10);
        numberOfHouse = r.Next(3, 5);
        numberOfBoxes = r.Next(30, 40);
        numberOfGuns = r.Next(20, 30);
        numberOfUtilities = r.Next(20, 30);
        //Gernate tree
        for (var i = 0; i < numberOfTree; i++)
        {
            var position = new Vector3(transform.position.x + r.Next(roomWidth), transform.position.y + r.Next(roomHeight), 0);
            var tree = PhotonNetwork.InstantiateRoomObject(treePrefab.name, position, Quaternion.Euler(0, 0, r.Next(360)));
            float treeSize = r.Next(200, 450) / 100f;
            tree.transform.localScale = new Vector3(treeSize, treeSize, 1);
            tree.name = "Tree " + i;
        }
        //Generate Houses
        for (int i = 0; i < numberOfHouse; i++)
        {
            bool isValid;
            int attempt = 30;
            Vector3 position;
            do
            {
                isValid = true;
                position = new Vector3(transform.position.x + r.Next(roomWidth), transform.position.y + r.Next(roomHeight), 0);
                Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(position.x - 14f, position.y + 24.5f), new Vector2(position.x + 17.5f, position.y - 24.5f));
                foreach (var collider in colliders)
                {
                    if (collider.CompareTag("Solid"))
                    {
                        isValid = false;
                        break;
                    }
                }
                attempt--;
            }
            while (!isValid && attempt > 0);
            // Instantiate the house once a valid position is found
            if (isValid)
            {
                var house = PhotonNetwork.InstantiateRoomObject(housePrefab.name, position, Quaternion.Euler(0, 0, 0));
                house.name = "House " + i;
            }
        }


        //Generate Boxes
        for (var i = 0; i < numberOfBoxes; i++)
        {
            var position = new Vector3(transform.position.x + r.Next(roomWidth), transform.position.y + r.Next(roomHeight), 0);
            var boxPrefab = boxesPrefab[r.Next(0, boxesPrefab.Count)];
            var box = PhotonNetwork.InstantiateRoomObject(boxPrefab.name, position, Quaternion.Euler(0, 0, r.Next(360)));
            box.name = "Box " + i;
        }
        //Generate Guns
        for (var i = 0; i < numberOfGuns; i++)
        {
            var position = new Vector3(transform.position.x + r.Next(roomWidth), transform.position.y + r.Next(roomHeight), 0);
            var gunPrefab = gunsPrefab[r.Next(0, gunsPrefab.Count)];
            var gun = PhotonNetwork.InstantiateRoomObject(gunPrefab.name, position, Quaternion.Euler(0, 0, r.Next(360)));
            gun.name = gunPrefab.name;
        }
        //Generate Utilities
        for (var i = 0; i < numberOfUtilities; i++)
        {
            var position = new Vector3(transform.position.x + r.Next(roomWidth), transform.position.y + r.Next(roomHeight), 0);
            var utilityPrefab = utilitesPrefab[r.Next(0, utilitesPrefab.Count)];
            var util = PhotonNetwork.InstantiateRoomObject(utilityPrefab.name, position, Quaternion.Euler(0, 0, r.Next(360)));
            util.name = utilityPrefab.name;
        }
        //Generate AI enemies
        for (var i = 0; i < numberOfAIPlayer; i++)
        {
            var position = new Vector3(transform.position.x + r.Next(roomWidth), transform.position.y + r.Next(roomHeight), 0);
            var enemy = PhotonNetwork.InstantiateRoomObject(playerAI.name, position, Quaternion.Euler(0, 0, r.Next(360)));
            enemy.name = "Enemy " + i;
        }

        var navMeshSurface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
    }
}
