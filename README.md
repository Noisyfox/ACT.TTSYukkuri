ACT.TTSYukkuri
============

概要
-------------
ACTのTTSをゆっくりさんに置き換えます
  
  
使い方
--------------
aqtk1-win  
ACT.TTSYukkuri.dll  
NAudio.dll  
NAudio.WindowsMediaFormat.dll  
をACTのインストールディレクトリにコピーします  
その後、プラグインとしてACT.TTSYukkuri.dllを追加してください
  
  
1) ディレイ読上げ  
/wait 1,ゆっくりです  
読上げのテキストに上記のように記述すると、トリガー検出から1秒後に「ゆっくりです」を読上げます  
  
  
2) TTS対応状況  
下記のTTSに対応しています  
・AquesTalk（ゆっくり）  
・CeVIO Creative Studio（さとうささら）※ただし製品版が必要  
・棒読みちゃん  
  
  
3) タイムラインでも使いたい  
[ACTTimelineWithTTSYukkuri-rev02.zip](https://github.com/anoyetta/ACT.TTSYukkuri/releases/download/ACTTimeline-rev02/ACTTimelineWithTTSYukkuri-rev02.zip "ACTTimelineWithTTSYukkuri-rev02.zip")  
こちらの改造版タイムラインをどうぞ  
  
  
4) ゆっくりが喋らない？  
ゆっくりは読める文字に制限があります  
文章の中に読めない文字が「一文字でも」混じっていると文章そのものを読みません  
  
以下、文字の対応状況です  
漢字 → よめます。ただしIMEで変換できる範囲です  
ひらがな → よめます  
カタカナ → よめます  
数字 → 読めます  
記号 → 一部読めません  
アルファベット → 英単語としては読めません。「えー」「びー」「しー」という風になります  
句読点 → 区切りとして読上げに間が開くようになります。読点は多めに入れるほうが自然な読上げになります  
  

        
最新リリース
--------------
**[こちらからダウンロードしてください](https://github.com/anoyetta/ACT.TTSYukkuri/releases/latest)**  


ライセンス
--------------
三条項BSDライセンス  
Copryright (c) 2014, anoyetta  
https://github.com/anoyetta/ACT.TTSYukkuri/blob/master/LICENSE  
  
  
NAudio
--------------
本ソフトは、NAudioライブラリを使用しており、その著作権はMark Heath氏に帰属します。  
配布元:     http://naudio.codeplex.com/  
ライセンス: http://naudio.codeplex.com/license  
  
    
AquesTalk（いわゆる、ゆっくりボイス）
--------------
本ソフトは、(株)アクエストの音声合成ライブラリAquesTalkを使用しており、その著作権は同社に帰属します。
営利目的での使用は当該ライブラリの使用ライセンスが必要となります。


謝辞
--------------
・GB19xx様  
https://github.com/GB19xx/ACT.TPMonitor  
のFF14ヘルパークラスを流用させていただきました  
  
・Qofar様  
https://gist.github.com/Qofar/11364454  
ACTのTTSメソッドを書き換えるロジックを流用させていただきました  


[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/anoyetta/act.ttsyukkuri/trend.png)](https://bitdeli.com/free "Bitdeli Badge")

