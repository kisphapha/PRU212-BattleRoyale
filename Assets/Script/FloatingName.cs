using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;
    public void UpdateName(string name)
    {
        if (name == null || name == string.Empty)
        {
            name = "Unnamed";
        }
        text.text = name;
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.position = target.transform.position + offset;
    }
}
