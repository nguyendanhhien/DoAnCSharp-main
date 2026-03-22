using UnityEngine;
using UnityEngine.SceneManagement; // Bắt buộc phải có thư viện này để chuyển Scene

public class MainMenuController : MonoBehaviour
{
    // Hàm này sẽ được gọi khi bấm nút Play
    public void PlayGame()
    {
        // Chuyển sang màn hình chơi game. 
        // LƯU Ý: Đổi "SampleScene" thành tên Scene chứa game của bạn (nếu bạn đặt tên khác)
        SceneManager.LoadScene("SampleScene");
    }
}