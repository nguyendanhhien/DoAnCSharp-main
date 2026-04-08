using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Vector2 minBounds;
    private Vector2 maxBounds;

    
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        
        Vector3 screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        Vector3 screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        objectWidth = sr.bounds.extents.x; 
        objectHeight = sr.bounds.extents.y; 

        
        minBounds = new Vector2(screenBottomLeft.x + objectWidth, screenBottomLeft.y + objectHeight);
        maxBounds = new Vector2(screenTopRight.x - objectWidth, screenTopRight.y - objectHeight);
    }

    void Update()
    {
        
        Vector3 inputPosition = Vector3.zero;
        bool isInput = false;

        
        if (Input.GetMouseButton(0)) 
        {
            inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isInput = true;
        }
        else if (Input.touchCount > 0) 
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                inputPosition = Camera.main.ScreenToWorldPoint(touch.position);
                isInput = true;
            }
        }

        
        if (isInput)
        {
            
            inputPosition.z = 0;

            
            transform.position = inputPosition;

            
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y, maxBounds.y);

            transform.position = clampedPosition;
        }
    }
}