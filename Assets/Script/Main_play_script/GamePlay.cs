using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class GamePlay : MonoBehaviour
{
    private int start_blue_building =1, start_red_building;

    public GameObject main_mark;
    public bool gameOver_bool = false;

    // Start is called before the first frame update
    void Start()
    {
        //적팀 메인건물 소환 위치값
        start_red_building = GameObject.Find("Resourse").GetComponent<Resourse>().height - 2;

        GameObject.Find("Resourse").GetComponent<Resourse>().make_Map_Tile(); //맵 타일 값 설정하기
        GameObject.Find("DrawField").GetComponent<Draw_Field>().first_Field(GameObject.Find("Resourse").GetComponent<Resourse>().map_Tile,
            GameObject.Find("Resourse").GetComponent<Resourse>().width,
            GameObject.Find("Resourse").GetComponent<Resourse>().height); 
        //맵 타일 그리기
        GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_Tower_B(new Vector2(0.5f + start_blue_building, 0.5f + start_blue_building)); //처음 블루 빌딩 생성
        GameObject blue_mark = Instantiate(main_mark, new Vector3(0.5f + start_blue_building, 0.5f + 1,-4), Quaternion.identity); //메인 표시 그리기

        GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_Tower_R(new Vector2(0.5f + start_red_building, 0.5f + start_red_building)); //처음 레드 빌딩 생성
        GameObject red_mark = Instantiate(main_mark, new Vector3(0.5f + start_red_building, 0.5f + start_red_building,-4), Quaternion.identity);

        GameObject.Find("Resourse").GetComponent<Resourse>().update_Visible(); //시야 재 설정
        GameObject.Find("Resourse").GetComponent<Resourse>().update_Influence();//영향력

        //타일 그리기
        GameObject.Find("DrawField").GetComponent<Draw_Field>().draw_Dark(GameObject.Find("Resourse").GetComponent<Resourse>().visible_Tile, 
            GameObject.Find("Resourse").GetComponent<Resourse>().blue_Influence_Tile ,
            GameObject.Find("Resourse").GetComponent<Resourse>().red_Influence_Tile, 
            GameObject.Find("Resourse").GetComponent<Resourse>().width,
            GameObject.Find("Resourse").GetComponent<Resourse>().height);
        
    }

    private void Update()
    {
        game_End();
    }

    public void game_End()
    {
        if (GameObject.Find("Resourse").GetComponent<Resourse>().red_Unit_Tile[start_red_building, start_red_building] != 10 && gameOver_bool == false)
        {
            GameObject.Find("Ui").GetComponent<UiScript>().game_Win_Ui();
            gameOver_bool = true;
        }
        if (GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[start_blue_building, start_blue_building] != 10 && gameOver_bool == false)
        {
            GameObject.Find("Ui").GetComponent<UiScript>().game_Lose_Ui();
            gameOver_bool = true;
        }
    }

}
