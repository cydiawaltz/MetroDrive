using System;
using AtsEx.PluginHost.Plugins;
using BveTypes.ClassWrappers;
using FastMember;
using TypeWrapping;
using ObjectiveHarmonyPatch;
using AtsEx.PluginHost.Native;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Reflection;
using SlimDX;
using SlimDX.Direct3D9;

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
        public int EB;
        //共有メモリ
        //MemoryMappedFileViewAccessor sendtounity;

        public MapPluginMain(PluginBuilder builder) : base(builder)
        {
            power = Native.Handles.Power.Notch;
            brake = Native.Handles.Brake.Notch;
            //Life life = new Life();
            //life.OnStart();
            //Native.BeaconPassed += new BeaconPassedEventHandler(life.BeaconPassed) ;
            //MemoryMappedFile a = MemoryMappedFile.CreateNew("ScenarioOpen", 4096);
            //sendtounity = a.CreateViewAccessor();
            //
            ClassMemberSet assistantDrawerMembers = BveHacker.BveTypes.GetClassInfoOf<AssistantDrawer>();
            FastMethod drawMethod = assistantDrawerMembers.GetSourceMethodOf(nameof(AssistantDrawer.Draw));
            HarmonyPatch drawPatch = HarmonyPatch.Patch(Name, drawMethod.Source,PatchType.Prefix);
            //string testTexPath = Path.Combine(Path.GetDirectoryName(Location),@"picture\aaa.jpg");//DLLが入っているフォルダまで
            //Model test = Model.CreateRectangleWithTexture(rectangleF, 0, 0,testTexPath);//四角形の3Dモデル
            Model p0 = CreatePowerModel("0");
            Model p1 = CreatePowerModel("1");
            Model p2 = CreatePowerModel("2");
            Model p3 = CreatePowerModel("3");
            Model p4 = CreatePowerModel("4");
            //brake
            Model b0 = CreateBrakeModel("0");
            Model b1 = CreateBrakeModel("1");
            Model b2 = CreateBrakeModel("2");
            Model b3 = CreateBrakeModel("3");
            Model b4 = CreateBrakeModel("4");
            Model b5 = CreateBrakeModel("5");
            Model b6 = CreateBrakeModel("6");
            Model b7 = CreateBrakeModel("7");
            Model b8 = CreateBrakeModel("8");
            Model eb = CreateBrakeModel("9");
            drawPatch.Invoked += (sender, e) =>
            {
                float width = Direct3DProvider.Instance.PresentParameters.BackBufferWidth;
                float height = Direct3DProvider.Instance.PresentParameters.BackBufferHeight;
                //3D modelをどこに配置するのか指定するかんじ device.settransform(文字略) 頂点位置の変換
                Device powerDevice = Direct3DProvider.Instance.Device;
                powerDevice.SetTransform(TransformState.World, Matrix.Translation(-width/2,height/2,0));
                powerDevice.SetTransform(TransformState.View, Matrix.Identity); 
                powerDevice.SetTransform(TransformState.Projection, Matrix.OrthoOffCenterLH(-width / 2, width / 2, -height / 2, height / 2, 0, 1));
                if (power == 0) { p0.Draw(Direct3DProvider.Instance, false); }
                if (power == 1) { p1.Draw(Direct3DProvider.Instance, false); }
                if (power == 2) { p2.Draw(Direct3DProvider.Instance, false); }
                if (power == 3) { p3.Draw(Direct3DProvider.Instance, false); }
                if (power == 4) { p4.Draw(Direct3DProvider.Instance, false); }
                Device brakeDevice = Direct3DProvider.Instance.Device;
                brakeDevice.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));
                brakeDevice.SetTransform(TransformState.View, Matrix.Identity);
                brakeDevice.SetTransform(TransformState.Projection, Matrix.OrthoOffCenterLH(width / 2, width / 2, -height / 2, height / 2, 0, 1));
                if (brake == 0) { b0.Draw(Direct3DProvider.Instance, false); }
                if (brake == 1) { b1.Draw(Direct3DProvider.Instance, false); }
                if (brake == 2) { b2.Draw(Direct3DProvider.Instance, false); }
                if (brake == 3) { b3.Draw(Direct3DProvider.Instance, false); }
                if (brake == 4) { b4.Draw(Direct3DProvider.Instance, false); }
                if (brake == 5) { b5.Draw(Direct3DProvider.Instance, false); }
                if (brake == 6) { b6.Draw(Direct3DProvider.Instance, false); }
                if (brake == 7) { b7.Draw(Direct3DProvider.Instance, false); }
                if (brake == 8) { b8.Draw(Direct3DProvider.Instance, false); }
                if (brake == Native.Handles.Brake.EmergencyBrakeNotch) { eb.Draw(Direct3DProvider.Instance, false); }
                return PatchInvokationResult.DoNothing(e);
            };
            Model CreatePowerModel(string notch)
            {
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\P"+notch+".png");
                RectangleF rectangleF = new RectangleF(0 / 2, 0 / 2, 200, -300);
                Model powerNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return powerNotch;
            }
            Model CreateBrakeModel(string  notch)
            {
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\B" + notch + ".png");
                RectangleF rectangleF = new RectangleF(0/ 2, 0 / 2, 200, -300);
                Model brakeNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return brakeNotch;
            }
            
        }
        public override void Dispose()
        {
            //Life life = new Life() ;
            //Native.BeaconPassed -= life.BeaconPassed;
            //sendtounity.Write(0,1);
        }
        public override TickResult Tick(TimeSpan elapsed)
        {
            Life life = new Life();
            //int lifetime = life.life;
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
            EB = Native.Handles.Brake.EmergencyBrakeNotch;//EB
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
