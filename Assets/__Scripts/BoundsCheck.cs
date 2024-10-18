using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    /// <summary>
    /// Keeps a veiw of the object in orthographic view
    /// </summary>
    [System.Flags]
    public enum eScreenLocs
    {
        onScreen = 0,
        offRight = 1,
        offLeft = 2,
        offUp = 3,
        offDown = 8
    }




    public enum eType { center, inset, outset };

    [Header("Inscribed")]
    public eType boundsType = eType.center;
    public float radius = 1f;
    public bool keepOnScreen = true;



    [Header("Dynamic")]
    public eScreenLocs screenLocs = eScreenLocs.onScreen;
    //public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;

    private void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        float checkRadius = 0;
        if (boundsType == eType.inset) checkRadius = -radius;
        if (boundsType == eType.outset) checkRadius = radius;

        Vector3 pos = transform.position;
        //isOnScreen = true;
        screenLocs = eScreenLocs.onScreen;

        //restrict the x to camwidth
        if (pos.x > camWidth + checkRadius)
        {
            pos.x = camWidth + checkRadius;
            //isOnScreen = false;
            screenLocs = eScreenLocs.offRight;
        }
        if (pos.x < -camWidth - checkRadius)
        {
            pos.x = -camWidth - checkRadius;
            // isOnScreen = false;
            screenLocs = eScreenLocs.offLeft;
        }
        //restrict the y to the camheight
        if (pos.y > camHeight + checkRadius)
        {
            pos.y = camHeight + checkRadius;
            //isOnScreen = false;
            screenLocs = eScreenLocs.offUp;
        }
        if (pos.y < -camHeight - checkRadius)
        {
            pos.y = -camHeight - checkRadius;
            //isOnScreen = false;
            screenLocs = eScreenLocs.offDown;
        }
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            //isOnScreen = true;
            screenLocs = eScreenLocs.onScreen;
        }
    }

        public bool isOnScreen
        {
        get { return (screenLocs == eScreenLocs.onScreen); }
        }

    public bool LocIs(eScreenLocs checkLoc)
    {
        if (checkLoc == eScreenLocs.onScreen)   return isOnScreen;
        return((screenLocs & checkLoc)== checkLoc);

        

        
    }
        
   















    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
