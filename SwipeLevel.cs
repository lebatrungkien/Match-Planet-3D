using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeLevel : MonoBehaviour
{
    public GameObject scrollBar; //object su dung de swipe level
    float scrollPos = 0; //luu vi tri scrollbar hien tai
    float[] pos; //luu vi tri cac level
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int childCount = transform.childCount;
        pos = new float[childCount]; //khoi tao pos voi kich thuoc bang so luong con object
        float distance = 1f / (childCount - 1f); //khoang cach giua cac level trong scrollbar
        //dat vi tri cho moi level trong mang pos
        for (int i = 0; i < childCount; i++)
        {
            pos[i] = distance * i;
        }
        //neu click chuot trai, scroll_pos se duoc cap nhat theo vi tri cua scrollbar
        //neu ko, kiem tra vi tri hien tai cua scroll_pos, neu scroll_pos nam trong khoang giua 2 vi tri level trong mang pos, scrollbar tu dong can chinh
        if (Input.GetMouseButton(0))
        {
            scrollPos = scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < childCount; i++)
            {
                if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);

                }
            }
        }

        //thuc hien thu phong level
        //neu scroll_pos nam giua vi tri 2 level trong pos, level tuong ung se duoc zoom, cac cap do khac se duoc thu nho
        for (int i = 0; i < childCount; i++)
        {
            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.0f, 1.0f), 0.1f);
                for (int a = 0; a < childCount; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.5f, 0.5f), 0.1f);
                    }
                }
            }
        }
    }

   
}


