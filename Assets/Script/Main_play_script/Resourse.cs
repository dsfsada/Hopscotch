using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resourse : MonoBehaviour
{
    //만약 [101, 101] 범위 변경하게 되면 build 범위부분도 같이 변경해주길
    //변수를 저장하는 곳
    public int[,] map_Tile = new int[101, 101]; //맵 타일 #1 : 빈땅 #2 : 식량 땅 #3 : 자원땅
    public int[,] visible_Tile = new int[101, 101]; //시야 타일
    public int[,] blue_Unit_Tile = new int[101, 101]; //유닛 위치 타일 유닛과 건물이 있을 것 #0 빈땅 #1 ~ 9 유닛 종류 #10~ 건물 종류
    public int[,] red_Unit_Tile = new int[101, 101]; //위와 동일

    public int[,] blue_Influence_Tile = new int[101, 101]; // 파랑팀 건물 영향력 (성벽 기준 2칸 : 여기서  자원을 먹을 수 있음)
    public int[,] red_Influence_Tile = new int[101, 101]; // 빨강팀 건물 영향력  (성벽 기준 2칸)
    /* 유닛 종류에 따른 코드 번호
        0 : 유닛 없음
        1: 기본 유닛
        2: 검 유닛
        3: 도끼 유닛
        4: 방패 유닛
        10 : 성벽 ( 기본 유닛 소환 가능 ) 
        11 : 검건물
        12 : 도끼건물
        13 : 방패건물
*/
    public Vector2[,] spawn_value = new Vector2[3, 3];  //건물 근처 유닛 스폰 가능하도록 선택한 건물의 근처 위치들의 값을 저장

    public int width, height;// 가로 세로 크기

    public List<GameObject> visible = new List<GameObject>(); //시야 타일 저장 위치 ( 사용처 : 턴 종류 후 바뀐 visible_Tile에 의한 다시 그리기 위한 삭제공간)
    public List<GameObject> blue_Unit = new List<GameObject>(); // 아군 유닛을 저장할 곳 (사용처 : 턴 종류 후 for 문을 통해 시야 작업 및 자원 처리)
    public List<GameObject> red_Unit = new List<GameObject>(); //적군 유닛을 저장할 곳

    public int wood = 0; //나무
    public int food = 0; //식량
    public int turn_food_up = 0;　//식량up
    public int turn_wood_up = 0;　//나무up


    public int turn_value = 0; //몇턴째인지

    public int the_world = 0; // 클릭 제한 # 0 클릭할 수 있음 1 : 클릭 못함

    //임시로 여기에 둔 것 좋은 장소 있다면 옮겨서 정리
    public void placeRedUnit()
    {
       
        red_Unit_Tile[15, 15] = Random.Range(11, 14);
        red_Unit_Tile[15, 18] = Random.Range(11, 14);
        red_Unit_Tile[18, 15] = Random.Range(11, 14);
        red_Unit_Tile[11, 11] = Random.Range(11, 14);
        red_Unit_Tile[11, 14] = Random.Range(11, 14);
        red_Unit_Tile[11, 18] = Random.Range(11, 14);
        red_Unit_Tile[14, 11] = Random.Range(11, 14);
        red_Unit_Tile[18, 11] = Random.Range(11, 14);
        red_Unit_Tile[8, 8] = Random.Range(11, 14); 
        red_Unit_Tile[8,12] = Random.Range(11, 14);
        red_Unit_Tile[8, 15] = Random.Range(11, 14);
        red_Unit_Tile[8, 19] = Random.Range(11, 14);
        red_Unit_Tile[12, 8] = Random.Range(11, 14);
        red_Unit_Tile[15, 8] = Random.Range(11, 14);
        red_Unit_Tile[19, 8] = Random.Range(11, 14);
        red_Unit_Tile[13, 4] = Random.Range(11, 14);
        red_Unit_Tile[4, 13] = Random.Range(11, 14);
        red_Unit_Tile[17, 4] = Random.Range(11, 14);
        red_Unit_Tile[4,17] = Random.Range(11, 14);
   
    }



    // 맵 타일 정보 입력
    public void make_Map_Tile()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                map_Tile[i, j] = Random.Range(1, 4);
                
            }
        }
        //red팀 ai 건물 배치 (배열)
        placeRedUnit();
    }
    //시야 업데이트
    public void update_Visible()
    {
        visible_Tile = new int[101, 101]; // 시야필드 초기화
        for(int i = visible.Count - 1; i >= 0; i--)
        {
            Destroy(visible[i]);
            visible.Remove(visible[i]);
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                int type = blue_Unit_Tile[i, j]; // 유닛 타입 가져오기 타입 별로 시야 범위가 다름
                //유닛
                if (type > 0 && type < 10) //유닛임 1~9
                {
                    for (int w = i - 1; w <= i + 1; w++)
                    {
                        for (int h = j - 1; h <= j + 1; h++)
                        {
                            if (w >= 0 && w < width && h >= 0 && h < height) //해당 좌표가 맵 범위 안에 있다면?
                            {
                                visible_Tile[w, h] = 1;
                            }
                        }
                    }
                }
                //성벽 10
                else if (type >= 10 && type < 99) //10 성벽
                {
                    for (int w = i - 3; w <= i + 3; w++)
                    {
                        for (int h = j - 3; h <= j + 3; h++)
                        {
                            if (w >= 0 && w < width && h >= 0 && h < height) //해당 좌표가 맵 범위 안에 있다면?
                            {
                                visible_Tile[w, h] = 1;
                            }
                        }
                    }
                }
            }
        }
    }
    //영향력 업데이트
    public void update_Influence()
    {
        blue_Influence_Tile = new int[101, 101];
        red_Influence_Tile = new int[101, 101];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //아군 성벽 기준 범위 탐색
                if (blue_Unit_Tile[i, j] == 10) // 해당 좌표가 성벽일때
                {
                    for (int w = i - 2; w <= i + 2; w++) // 2타일 기준
                    {
                        for (int h = j - 2; h <= j + 2; h++)
                        {
                            if (w >= 0 && w < width && h >= 0 && h < height) //범위 안
                            {
                                blue_Influence_Tile[w, h] = 1; //해당 타일은 내 범위
                            }
                        }
                    }
                }
                //적군 성벽 기준 범위 탐색
                if  (red_Unit_Tile[i, j] == 10) // 해당 좌표가 성벽일때
                {
                    for (int w = i - 2; w <= i + 2; w++) // 2타일 기준
                    {
                        for (int h = j - 2; h <= j + 2; h++)
                        {
                            if (w >= 0 && w < width && h >= 0 && h < height) //범위 안
                            {
                                red_Influence_Tile[w, h] = 1; //해당 타일은 내 범위
                            }
                        }
                    }
                }


            }
        }
    }
    // 자원 업데이트 (영향력 내의 건물에서 식량,나무 얻음
    public void update_Resourse()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //온전히 내 땅의 영향력일 때
                if (blue_Influence_Tile[i, j] == 1 && red_Influence_Tile [i,j] !=1)
                {
                    //해당 필드가 나무라면
                    if (map_Tile[i, j] == 2) food += turn_food_up; //식량 +3
                    else if (map_Tile[i, j] == 3) wood+= turn_wood_up;//나무 +1
                }
                if (blue_Unit_Tile[i, j] == 1)//해당 타일에 내 기본 유닛이 있으면?
                {
                    if (map_Tile[i, j] == 2) food += 5; //식량 +5 
                    else if (map_Tile[i, j] == 3) wood += 3;//나무 +3
                }
            }
        }
    }


    int[,] check_Unit = new int[9, 2] { { 0,0}, { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };
    // 전투 업데이트 ( 전투로 인한 유닛 업데이트)
    // 블루팀 입장에서 할 예정 2,3,4 > 1 = 10, 2>3>4>2 (블루 팀만 해도 괜찮음 ,, 블루와 레드의 상호작용이라... 블루 입장에서 모든 레드 확인 가능)
    // 같은 코드의 경우 (다른 좌표에 있음 = 둘 다 생존  , 같은 좌표에 있음 = 둘 다 사망)
    // 확인 순서 블루 자기 위치 -> 왼쪽 위부터 순서대로 확인 
    // 2,3,4 전투 유닛의 경우 다른 모든 숫자와 비교 , 그 외에는 주변에 전투 유닛이 있는지만 확인
    public void update_Fight_Unit()
    {
        //모든 타일 확인 할 거임
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //타입 가져오기
                int type = blue_Unit_Tile[i, j];
                //유닛 타일이라면
                if (type != 0)
                {
                    //건물 또는 기본 유닛일때
                    if (type >= 10 || type == 1)
                    {
                        //주변 확인
                        for (int w = i - 1; w <= i + 1; w++)
                        {
                            for (int h = j - 1; h <= j + 1; h++)
                            {
                                if (w >= 0 && w < width && h >= 0 && h < height) //비교 할 좌표가 안에 있을 때
                                {
                                    // 비교할 좌표에 있는 적군 유닛이 전투 유닛이라면
                                    if (red_Unit_Tile[w, h] > 1 && red_Unit_Tile[w, h] < 10)
                                    {
                                        //해당 장소의 내 것 삭제
                                        blue_Unit_Tile[i, j] = 0;
                                    }
                                }
                            }
                        }
                    }
                    //검 유닛이라면
                    else if (type == 2)
                    {
                        if (red_Unit_Tile[i, j] == 2)
                        {
                            red_Unit_Tile[i, j] = 0;
                            blue_Unit_Tile[i, j] = 0;
                        }
                        else
                            for (int z = 0; z < 9; z++)
                            {
                                int w = i + check_Unit[z, 0];
                                int h = j + check_Unit[z, 1];
                                if (w >= 0 && w < width && h >= 0 && h < height) //비교 할 좌표가 안에 있을 때
                                {
                                    //해당 위치가 도끼병이라면? 
                                    if (red_Unit_Tile[w, h] == 3) red_Unit_Tile[w, h] = 0;
                                    //반대로 방패병이라면?
                                    else if (red_Unit_Tile[w, h] == 4)
                                    {
                                        blue_Unit_Tile[i, j] = 0;
                                        break;
                                    }
                                    //기초 유닛 또는 건물이라면?
                                    else if (red_Unit_Tile[w, h] == 1 || red_Unit_Tile[w, h] >= 10) red_Unit_Tile[w, h] = 0;
                                }
                            }
                    }
                    //도끼병일 때
                    else if (type == 3)
                    {
                        if (red_Unit_Tile[i, j] == 3)
                        {
                            red_Unit_Tile[i, j] = 0;
                            blue_Unit_Tile[i, j] = 0;
                        }
                        else
                            for (int z = 0; z < 9; z++)
                            {
                                int w = i + check_Unit[z, 0];
                                int h = j + check_Unit[z, 1];
                                if (w >= 0 && w < width && h >= 0 && h < height) //비교 할 좌표가 안에 있을 때
                                {
                                    //해당 위치가 방패병이라면? 
                                    if (red_Unit_Tile[w, h] == 4) red_Unit_Tile[w, h] = 0;
                                    //반대로 검병이라면?
                                    else if (red_Unit_Tile[w, h] == 2)
                                    {
                                        blue_Unit_Tile[i, j] = 0;
                                        break;
                                    }
                                    //기초 유닛 또는 건물이라면?
                                    else if (red_Unit_Tile[w, h] == 1 || red_Unit_Tile[w, h] >= 10) red_Unit_Tile[w, h] = 0;
                                }
                            }
                    }
                    //방패병일 때
                    else if (type == 4)
                    {
                        if (red_Unit_Tile[i, j] == 4)
                        {
                            red_Unit_Tile[i, j] = 0;
                            blue_Unit_Tile[i, j] = 0;
                        }
                        else
                            for (int z = 0; z < 9; z++)
                            {
                                int w = i + check_Unit[z, 0];
                                int h = j + check_Unit[z, 1];
                                if (w >= 0 && w < width && h >= 0 && h < height) //비교 할 좌표가 안에 있을 때
                                {
                                    //해당 위치가 검병이라면? 
                                    if (red_Unit_Tile[w, h] == 2) red_Unit_Tile[w, h] = 0;
                                    //반대로 도끼병이라면?
                                    else if (red_Unit_Tile[w, h] == 3)
                                    {
                                        blue_Unit_Tile[i, j] = 0;
                                        break;
                                    }
                                    //기초 유닛 또는 건물이라면?
                                    else if (red_Unit_Tile[w, h] == 1 || red_Unit_Tile[w, h] >= 10) red_Unit_Tile[w, h] = 0;
                                }
                            }
                    }
                }
            }
        }
        
    }
    public void draw_Unit()
    {
        // 필드 내 유닛 다시 그리기 위한 작업 ( 유닛들 전부 지우기)
        for (int i = blue_Unit.Count - 1; i >= 0; i--)
        {
            //아군유닛 삭제
            Destroy(blue_Unit[i]);
            blue_Unit.Remove(blue_Unit[i]);
        }
        for (int i = red_Unit.Count - 1; i >= 0; i--)
        {
            //적군 유닛 삭제
            Destroy(red_Unit[i]);
            red_Unit.Remove(red_Unit[i]);
        }
        //유닛 다시 그리기
        GameObject.Find("SpawnObj").GetComponent<SpawnObj>().update_Unit_Draw(blue_Unit_Tile, red_Unit_Tile, width, height);
    }
    
    public void activeEnemy() // 적 유닛 움직이기
    {
        for (int i = 0; i < red_Unit.Count; i++)
        {
            if (red_Unit[i].CompareTag("Enemy")) // tag 비교후 tag가 "Enemy"면 해당 객체의 MoveEnemy함수 호출
            {
                red_Unit[i].GetComponent<EnemyMove>().MoveEnemy();
            }
            else if (red_Unit[i].CompareTag("Building")) // tag 비교후 tag가 "Building"면 해당 객체의 SpawnEnemy함수 호출
            {
                red_Unit[i].GetComponent<EnemySpawn>().SpawnEnemy();
            }
        }
    }
}
