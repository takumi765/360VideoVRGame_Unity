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
    private string[] Scenes = {"Lake","Mountain","Beach"};

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
            if(i+1 >= Scenes.Length){
                i=0;
            }else{
                i++;
            }
		}else  if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
            if(i-1 < 0){
                i=Scenes.Length-1;
            }else{
                i--;
            }
		}
        VideoPlayerComponent.clip = Resources.Load<VideoClip>($"360Video/{Scenes[i]}");
	}
}