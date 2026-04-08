using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    
    public float scrollSpeed = 0.5f;

    private Renderer _renderer;
    private Vector2 _savedOffset;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);

        
        Vector2 offset = new Vector2(0, y);
        _renderer.material.mainTextureOffset = offset;
    }
}