using UnityEngine;

[CreateAssetMenu(fileName = "NewShip", menuName = "ChienCo/ShipData")]
public class ShipData : ScriptableObject
{
    public string shipName;
    public Sprite shipSprite;

    [Header("Chỉ số cơ bản")]
    public int baseHealth = 100;
    public int baseDamage = 10;

    [Header("Cấu hình Đạn")]
    public Sprite normalBullet; // <--- THÊM DÒNG NÀY (Đạn thường)
    public Sprite frenzyBullet; // (Đạn to/Bạo tẩu)
    public float frenzyFireRate = 0.1f;
}