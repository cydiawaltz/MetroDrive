using System;
using AtsEx.PluginHost.Native;
using System.Threading.Tasks;

namespace MetroDrive
{
    internal class Life
    {
        
        //級数によって変更
        //減点
        public int overatc;
        public bool overatcset;//速度超過時に音にする
        public int overtime;//時間超過時は警告表示を出さず、文字の色
        public bool restartset;//再加速時
        public int restart;
        public int EBbrake;
        public int EB;
        public bool EBbrakeset;
        public int EBstop;
        public bool EBstopset;//駅構内でEBを使用したとき
        //加点
        public int teitu;
        public bool teituset;//定通時
        public int good;
        public bool goodset;//good
        public int great;
        public bool greatset;//grate
        public int bonus;
        public bool bonusset;//ボーナス（各死刑的）
        public int life;
        //その他
        public int GoukakuHani;
        public int lifetime;
        //以下同じ
        public int atc;
        public bool HideHorn;
        //以下Mainから取得
        public double speed;
        public int arrive;
        public int power;
        public int brake;
        public int index;
        public int nowMilli;
        public int passMilli;
        public bool pass;
        public double nowLocation;
        public double NeXTLocation;
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
            greatset = false;
            //加点
            teitu = 3;//定通
            good = 3;//Good停車
            great = 5;//Grate!停車
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
        public void Update()//毎フレーム呼び出す
        {
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
            if(pass == false)
            {
                //範囲外
                if(Math.Abs(nowLocation - NeXTLocation)>GoukakuHani )
                {
                    life -= overtime;//５秒以上遅れたら１秒ごとに減点
                    Delay(1000);
                }
                //範囲内かつ停車していない
                if(Math.Abs(nowLocation - NeXTLocation)<GoukakuHani && speed>0)
                {
                    life-= overtime;
                    Delay(1000);
                }
            }
            else
            {
                if(arrive - nowMilli >5000 && NeXTLocation > nowLocation)
                {
                    life -= overtime;
                    Delay(1000);
                }
                if(Math.Abs(arrive - nowMilli)<1000 && NeXTLocation == nowLocation)
                {
                    life += teitu;
                    Delay(1000);
                }
            }
            if(brake == EB)//非常制動
            {
                EBbrakeset =true;
                life -= EBbrake;
                Delay(2000);
                EBbrakeset = false;
            }
            //*good!/if(Math.Abs(NowLocation - NeXTLocation)<0.5 && speed == 0.1)
            {
                life += good;
                goodset = true;
                Delay(2000);
                goodset = false;
            }
            //*great!/if(Math.Abs(NowLocation - NeXTLocation)<0.5 && speed == 0.1 && Math.Abs(now - arrive))
            {
                life += great;
                greatset = true;
                Delay(2000);
                greatset = false;
            }
            //オーバーラン
            if (nowLocation > GoukakuHani + NeXTLocation)//過走時
            {
                if (speed == 0)
                {
                    int overrun = Convert.ToInt32(nowLocation - NeXTLocation);
                    life -= overrun;
                    Delay(5000);
                }
            }
        }
        static async void Delay(int e)
        {
            await Task.Delay(e);
            return;
        }
        void OnHorn()//警笛イベントのときに呼ばれる
        {
            if(HideHorn == true)//警笛ボーナス
            {
                life += bonus;
                bonusset = true;
                Delay(1000);
                bonusset = false;
            }
        }
    }
}
