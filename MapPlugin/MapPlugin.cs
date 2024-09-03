using System;
using AtsEx.PluginHost.Plugins;
using BveTypes.ClassWrappers;
using FastMember;
using TypeWrapping;
using ObjectiveHarmonyPatch;
using AtsEx.PluginHost.Native;
//using AtsEx.PluginHost.MapStatements;
using AtsEx.Extensions.MapStatements;
using System.Collections.Generic;
using Mackoy.Bvets;
using System.Timers;
using System.IO.Pipes;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace MetroDrive.MapPlugin
{
    [Plugin(PluginType.MapPlugin)]
    public class MapPluginMain : AssemblyPluginBase
    {
        int index;
        double NeXTLocation;
        double nowLocation;
        int EB;
        int power;
        int brake;
        int nowMilli;
        double speed;
        string now;
        string arrive;
        int arriveMilli;
        int passMilli;
        bool pass;
        TimeDrawer timeDrawer;
        Life life;
        UIDrawer uIDrawer;
        Pause pause;
        int atc;
        bool hideHorn;
        //フラグ
        bool isOverATC;
        bool isDelay;
        bool isEB;
        bool isTeituu;
        bool isGood;
        bool isGreat;
        bool isEBStop;
        bool isOverRun;
        bool isRestart;
        bool isFixOver;//「停止位置を修正します」みたいな感じのウィザード
        int time;
        bool isBonus;//ボーナスがある時はこれで時刻表を追加する ->　これの時は条件付きでEndGameを茅場町で飛ばす
        bool isTimeOut;
        bool isTaiken;
        int pointerIndex;
        bool isEnterPressed; 
        bool isExitScenario;
        bool isVoiceOn;
        bool isUIOff;
        //bool isGame;//ゲーム実行中意外はFormをオフに ->拡張機能の方に移植
        bool isPause;
        int timer;
        int nativepower;
        int nativebrake;
        double goukakuhani;
        public string sharedMes;
        TimeSpan totalElapsed = TimeSpan.Zero;
        public MapPluginMain(PluginBuilder builder) : base(builder)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Launch();
            }
            sharedMes = "none";
            life = new Life();
            timeDrawer = new TimeDrawer();
            uIDrawer = new UIDrawer();
            pause = new Pause();
            Native.BeaconPassed += new BeaconPassedEventHandler(BeaconPassed);
            Native.HornBlown += new HornBlownEventHandler(life.OnHorn);
            BveHacker.MainFormSource.KeyDown += OnKeyDown;
            FlagStart();
            IStatementSet statementSet = Extensions.GetExtension<IStatementSet>();
            ClauseFilter metroFilter = new ClauseFilter("MetroDrive", ClauseType.Element);
            Statement levelStatement = statementSet.FindUserStatement("Wattz",metroFilter,new ClauseFilter("Level",ClauseType.Function));
            Statement bonusStatement = statementSet.FindUserStatement("Wattz", metroFilter, new ClauseFilter("isBonus", ClauseType.Function));
            Statement taikenStatement = statementSet.FindUserStatement("Wattz", metroFilter, new ClauseFilter("isTaiken", ClauseType.Function));
            //Statement locationStatement = statementSet.FindUserStatement("Wattz", metroFilter, new ClauseFilter("addedLocation", ClauseType.Function));
            //Statement arrivalStatement = statementSet.FindUserStatement("Wattz", metroFilter, new ClauseFilter("addedArrival", ClauseType.Function));
            Statement kayabaStatement = statementSet.FindUserStatement("Wattz", metroFilter, new ClauseFilter("isKayaba", ClauseType.Function));
            //keysはstru~[].putの[]のなか
            /*if (levelStatement.Source.Clauses[4].Args[0].ToString() == "Easy")
            {
                life.OnStartEasy();
            }*/
            life.OnStartEasy();//仮


            ClassMemberSet assistantDrawerMembers = BveHacker.BveTypes.GetClassInfoOf<AssistantDrawer>();
            FastMethod drawMethod = assistantDrawerMembers.GetSourceMethodOf(nameof(AssistantDrawer.Draw));
            HarmonyPatch drawPatch = HarmonyPatch.Patch(Name, drawMethod.Source, PatchType.Prefix);
            timeDrawer.CreateModel(Location);
            uIDrawer.CreateModel(Location);
            pause.CreateModel(Location);
            drawPatch.Invoked += (sender, e) =>
            {
                timeDrawer.Patch(NeXTLocation, nowLocation,isUIOff, life.GoukakuHani);
                uIDrawer.UIDraw(power, nativebrake, EB, life.isTeituu, life.life,isUIOff);
                uIDrawer.LifeDraw(life.life,isUIOff);
                pause.PauseMenuDrawer(pointerIndex, isEnterPressed, isExitScenario, isVoiceOn,isUIOff);
                return PatchInvokationResult.DoNothing(e);
            };
            
            //計器照明を有効に
            InputEventArgs inputEventArgs = new InputEventArgs(-2, 15);
            BveHacker.KeyProvider.KeyDown_Invoke(inputEventArgs);
            BveHacker.KeyProvider.KeyUp_Invoke(inputEventArgs);
            isTimeOut = false;
            //MessageBox.Show("MetroDriveプラグインが読み込まれました");
            //NamedPipe((int)NeXTLocation);
            isPause= false;
        }
        public override void Dispose()
        {
            Native.BeaconPassed -= BeaconPassed;
            Native.HornBlown -= life.OnHorn;
        }
        public override TickResult Tick(TimeSpan elapsed)
        {
            totalElapsed += elapsed;
            if(totalElapsed.TotalSeconds >= 1)
            {
                FlagBuilder();
                totalElapsed -= TimeSpan.FromSeconds(1);
            }
            var station = BveHacker.Scenario.Route.Stations[index] as Station;
            if (station == null)
            {
                arriveMilli = station.ArrivalTimeMilliseconds;
                passMilli = station.DepartureTimeMilliseconds;
                pass = station.Pass;
            }
            index = BveHacker.Scenario.Route.Stations.CurrentIndex + 1;//Index
            nowLocation = Native.VehicleState.Location;//現在位置を設定
            NeXTLocation = BveHacker.Scenario.Route.Stations[index].Location;//次駅位置
            EB = 9;//EB
            //power = BveHacker.Handles.Power.Notch;//PowerNotch
            //brake = BveHacker.Handles.Brake.Notch;//BrakeNotch
            nativepower = Native.Handles.Power.Notch;//ATCの方
            nativebrake = Native.Handles.Brake.Notch;
            power = BveHacker.Scenario.Vehicle.Instruments.Cab.Handles.PowerNotch;//入力の方
            brake = BveHacker.Scenario.Vehicle.Instruments.Cab.Handles.BrakeNotch;
            nowMilli = BveHacker.Scenario.TimeManager.TimeMilliseconds;//Now
            speed = Native.VehicleState.Speed;//speed
            now = BveHacker.Scenario.TimeManager.Time.ToString("hhmmss");
            arrive = station.DepartureTime.ToString("hhmmss");
            if(brake > 0||nativebrake > 0)
            {
                //power = 0;
                //nativepower = 0;
                BveHacker.Scenario.Vehicle.Instruments.Cab.Handles.PowerNotch = 0;
            }
            if (pass == true && NeXTLocation == nowLocation)
            {
                OnPass();
            }
            life.NewUpdate(isOverATC, isDelay, isEB, isTeituu, isGood, isGreat, isEBStop, isOverRun, nowLocation, NeXTLocation, isRestart);
            timeDrawer.Tick(index, now, NeXTLocation, nowLocation, arrive);
            uIDrawer.tick(life.life);
            if (isExitScenario)
            {
                sharedMes = "Exit";
            }
            timer++;
            if(timer == 2)//1だと動かない（うさぷら側の仕様？？）
            {
                InputEventArgs inputEventArgs2 = new InputEventArgs(2, 8);
                BveHacker.KeyProvider.LeverMoved_Invoke(inputEventArgs2);
            }
            
            return new MapPluginTickResult();
        }
        public void BeaconPassed(BeaconPassedEventArgs e)
        {
            switch (e.Type)
            {
                case 10://信号0
                    atc = 0;
                    if (isVoiceOn) { /*音を再生する処理*/}
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
                    hideHorn = true;
                    break;
                case 901://隠し警笛終了
                    hideHorn = false;
                    break;
                case 902:
                    //「墨堤通り」ダイアログ
                    break;
                case 903:
                    //発車判定
                    break;
                case 904:
                    //停車/通過判定
                    break;
                case 905:
                    //「次は停車駅です」とか
                    break;
            }
        }
        void FlagBuilder()
        {
            if (speed < atc && power > 0) { isOverATC = true; }
            if (nowMilli - arriveMilli < 0)
            {
                if (pass == true) {
                    if (Math.Abs(nowLocation - NeXTLocation) > life.GoukakuHani)
                    { isDelay = true; }
                    if (Math.Abs(nowLocation - NeXTLocation) < life.GoukakuHani && speed > 0)
                    { isDelay = true; }
                }
                else 
                {
                    if (nowLocation < NeXTLocation)
                    {
                        isDelay = true;
                    }
                }
            }
            if (Math.Abs(arriveMilli - nowMilli) < 1000 && NeXTLocation == nowLocation)
            { isTeituu = true; }
            if (brake == EB && speed > 5 && NeXTLocation - nowLocation < 150)
            { isEBStop = true; }
            if (brake == EB && speed > 5 && NeXTLocation - nowLocation >= 150)
            { isEB = true; }
        }
        void OnStop()//停車時に一回だけ呼び出される
        {
            if (NeXTLocation - nowLocation > life.GoukakuHani)
            { isOverRun = true; }
            if (Math.Abs(nowLocation - NeXTLocation) < 0.5 && Math.Abs(nowMilli - arriveMilli) >= 2000)
            { isGood = true; }
            if (Math.Abs(nowLocation - NeXTLocation) < 0.5 && Math.Abs(nowMilli - arriveMilli) < 2000)
            { isGreat = true; }
        }
        void OnPass()
        {
            if (Math.Abs(arriveMilli - nowMilli) < 1000 && Math.Abs(NeXTLocation - nowLocation) < 5)
            { isTeituu = true; }
        }
        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P) { pause.OnInputP(); }
            if (e.KeyCode == Keys.Return) { isEnterPressed = true; }
            if(e.KeyCode == Keys.R)
            {
                isUIOff = !isUIOff;
            }
        }
        /*void EndScenario()
        {
            sharedMes = "endscenario";
            BveHacker.MainForm.UnloadScenario();
            BveHacker.MainFormSource.Hide();
        }*/
        void FlagStart()
        {
            isOverATC = false;
            isRestart = false;
            isEB = false;
            isEBStop = false;
            isTeituu = false;
            isGood = false;
            isGreat = false;
        }
        void FixOverRun()//オーバーランを直す
        {
            /*isFixOver = true;
            speed = 20;
            if (nowLocation - NeXTLocation > life.GoukakuHani)
            {
                speed = 0;
                isFixOver = false;
            }*/
            //UserVehicleLocationManager.SetLocation(NeXTLocation,false);//距離呈の変更

        }
        /*public void OnAddStation()
        {
            StationList stations = BveHacker.Scenario.Route.Stations;
            try
            {
                Station newStation = new Station("茅場町")
                {
                    Location = addStaPosi,
                    ArrivalTimeMilliseconds = addStaArrival,
                    Pass = false,
                    IsTerminal = true
                };
                stations.Insert(newStation);
            }
            catch(Exception e)
            {
                MessageBox.Show("茅場町駅のロードに失敗しました。運転を終了します。エラーメッセージ:"+e);
                //BVEを終了するメソッドを呼び出
                EndScenario();
            }
        }*/
        /*void LoadScenario(string path)
        {
            BveHacker.MainForm.OpenScenario(path);
        }*/
        /*
         *メモ
         *終了コード（EndBVEのwritething）
         *0→
         *1→茅場町追加ミス
         */
    }
 }
