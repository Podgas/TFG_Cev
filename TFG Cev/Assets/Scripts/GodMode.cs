using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodMode : MonoBehaviour
{

    public bool isGodMode;
    [SerializeField]
    GameObject godModePanel;

    [SerializeField]
    PlayerController player;

    #region SINGLETON PATTERN
    public static GodMode _instance;
    public static GodMode Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GodMode>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("GodMode");
                    _instance = container.AddComponent<GodMode>();
                }
            }

            return _instance;
        }
    }
    #endregion

    private void Update()
    {
        if (Input.GetButtonDown("Select"))
        {
            if (isGodMode)
            {
                Cursor.visible = false;
                isGodMode = false;
                godModePanel.SetActive(false);
            }
            else
            {
                Cursor.visible = true;
                isGodMode = true;
                godModePanel.SetActive(true);
            }
            
        }

    }

    public void GoTo(Transform pos)
    {
        player.GoTo(pos);
    }

    public void SwitchModel()
    {
        if (player.model == player.testModel)
            player.SwitchModel(player.playerModel);
        else
            player.SwitchModel(player.testModel);
    }
    

}
