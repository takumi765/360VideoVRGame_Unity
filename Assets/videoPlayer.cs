using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class videoPlayer : MonoBehaviour
{
    // インスペクター上でVideoPlayerを指定
	public VideoPlayer VideoPlayerComponent;

    // シーンの設定
    private int i=0;
    private string[] Scenes = {"Lake.mp4","Mountain.mp4","Beach.mp4"};

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
            if(i+1 >= Scenes.Length){
                i=0;
            }else{
                i++;
            }
			VideoPlayerComponent.url = $"Assets/Scenes/360Video/{Scenes[i]}";
		}else  if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
            if(i-1 < 0){
                i=Scenes.Length-1;
            }else{
                i--;
            }
			VideoPlayerComponent.url = $"Assets/Scenes/360Video/{Scenes[i]}";
		}
	}
}