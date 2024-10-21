using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Tilemaps;
using UnityEngine;

public class Draw_Field : MonoBehaviour
{
    // 타일을 그리는 것과 관련된 함수와 변수를 저장함

    public GameObject empty_Field; // 빈 땅  # 1
    public GameObject food_Field; // 식량 땅 # 2
    public GameObject wood_Field; // 나무 땅 # 3
    public GameObject dark_Field; //검은 시야 땅
                                  //
    public GameObject blue_Influence; // 파랑색 영향력 타일
    public GameObject red_Influence; // 빨강색 영향력 타일
 

    public void first_Field(int[,] data, int x, int y) //data : 내 시야 정보 , blue : 파랑팀 건물 영향력, red : 빨강팀 건물 영향력
    {
        // 작동 확인용
        for(int i = 0; i < x; i++)
            for(int j =0;  j < y; j++)
            {
                if (data[i, j] == 1)
                {
                    GameObject temp = Instantiate(empty_Field, new Vector3(i * 1f + 0.5f, j * 1f + 0.5f, 0), Quaternion.identity);
                    temp.transform.SetParent(this.transform);
                }
                else if (data[i, j] == 2)
                {
                    GameObject temp = Instantiate(food_Field, new Vector3(i * 1f + 0.5f, j * 1f + 0.5f, 0), Quaternion.identity);
                    temp.transform.SetParent(this.transform);
                }
                else if (data[i, j] == 3)
                {
                    GameObject temp = Instantiate(wood_Field, new Vector3(i * 1f + 0.5f, j * 1f + 0.5f, 0), Quaternion.identity);
                    temp.transform.SetParent(this.transform);
                }
            }
    }

    //시야 그리는 타일
    public void draw_Dark(int[,] data, int[,] blue, int[,] red,  int x,int y) 
    {

        /*foreach(GameObject tile in GameObject.Find("Resourse").GetComponent<Resourse>().visible)
        {
            Destroy(tile);
        }*/
        for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
            {
                if (data[i, j] == 0)
                {
                    GameObject dark = Instantiate(dark_Field, new Vector3(i * 1f + 0.5f, j * 1f + 0.5f, -3), Quaternion.identity);
                    dark.transform.SetParent(this.transform);
                    GameObject.Find("Resourse").GetComponent<Resourse>().visible.Add(dark);
                }
                else
                {
                    if (blue[i, j] == 1&& red[i, j] ==0)
                    {
                        GameObject blue_In = Instantiate(blue_Influence, new Vector3(i * 1f + 0.5f, j * 1f + 0.5f, -1.5f), Quaternion.identity);
                        blue_In.transform.SetParent(this.transform);
                        GameObject.Find("Resourse").GetComponent<Resourse>().visible.Add(blue_In);
                    }
                    if (red[i, j] == 1&& blue[i,j] == 0)
                    {
                        GameObject red_In = Instantiate(red_Influence, new Vector3(i * 1f + 0.5f, j * 1f + 0.5f, -1.5f), Quaternion.identity);
                        red_In.transform.SetParent(this.transform);
                        GameObject.Find("Resourse").GetComponent<Resourse>().visible.Add(red_In);
                    }


                }
                
            }
    }
}
