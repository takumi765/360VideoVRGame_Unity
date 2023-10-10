using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class videoPlayer : MonoBehaviour
{
    // インスペクター上でVideoPlayerを指定
	public VideoPlayer VideoPlayerComponent;
    GameObject SaveCsv;
    saveCsv saveCsv_Script;

    // シーンの設定
    private int i=0;
    private string[] Scenes = {"Lake","Mountain","Beach"};

    // Log用
    private int count = 0;

    void Start(){
        /**
         * SaveCsvのスクリプトを参照する
         * @see https://docs.unity3d.com/ja/current/ScriptReference/GameObject.Find.html
         */
        SaveCsv = GameObject.Find("SaveCsv");

        /**
         * コンポーネントを返す
         * @see https://docs.unity3d.com/ja/current/ScriptReference/GameObject.GetComponent.html
         */
        saveCsv_Script = SaveCsv.GetComponent<saveCsv>();
        saveCsv_Script.Scene = Scenes[0];
    }

	void Update()
	{
        bool flag = false;
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
            flag = true;
            if(i+1 >= Scenes.Length){
                i=0;
            }else{
                i++;
            }
		}else  if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
            flag = true;
            if(i-1 < 0){
                i=Scenes.Length-1;
            }else{
                i--;
            }
		}

        if(flag){
            // エクセルに出力する内容を格納する
            saveCsv_Script.Scene = Scenes[i];
            saveCsv_Script.nowTime = DateTime.Now;
            saveCsv_Script.SaveData();
            VideoPlayerComponent.clip = Resources.Load<VideoClip>($"360Video/{Scenes[i]}");
            flag = false;
        }

        // Log用
        count++;
        if(count == 50){
            // エクセルに出力する内容を格納する
            saveCsv_Script.Scene = Scenes[i];
            saveCsv_Script.nowTime = DateTime.Now;
            saveCsv_Script.SaveData();
            count=0;
        }
	}
}