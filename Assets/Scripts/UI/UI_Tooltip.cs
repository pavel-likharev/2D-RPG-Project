using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tooltip : MonoBehaviour
{
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;

    private float xLimit;
    private float yLimit;

    private void Start()
    {

    }

    public void SetTooltipPosition()
    {
        xLimit = Screen.width / 2;
        yLimit = Screen.height / 2;

        float newXOffset;
        float newYOffset;

        Vector2 mousePosition = Input.mousePosition;

        if (mousePosition.x > xLimit)
            newXOffset = -xOffset;
        else
            newXOffset = xOffset;

        if (mousePosition.y > yLimit)
            newYOffset = -yOffset;
        else
            newYOffset = yOffset;


        transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);
     }
}
