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
        

        public MapPluginMain(PluginBuilder builder) : base(builder)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Launch();
            }
            Life life = new Life();
            TimeDrawer timeDrawer = new TimeDrawer();
            UIDrawer uIDrawer = new UIDrawer();
            //power = Native.Handles.Power.Notch;
            //brake = Native.Handles.Brake.Notch;
            //timeDraw.now = ;
            //life.OnStart();
            //Native.BeaconPassed += new BeaconPassedEventHandler(life.BeaconPassed) ;
            //MemoryMappedFile a = MemoryMappedFile.CreateNew("ScenarioOpen", 4096);
            //sendtounity = a.CreateViewAccessor();
            //
            timeDrawer.Location = Location; 
            ClassMemberSet assistantDrawerMembers = BveHacker.BveTypes.GetClassInfoOf<AssistantDrawer>();
            FastMethod drawMethod = assistantDrawerMembers.GetSourceMethodOf(nameof(AssistantDrawer.Draw));
            HarmonyPatch drawPatch = HarmonyPatch.Patch(Name, drawMethod.Source, PatchType.Prefix);
            timeDrawer.CreateModel();
            uIDrawer.CreateModel();
            drawPatch.Invoked += (sender, e) =>
            {
                timeDrawer.Patch();
                uIDrawer.UIDraw();
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
            TimeDrawer timeDraw = new TimeDrawer();
            Life life = new Life();
            var station = BveHacker.Scenario.Route.Stations[life.index] as Station;
            if (station == null)
            {
                life.arrive = station.ArrivalTimeMilliseconds;
                life.passMilli = station.DepartureTimeMilliseconds;
                life.pass = station.Pass;
            }
            timeDraw.arrive = station.DepartureTime.ToString("hhmmss");
            timeDraw.now = BveHacker.Scenario.TimeManager.Time.ToString("hhmmss");
            timeDraw.arrione = timeDraw.arrive.Substring(0,1);
            timeDraw.arritwo = timeDraw.arrive.Substring(1, 1);
            timeDraw.arrithr = timeDraw.arrive.Substring(2, 1);
            timeDraw.arrifou = timeDraw.arrive.Substring(3, 1);
            timeDraw.arrifiv = timeDraw.arrive.Substring(4, 1);
            timeDraw.arrisix = timeDraw.arrive.Substring(5, 1);
            timeDraw.nowone = timeDraw.now.Substring(0, 1);
            timeDraw.nowtwo = timeDraw.now.Substring(1, 1);
            timeDraw.nowthr = timeDraw.now.Substring(2, 1);
            timeDraw.nowfou = timeDraw.now.Substring(3, 1);
            timeDraw.nowfiv = timeDraw.now.Substring(4, 1);
            timeDraw.nowsix = timeDraw.now.Substring(5, 1);
            timeDraw.next = Convert.ToInt32(life.NeXTLocation - life.nowLocation).ToString();
            timeDraw.nextone = timeDraw.next[0].ToString();
            if(life.NeXTLocation - life.nowLocation >= 10) { timeDraw.nexttwo = timeDraw.next[1].ToString(); }
            if(life.NeXTLocation - life.nowLocation >= 100) { timeDraw.nextthr = timeDraw.next[2].ToString(); }
            if(life.NeXTLocation - life.nowLocation >= 1000) { timeDraw.nextfou = timeDraw.next[3].ToString();}
            else { timeDraw.nextfou = "nothing"; }
            //int lifetime = life.life;
            life.index = BveHacker.Scenario.Route.Stations.CurrentIndex + 1;//Index
            life.nowLocation = Native.VehicleState.Location;//現在位置を設定
            life.NeXTLocation = BveHacker.Scenario.Route.Stations[life.index].Location;//次駅位置
            life.EB = Native.Handles.Brake.EmergencyBrakeNotch;//EB
            life.power = Native.Handles.Power.Notch;//PowerNotch
            life.brake = Native.Handles.Brake.Notch;//BrakeNotch
            life.nowMilli = BveHacker.Scenario.TimeManager.TimeMilliseconds;//Now
            life.speed = Native.VehicleState.Speed;//speed
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
            return new MapPluginTickResult();
        }
    }
}
