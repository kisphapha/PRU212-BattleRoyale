using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputName : MonoBehaviour
{
    public TMP_InputField nameInputField;
    // Start is called before the first frame update
    public void Start()
    {
        //Adds a listener to the main input field and invokes a method when the value changes.
        nameInputField.onValueChanged.AddListener(delegate { SetPlayerName(); });
    }
    public void SetPlayerName()
    {
        //Debug.Log(nameInputField.text);
        var persitentData = FindObjectOfType<PersistentData>();
        if (persitentData != null)
        {
            persitentData.playerName = nameInputField.text;
        }
    }
}
