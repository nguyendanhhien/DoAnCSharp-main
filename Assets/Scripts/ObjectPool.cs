using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [Header("Cài đặt")]
    public GameObject bulletPrefab;
    public int poolAmount = 50;

    // Danh sách chứa đạn
    private List<GameObject> pooledObjects;

    void Awake() // <--- QUAN TRỌNG: Dùng Awake thay vì Start
    {
        // 1. Setup Singleton
        if (instance == null) instance = this;

        // 2. Xây kho đạn NGAY LẬP TỨC
        // Code này sẽ chạy trước tất cả các hàm Start() của máy bay
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        // --- LỚP BẢO VỆ (FIX LỖI TRONG HÌNH) ---
        // Nếu vì lý do gì đó danh sách vẫn chưa có, trả về null ngay để không báo lỗi đỏ
        if (pooledObjects == null)
        {
            return null;
        }

        // Duyệt danh sách tìm đạn
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            // Kiểm tra kỹ hơn: Nếu viên đạn lỡ bị xóa (Destroy) thì bỏ qua
            if (pooledObjects[i] == null) continue;

            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}