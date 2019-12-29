using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class XR_Setting2 : MonoBehaviour
{
    public GameObject Back_Button;		//! 뒤로가기 파일
    public static int XR_Num = 0;
    public static int status = 0;

    //Todo: START
    void Start()
    {
        status = 0;
        //!? SceneManager 에 따라 Cardboard 환경 설정
        if (SceneManager.GetActiveScene().name == "MainMenuUI")
        {

            //! 디바이스 불러오기
            //StartCoroutine(LoadDevice("None", true));
            // StartCoroutine(LoadDevice("cardboard", true));

            //! Debug
          //  ConsoleProDebug.Watch("XR_Setting2", "Main_Menu");

            //! 기종 번호
            XR_Num = 0;
        }
        if (SceneManager.GetActiveScene().name == "SceneManager")
        {
            //StartCoroutine(LoadDevice("cardboard", true));
            XR_Num = 1;
        }
        if (SceneManager.GetActiveScene().name == "School_Cpr")
        {
            //   StartCoroutine(LoadDevice("cardboard", true));
            XR_Num = 1;
        }
        if (SceneManager.GetActiveScene().name == "MR_FireExting")
        {
            //   StartCoroutine(LoadDevice("cardboard", true));
            XR_Num = 1;
        }
        if (SceneManager.GetActiveScene().name == "Ship_Test")
        {
            //   StartCoroutine(LoadDevice("cardboard", true));
            XR_Num = 1;
        }
        if (SceneManager.GetActiveScene().name == "School_EarthQuake")
        {
            //  StartCoroutine(LoadDevice("cardboard", true));
            XR_Num = 1;
        }
        if (SceneManager.GetActiveScene().name == "Road_Scene")
        {
            //   StartCoroutine(LoadDevice("cardboard", true));
            XR_Num = 1;
        }
        if (SceneManager.GetActiveScene().name == "School_Fire")
        {
            //  StartCoroutine(LoadDevice("cardboard", true));
            XR_Num = 1;
        }
        if (SceneManager.GetActiveScene().name == "VRScene_Change")
        {
            //  if (XR_Num == 0)
            //     StartCoroutine(LoadDevice("None", true));

            //   else

            StartCoroutine(LoadDevice("cardboard", true));
        }
        if (SceneManager.GetActiveScene().name == "VRVideo" || SceneManager.GetActiveScene().name == "VRVideo2" || SceneManager.GetActiveScene().name == "VRVideo3")
        {
            //StartCoroutine(LoadDevice("cardboard", true));
            XR_Num = 1;
        }

        if (SceneManager.GetActiveScene().name == "BlueTooth_Test")
        {
            //    StartCoroutine(LoadDevice("cardboard", true));
            XR_Num = 1;
        }
        if (SceneManager.GetActiveScene().name == "BlueTooth_AR_01")
        {
            XR_Num = 1;
        }

        if (SceneManager.GetActiveScene().name == "Nose_Test")
        {
            StartCoroutine(LoadDevice("cardboard", true));
            XR_Num = 1;
        }

        if (SceneManager.GetActiveScene().name == "Fire_BlueTooth")
        {
            //    XR_Num = 0;
            //    switch(status)
            //    {
            //     case 0:
            //         StartCoroutine(LoadDevice("None", true));
            //       XR_Num = 0;
            //          break;
            //       case 1:
            StartCoroutine(LoadDevice("cardboard", true));
            //        XR_Num = 1;
            //        break;
            //    case 2:
            //       break;
            //  }

        }

        if (SceneManager.GetActiveScene().name == "VRARTEST")
        {
            XR_Num = 0;
            switch (status)
            {
                case 0:
                    StartCoroutine(LoadDevice("None", true));
                    XR_Num = 0;
                    break;
                case 1:
                    StartCoroutine(LoadDevice("cardboard", true));
                    XR_Num = 1;
                    break;
                case 2:
                    break;
            }

        }

        //Todo: 엔딩 씬 XRSetting
        if (SceneManager.GetActiveScene().name == "GoodJobSceen_Violence" ||
            SceneManager.GetActiveScene().name == "GoodJobScreen_AED" ||
            SceneManager.GetActiveScene().name == "GoodJobScreen_BusStop" ||
            SceneManager.GetActiveScene().name == "GoodJobScreen_Fire" ||
            SceneManager.GetActiveScene().name == "GoodJobScreen_Road" ||
            SceneManager.GetActiveScene().name == "DeadEnding_Road" ||
            SceneManager.GetActiveScene().name == "CommingSoon_29" ||
            SceneManager.GetActiveScene().name == "School_Fire_DeadEnding_31"
            )
        {
            //    StartCoroutine(LoadDevice("cardboard", true));
            StartCoroutine(LoadDevice("None", true));
            XR_Num = 0;
        }


    }


    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape)) { Back_Button.SetActive(true); }
#endif
        //TODO: 안드로이드 플랫폼으로 사용중일 때
        if (Application.platform == RuntimePlatform.Android)
        {
            //TODO: 안드로이드 폰에는 뒤로가기 장치가 있어서 뒤로 보낼 수 있음.
            if (Input.GetKey(KeyCode.Escape))
            {
                //TODO: 활성화된 Scene번호 
                //? 8,5,7,12
                //? 14,15,19 ???14
                //? 21,20,26
                if (SceneManager.GetActiveScene().name == "SceneManager")
                {
                    //? Scenes/VRScene_Change (5) Move
                    //  Load_Scene.number = 5;
                    StartCoroutine(LoadDevice("None", true));
                    SceneManager.LoadScene("MainMenuUI");
                }

                //TODO: Scene 번호
                //? 6
                //Todo: 엔딩 번호

                //TODO: Scene 번호
                //? 0
                

            }

            if (TerminalController.cnt_Mode == 1)
            {
                StartCoroutine(LoadDevice("cardboard", true));
                TerminalController.cnt_Mode = 2;
            }

            else if (TerminalController.cnt_Mode >= 2)
            {
                XR_Num = 1;
            }
        }
    }

    //Todo: 디바이스 불러오기 ( None : 일반모드,  cardboard : VR 모드 )
    IEnumerator LoadDevice(string newDevice, bool enable)
    {
        //! 사운드 정지
      //  SoundManager._instance.StopAllSE();
       // SoundManager._instance.StopBGM();

        //! 디바이스 불러오기
        UnityEngine.XR.XRSettings.LoadDeviceByName(newDevice);
        yield return null;
        UnityEngine.XR.XRSettings.enabled = enable;
    }
}
