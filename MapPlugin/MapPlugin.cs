using System;
using AtsEx.PluginHost.Plugins;
using BveTypes.ClassWrappers;
using FastMember;
using TypeWrapping;
using ObjectiveHarmonyPatch;
using System.IO;
using System.Drawing;
using SlimDX;
using SlimDX.Direct3D9;

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
        public MapPluginMain(PluginBuilder builder) : base(builder)
        {
            //if (!System.Diagnostics.Debugger.IsAttached)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}
            life = new Life();
            timeDrawer = new TimeDrawer();
            uIDrawer = new UIDrawer();
            //Native.BeaconPassed += new BeaconPassedEventHandler(life.BeaconPassed) ;
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
                uIDrawer.UIDraw(power,brake,EB);
                return PatchInvokationResult.DoNothing(e);
            };
            
            
        }
        public override void Dispose()
        {
            //Life life = new Life() ;
            //Native.BeaconPassed -= life.BeaconPassed;
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
            //int lifetime = life.life;
            index = BveHacker.Scenario.Route.Stations.CurrentIndex + 1;//Index
            nowLocation = Native.VehicleState.Location;//現在位置を設定
            NeXTLocation = BveHacker.Scenario.Route.Stations[index].Location;//次駅位置
            EB = Native.Handles.Brake.EmergencyBrakeNotch;//EB
            power = Native.Handles.Power.Notch;//PowerNotch
            brake = Native.Handles.Brake.Notch;//BrakeNotch
            nowMilli = BveHacker.Scenario.TimeManager.TimeMilliseconds;//Now
            speed = Native.VehicleState.Speed;//speed
            now = BveHacker.Scenario.TimeManager.Time.ToString("hhmmss");
            arrive = station.DepartureTime.ToString("hhmmss");
            //持ち時間に応じて
            /*if(lifetime = 0)
            {
                brake = EB;
                if(speed = 0)
                {
                    //BVEを終了
                }
            }*/
            //life.Update();
            timeDrawer.Tick(index, now, NeXTLocation, nowLocation, arrive);
            return new MapPluginTickResult();
        }
    }
}
