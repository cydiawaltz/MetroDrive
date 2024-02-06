using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtsEx.PluginHost;

namespace AtsExCsTemplate.MapPlugin
{
    internal class life
    {
        //級数によって変更
        //減点
        int overatc;
        public bool overatcset;//速度超過時に音にする
        int overtime;//時間超過時は警告表示を出さず、文字の色のみ
        int restart;
        public bool restartset;//再加速時
        int EBbrake;
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
        public double NowLocation;
        public double NeXTLocation;
        public int Power;
        public int Brake;
        public int now;//ミリ秒でいくので表示側でTimeSpanに変換してくれ
        double speed;
        public int arrival;//以上同文
        public int pass;//以上同文
        public bool passset;
        int index;
        public void OnStart()//初期化
        {
            
            return;
        }
    }
}
