using System;
using System.Threading.Tasks;

namespace MetroDrive
{
    internal class Life
    {
        
        //級数によって変更
        //減点
        public int overatc;
        public bool isOveratc;//速度超過時に音にする
        public int overtime;//時間超過時は警告表示を出さず、文字の色
        public bool isRestart;//再加速時
        public int restart;
        public int EBbrake;
        //public int EB;
        public bool isEBbrake;
        public int EBstop;
        public bool isEBStop;//駅構内でEBを使用したとき
        //加点
        public int teitu;
        public bool isTeituu;//定通時
        public int good;
        public bool isGood;//good
        public int great;
        public bool isGreat;//grate
        public int bonus;
        public bool isBonus;//ボーナス（各死刑的）
        public int life;
        //その他
        public int GoukakuHani;
        //以下同じ
        public int atc;
        public bool HideHorn;
        public void OnStartFreeRun()//初期化
        {
            //難しさごとに変更（現在:初級）
            life = 999;
            //減点
            overatc = 0;//予告無視
            overtime = 0;//時間超過（一秒おき）
            restart = 0;//駅構内再加速
            EBbrake = 0;//非常ブレーキ
            EBstop = 0;//非常聖堂停車
            //加点
            teitu = 0;//定通
            good = 0;//Good停車
            great = 0;//Grate!停車
            bonus = 0;//ボーナス
            //その他
            GoukakuHani = 200;//合格範囲
            //以下共通設定
            atc = 110;
            HideHorn = false;
        }
        public void OnStartElement()//初期化
        {
            //難しさごとに変更（現在:初級）
            life = 999;
            //減点
            overatc = 0;//予告無視
            overtime = 0;//時間超過（一秒おき）
            restart = 0;//駅構内再加速
            EBbrake = 0;//非常ブレーキ
            EBstop = 0;//非常聖堂停車
            //加点
            teitu = 0;//定通
            good = 0;//Good停車
            great = 0;//Grate!停車
            bonus = 0;//ボーナス
            //その他
            GoukakuHani = 8;//合格範
            //以下共通設定
            atc = 110;
            HideHorn = false;
        }
        public void OnStartEasy()//初期化
        {
            //難しさごとに変更（現在:初級）
            life = 50;
            //減点
            overatc = 2;//予告無視
            overtime = 1;//時間超過（一秒おき）
            restart = 5;//駅構内再加速
            EBbrake = 5;//非常ブレーキ
            EBstop = 5;//非常聖堂停車
            //加点
            teitu = 3;//定通
            good = 3;//Good停車
            great = 5;//Grate!停車
            bonus = 2;//ボーナス
            //その他
            GoukakuHani = 4;//合格範囲
            //以下共通設定
            atc = 110;
            HideHorn = false;
        }
        public void OnStartNormal()//初期化
        {
            //難しさごとに変更（現在:初級）
            life = 40;
            //減点
            overatc = 4;//予告無視
            overtime = 1;//時間超過（一秒おき）
            restart = 5;//駅構内再加速
            EBbrake = 10;//非常ブレーキ
            EBstop = 10;//非常聖堂停車
            //加点
            teitu = 5;//定通
            good = 3;//Good停車
            great = 5;//Grate!停車
            bonus = 2;//ボーナス
            //その他
            GoukakuHani = 2;//合格範囲
            //以下共通設定
            atc = 110;
            HideHorn = false;
        }
        public void OnStartHard()//初期化
        {
            //難しさごとに変更（現在:初級）
            life = 30;
            //減点
            overatc = 4;//予告無視
            overtime = 2;//時間超過（一秒おき）
            restart = 5;//駅構内再加速
            EBbrake = 5;//非常ブレーキ
            EBstop = 5;//非常聖堂停車
            //加点
            teitu = 3;//定通
            good = 3;//Good停車
            great = 5;//Grate!停車
            bonus = 2;//ボーナス
            //その他
            GoukakuHani = 4;//合格範囲
            //以下共通設定
            atc = 80;
            HideHorn = false;
        }
        public void OnStartVeryHard()//初期化
        {
            //難しさごとに変更（現在:初級）
            life = 10;
            //減点
            overatc = 5;//予告無視
            overtime = 3;//時間超過（一秒おき）
            restart = 15;//駅構内再加速
            EBbrake = 10;//非常ブレーキ
            EBstop = 10;//非常聖堂停車
            //加点
            teitu = 2;//定通
            good = 2;//Good停車
            great = 3;//Grate!停車
            bonus = 1;//ボーナス
            //その他
            GoukakuHani = 1;//合格範囲
            //以下共通設定
            atc = 110;
            HideHorn = false;
        }

        public void NewUpdate(bool isOverATC, bool isDelay, bool isEB, bool isTeituu, bool isGood, bool isGreat, bool isEBStop, bool isOverRun, double nowLocation, double NeXTLocation, bool isRestart)
        {
            if (isOverATC == true)
            {
                life -= overatc;
                isOverATC = false;
            }
            if (isDelay == true)
            {
                life -= overtime;
                isDelay = false;
            }
            if (isEB == true)
            {
                life -= EBbrake;
                isEB = false;
            }
            if (isTeituu == true)
            {
                life += teitu;
                isTeituu = false;
            }
            if (isGood == true)
            {
                life += good;
                isGood = false;
            }
            if (isGreat == true)
            {
                life += great;
                isGreat = false;
            }
            if (isEBStop == true)
            {
                life -= EBstop;
                isEBStop = false;
            }
            if (isOverRun == true)
            {
                int overrun = Convert.ToInt32(nowLocation - NeXTLocation);
                life -= overrun;
                isOverRun = false;
            }
            if (isRestart == true)
            {
                life -= restart;
                isRestart = false;
            }
        }
        public void Update(double speed,int power,int brake,double nowLocation,double NeXTLocation,bool pass,int arriveMilli,int nowMilli,int EB)//毎フレーム呼び出す
        {
            //ATC超過
            if(speed < atc && power > 0) 
            {
                life -= overatc;
                isOveratc = true;
            }
            else{
                isOveratc = false;
            }
            //遅れ
            if(pass == false && nowMilli - arriveMilli >0)
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
                if(arriveMilli - nowMilli >5000 && NeXTLocation > nowLocation)
                {
                    life -= overtime;
                    Delay(1000);
                }
                if(Math.Abs(arriveMilli - nowMilli)<1000 && NeXTLocation == nowLocation)
                {
                    life += teitu;
                    Delay(1000);
                }
            }
            if(brake == EB && speed > 5)//非常制動
            {
                isEBbrake =true;
                life -= EBbrake;
                Delay(2000);
                isEBbrake = false;
            }
            //フラグを設定する
            if(Math.Abs(nowLocation - NeXTLocation)<0.5 && speed < 0.1 )//Good
            {
                life += good;
                isGood = true;
                Delay(2000);
                isGood = false;
            }
            if(Math.Abs(nowLocation - NeXTLocation)<0.5 && speed == 0.1 && Math.Abs(nowMilli - arriveMilli)<2000)//Great
            {
                life += great;
                isGreat = true;
                Delay(2000);
                isGreat = false;
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
            //TEST
            isTeituu = false;
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
                isBonus = true;
                Delay(1000);
                isBonus = false;
            }
        }
    }
}
