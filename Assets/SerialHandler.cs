using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System;


public class SerialHandler : MonoBehaviour
{
    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;


    //ポート名
    public string portName = "COM8";
    public int baudRate    = 115200;
    // シリアルポート
    private SerialPort serialPort_;
    private Thread thread_, thread2_;
    private bool isRunning_ = false;
    // スレッド監視
    public int serialStatus = 0;  // 0 : waiting, 1 : success, 2 : failure
    private bool thread2_status = false;
    // センサ値格納
    public double roll=0.0, pitch=0.0, yaw=0.0;
    // rawデータから各値抽出
    private bool IsGyrodata = false;
    private bool gyroFlag = false;
    private int countGyrodata = 0;
    
    void Start()
    {
        Debug.Log("Start");
        StartCoroutine("Open");
    }

    void Update()
    {
        if(serialStatus == 1){
            if (Input.GetKey(KeyCode.Space)){
                Close();
                StartCoroutine("Open");
            }
        }
    }

    void OnDestroy()
    {
        Close();
    }

    void SerialOpen(){
        thread2_status = true;
        serialStatus = 0;
        serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        try{
            serialPort_.Open();
        }
        catch (Exception e){
            serialStatus = 2;
        }
        if (serialStatus == 0) serialStatus = 1;
        thread2_status = false;
    }

    private IEnumerator Open()
    {
        serialStatus = 2;
        while (serialStatus != 1){
                Debug.Log("センサ接続トライ中");
            if (serialStatus == 2){
                thread2_ = new Thread(SerialOpen);
                thread2_.Start();
                yield return null;
            }
            while (thread2_status) { yield return null; }
            thread2_.Join();
            yield return null;
        }
        serialPort_.ReadTimeout = 1; // 50
        isRunning_ = true;

        thread_ = new Thread(Read);
        thread_.Start();

        // オフセット角も更新する
        rotate Rotate;
        GameObject obj = GameObject.Find("Main Camera");
        Rotate = obj.GetComponent<rotate>();
        Rotate.offsetFlag = true;

        // check
        Debug.Log("OPEN!");
    }

    private void Close()
    {
        isRunning_ = false;

        if (thread_ != null && thread_.IsAlive) {
            thread_.Join();
        }

        if (serialPort_ != null && serialPort_.IsOpen) {
            serialPort_.Close();
            serialPort_.Dispose();
        }
        Debug.Log("CLOSE!");
    }

    private void Read()
    {
        int rcv = 0;
        int pastrcv = 0;
        int plus       = 0;
        int minus      = 0;
        double preRoll = 0;
        double preYaw  = 0;
        double prePitch= 0;

        // COMPort確認
        // string[] ports = SerialPort.GetPortNames();
        // foreach(string port in ports){
        //     // Debug.Log(port);
        // }

        while (isRunning_ && serialPort_ != null && serialPort_.IsOpen) {
            try {
                rcv = serialPort_.ReadByte();
                // Debug.Log(rcv);
            } catch (System.Exception e) {
                Debug.LogWarning(e.Message);
            }

            // 生データからroll,pitch,yawを計算する
            if(IsGyrodata){
                countGyrodata++;
                switch(countGyrodata){
                    case 1: 
                        this.roll = rcv; 
                        break;
                    case 2: 
                        this.roll += rcv*Math.Pow(2,8); 
                        this.roll = this.roll/32768.0*180;
                        break;
                    case 3:
                        this.pitch = rcv;
                        break;
                    case 4:
                        this.pitch += rcv*Math.Pow(2,8);
                        this.pitch = this.pitch/32768.0*180;
                        break;
                    case 5:
                        this.yaw = rcv;
                        break;
                    case 6:
                        this.yaw += rcv*Math.Pow(2,8);
                        this.yaw = this.yaw/32768.0*180;
                        break;
                    case 7: 
                        gyroFlag = true;
                        IsGyrodata = false;
                        break;
                }

                // roll,pitch,yawを計算し終えたら各値を加工する
                if(gyroFlag){
                    gyroFlag = false;
                    countGyrodata = 0;
                    // センサ値を[-180,180]に変換する
                    if(this.roll > 180){
                        this.roll -= 360;
                    }
                    // センサ値を[0→90→0],[0→-90→0]に変換する
                    if (this.pitch > 180) {
                        this.pitch -= 360;
                    }
                    // センサ値を[-180,180]に変換する
                    // if (yaw > 180) {
                    //     yaw -= 360;
                    // }

                    if(preRoll*this.roll <= 0){
                        if(Mathf.Abs((float)this.roll) > 150){
                            if(preRoll < 0){
                                minus++;
                            }else if(preRoll > 0){
                                plus++;
                            }
                        }
                        preRoll=this.roll;
                    }
                    this.roll += (plus-minus)*360;

                    prePitch = this.pitch;
                    preYaw   = this.yaw;

                    // Check
                    // Debug.Log($"roll:{roll}, pitch:{pitch}, yaw:{yaw}");
                }
            }else{
                // 取り出したい値がある位置を探す
                if(rcv==0x53 && pastrcv==0x55){
                    IsGyrodata = true;
                    // Debug.Log("Gyrodata is comming!");
                }
                pastrcv = rcv;
            }

            
        }
    }

    public void Write(string message)
    {
        try {
            serialPort_.Write(message);
        } catch (System.Exception e) {
            Debug.LogWarning(e.Message);
        }
    }
}


// PAST CODE
// byte[] buffer = new byte[100];
// serialPort_.Read(buffer, 0, buffer.Length);

// for(int i=0;i<buffer.Length-7;i++){
//     if(buffer[i]==0x55 && buffer[i+1]==0x53){
//         int start=i+2;
//         this.roll  = (buffer[start+1]*Math.Pow(2,8)+buffer[start+0])/32768.0*180;
//         this.pitch = (buffer[start+3]*Math.Pow(2,8)+buffer[start+2])/32768.0*180;
//         this.yaw   = (buffer[start+5]*Math.Pow(2,8)+buffer[start+4])/32768.0*180;

//         // センサ値を[-180,180]に変換する
//         if(this.roll > 180){
//             this.roll -= 360;
//         }
//         // センサ値を[0→90→0],[0→-90→0]に変換する
//         if (this.pitch > 180) {
//             this.pitch -= 360;
//         }
//         // センサ値を[-180,180]に変換する
//         // if (yaw > 180) {
//         //     yaw -= 360;
//         // }

//         if(preRoll*this.roll <= 0){
//             if(Mathf.Abs((float)this.roll) > 150){
//                 if(preRoll < 0){
//                     minus++;
//                 }else if(preRoll > 0){
//                     plus++;
//                 }
//             }
//             preRoll=this.roll;
//         }
//         this.roll += (plus-minus)*360;

//         prePitch = this.pitch;
//         preYaw   = this.yaw;

//         // Check
//         Debug.Log($"roll:{roll}, pitch:{pitch}, yaw:{yaw}");
//     }
// }