using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;
    [SerializeField] private float parallaxEffectY;

    private float xPosition;
    private float yPosition;

    private float length;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");

        length = GetComponent<SpriteRenderer>().bounds.size.x;

        xPosition = transform.position.x;
        yPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);

        float distanceToMove = cam.transform.position.x * parallaxEffect;

        float distanceToMoveY = cam.transform.position.y * parallaxEffectY;

        transform.position = new Vector3(xPosition + distanceToMove,yPosition + distanceToMoveY );

        if(distanceMoved > xPosition + length)
            xPosition = xPosition + length;
        else if(distanceMoved < xPosition - length)
            xPosition = xPosition - length;
    }
}
