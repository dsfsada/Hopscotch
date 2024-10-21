using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Start_screen_ui : MonoBehaviour
{
    private Button start_Button;    //게임 시작버튼
    private Button exit_Button;     //종료 버튼

    private Button tuto_On_Button;     //방법 설명 버튼
    private Button tuto_Next_Button;//넘기기
    private Button tuto_Exit_Button;//종료

    private int tuto_number = 1;

    //게임 방법 설명
    private VisualElement tuto;
    private VisualElement tuto1;
    private VisualElement tuto2;
    private VisualElement tuto3;
    private VisualElement tuto4;
    private VisualElement tuto5;
    private VisualElement tuto6;
    private VisualElement tuto7;
    private VisualElement tuto8;
    private VisualElement tuto9;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        start_Button = root.Q<Button>("start_button"); //시작 버튼
        exit_Button = root.Q<Button>("exit_button"); //종료 버튼
        tuto_On_Button = root.Q<Button>("tuto_button"); //튜토 버튼

        tuto_Next_Button = root.Q<Button>("next");  //튜토 다음
        tuto_Exit_Button = root.Q<Button>("x");     //튜토 화면 종료

        //튜토 화면 표시
        tuto = root.Q<VisualElement>("tuto");
        tuto1 = root.Q<VisualElement>("tuto_1");
        tuto2 = root.Q<VisualElement>("tuto_2");
        tuto3 = root.Q<VisualElement>("tuto_3");
        tuto4 = root.Q<VisualElement>("tuto_4");
        tuto5 = root.Q<VisualElement>("tuto_5");
        tuto6 = root.Q<VisualElement>("tuto_6");
        tuto7 = root.Q<VisualElement>("tuto_7");
        tuto8 = root.Q<VisualElement>("tuto_8");
        tuto9 = root.Q<VisualElement>("tuto_9");

    }

    // Update is called once per frame
    void Update()
    {
        start_Button.RegisterCallback<ClickEvent>(OnStartButtonClicked); //시작 버튼 클릭
        exit_Button.RegisterCallback<ClickEvent>(OnExitButtonClicked); //시작 버튼 클릭
        tuto_On_Button.RegisterCallback<ClickEvent>(OnTutoOnButtonClicked);



        tuto_Next_Button.RegisterCallback<ClickEvent>(OnTutoNextButtonClicked); //튜토 넘기기 버튼
        tuto_Exit_Button.RegisterCallback<ClickEvent>(OnTutoExitButtonClicked); //튜토 종료버튼

        Tuto();
    }

    public void OnStartButtonClicked(ClickEvent evt)
    {
        SceneManager.LoadScene("Main_play_scene"); //Main_play_scene 신을 불러오겠다.
    }


    public void OnExitButtonClicked(ClickEvent evt)
    {
            //UnityEditor.EditorApplication.isPlaying = false;
            //Application.Quit(); //게임 종료

    }

    public void OnTutoOnButtonClicked(ClickEvent evt) //튜토 켜줌
    {
        tuto.RemoveFromClassList("tuto_hide");
    }

        public void OnTutoNextButtonClicked(ClickEvent evt) //튜토 number조절(Tuto()에섯 사용)
    {
        if(tuto_number < 9)
        {
            tuto_number += 1;
        }
    }

    public void OnTutoExitButtonClicked(ClickEvent evt) //튜토 끄는 버튼
    {
        tuto.AddToClassList("tuto_hide");
        tuto_number = 1;

        tuto1.style.display = DisplayStyle.None;
        tuto2.style.display = DisplayStyle.None;
        tuto3.style.display = DisplayStyle.None;
        tuto4.style.display = DisplayStyle.None;
        tuto5.style.display = DisplayStyle.None;
        tuto6.style.display = DisplayStyle.None;
        tuto7.style.display = DisplayStyle.None;
        tuto8.style.display = DisplayStyle.None;
        tuto9.style.display = DisplayStyle.None;
    }

    public void Tuto()//튜토리얼 번호에 따른 화면
    {
        if (tuto_number == 1)
        {
            tuto1.style.display = DisplayStyle.Flex;;
        }else if (tuto_number == 2)
        {
            tuto2.style.display = DisplayStyle.Flex;
        }
        else if (tuto_number == 3)
        {
            tuto3.style.display = DisplayStyle.Flex;
        }
        else if (tuto_number == 4)
        {
            tuto4.style.display = DisplayStyle.Flex;
        }
        else if (tuto_number == 5)
        {
            tuto5.style.display = DisplayStyle.Flex;
        }
        else if (tuto_number == 6)
        {
            tuto6.style.display = DisplayStyle.Flex;
        }
        else if (tuto_number == 7)
        {
            tuto7.style.display = DisplayStyle.Flex;
        }
        else if (tuto_number == 8)
        {
            tuto8.style.display = DisplayStyle.Flex;
        }
        else if (tuto_number == 9)
        {
            tuto9.style.display = DisplayStyle.Flex;
        }

    }
}
