using System;
using AtsEx.PluginHost.Plugins;
using BveTypes.ClassWrappers;
using FastMember;
using TypeWrapping;
using ObjectiveHarmonyPatch;
using AtsEx.PluginHost.Native;
using AtsEx.PluginHost.MapStatements;
using System.Collections.Generic;
using Mackoy.Bvets;
using System.Timers;

namespace MetroDrive
{
    [Plugin(PluginType.MapPlugin)]
    internal class MapPluginMain : AssemblyPluginBase
    {
        //共有メモリ
        //MemoryMappedFileViewAccessor sendtounity;
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
        public MapPluginMain(PluginBuilder builder) : base(builder)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Launch();
            }
            life = new Life();
            timeDrawer = new TimeDrawer();
            uIDrawer = new UIDrawer();
            Native.BeaconPassed += new BeaconPassedEventHandler(BeaconPassed) ;
            FlagStart();
            //MemoryMappedFile a = MemoryMappedFile.CreateNew("ScenarioOpen", 4096);
            //sendtounity = a.CreateViewAccessor();
            ClassMemberSet assistantDrawerMembers = BveHacker.BveTypes.GetClassInfoOf<AssistantDrawer>();
            FastMethod drawMethod = assistantDrawerMembers.GetSourceMethodOf(nameof(AssistantDrawer.Draw));
            HarmonyPatch drawPatch = HarmonyPatch.Patch(Name, drawMethod.Source, PatchType.Prefix);
            timeDrawer.CreateModel(Location);
            uIDrawer.CreateModel(Location);
            drawPatch.Invoked += (sender, e) =>
            {
                timeDrawer.Patch(NeXTLocation,nowLocation);
                uIDrawer.UIDraw(power,brake,EB,life.isTeituu,life.life);
                uIDrawer.LifeDraw(life.life);
                return PatchInvokationResult.DoNothing(e);
            };
            Namespace ns = Namespace.GetUserNamespace("CydiaWaltz").Child("MetroDrive");
            Identifier difficulty = new Identifier(ns, "Level");
            IReadOnlyList<IHeader> headers = BveHacker.MapHeaders.GetAll(difficulty);
            for (int i = 0; i < headers.Count; i++)
            {
                IHeader header = headers[i];
                if(header.Name.FullName == "Easy")
                {
                    life.OnStartEasy();
                }
                life.OnStartEasy();
            }
            //計器照明を有効に
            InputEventArgs inputEventArgs = new InputEventArgs(-2, 15);
            BveHacker.KeyProvider.KeyDown_Invoke(inputEventArgs);
            BveHacker.KeyProvider.KeyUp_Invoke(inputEventArgs);
            //毎秒実行
            Timer timer = new Timer(1000);
            timer.Elapsed += FlagBuilder;
            timer.Enabled = true;
        }
        public override void Dispose()
        {
            //Life life = new Life() ;
            Native.BeaconPassed -= BeaconPassed;
            //sendtounity.Write(0,1);
        }
        public override TickResult Tick(TimeSpan elapsed)
        {
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
            power = Native.Handles.Power.Notch;//PowerNotch
            brake = Native.Handles.Brake.Notch;//BrakeNotch
            nowMilli = BveHacker.Scenario.TimeManager.TimeMilliseconds;//Now
            speed = Native.VehicleState.Speed;//speed
            now = BveHacker.Scenario.TimeManager.Time.ToString("hhmmss");
            arrive = station.DepartureTime.ToString("hhmmss");
            life.atc = atc;

            life.NewUpdate(isOverATC, isDelay, isEB, isTeituu, isGood, isGreat, isEBStop, isOverRun, nowLocation, NeXTLocation, isRestart);
            timeDrawer.Tick(index, now, NeXTLocation, nowLocation, arrive);
            uIDrawer.tick(life.life);
            //持ち時間が0になったらBVEを終了する処理
            return new MapPluginTickResult();
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
                    hideHorn = true;
                    break;
                case 901://隠し警笛終了
                    hideHorn = false;
                    break;
            }
        }
        void FlagBuilder(object sender, ElapsedEventArgs e)
        {
            if (speed < atc && power > 0) { isOverATC = true; }
            if (nowMilli - arriveMilli < 0)
            {
                if(pass == true) {
                    if (Math.Abs(nowLocation - NeXTLocation) > life.GoukakuHani)
                    { isDelay = true; }
                    if (Math.Abs(nowLocation - NeXTLocation) < life.GoukakuHani && speed > 0)
                    { isDelay = true; }
                }
                else{
                    if (nowLocation < NeXTLocation)
                    {
                        isDelay = true;
                    }
                }
            }
            if(Math.Abs(arriveMilli - nowMilli) < 1000 && NeXTLocation == nowLocation) 
            { isTeituu = true; }
            if (brake == EB && speed > 5 && NeXTLocation - nowLocation < 150)
            { isEBStop = true; }
            if (brake == EB && speed > 5 && NeXTLocation - nowLocation >= 150)
            { isEB = true; }
            life.life--;
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
            if (Math.Abs(arriveMilli - nowMilli) < 1000 && Math.Abs(NeXTLocation - nowLocation)<5)
            { isTeituu = true; }
        }
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
        }
    }
}
