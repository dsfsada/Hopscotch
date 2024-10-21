using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public float temp_value;
    public float Wheelspeed; //카메라 줌인 속도
    public float MoveSpeed; //카메라 이동 속도
    public float dragSpeed; //카메라 드래그 이동속도
    float clickPointX, clickPointY;
    Camera Camera;
    int max_x;
    int max_y;
    // Start is called before the first frame update
    void Start()
    {
        max_x = GameObject.Find("Resourse").GetComponent<Resourse>().width;
        max_y = GameObject.Find("Resourse").GetComponent<Resourse>().height;
        Camera = GameObject.Find("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라 줌인 줌아웃
        float scroll = Input.GetAxis("Mouse ScrollWheel") * Wheelspeed;
        //카메라 줌 인 정도를 나타냄 # 확대 최대치 : 4  축소 최대치 : 10
        temp_value = Camera.orthographicSize;
        // scroll < 0 : scroll down하면 줌인
        if (Camera.orthographicSize <= 4f && scroll > 0)
        {
            
            Camera.orthographicSize = temp_value; // maximize zoom in

            // 최대로 Zoom in 했을 때 특정 값을 지정했을 때

            // 최대 줌 인 범위를 벗어날 때 값에 맞추려고 한번 줌 아웃 되는 현상을 방지
        }

        // scroll > 0 : scroll up하면 줌아웃
        else if (Camera.orthographicSize >= 10f && scroll < 0)
        {
            Camera.orthographicSize = temp_value; // maximize zoom out
        }
        else
            Camera.orthographicSize -= scroll * 0.5f;

        dragMove();
        //카메라 상화 좌우  이동

        if(Input.GetKey(KeyCode.RightArrow)) {
            transform.position += Vector3.right * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * MoveSpeed * Time.deltaTime;
        }
        //범위 초과 불가능하게 만들기
        if(transform.position.x < temp_value)
        {
            transform.position = new Vector3(temp_value, transform.position.y,-10);
        }
        if (transform.position.x > max_x-temp_value)
        {
            transform.position = new Vector3(max_x - temp_value, transform.position.y, -10);
        }
        if (transform.position.y < temp_value/2)
        {
            transform.position = new Vector3(transform.position.x, temp_value / 2, -10);
        }
        if (transform.position.y > max_x - (temp_value/2))
        {
            transform.position = new Vector3(transform.position.x, max_y - (temp_value / 2), -10);
        }
    }
   void dragMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickPointX = Input.mousePosition.x;
            clickPointY = Input.mousePosition.y;
        }
        if (Input.GetMouseButton(0))
        {
            // (현재 마우스 위치 - 최초 위치)의 음의 방향으로 카메라 이동
            Vector2 position = Camera.main.ScreenToViewportPoint(-new Vector3(Input.mousePosition.x - clickPointX, Input.mousePosition.y - clickPointY, 0));
            Vector2 move = position * dragSpeed * Time.deltaTime;

            Camera.main.transform.Translate(move);
        }
    }
}

