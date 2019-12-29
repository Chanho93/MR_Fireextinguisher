using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//Todo: 마그네슘센서를 당겼을 때 Reset 실시
public class CardboardTriggerControlMono : MonoBehaviour
{
    public bool magnetDetectionEnabled = true;
    public Transform vrCamera;
    
    public float toggleAngle = 10.0f;
    public float RightAngle = 165.0f;
    public float LeftAngle = 10.0f;
    public float speed = 6.0f;
    public static bool moveforward;
    private CharacterController cc; //기본적으로 제공되는 character controller

    private bool StartRecord = true;
    public Fire fireScript;
    public GameObject REC;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        CardboardMagnetSensor.SetEnabled(magnetDetectionEnabled);
        // Disable screen dimming:
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Update()
    {
        if (!magnetDetectionEnabled) return;
        if (CardboardMagnetSensor.CheckIfWasClicked())
        {
            //Debug.Log("DO SOMETHING HERE");  // PERFORM ACTION.
            CardboardMagnetSensor.ResetClick();
            //moveforward = true;
            //! 카드보드 자석 버튼 VR씬에서 버튼처리시 VR타겟이미지씬으로 그 외 씬에서는 버튼처리시 메인화면으로


            //이 주석 밑으로 다음 주석까지의 구역이 마스네틱을 움직였을때의 행동을 처리함.
            if (StartRecord)
            {
                fireScript.StartRecord();
                REC.SetActive(true);
                StartRecord = false;
            }
            else
            {
                fireScript.EndRecord();
                REC.SetActive(false);
                StartRecord = true;
            }
            //여기까지

        }

        else if (!CardboardMagnetSensor.CheckIfWasClicked())
        {
            moveforward = false;
        }

        else
        {
            moveforward = false;
        }
        MoveTg();
    }


    //Todo: 카메라 움직이는 속도
    void MoveTg()
    {
        if (moveforward)
        {
            //! 속도 조절
            Vector3 forward = vrCamera.TransformDirection(Vector3.forward);
            cc.SimpleMove(forward * speed);
        }

    }
}