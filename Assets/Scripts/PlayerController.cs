using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Giới hạn biên màn hình (tự động tính toán)
    private Vector2 minBounds;
    private Vector2 maxBounds;

    // Kích thước của máy bay (để trừ hao khi chạm biên)
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        // 1. Tính toán giới hạn màn hình dựa trên Camera
        Vector3 screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        Vector3 screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        // 2. Lấy kích thước máy bay từ Sprite Renderer
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        objectWidth = sr.bounds.extents.x; // Một nửa chiều rộng
        objectHeight = sr.bounds.extents.y; // Một nửa chiều cao

        // 3. Đặt biên giới hạn (Cộng trừ kích thước máy bay để nó không bị lấp ló 1 nửa ra ngoài)
        minBounds = new Vector2(screenBottomLeft.x + objectWidth, screenBottomLeft.y + objectHeight);
        maxBounds = new Vector2(screenTopRight.x - objectWidth, screenTopRight.y - objectHeight);
    }

    void Update()
    {
        // Biến lưu vị trí ngón tay/chuột
        Vector3 inputPosition = Vector3.zero;
        bool isInput = false;

        // KIỂM TRA INPUT (Hỗ trợ cả PC và Mobile)
        if (Input.GetMouseButton(0)) // Dùng chuột trái (trên PC) hoặc chạm (trên Mobile đều nhận là Mouse 0)
        {
            inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isInput = true;
        }
        else if (Input.touchCount > 0) // Cụ thể cho Mobile (Touch)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                inputPosition = Camera.main.ScreenToWorldPoint(touch.position);
                isInput = true;
            }
        }

        // XỬ LÝ DI CHUYỂN
        if (isInput)
        {
            // Đặt Z = 0 để đảm bảo nó nằm trên mặt phẳng 2D
            inputPosition.z = 0;

            // Di chuyển máy bay đến vị trí ngón tay
            // Có thể dùng Vector3.Lerp nếu muốn nó đuổi theo mềm mại hơn (tùy chọn)
            transform.position = inputPosition;

            // GIỚI HẠN TỌA ĐỘ (CLAMP)
            // Ép vị trí X và Y nằm trong khung màn hình đã tính ở Start
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y, maxBounds.y);

            transform.position = clampedPosition;
        }
    }
}