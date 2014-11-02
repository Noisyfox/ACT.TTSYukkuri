ACT.TTSYukkuri
============

概要
-------------
ACTのTTSをゆっくりさんに置き換えます
  
  
使い方
--------------
ACT.TTSYukkuri.dll  
SoundPlayer.exe  
aqtk1-win  
をACTのインストールディレクトリにコピーします  
その後、プラグインとしてACT.TTSYukkuri.dllを追加してください
  
  
1) ディレイ読上げ  
/wait 1,ゆっくりです  
読上げのテキストに上記のように記述すると、トリガー検出から1秒後に「ゆっくりです」を読上げます  
  
  
2) TTS対応状況  
下記のTTSに対応しています  
・AquesTalk（ゆっくり）  
・CeVIO Creative Studio（さとうささら）※ただし製品版が必要  
  
  
3) タイムラインでも使いたい  
[ACTTimelineWithTTSYukkuri-rev02.zip](https://github.com/anoyetta/ACT.TTSYukkuri/releases/download/ACTTimeline-rev02/ACTTimelineWithTTSYukkuri-rev02.zip "ACTTimelineWithTTSYukkuri-rev02.zip")  
こちらの改造版タイムラインをどうぞ  
  
  
4) ゆっくりが喋らない？  
ゆっくりは読める文字に制限があります  
文章の中に読めない文字が「一文字でも」混じっていると文章そのものを読みません  
  
以下、文字の対応状況です  
漢字 → 読めません  
ひらがな → よめます  
カタカナ → 基本的に読めますが「ディ」「ヴォ」といった日本語本来にはない発音は読めません  
数字 → 読めます  
記号 → 読めません  
アルファベット → 一文字ずつフォネティックコードで読上げます。実質使用できません  
句読点 → 区切りとして読上げに間が開くようになります。読点は多めに入れるほうが自然な読上げになります  
  

        
最新リリース
--------------
[ACT.TTSYukkuri-v1.3.3.zip](https://github.com/anoyetta/ACT.TTSYukkuri/releases/download/v1.3.3/ACT.TTSYukkuri-v1.3.3.zip "ACT.TTSYukkuri-v1.3.3.zip")  

  
  
AquesTalk（いわゆる、ゆっくりボイス）
--------------
本ソフトは、(株)アクエストの音声合成ライブラリAquesTalkを使用しており、その著作権は同社に帰属します。
営利目的での使用は当該ライブラリの使用ライセンスが必要となります。



