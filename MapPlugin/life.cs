using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AtsEx.PluginHost.Native;
using AtsEx.PluginHost.Plugins;

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
        public int life;
        //その他
        int GoukakuHani;
        public int lifetime;
        //以下同じ
        int atc;
        bool HideHorn;
        //以下Mainから取得
        double speed;
        int arrive;
        int power;
        int brake;
        int index;
        int now;
        bool pass;
        double NowLocation;
        double NeXTLocation;
        public void OnStart()//初期化
        {
            //難しさごとに変更（現在:初級）
            life = 30;
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
        public void async Update()//毎フレーム呼び出す
        {
            //Mainから取得した関数
            MapPluginMain mapPluginMain = new MapPluginMain();
            speed = mapPluginMain.speed;
            arrive = mapPluginMain.arrive;
            pass = mapPluginMain.pass;
            power = mapPluginMain.power;
            brake = mapPluginMain.brake;
            index = mapPluginMain.index;
            NowLocation = mapPluginMain.NowLocation;
            NeXTLocation = mapPluginMain.NeXTLocation;
            //ATC超過
            if(speed < atc && brake == 0) 
            {
                life -= overatc;
                bool overatcset = true;
            }
            else{
                bool overatcset = false;
            }
            //遅れ
            if(pass == true)
            {
                //範囲外
                if(Math.Abs(NowLocation - NeXTLocation)>GoukakuHani )
                {
                    life -= overtime;//５秒以上遅れたら１秒ごとに減点
                    await Task.Delay(1000);
                }
                //範囲内かつ停車していない
                if(Math.Abs(NowLocation - NeXTLocation)<GoukakuHani && speed>0)
                {
                    life-= overtime;
                    await Task.Delay(1000);
                }
            }
            else
            {
                if(arrival - now >5000 && NeXTLocation > NowLocation)
                {
                    life -= overtime;
                    await Task.Delay(1000);
                }
                if(Math.Abs(arrival - now)<1000 && NeXTLocation == NowLocation)
                {
                    life += teitu;
                    await Task.Delay(1000);
                }
            }
            if(Brake == mapPluginMain.EB)
            {
                EBbrakeset =true;
                life -= EBbrake;
                await Task.Delay(1000);
                EBbrakeset = false;
            }
            //警笛ボーナス
            if(HideHorn = true)
            {
                if(/*警笛が鳴ったら*/)
                {
                    life += bonus;
                    bonusset = true;
                    await Task.Delay(1000);
                }
                else{bonusset = false;}
            }
        }
    }
}
