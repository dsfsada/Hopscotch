using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEditor.FilePathAttribute;
//using static UnityEditor.PlayerSettings;

public class ClickEventScript : MonoBehaviour
{
    //마우스 클릭과 관련된 행동 다 정리
    
    public GameObject tile_cover; // 클릭한 위치 보여줄 커버
    public GameObject click_Obj; //마우스로 클릭한 오브젝트;
    public GameObject save_Obj_Foruibutton; //uibutton을 위한 오브젝트;
    public GameObject tile_cover_copy; //복제한 커버를 넣을꺼임

    public GameObject building_selection_summoning_area; //빌딩 클릭시 소환 가능 영역표시
    private GameObject building_selection_summoning_area_copy; //빌딩 클릭시 소환 가능 영역표시 복사

    Vector3 MousePoint; // 마우스 포인트 좌표 가져오기
    Camera Camera;

    public bool Location_prohibited = true; //ui button클릭시 위치값 변경 불가능하게 만들기 위해

    // Start is called before the first frame update
    void Start()
    {
        Camera = GameObject.Find("Camera").GetComponent<Camera>(); //카메라 불러옴
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camera_bottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 camera_top = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane));
        Vector3 camera_quarter_view = camera_bottom + (camera_top - camera_bottom) * 1 / 5;
        //카메라의 1/5부분은 클릭이 불가하게 만들기 위해서 범위를 구함

        //왼쪽 마우스로 클릭한 오브젝트 가져오기
        if (Input.GetMouseButtonDown(0) && GameObject.Find("Resourse").GetComponent<Resourse>().the_world == 0) //마우스 왼쪽 버튼 : 마우스 포인트에 있는 오브젝트 가져오기
        {
            Destroy(building_selection_summoning_area_copy); //기존 타일선택 소환영역 삭제
            Destroy(tile_cover_copy); //기존 테두리 삭제
            // 마우스 클릭한 좌표 가져오기
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (pos.y > camera_quarter_view.y)
            {
                //해당 좌표에 있는 오브젝트 찾기
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
                if (hit.collider != null)
                {
                    click_Obj = hit.transform.gameObject;
                }

                Vector2 click_Obj_position = click_Obj.transform.position;  //유닛 소환할 위치(마우스 클릭 위치)
                if (GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)click_Obj_position.x, (int)click_Obj_position.y] == 0//해당 좌표에 아군 건물또는 유닛이 존재하지 않을 때 
                        && GameObject.Find("Resourse").GetComponent<Resourse>().visible_Tile[(int)click_Obj_position.x, (int)click_Obj_position.y] != 0)
                {
                    GameObject.Find("Ui").GetComponent<SpawnPerson>().unitSummon_ClickOnFloor(click_Obj_position); //클릭한 위치에 유닛소환
                    GameObject.Find("Ui").GetComponent<SpawnPerson>().spawn_possible = false; //spawnPerson스크립트에서 소환가능한 x값을 false로 설정
                }

                //클릭된 오브젝트의 태그 비교후 실행(uiscript에서 ui표시하는 함수 실행)
                GameObject.Find("Ui").GetComponent<UiScript>().ui_Open_Down(click_Obj);

                //ui를 update가 아닌 클릭해서 돌때 실행되게 만듬
                //GameObject.Find("Ui").GetComponent<UiScript>().Uistart();
            }

            if (click_Obj.tag == "Unit" || click_Obj.tag == "Building")
            {
                //스폰펄슨이랑 빌드 스크립트에서 사용
                save_Obj_Foruibutton = click_Obj; //버튼ui를 누를때 맵 타일위치값을 가져오게 되어도 기존 클릭되어 있는 save_Obj_Foruibutton는 유지됨
                tile_cover_copy = Instantiate(tile_cover, new Vector3(click_Obj.transform.position.x, click_Obj.transform.position.y, -3), Quaternion.identity); //마우스 위치에 테두리 생성

            }
            if (click_Obj.tag == "Building") //빌딩을 클릭하거나 소환가능상태일때
            {
                building_selection_summoning_area_copy = Instantiate(building_selection_summoning_area, new Vector3(save_Obj_Foruibutton.transform.position.x, save_Obj_Foruibutton.transform.position.y, -2), Quaternion.identity);
            }//건물 선택시 주변의 유닛 소환 가능 

        }
        //마우스 오른쪽 버튼 : 선택된 오브젝트에 따른 행동
        if (Input.GetMouseButtonDown(1) && click_Obj != null)
        {
            if (click_Obj.tag == "Unit") //태그가 유닛이라면 이동 준비 
            {
                //blue_Unit_Tile의 해당 좌표에 선택된 유닛( Click_object) 의 번호
                int unit_type = GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile
                    [(int)click_Obj.transform.position.x, (int)click_Obj.transform.position.y];

                //현재 선택된 click_object의 원래 좌표 
                Vector2 unit_pos = new Vector2(click_Obj.transform.position.x, click_Obj.transform.position.y);

                MousePoint = Input.mousePosition;
                MousePoint = Camera.ScreenToWorldPoint(MousePoint);
                if (GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)MousePoint.x, (int)MousePoint.y] == 0//해당 좌표에 아군 건물또는 유닛이 존재하지 않을 때 
                    && GameObject.Find("Resourse").GetComponent<Resourse>().visible_Tile[(int)MousePoint.x, (int)MousePoint.y] != 0)//해당 좌표의 시야가 있을 때
                {
 
                    // 현재 식량 얼마인지 확인하기
                    int blue_food = GameObject.Find("Resourse").GetComponent<Resourse>().food;
                    //식량 확인
                    float Cal = Mathf.Abs(unit_pos.x - (int)MousePoint.x -0.5f ) + Mathf.Abs(unit_pos.y - (int)MousePoint.y - 0.5f);

                    if (blue_food >= Cal)
                    {
                        GameObject.Find("Resourse").GetComponent<Resourse>().food -= (int)Cal; //식량 설정
                        click_Obj.transform.position = new Vector3((int)MousePoint.x + 0.5f, (int)MousePoint.y + 0.5f, -1);

                        GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)MousePoint.x, (int)MousePoint.y] = unit_type;
                        GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)unit_pos.x, (int)unit_pos.y] = 0;
                        tile_cover_copy.transform.position = new Vector3((int)MousePoint.x + 0.5f, (int)MousePoint.y + 0.5f, -3);
                    }
                }
            }
        }

        //// 스페이스를 눌렀을 때 && 임시로 만들어놓은 기능 차후 ui의 버튼으로 옮길 예정입니다.
        if (Input.GetKeyDown(KeyCode.Space) && click_Obj != null)
        {
            if (click_Obj.tag == "MapTile") //맵 타일이라면 기본 유닛 소환하기
            {
                if (GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)click_Obj.transform.position.x, (int)click_Obj.transform.position.y] == 0)
                {
                    Vector2 unit_pos = new Vector2(click_Obj.transform.position.x, click_Obj.transform.position.y);
                    if (GameObject.Find("Resourse").GetComponent<Resourse>().visible_Tile[(int)unit_pos.x, (int)unit_pos.y] != 0)
                    {
                        GameObject.Find("SpawnObj").GetComponent<SpawnObj>().SpawnBasicUnit(unit_pos);
                    }
                }
            }
        }
        // A  버튼 검 아군 소환
        if (Input.GetKeyDown(KeyCode.A) && click_Obj != null)
        {
            if (click_Obj.tag == "MapTile") //맵 타일이라면 검 블루 소환하기
            {
                if (GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)click_Obj.transform.position.x, (int)click_Obj.transform.position.y] == 0)
                {
                    Vector2 unit_pos = new Vector2(click_Obj.transform.position.x, click_Obj.transform.position.y);
                    //if (GameObject.Find("Resourse").GetComponent<Resourse>().visible_Tile[(int)unit_pos.x, (int)unit_pos.y] != 0) 잠깐 꺼둠
                    {
                        GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_knife_Blue_Unit(unit_pos);
                    }
                }
            }
        }
        // S 버튼 도끼 아군 소환
        if (Input.GetKeyDown(KeyCode.S) && click_Obj != null)
        {
            if (click_Obj.tag == "MapTile") //맵 타일이라면 도끼 블루 소환하기
            {
                if (GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)click_Obj.transform.position.x, (int)click_Obj.transform.position.y] == 0)
                {
                    Vector2 unit_pos = new Vector2(click_Obj.transform.position.x, click_Obj.transform.position.y);
                    //if (GameObject.Find("Resourse").GetComponent<Resourse>().visible_Tile[(int)unit_pos.x, (int)unit_pos.y] != 0) 잠깐꺼둠
                    {
                        GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_axe_Blue_Unit(unit_pos);
                    }
                }
            }
        }
        // D 버튼 방패 아군 소환
        if (Input.GetKeyDown(KeyCode.D) && click_Obj != null)
        {
            if (click_Obj.tag == "MapTile") //맵 타일이라면 방패 블루 소환하기
            {
                if (GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)click_Obj.transform.position.x, (int)click_Obj.transform.position.y] == 0)
                {
                    Vector2 unit_pos = new Vector2(click_Obj.transform.position.x, click_Obj.transform.position.y);
                    //if (GameObject.Find("Resourse").GetComponent<Resourse>().visible_Tile[(int)unit_pos.x, (int)unit_pos.y] != 0) 잠깐꺼둠
                    {
                        GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_shield_Blue_Unit(unit_pos);
                    }
                }
            }
        }

        // Z 버튼 적군 적 소환
        if (Input.GetKeyDown(KeyCode.Z) && click_Obj != null)
        {
            if (click_Obj.tag == "MapTile") //맵 타일이라면 검 Red 소환하기
            {
                if (GameObject.Find("Resourse").GetComponent<Resourse>().red_Unit_Tile[(int)click_Obj.transform.position.x, (int)click_Obj.transform.position.y] == 0)
                {
                    Vector2 unit_pos = new Vector2(click_Obj.transform.position.x, click_Obj.transform.position.y);
                    GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_knife_Red_Unit(unit_pos);
                }
            }
        }
        // X 버튼 도끼 적 소환
        if (Input.GetKeyDown(KeyCode.X) && click_Obj != null)
        {
            if (click_Obj.tag == "MapTile") //맵 타일이라면 도끼 레드 소환하기
            {
                if (GameObject.Find("Resourse").GetComponent<Resourse>().red_Unit_Tile[(int)click_Obj.transform.position.x, (int)click_Obj.transform.position.y] == 0)
                {
                    Vector2 unit_pos = new Vector2(click_Obj.transform.position.x, click_Obj.transform.position.y);
                    GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_axe_Red_Unit(unit_pos);
                }
            }
        }
        // c 버튼 방패 적 소환
        if (Input.GetKeyDown(KeyCode.C) && click_Obj != null)
        {
            if (click_Obj.tag == "MapTile") //맵 타일이라면 방패 적 소환하기
            {
                if (GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)click_Obj.transform.position.x, (int)click_Obj.transform.position.y] == 0)
                {
                    Vector2 unit_pos = new Vector2(click_Obj.transform.position.x, click_Obj.transform.position.y);
                    GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_shield_Red_Unit(unit_pos);
                }
            }
        }
        //V 버튼 적 타워 소환
        if (Input.GetKeyDown(KeyCode.V) && click_Obj != null)
        {
            if (click_Obj.tag == "MapTile") //맵 타일이라면 방패 적 소환하기
            {
                if (GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)click_Obj.transform.position.x, (int)click_Obj.transform.position.y] == 0)
                {
                    Vector2 unit_pos = new Vector2(click_Obj.transform.position.x, click_Obj.transform.position.y);
                    GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_Tower_R(unit_pos);
                }
            }
        }
    }
}