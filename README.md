ACT.TTSYukkuri
============
[![Downloads](https://img.shields.io/github/downloads/anoyetta/ACT.TTSYukkuri/total.svg)](https://github.com/anoyetta/ACT.TTSYukkuri/releases)

# 移行しました
ACT.SpecialSpellTimer, ACT.TTSYukkuri, ACT.UltraScouter は配布を統合し下記のプロジェクトに移行しました。  
**ACT.Hojoring**  
**https://github.com/anoyetta/ACT.Hojoring**  
こちらから最新版を取得してください。

概要
-------------
ACTのTTSをゆっくりなどの他の合成音声エンジンに置き換えます
  
    
使い方
--------------
1. 準備  
**[.NET Framework 4.7](https://www.microsoft.com/en-us/download/details.aspx?id=55170)** をインストールします。  
※TTSYukkuri の動作には. NET Framework 4.7 以降が必要です。  

2. インストール  
<pre>
<b>Step 1.</b>
  OpenJTalk
  Yukkuri
  FFXIV.Framework.TTS.Server.NLog.config
  ffmpeg.exe
  FFXIV.Framework.TTS.Server.exe
  ACT.TTSYukkuri.Core.dll
  ACT.TTSYukkuri.dll
  DSharpPlus.dll
  DSharpPlus.VoiceNext.dll
  FFXIV.Framework.dll
  FFXIV.Framework.TTS.Common.dll
  FFXIV.Framework.TTS.dll
  FontAwesome.WPF.dll
  MahApps.Metro.dll
  NAudio.dll
  Newtonsoft.Json.dll
  NLog.dll
  Prism.dll
  Prism.Wpf.dll
  ReactiveProperty.dll
  ReactiveProperty.NET46.dll
  RucheHome.Voiceroid.dll
  RucheHomeLib.dll
  System.Reactive.Core.dll
  System.Reactive.Interfaces.dll
  System.Reactive.Linq.dll
  System.Reactive.PlatformServices.dll
  System.Reactive.Windows.Threading.dll
  System.Windows.Interactivity.dll
  をACTのインストールディレクトリにコピーします。

<b>Step 2.</b>
  lib\libopus.dll
  lib\libsodium.dll
  を必ずACTのインストールディレクトリにコピーします。

<b>Step 3.</b>
  ACTのプラグインリストからプラグインとして ACT.TTSYukkuri.dll を追加してください。
</pre>
    
#### ディレイ読上げ  
/wait 1,ゆっくりです  
読上げるテキストに上記のように記述すると、トリガー検出から1秒後に「ゆっくりです」を読上げます  
  
    
#### TTS対応状況  
下記のTTSに対応しています  
  
##### AquesTalk（ゆっくり）  
ニコニコ動画の「ゆっくり実況」で有名なTTSエンジン。  
ゲームのアラートとして使用する場合は読上げ速度を早めにしておくのがオススメです。  
AquesTal10になり非常にクリアな音質になりました。  
  
##### Open JTalk（MEI）  
名工大が開発したオープンソースのTTSエンジン。  
男性の声と「MEI（メイ）」という女性の声が使えます。  
これを使う場合はあまり句読点を入れないほうが自然に喋るような気がします。  
おまけで初音ミクの声から合成したボイスデータも同梱しています（type-α, type-β）が、元々が歌うためのものなのでかなり聞き取りづらいです。  
  
##### Voice Text Web API  
HOYAサービス株式会社が提供しているWeb経由で使用できるTTSエンジン。  
ユーザ登録をしてAPIKey（アプリケーションからアクセスするためのパスワードのようなもの）を発行して貰う必要がありますが無料で使えます。  
テレ東の「モヤモヤさまぁ～ず」のナレーションの「ショウ」君が有名です。  
発声の際にWebにアクセスするためレスポンスは非常に悪いですが、同じ文章の2回目以降の発声はローカル側のキャッシュを利用するため気にならなくなります。  
音声やイントネーション等の品質は非常に高いと思います。  
  
※注意  
パーティメンバのパラメータを数値等の代名詞付きで読上げているような場合は使用しないほうが良いです。  
毎回読上げる文章の内容が変化するためキャッシュが効きません。  
  
##### CeVIO Creative Studio（さとうささら）※ただし製品版が必要  
別途、有償のソフトが必要です。  
有償なだけあって高品質なTTSエンジンです。
  
##### 棒読みちゃん  
棒読みちゃんに読上げてもらいます。  
棒読みちゃんには文章だけを連携して、文章の解析、速度・ピッチ等の調整、TTSの再生等は棒読みちゃん側の制御になります。  
当プラグイン側で再生していないため、TTSに対するサウンドデバイスの変更や再生方式の変更は効きません。  
  
##### VOICEROID  
v3.1.0からVOICEROIDにも対応しました。  
しかしVOICEROIDには外部から操作するためのAPIがないため使い勝手がよくありません。参考程度に考えてください。  
  
#### タイムラインでも使いたい  
**[こちらの改造版タイムラインをどうぞ](https://github.com/anoyetta/act_timeline/releases)**  
  
    
  
#### TimeLineから使った時にwaveが既定のデバイスからしかならない  
TimeLineから普通にwaveを鳴らすとTTSYukkuri経由ではなくTimeLine本体がwaveを再生します  
TimeLine本体には再生デバイスを選ぶ機能がないため、既定のデバイスでの再生となります  
下記の例のようにwave再生の指定を変更してTTSYukkuri経由で再生するように変更してください  
<pre>
例) TTSYukkuri経由でwaveを再生させる
alertall アトミックレイ before 1 speak "TTSYukkuri" "se_maoudamashii_chime10.wav"
※ ゆっくりに喋らせる文字列としてファイル名を指定します
</pre>
  
    
#### 再生方式について  
環境に合わせて再生方式を選べるようになっています

##### WaveOut  
Windowsの伝統的なAPIによる再生。  
可もなく不可もなくといったところ。ほとんどの環境で動作すると思われる。  
  
##### DirectSound  
Windowsのゲーム向けAPIによる再生。  
ゲーム向けであるため低遅延及び同時再生に強いのが特徴。  
  
##### WASAPI（デフォルト）  
Windows Vista から搭載された新しいAPIによる再生。  
WaveOut同様に可もなく不可もなく動作する。特にこだわりがない場合はこれを選んでおけばよい。  
  
##### ASIO  
高級サウンドカード等が対応している高品質な汎用API。  
テストは出来ていませんが対応するサウンドカードを使っている人は使ってみるといいかもしれない。  
  
##### 遅延やノイズについて  
DirectSound <<<< WASAPI < WaveOut  
計測してみた結果、喋りだしまでの時間は上記のとおりでした（左ほど高速）。  
DirectSoundは、処理時間の桁が違います（WASAPIに対して6倍程度高速）。  
WASAPIは、WaveOutと比較して2倍程度高速でした。  
ASIOは環境が無いため試せていません。  
  
ただし、OSやPCのスペック、サウンドデバイス等の様々な外部要素の影響によってDirectSoundではノイズが発生する場合があります。  
自身の環境で動かしてみてマッチするものを選んで下さい。  
体感できる差がないという場合は、DirectSoundを選択しておけば理論上は最速になります。  
WaveOutとWASAPIで迷う場合は、WASAPIを選択しておけば理論上はより高速に動作します。  
  
    
最新リリース
--------------
**[こちらからダウンロードしてください](https://github.com/anoyetta/ACT.TTSYukkuri/releases/latest)**  
  
    
ライセンス
--------------
三条項BSDライセンス  
Copryright (c) 2014, anoyetta  
https://github.com/anoyetta/ACT.TTSYukkuri/blob/master/LICENSE  
ただし配布されたDLLやEXEのリバースエンジニアリングやリソース・変数の解析は禁止します。  
  
    
NAudio
--------------
本ソフトは、NAudioライブラリを使用しており、その著作権はMark Heath氏に帰属します。  
配布元:     http://naudio.codeplex.com/  
ライセンス: http://naudio.codeplex.com/license  
  
    
AquesTalk（いわゆる、ゆっくりボイス）
--------------
本ソフトは、株式会社アクエストの音声合成ライブラリAquesTalk, AqKanji2Koeを使用しており、その著作権は同社に帰属します。  
営利目的での使用は当該ライブラリの使用ライセンスが必要となります。  
  
    
VoiceroidUtil
--------------
本ソフトは、VoiceroidUtil, RucheHomeLib をMITライセンスに基づき使用しています。  
https://github.com/ruche7/VoiceroidUtil  
  
    
謝辞
--------------
・GB19xx様  
https://github.com/GB19xx/ACT.TPMonitor  
のFF14ヘルパークラスを流用させていただきました  
  
    
お問合せ
--------------
不具合報告、要望、質問及び最新版情報などはTwitterにて  
GitHubと連動しているためツイートは少々五月蠅いかもしれません  
https://twitter.com/anoyetta  
