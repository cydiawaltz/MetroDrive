using System;
using AtsEx.PluginHost.Plugins;
using BveTypes.ClassWrappers;
using FastMember;
using TypeWrapping;
using ObjectiveHarmonyPatch;
using AtsEx.PluginHost.Native;
using System.Threading.Tasks;

namespace AtsExCsTemplate.MapPlugin
{
    [PluginType(PluginType.MapPlugin)]
    internal class MapPluginMain : AssemblyPluginBase
    {
        public double speed;
        public int arrive;
        public int past;
        public int power;
        public int brake;
        public int index;
        public int now;
        public bool pass;
        public double NowLocation;
        public double NeXTLocation;
        //pipeserver

        public MapPluginMain(PluginBuilder builder) : base(builder)
        {
            Life life = new Life();
            life.OnStart();
            Native.BeaconPassed += new BeaconPassedEventHandler(life.BeaconPassed) ;
            //
            ClassMemberSet assistantDrawerMembers = BveHacker.BveTypes.GetClassInfoOf<AssistantDrawer>();
            FastMethod drawMethod = assistantDrawerMembers.GetSourceMethodOf(nameof(AssistantDrawer.Draw));
            HarmonyPatch drawPatch = HarmonyPatch.Patch(Name, drawMethod.Source,PatchType.Prefix);
            Model model = Model.CreateRectangleWithTexture(, 0, 0,/*画像パス*/);//四角形の3Dモデル
            drawPatch.Invoked += (sender, e) =>
            {
                //3D modelをどこに配置するのか指定するかんじ device.settransform(文字略) 頂点位置の変換
                model.Draw(Direct3DProvider.Instance, false);
            };
           

        }
        public override void Dispose()
        {
            Life life = new Life() ;
            Native.BeaconPassed -= life.BeaconPassed;
        }
        public override TickResult Tick(TimeSpan elapsed)
        {
            Life life = new Life();
            int lifetime = life.life;
            index = BveHacker.Scenario.Route.Stations.CurrentIndex + 1;//Index
            var station = BveHacker.Scenario.Route.Stations[index] as Station;
            if (station == null)
            {
                arrive = station.ArrivalTimeMilliseconds;
                past = station.DepartureTimeMilliseconds;
                pass = station.Pass;
            }
            NowLocation = Native.VehicleState.Location;//現在位置を設定
            NeXTLocation = BveHacker.Scenario.Route.Stations[index].Location;//次駅位置
            power = Native.Handles.Power.Notch;//PowerNotch
            brake = Native.Handles.Brake.Notch;//BrakeNotch
            now = BveHacker.Scenario.TimeManager.TimeMilliseconds;//Now
            speed = Native.VehicleState.Speed;//speed
            //持ち時間に応じて
            if(lifetime = 0)
            {
                brake = Native.Handles.Brake.EmergencyBrakeNotch;
                if(speed = 0)
                {
                    //BVEを終了
                }
            }
            life.Update();
            return new MapPluginTickResult();
        }
    }
}
