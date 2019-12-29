using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TechTweaking.Bluetooth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TerminalController : MonoBehaviour
{
	public Text devicNameText;
	public Text status;
	public ScrollTerminalUI readDataText;                   //ScrollTerminalUI is a script used to control the ScrollView text
	
	public GameObject InfoCanvas;
	public GameObject DataCanvas;
    public GameObject GOJ;
	private  BluetoothDevice device;
	public Text dataToSend;
    
    public static int cnt_Mode = 0;

    int num = 0;
    string[] data = new string[10];
    string[] lines = new string[10];

    string[] stringPin1 = new string[2];                   //buttonPin값
    string[] stringPin2 = new string[2];                   //safabutton값

    string[] stringAccel = new string[5];                  //accel x, y, z 값
    string[] stringGyro = new string[5];                   //gyro x, y, z 값

    int[] intSafePin = new int[1];                         //소화기 핀 값           Int                  
    int[] intSafeButton = new int[1];                      //소화기 버튼 값         Int

    int[] intAccel = new int[5];
    int[] intGyro = new int[5];

    int status_Plus = 0;

    public GameObject[] FirePin;

    public int anim_status = 0;

    void Awake ()
	{
        //Todo: 블루투스
        //static void BluetoothAdapter.askEnableBluetooth() 블루투스를 사용하도록 사용자에게 요청한다.
        BluetoothAdapter.askEnableBluetooth ();//Ask user to enable Bluetooth
        //static Action<BluetoothDevice> OnDeviceOFF  = BluetoothDevice 인스턴스를 찾았지만 연결할 수 없을 때 발생하고 해당 참조값을 전달한다.
        BluetoothAdapter.OnDeviceOFF += HandleOnDeviceOff;
        //static Action<BluetoothDevice> OnDevicePicked = 기본 Android Bluetooth 장치 목록에서 사용자가 선택한 장치에서 발생한다.
		BluetoothAdapter.OnDevicePicked += HandleOnDevicePicked; //To get what device the user picked out of the devices list\
        cnt_Mode = 0;
    }

    void Start()
    {
        

        StartCoroutine(LoadDevice("None", true));

        PlayFire(anim_status);
    }

    void Update()
    {
        //transform.Rotate(0, FireAimController.Instance.Gz * -0.003f * Time.deltaTime, FireAimController.Instance.Gx * -0.003f * Time.deltaTime);
        Split_Data();
        Angle_xyz();

        ButtonPinAnimation();

        if(intSafeButton[0] == 0)
        {
            FirePin[1].gameObject.SetActive(true);
        }

        else if(intSafeButton[0] == 1)
        {
            FirePin[1].gameObject.SetActive(false);
        }
    }
    
    //Todo: 블루투스 기기에 연결 할 수 없는지 확인하는 함수
    void HandleOnDeviceOff (BluetoothDevice device)
	{
        // string,IsNullOrEmpty(string value) 지정된 문자열이 null이거나 빈 문자열인지 나타내는 함수
		if (!string.IsNullOrEmpty (device.Name))
			status.text = "Couldn't connect to " + device.Name + ", device might be OFF";
		else if (!string.IsNullOrEmpty (device.MacAddress)) {
			status.text = "Couldn't connect to " + device.MacAddress + ", device might be OFF";
		} 
	}

	void HandleOnDevicePicked (BluetoothDevice device)//Called when device is Picked by user
	{
		this.device = device;//save a global reference to the device
        device.setEndByte(10);  // 다바이스의 데이터를 패킷으로 패킷화하면 각 패킷은 바이트로 끝나게 한다.
        // Func<BluetoothDevice,IEnumerator> ReadingCorotine = Null이 아니면 참조된 IEnumerator가 읽기가 시작된 직후에 시작된다
        device.ReadingCoroutine = ManageConnection;
        devicNameText.text = "Remote Device : " + device.Name;
	}

	
	public void showDevices ()
	{
        // 기본 안드로이드 블루투스 장비 목록을 표시한다.
		BluetoothAdapter.showDevices ();//show a list of all devices//any picked device will be sent to this.HandleOnDevicePicked()
	}
	
    //Todo: 블루투스 디바이스 기기 연결
	public void connect ()//Connect to the public global variable "device" if it's not null.
	{
		if (device != null) {
			device.connect ();
			status.text = "Trying to connect...";
		}
	}
	
    //Todo: 블루투스 디바이스 연결 해제
	public void disconnect ()//Disconnect the public global variable "device" if it's not null.
	{
		if (device != null)
			device.close ();
	}

    //Todo: 데이터를 ASCII Byte로 패킷 
	public void send ()
	{		
		if (device != null && !string.IsNullOrEmpty (dataToSend.text)) {
			device.send (System.Text.Encoding.ASCII.GetBytes (dataToSend.text + (char)10));//10 is our seperator Byte (sepration between packets)
		}
	}
    

    IEnumerator ManageConnection(BluetoothDevice device)            //Bluetooth에서 보내는 데이터를 받고 형 변환을 해주는 코루틴
    {//Manage Reading Coroutine
        
        //Switch to Terminal View
        InfoCanvas.SetActive(false);
        DataCanvas.SetActive(true);
        
        while (device.IsReading)
        {
            cnt_Mode++;
            XR_Setting2.status = 1;
            
            if (device.IsDataAvailable)
            {
                
                byte[] msg = device.read();

                if (msg != null)
                {
                    string content = System.Text.ASCIIEncoding.ASCII.GetString(msg);
                    lines = content.Split(new char[] { '\n' });
                    data[num] = lines[0];
                    readDataText.add(device.Name, stringPin1[1]);
                    readDataText.add(device.Name, stringPin2[1]);
                    num++;
                    if (num == 4)
                    {
                        num = 0;
                    }
                    //readDataText.add(device.Name, data[0]);
                }
            }
            yield return null;
        }

        //Switch to Menue View after reading stoped
        DataCanvas.SetActive(false);
        InfoCanvas.SetActive(true);

    }
    

    void OnDestroy ()
	{
		BluetoothAdapter.OnDevicePicked -= HandleOnDevicePicked; 
		BluetoothAdapter.OnDeviceOFF -= HandleOnDeviceOff;
	}

    void Split_Data()                                               //형변환을 해주는 함수
    {
        stringPin1 = data[1].Split('=');          //소화기 핀 값
        stringPin2 = data[0].Split('=');          //소화기 버튼 값

        stringAccel = data[2].Split(',');         //가속도 x,y,z값
        stringGyro = data[3].Split(',');          //기울기 x,y,z값

        intAccel[0] = Int32.Parse(stringAccel[1]);
        intAccel[1] = Int32.Parse(stringAccel[2]);
        intAccel[2] = Int32.Parse(stringAccel[3]);

        intGyro[0] = Int32.Parse(stringGyro[1]);
        intGyro[1] = Int32.Parse(stringGyro[2]);
        intGyro[2] = Int32.Parse(stringGyro[3]);

        intSafePin[0] = Int32.Parse(stringPin1[1]);
        intSafeButton[0] = Int32.Parse(stringPin2[1]);

        Fire_Controller.Instance.Gx = intGyro[0];
        Fire_Controller.Instance.Gy = intGyro[1];
        Fire_Controller.Instance.Gz = intGyro[2];
    }

    void ButtonPinAnimation()                                       //소화기의 애니메이션을 처리하는 함수
    {
        if (intSafeButton[0] == 1)
        {
            status_Plus++;
            if (status_Plus == 1)
            {
                StartCoroutine(FireButtonPin());
                anim_status = 2;
            }
        }

        if (intSafeButton[0] == 0)
        {
            Fire_Controller.Instance.intFireButton++;
            if (Fire_Controller.Instance.intFireButton == 1)
            {
                StartCoroutine(PushButton());
                anim_status = 3;
                Fire_Controller.Instance.intFireButtonNot = 0;
            }
        }

        if (intSafeButton[0] == 1)
        {
            Fire_Controller.Instance.intFireButtonNot++;
            if (Fire_Controller.Instance.intFireButtonNot == 1)
            {
                StartCoroutine(NotPushButton());
                anim_status = 2;
                Fire_Controller.Instance.intFireButton = 0;
            }
        }
    }

    public void PlayFire(int anim_status)
    {
        switch (anim_status)
        {
            case 0:
                break;
            case 1:
                StartCoroutine(FireButtonPin());
                break;
            case 2:
                StartCoroutine(PushButton());
                break;
            case 3:
                StartCoroutine(NotPushButton());
                break;
            case 4:
                break;
        }
    }

    IEnumerator FireButtonPin()
    {
        FirePin[0].transform.Translate(new Vector3(0, 0, 0.3f));
        Invoke("Pin_False", 4);
        FirePin[1].gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator PushButton()
    {
        Fire_Controller.Instance.fireButton = true;
        FirePin[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator NotPushButton()
    {
        Fire_Controller.Instance.fireButton = false;
        FirePin[1].gameObject.SetActive(false);
        anim_status = 2;
        yield return new WaitForSeconds(0.1f);
    }

    void Pin_False()
    {
        FirePin[0].gameObject.SetActive(false);
    }

    void Angle_xyz()                                            //Bluetooth 소화기 컨트롤러의 가속도 값을 통하여 각도값을 계산해주는 함수
    {
        Fire_Controller.Instance.pitch = Mathf.Atan(intAccel[0] / Mathf.Sqrt(intAccel[1] * intAccel[1] + intAccel[2] * intAccel[2])) * 180 / Mathf.PI;         
        Fire_Controller.Instance.roll = Mathf.Atan(intAccel[1] / Mathf.Sqrt(intAccel[0] * intAccel[0] + intAccel[2] * intAccel[2])) * 180 / Mathf.PI;         //상하 상 = -값, 하 = +값
    }

    IEnumerator LoadDevice(string newDevice, bool enable)
    {
        UnityEngine.XR.XRSettings.LoadDeviceByName(newDevice);
        yield return null;
        UnityEngine.XR.XRSettings.enabled = enable;
    }

}
    