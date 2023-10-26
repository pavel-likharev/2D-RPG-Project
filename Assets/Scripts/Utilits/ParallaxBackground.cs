using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float widthBG;

    private void Awake()
    {
        widthBG = GetComponent<SpriteRenderer>().size.x;
    }

    private void Start()
    {
        xPosition = transform.position.x;
        
    }

    private void LateUpdate()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > xPosition + widthBG)
        {
            xPosition = xPosition + widthBG;
        }
        else if (distanceMoved < xPosition - widthBG)
        {
            xPosition = xPosition - widthBG;
        }
    }
}
