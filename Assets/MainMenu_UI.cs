using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    public GameObject Ari_UI;
    public GameObject Speech_UI;
    public Text Shark_Text;
    public GameObject Exit_UI;
    private Animator anim_Ari;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartUI());
        anim_Ari = Ari_UI.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit_UI.SetActive(true);
        }
    }
    public void ExitUI_Yes()
    {
        Application.Quit();
    }

    public void ExitUI_NO(){
        Exit_UI.SetActive(false);
    }

    public void AriButton()
    {
        StartCoroutine(MoveScene());
    }

    IEnumerator MoveScene() //버튼 클릭시 작동(아리 캐릭터)
    {
        Ari_UI.GetComponent<AudioSource>().Play();
        anim_Ari.SetBool("Click", true);
        Speech_UI.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(LoadDevice("Vuforia", true)); //메인메뉴에서 화제진압씬으로 넘어갈 때 Vuforia가 활성화되면서 VR모드로 바뀌면서 씬 전환됨.
    }

    IEnumerator StartUI() //처음 실행시 작동
    {
        Speech_UI.SetActive(false);
        yield return new WaitForSeconds(0.8f);
        Speech_UI.SetActive(true);
        Shark_Text.color = Color.yellow;
        Shark_Text.text = "메뉴를 꾸욱~눌러 주세요!";
    }

    IEnumerator LoadDevice(string newDevice, bool enable)
    {
        //! 디바이스 불러오기
        UnityEngine.XR.XRSettings.LoadDeviceByName(newDevice);
        yield return null;
        UnityEngine.XR.XRSettings.enabled = enable;
        SceneManager.LoadScene("SceneManager"); //VR모드로 설정하고 씬전환
    }
}
