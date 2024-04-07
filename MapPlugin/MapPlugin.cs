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
        public float width;
        public float height;
        public string arrione;
        public string arritwo;
        public string arrithr;
        public string arrifou;
        public string arrifiv;
        public string arrisix;
        public string nowone;
        public string nowtwo;
        public string nowthr;
        public string nowfou;
        public string nowfiv;
        public string nowsix;
        //共有メモリ
        //MemoryMappedFileViewAccessor sendtounity;

        public MapPluginMain(PluginBuilder builder) : base(builder)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Launch();
            }
            power = Native.Handles.Power.Notch;
            brake = Native.Handles.Brake.Notch;
            //timeDraw.now = ;
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
            //時間系(arri)
            Model a0 = CreateTimeModel("arrive", 0, 0, 0);//SetTransformで横の位置は変更するゾ
            Model a1 = CreateTimeModel("arrive", 1, 0, 0);
            Model a2 = CreateTimeModel("arrive", 2, 0, 0);
            Model a3 = CreateTimeModel("arrive", 3, 0, 0);
            Model a4 = CreateTimeModel("arrive", 4, 0, 0);
            Model a5 = CreateTimeModel("arrive", 5, 0, 0);
            Model a6 = CreateTimeModel("arrive" ,6, 0, 0);
            Model a7 = CreateTimeModel("arrive", 7, 0, 0);
            Model a8 = CreateTimeModel("arrive", 8, 0, 0);
            Model a9 = CreateTimeModel("arrive", 9, 0, 0);
            //(now)
            Model n0 = CreateTimeModel("now", 0, 0, 0);
            Model n1 = CreateTimeModel("now", 1, 0, 0);
            Model n2 = CreateTimeModel("now", 2, 0, 0);
            Model n3 = CreateTimeModel("now", 3, 0, 0);
            Model n4 = CreateTimeModel("now", 4, 0, 0);
            Model n5 = CreateTimeModel("now", 5, 0, 0);
            Model n6 = CreateTimeModel("now", 6, 0, 0);
            Model n7 = CreateTimeModel("now", 7, 0, 0);
            Model n8 = CreateTimeModel("now", 8, 0, 0);
            Model n9 = CreateTimeModel("now", 9, 0, 0);
            //固定UI
            Model arv = CreateArvModel();
            Model now = CreateNowModel();
            drawPatch.Invoked += (sender, e) =>
            {
                width = Direct3DProvider.Instance.PresentParameters.BackBufferWidth;
                height = Direct3DProvider.Instance.PresentParameters.BackBufferHeight;
                //3D modelをどこに配置するのか指定するかんじ device.settransform(文字略) 頂点位置の変換
                Device device = Direct3DProvider.Instance.Device;
                device.SetTransform(TransformState.World, Matrix.Translation(-width/2,height/2,0));
                device.SetTransform(TransformState.View, Matrix.Identity); 
                device.SetTransform(TransformState.Projection, Matrix.OrthoOffCenterLH(-width / 2, width / 2, -height / 2, height / 2, 0, 1));
                if (power == 0) { p0.Draw(Direct3DProvider.Instance, false); }
                if (power == 1) { p1.Draw(Direct3DProvider.Instance, false); }
                if (power == 2) { p2.Draw(Direct3DProvider.Instance, false); }
                if (power == 3) { p3.Draw(Direct3DProvider.Instance, false); }
                if (power == 4) { p4.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(width/2, height / 2, 0));//(float)Math.Sin(Native.VehicleState.Time.TotalSeconds)*100
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
                arv.Draw(Direct3DProvider.Instance, false);
                now.Draw(Direct3DProvider.Instance, false);
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));//arrive１個目
                if (arrione == "0") { a0.Draw(Direct3DProvider.Instance, false); }
                if (arrione == "1") { a1.Draw(Direct3DProvider.Instance, false); }
                if (arrione == "2") { a2.Draw(Direct3DProvider.Instance, false); }
                if (arrione == "3") { a3.Draw(Direct3DProvider.Instance, false); }
                if (arrione == "4") { a4.Draw(Direct3DProvider.Instance, false); }
                if (arrione == "5") { a5.Draw(Direct3DProvider.Instance, false); }
                if (arrione == "6") { a6.Draw(Direct3DProvider.Instance, false); }
                if (arrione == "7") { a7.Draw(Direct3DProvider.Instance, false); }
                if (arrione == "8") { a8.Draw(Direct3DProvider.Instance, false); }
                if (arrione == "9") { a9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));//arrive２個め
                if (arritwo == "0") { a0.Draw(Direct3DProvider.Instance, false); }
                if (arritwo == "1") { a1.Draw(Direct3DProvider.Instance, false); }
                if (arritwo == "2") { a2.Draw(Direct3DProvider.Instance, false); }
                if (arritwo == "3") { a3.Draw(Direct3DProvider.Instance, false); }
                if (arritwo == "4") { a4.Draw(Direct3DProvider.Instance, false); }
                if (arritwo == "5") { a5.Draw(Direct3DProvider.Instance, false); }
                if (arritwo == "6") { a6.Draw(Direct3DProvider.Instance, false); }
                if (arritwo == "7") { a7.Draw(Direct3DProvider.Instance, false); }
                if (arritwo == "8") { a8.Draw(Direct3DProvider.Instance, false); }
                if (arritwo == "9") { a9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));//arrive３個目
                if (arrithr == "0") { a0.Draw(Direct3DProvider.Instance, false); }
                if (arrithr == "1") { a1.Draw(Direct3DProvider.Instance, false); }
                if (arrithr == "2") { a2.Draw(Direct3DProvider.Instance, false); }
                if (arrithr == "3") { a3.Draw(Direct3DProvider.Instance, false); }
                if (arrithr == "4") { a4.Draw(Direct3DProvider.Instance, false); }
                if (arrithr == "5") { a5.Draw(Direct3DProvider.Instance, false); }
                if (arrithr == "6") { a6.Draw(Direct3DProvider.Instance, false); }
                if (arrithr == "7") { a7.Draw(Direct3DProvider.Instance, false); }
                if (arrithr == "8") { a8.Draw(Direct3DProvider.Instance, false); }
                if (arrithr == "9") { a9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));//now
                if (arrifou == "0") { a0.Draw(Direct3DProvider.Instance, false); }
                if (arrifou == "1") { a1.Draw(Direct3DProvider.Instance, false); }
                if (arrifou == "2") { a2.Draw(Direct3DProvider.Instance, false); }
                if (arrifou == "3") { a3.Draw(Direct3DProvider.Instance, false); }
                if (arrifou == "4") { a4.Draw(Direct3DProvider.Instance, false); }
                if (arrifou == "5") { a5.Draw(Direct3DProvider.Instance, false); }
                if (arrifou == "6") { a6.Draw(Direct3DProvider.Instance, false); }
                if (arrifou == "7") { a7.Draw(Direct3DProvider.Instance, false); }
                if (arrifou == "8") { a8.Draw(Direct3DProvider.Instance, false); }
                if (arrifou == "9") { a9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));//now
                if (arrifiv == "0") { a0.Draw(Direct3DProvider.Instance, false); }
                if (arrifiv == "1") { a1.Draw(Direct3DProvider.Instance, false); }
                if (arrifiv == "2") { a2.Draw(Direct3DProvider.Instance, false); }
                if (arrifiv == "3") { a3.Draw(Direct3DProvider.Instance, false); }
                if (arrifiv == "4") { a4.Draw(Direct3DProvider.Instance, false); }
                if (arrifiv == "5") { a5.Draw(Direct3DProvider.Instance, false); }
                if (arrifiv == "6") { a6.Draw(Direct3DProvider.Instance, false); }
                if (arrifiv == "7") { a7.Draw(Direct3DProvider.Instance, false); }
                if (arrifiv == "8") { a8.Draw(Direct3DProvider.Instance, false); }
                if (arrifiv == "9") { a9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));//now
                if (arrisix == "0") { a0.Draw(Direct3DProvider.Instance, false); }
                if (arrisix == "1") { a1.Draw(Direct3DProvider.Instance, false); }
                if (arrisix == "2") { a2.Draw(Direct3DProvider.Instance, false); }
                if (arrifiv == "3") { a3.Draw(Direct3DProvider.Instance, false); }
                if (arrisix == "4") { a4.Draw(Direct3DProvider.Instance, false); }
                if (arrisix == "5") { a5.Draw(Direct3DProvider.Instance, false); }
                if (arrisix == "6") { a6.Draw(Direct3DProvider.Instance, false); }
                if (arrisix == "7") { a7.Draw(Direct3DProvider.Instance, false); }
                if (arrisix == "8") { a8.Draw(Direct3DProvider.Instance, false); }
                if (arrisix == "9") { a9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));//now
                if (nowone == "0") { a0.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "1") { a1.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "2") { a2.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "3") { a3.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "4") { a4.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "5") { a5.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "6") { a6.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "7") { a7.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "8") { a8.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "9") { a9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));//now
                if (nowtwo == "0") { a0.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "1") { a1.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "2") { a2.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "3") { a3.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "4") { a4.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "5") { a5.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "6") { a6.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "7") { a7.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "8") { a8.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "9") { a9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));//now
                if (nowthr == "0") { a0.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "1") { a1.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "2") { a2.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "3") { a3.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "4") { a4.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "5") { a5.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "6") { a6.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "7") { a7.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "8") { a8.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "9") { a9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));//now
                if (nowfou == "0") { a0.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "1") { a1.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "2") { a2.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "3") { a3.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "4") { a4.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "5") { a5.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "6") { a6.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "7") { a7.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "8") { a8.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "9") { a9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));//now
                if (nowfiv == "0") { a0.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "1") { a1.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "2") { a2.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "3") { a3.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "4") { a4.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "5") { a5.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "6") { a6.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "7") { a7.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "8") { a8.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "9") { a9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));//now
                if (nowsix == "0") { a0.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "1") { a1.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "2") { a2.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "3") { a3.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "4") { a4.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "5") { a5.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "6") { a6.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "7") { a7.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "8") { a8.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "9") { a9.Draw(Direct3DProvider.Instance, false); }
                return PatchInvokationResult.DoNothing(e);
            };
            Model CreatePowerModel(string notch)
            {
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\P"+notch+".png");
                RectangleF rectangleF = new RectangleF(0 / 2, 0 / 2, 150, -225);
                Model powerNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return powerNotch;
            }
            Model CreateBrakeModel(string  notch)
            {
                width = Direct3DProvider.Instance.PresentParameters.BackBufferWidth;
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\B" + notch + ".png");
                RectangleF rectangleF = new RectangleF(-150, 0 / 2, 150, -225);
                Model brakeNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return brakeNotch;
            }
            Model CreateArvModel ()
            {
                width = Direct3DProvider.Instance.PresentParameters.BackBufferWidth;
                height = Direct3DProvider.Instance.PresentParameters.BackBufferHeight;
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\arrive\arv.png");
                RectangleF rectangleF = new RectangleF(-width/4,  -height/ 2, 180, -100);
                Model brakeNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return brakeNotch;
            }
            Model CreateTimeModel(string type,int number,float x,float y)//第一引数にはarrive/nowで入れる
            {
                width = Direct3DProvider.Instance.PresentParameters.BackBufferWidth;
                height = Direct3DProvider.Instance.PresentParameters.BackBufferHeight;
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\"+type+@"\"+number+".png");
                RectangleF rectangleF = new RectangleF(0-x / 4, 0-y / 2, 180, -100);
                Model timeModel = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return timeModel;
            }
            Model CreateNowModel()
            {
                width = Direct3DProvider.Instance.PresentParameters.BackBufferWidth;
                height = Direct3DProvider.Instance.PresentParameters.BackBufferHeight;
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\now\now.png");
                RectangleF rectangleF = new RectangleF(-width / 4, -height / 2, 180, -100);
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
            TimeDraw timeDraw = new TimeDraw();
            var station = BveHacker.Scenario.Route.Stations[index] as Station;
            if (station == null)
            {
                arrive = station.ArrivalTimeMilliseconds;
                past = station.DepartureTimeMilliseconds;
                pass = station.Pass;
                
            }
            timeDraw.arrive = station.DepartureTime.ToString("hhmmss");
            timeDraw.now = BveHacker.Scenario.TimeManager.Time.ToString("hhmmss");
            arrione = timeDraw.arrive.Substring(0,1);
            arritwo = timeDraw.arrive.Substring(1, 1);
            arrithr = timeDraw.arrive.Substring(2, 1);
            arrifou = timeDraw.arrive.Substring(3, 1);
            arrifiv = timeDraw.arrive.Substring(4, 1);
            arrisix = timeDraw.arrive.Substring(5, 1);
            nowone = timeDraw.now.Substring(0, 1);
            nowtwo = timeDraw.now.Substring(1, 1);
            nowthr = timeDraw.now.Substring(2, 1);
            nowfou = timeDraw.now.Substring(3, 1); 
            nowfiv = timeDraw.now.Substring(4, 1);
            nowsix = timeDraw.now.Substring(5, 1);
            Life life = new Life();
            //int lifetime = life.life;
            index = BveHacker.Scenario.Route.Stations.CurrentIndex + 1;//Index
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
