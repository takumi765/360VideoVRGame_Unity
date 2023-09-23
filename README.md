# 360VideoVRGame_Unity
## 使用するもの
- ジャイロセンサ(WitMotion 社,BWT901CL)
- 360°動画※ファイル容量が大きかったためpushしていません．ご自身で設定ください
## 使用方法
1. オブジェクトの作成と階層構造

Create Empty　→　「VReye」「GameController」「SerialHandler」

Create→Video→VideoPlayer　→　「360Video」

![image](https://github.com/takumi765/360VideoVRGame_Unity/assets/82143606/527fbfb2-26a5-430b-b8df-35643573fe84)

3. SerialHandlerの設定

Assetsの中のSerialHandler.csをアタッチする

4. MainCameraの設定

「VReye」の下の階層にドラッグアンドドロップで持ってくる

Assets内のrotate.csをアタッチし，Serial Handler項目にSerialHandlerを設定する

5. GameControllerの設定

Assetsの中のGameController.csをアタッチする

5. 360Videoの設定

360°動画を読み込み，VideoClip項目からその動画を選択する．

## 注意
試行錯誤した際にインストールしたものも入ってしまっています．（SkySeries等）
面倒くさかったので消す"予定"．
