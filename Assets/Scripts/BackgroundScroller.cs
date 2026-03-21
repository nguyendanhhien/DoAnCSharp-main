using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // Tốc độ trôi nền (chỉnh trong Inspector, ví dụ: 0.1)
    public float scrollSpeed = 0.5f;

    private Renderer _renderer;
    private Vector2 _savedOffset;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Tính toán độ lệch dựa trên thời gian
        // Mathf.Repeat làm giá trị luôn chạy từ 0 đến 1 rồi lặp lại
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);

        // Cập nhật offset cho texture (di chuyển ảnh bên trong Quad)
        Vector2 offset = new Vector2(0, y);
        _renderer.material.mainTextureOffset = offset;
    }
}