using TMPro;
using UnityEngine;

public class LoadStatistic : MonoBehaviour
{
    public TextMeshProUGUI totalKills;
    public TextMeshProUGUI totalWins;
    private void Start()
    {
        var kills = PlayerPrefs.GetInt("kills");
        var wins = PlayerPrefs.GetInt("wins");
        totalKills.text = "Total Kills : " + kills;
        totalWins.text = "Total Wins : " + wins;
    }
}