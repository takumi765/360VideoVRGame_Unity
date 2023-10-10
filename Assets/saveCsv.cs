using System;
using System.IO;
using System.Text;
using UnityEngine;

// csvに保存するためのコード
// SaveCsvへアタッチ
public class saveCsv : MonoBehaviour
{
    // System.IO
    private StreamWriter sw;
    
    public string Scene;
    public DateTime startTime;
    public DateTime nowTime;
    public float horAngle = 0.0f;
    public float verAngle = 0.0f;
    public float horAngleCon = 0.0f; 
    public float verAngleCon = 0.0f; 

    // Start is called before the first frame update
    void Start()
    {
        // スタート時間
        this.nowTime = DateTime.Now;
        this.startTime = DateTime.Now;
        
        // ファイル名
        string logFilePass = "logFile";
        //ファイルパスを構築
        string logPass = logFilePass + "/" + $"{this.nowTime.Month.ToString()}月{this.nowTime.Day.ToString()}日{this.nowTime.Hour.ToString()}時{this.nowTime.Minute.ToString()}分.csv";
        //ディレクトリがあるか確認
        if (!System.IO.Directory.Exists(logFilePass)){
            //ディレクトリ作成
            System.IO.Directory.CreateDirectory(logFilePass);
        }
        
        // 新しくcsvファイルを作成して、{}の中の要素分csvに追記をする
        this.sw = new StreamWriter(
            logPass,
            false,
            Encoding.GetEncoding("UTF-8")
        );

        // CSV1行目のカラムで、StreamWriter オブジェクトへ書き込む
        string[] s1 = { "Scene", "Time", "Rotate_Hor", "Rotate_Ver", "ConRotate_Hor", "ConRotate_Ver" }; 

        // s1の文字列配列のすべての要素を「,」で連結する
        string s2 = string.Join(",", s1);

        // s2文字列をcsvファイルへ書き込む
        this.sw.WriteLine(s2);
    }

    // Update is called once per frame
    void Update()
    {
        // Enterキーが押されたらcsvへの書き込みを終了する
        if (Input.GetKeyDown(KeyCode.Return))
        {
            /**
             * @see https://docs.microsoft.com/ja-jp/dotnet/api/system.io.streamwriter.close?view=net-6.0#System_IO_StreamWriter_Close
             */
            this.sw.Close();
        }
    }

    public void SaveData()
    {
        TimeSpan elapsedTime = this.nowTime - this.startTime;
        string[] s1 = { 
            this.Scene, 
            elapsedTime.ToString(),
            this.horAngle.ToString(),
            this.verAngle.ToString(),
            this.horAngleCon.ToString(),
            this.verAngleCon.ToString()
        };
        string s2 = string.Join(",", s1);
        this.sw.WriteLine(s2);
        this.sw.Flush();
    }
}