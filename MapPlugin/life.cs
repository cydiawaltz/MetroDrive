using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtsEx.PluginHost;
using AtsEx.PluginHost.Native;

namespace AtsExCsTemplate.MapPlugin
{
    internal class Life
    {
        //級数によって変更
        //減点
        int overatc;
        public bool overatcset;//速度超過時に音にする
        int overtime;//時間超過時は警告表示を出さず、文字の色のみ
        int restart;
        public bool restartset;//再加速時
        int EBbrake;
        int EB;
        public bool EBbrakeset;
        int EBstop;
        public bool EBstopset;//駅構内でEBを使用したとき
        //加点
        int teitu;
        public bool teituset;//定通時
        int good;
        public bool goodset;//good
        int grate;
        public bool grateset;//grate
        int bonus;
        public bool bonusset;//ボーナス（各死刑的）
        //その他
        int GoukakuHani;
        public int lifetime;
        //以下同じ
        int atc;
        bool HideHorn;
        public void OnStart()//初期化
        {
            //難しさごとに変更（現在:初級）
            //減点
            overatc = 2;//予告無視
            overtime = 1;//時間超過（一秒おき）
            restart = 5;//駅構内再加速
            EBbrake = 5;//非常ブレーキ
            EBstop = 5;//非常聖堂停車
            overatcset = false;
            restartset = false;
            EBbrakeset = false;
            EBstopset = false;
            teituset = false;
            goodset = false;
            grateset = false;
            EB = 8;//EBとして認識する値
            //加点
            teitu = 3;//定通
            good = 3;//Good停車
            grate = 5;//Grate!停車
            bonus = 2;//ボーナス
            //その他
            GoukakuHani = 4;//合格範囲
            lifetime = 30;//初期持ち時間
            //以下共通設定
            atc = 80;
            HideHorn = false;
        }
        public void BeaconPassed(BeaconPassedEventArgs e)
        {
            switch (e.Type)
            {
                case 10://信号0
                    atc = 0;
                    break;
                case 18://ATC信号40
                    atc = 40;
                    break;
                case 19://ATC45
                    atc = 45;
                    break;
                case 21://ATC55
                    atc = 55;
                    break;
                case 23: //ATC65
                    atc = 65;
                    break;
                case 25://ATC75
                    atc = 75;
                    break;
                case 26://Atc80
                    atc = 80;
                    break;
                case 900://隠し警笛開始
                    HideHorn = true;
                    break;
                case 901://隠し警笛終了
                    HideHorn = false;
                    break;
            }
        }
    }
}
