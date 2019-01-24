using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMenu : MonoBehaviour {

    public enum MenuChoice
    {
        Upgrade,
        Refund
    }

    public MenuChoice menuChoice;
    public int refundValue;
    public int upgradeValue;
    
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (menuChoice == MenuChoice.Refund)
                SendMessageUpwards("RefundTower", refundValue);
            else if (menuChoice == MenuChoice.Upgrade)
                SendMessageUpwards("UpgradeTower", upgradeValue);
        }
    }

}
