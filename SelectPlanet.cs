using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlanet : MonoBehaviour
{
    public GameObject[] gameObjects3D;  // Mảng các GameObject 3D cần lựa chọn
    public Button[] buttons;            // Mảng các Button trong Scroll View

    private void Start()
    {
        // Gán sự kiện click cho mỗi button
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;  // Lưu chỉ số gameObject
            buttons[i].onClick.AddListener(() => SelectGameObject(index));
        }
    }

    // Hàm để chọn gameObject 3D
    void SelectGameObject(int index)
    {
        for (int i = 0; i < gameObjects3D.Length; i++)
        {
            // Chỉ kích hoạt GameObject đã được chọn
            gameObjects3D[i].SetActive(i == index);
        }
        // Di chuyển camera hoặc thiết lập góc nhìn phù hợp nếu cần
        Camera.main.transform.position = gameObjects3D[index].transform.position + new Vector3(0, 0, -10);  // Di chuyển camera
    }
}
