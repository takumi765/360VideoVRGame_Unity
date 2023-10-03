using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public SerialHandler serialHandler;

    void Start(){
        // 
    }

    // オフセット変数
    private int offsetCount = 0;
    public bool offsetFlag = true;
    private double rollOffset = 0;
    private double pitchOffset = 0;
    // ローパスフィルタ変数
    private double filterGain = 0.75;
    private double preRoll = 0;
    private double prePitch = 0;

    void Update(){
        if(serialHandler.serialStatus == 1){
            // transformを取得
            Transform myTransform = this.transform;

            double roll, pitch;
            // オフセット処理
            if(offsetFlag){
                if(offsetCount >= 7){// 5回目以降の値を取る※適当，少し時間がたってからの方が値が安定していそうだから
                    Debug.Log("Setting Offset");
                    // rollOffset = serialHandler.roll;
                    rollOffset = serialHandler.yaw;
                    pitchOffset = serialHandler.pitch;
                    offsetFlag = !offsetFlag;
                    offsetCount = 0;
                    // Debug.Log($"offset roll:{rollOffset}, pitch:{pitchOffset}");
                }
                offsetCount++;
            }
            // roll = serialHandler.roll - rollOffset;
            roll = serialHandler.yaw - rollOffset;
            pitch = serialHandler.pitch - pitchOffset;

            // ローパスフィルタ処理
            roll = preRoll * filterGain + roll * (1 - filterGain);
            pitch = prePitch * filterGain + pitch * (1 - filterGain);
            preRoll = roll;
            prePitch = pitch;
            // Debug.Log($"roll:{roll}, pitch:{pitch}");

            // ワールド座標を基準に、回転を取得
            Vector3 worldAngle = myTransform.eulerAngles;
            // worldAngle.y = (float)roll;
            worldAngle.y = -(float)roll;
            worldAngle.x = (float)pitch;
            myTransform.eulerAngles = worldAngle;

            // キーボード入力
            if(Input.GetKey(KeyCode.A)){ // 右
                rollOffset += 0.5f;
            }
            if(Input.GetKey(KeyCode.D)){ // 左
                rollOffset -= 0.5f;
            }
            if(Input.GetKey(KeyCode.W)){ // 上
                pitchOffset += 0.5f;
            }
            if(Input.GetKey(KeyCode.S)){ // 下
                pitchOffset -= 0.5f;
            }
        }
    }
}