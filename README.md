# 360VideoVRGame_Unity
## 使用するもの
- ジャイロセンサ(WitMotion 社,BWT901CL)
- 360°動画※ファイル容量が大きかったためpushしていません．ご自身で設定ください
## 使用方法
1. オブジェクトの作成と階層構造
    1. Create Empty　→　「VReye」「GameController」「SerialHandler」

![image](https://github.com/takumi765/360VideoVRGame_Unity/assets/82143606/527fbfb2-26a5-430b-b8df-35643573fe84)

2. SerialHandlerの設定
    1. Assetsの中のSerialHandler.csをアタッチする

3. MainCameraの設定
    1. 「VReye」の下の階層にドラッグアンドドロップで持ってくる
    1. Assets内のrotate.csをアタッチし，Serial Handler項目にSerialHandlerを設定する

4. GameControllerの設定
    1. Assetsの中のGameController.csをアタッチする

5. 360Videoの設定
    1. 360°動画を読み込み，Hielarchy内にドラッグアンドドロップ（画像中の360Videoのこと）
    1.Render ModeをRenderTextureに変更し，Target Texture項目にNew Render Textureを選択する
    1. unityエディタ上部の「window」→「Rendering」→「Lighting」→「Environment」→「Skybox Material」をデフォルトではなく，今製作したものに変更する

6. 動かしてみましょう
