using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI playerCountDisplay;
    public int playerCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerChange(int amount)
    {
        playerCount += amount;
        playerCountDisplay.text = "Players left : " + playerCount.ToString();
    }
    
}
