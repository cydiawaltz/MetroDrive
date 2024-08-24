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
using AtsEx.PluginHost;
using System.Reflection;

namespace MetroDrive
{
    internal class TimeDrawer//このクラスでは、
    {
        //public string Location;
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
        public string next;
        public string nextone;//一桁目(前から数えるので)
        public string nexttwo;
        public string nextthr;
        public string nextfou;
        public Model a0;
        public Model a1;
        public Model a2;
        public Model a3;
        public Model a4;
        public Model a5;
        public Model a6;
        public Model a7;
        public Model a8;
        public Model a9;
        public Model n0;
        public Model n1;
        public Model n2;
        public Model n3;
        public Model n4;
        public Model n5;
        public Model n6;
        public Model n7;
        public Model n8;
        public Model n9;
        public Model r0;
        public Model r1;
        public Model r2;
        public Model r3;
        public Model r4;
        public Model r5;
        public Model r6;
        public Model r7;
        public Model r8;
        public Model r9;
        public Model arv;
        public Model nowModel;
        public Model arvColon;
        public Model nowColon;
        public Model ato;
        public Model meter;
        Model over;
        Model teisiiti;
        public void CreateModel(string Location)//Mainの中で呼び出すやつ
        {
            //string testTexPath = Path.Combine(Path.GetDirectoryName(Location),@"picture\aaa.jpg");//DLLが入っているフォルダまで
            //Model test = Model.CreateRectangleWithTexture(rectangleF, 0, 0,testTexPath);//四角形の3Dモデル
            //時間系(arri)
            a0 = CreateTimeModel("arrive", 0, 0, 0);//SetTransformで横の位置は変更するゾ
            a1 = CreateTimeModel("arrive", 1, 0, 0);
            a2 = CreateTimeModel("arrive", 2, 0, 0);
            a3 = CreateTimeModel("arrive", 3, 0, 0);
            a4 = CreateTimeModel("arrive", 4, 0, 0);
            a5 = CreateTimeModel("arrive", 5, 0, 0);
            a6 = CreateTimeModel("arrive", 6, 0, 0);
            a7 = CreateTimeModel("arrive", 7, 0, 0);
            a8 = CreateTimeModel("arrive", 8, 0, 0);
            a9 = CreateTimeModel("arrive", 9, 0, 0);
            //(now)
            n0 = CreateTimeModel("now", 0, 0, 0);
            n1 = CreateTimeModel("now", 1, 0, 0);
            n2 = CreateTimeModel("now", 2, 0, 0);
            n3 = CreateTimeModel("now", 3, 0, 0);
            n4 = CreateTimeModel("now", 4, 0, 0);
            n5 = CreateTimeModel("now", 5, 0, 0);
            n6 = CreateTimeModel("now", 6, 0, 0);
            n7 = CreateTimeModel("now", 7, 0, 0);
            n8 = CreateTimeModel("now", 8, 0, 0);
            n9 = CreateTimeModel("now", 9, 0, 0);
            //残り距離
            r0 = CreateAnyModel(@"picture\remain\0.png", 0, 0, 40, 80);
            r1 = CreateAnyModel(@"picture\remain\1.png", 20, 0, 20, 80);
            r2 = CreateAnyModel(@"picture\remain\2.png", 0, 0, 40, 80);
            r3 = CreateAnyModel(@"picture\remain\3.png", 0, 0, 40, 80);
            r4 = CreateAnyModel(@"picture\remain\4.png", 0, 0, 40, 80);
            r5 = CreateAnyModel(@"picture\remain\5.png", 0, 0, 40, 80);
            r6 = CreateAnyModel(@"picture\remain\6.png", 0, 0, 40, 80);
            r7 = CreateAnyModel(@"picture\remain\7.png", 0, 0, 40, 80);
            r8 = CreateAnyModel(@"picture\remain\8.png", 0, 0, 40, 80);
            r9 = CreateAnyModel(@"picture\remain\9.png", 0, 0, 40, 80);
            //固定UI
            arv = CreateArvModel();
            nowModel = CreateNowModel();
            arvColon = CreateAnyModel(@"picture\arrive\colon.png", 0, 0, 30, 60);
            nowColon = CreateAnyModel(@"picture\now\colon.png", 0, 0, 30, 60);
            ato = CreateAnyModel(@"picture\remain\ato.png", 0, 0, 150, 80);
            over = CreateAnyModel(@"picture\remain\over.png", 0, 0, 200, 80);
            teisiiti = CreateAnyModel(@"picture\remain\teisiiti.png", 0, 0, 250, 80);
            meter = CreateAnyModel(@"picture\remain\m.png", 0, 0, 50, 60);
            
            Model CreateArvModel()
            {
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\arrive\arv.png");
                RectangleF rectangleF = new RectangleF(0 / 4, 0 / 2, 150, -80);
                Model brakeNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return brakeNotch;
            }
            Model CreateTimeModel(string type, int number, float x, float y)//第一引数にはarrive/nowで入れる
            {
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\" + type + @"\" + number + ".png");
                RectangleF rectangleF = new RectangleF(0 - x / 4, 0 - y / 2, 40, -60);
                Model timeModel = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return timeModel;
            }
            Model CreateNowModel()
            {
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\now\now.png");
                RectangleF rectangleF = new RectangleF(0 / 4, 0 / 2, 150, -80);
                Model brakeNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return brakeNotch;
            }
            Model CreateAnyModel(string path, float x, float y, float sizex, float sizey)
            {
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), path);
                RectangleF rectangleF = new RectangleF(x, y, sizex, -sizey);
                Model brakeNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return brakeNotch;
            }
        }
        public void Patch(double NeXTLocation,double nowLocation,bool isUIOff,double Goukakuhani)//まずこれを呼ぶ
        {
            if(isUIOff == false)
            {
                width = Direct3DProvider.Instance.PresentParameters.BackBufferWidth;
                height = Direct3DProvider.Instance.PresentParameters.BackBufferHeight;
                //3D modelをどこに配置するのか指定するかんじ device.settransform(文字略) 頂点位置の変換
                Device device = Direct3DProvider.Instance.Device;
                device.SetTransform(TransformState.View, Matrix.Identity);
                device.SetTransform(TransformState.Projection, Matrix.OrthoOffCenterLH(-width / 2, width / 2, -height / 2, height / 2, 0, 1));
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 450, -height / 2+90, 0));
                arv.Draw(Direct3DProvider.Instance, false);
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 450, -height / 2+170, 0));
                nowModel.Draw(Direct3DProvider.Instance, false);
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 300, -height / 2 + 80 , 0));//arrive１個目
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
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 260, -height / 2 + 80, 0));//arrive２個め
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
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 230, -height / 2 + 80, 0));
                arvColon.Draw(Direct3DProvider.Instance, false);
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 200, -height / 2 + 80, 0));//arrive３個目
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
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 160, -height / 2 + 80, 0));//now
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
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 130, -height /2+80, 0));//now
                arvColon.Draw(Direct3DProvider.Instance, false);
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 100, -height / 2 + 80, 0));//now
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
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 60, -height / 2 + 80, 0));//now
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
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 300, -height / 2+160, 0));//now
                if (nowone == "0") { n0.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "1") { n1.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "2") { n2.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "3") { n3.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "4") { n4.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "5") { n5.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "6") { n6.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "7") { n7.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "8") { n8.Draw(Direct3DProvider.Instance, false); }
                if (nowone == "9") { n9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 260, -height / 2+160, 0));//now
                if (nowtwo == "0") { n0.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "1") { n1.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "2") { n2.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "3") { n3.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "4") { n4.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "5") { n5.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "6") { n6.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "7") { n7.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "8") { n8.Draw(Direct3DProvider.Instance, false); }
                if (nowtwo == "9") { n9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 230, -height / 2+160, 0));
                nowColon.Draw(Direct3DProvider.Instance, false);
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 200, -height / 2+160, 0));//now
                if (nowthr == "0") { n0.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "1") { n1.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "2") { n2.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "3") { n3.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "4") { n4.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "5") { n5.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "6") { n6.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "7") { n7.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "8") { n8.Draw(Direct3DProvider.Instance, false); }
                if (nowthr == "9") { n9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 160, -height / 2+160, 0));//now
                if (nowfou == "0") { n0.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "1") { n1.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "2") { n2.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "3") { n3.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "4") { n4.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "5") { n5.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "6") { n6.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "7") { n7.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "8") { n8.Draw(Direct3DProvider.Instance, false); }
                if (nowfou == "9") { n9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 130, -height / 2+160, 0));
                nowColon.Draw(Direct3DProvider.Instance, false);
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 100, -height / 2+160, 0));//now
                if (nowfiv == "0") { n0.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "1") { n1.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "2") { n2.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "3") { n3.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "4") { n4.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "5") { n5.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "6") { n6.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "7") { n7.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "8") { n8.Draw(Direct3DProvider.Instance, false); }
                if (nowfiv == "9") { n9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 60, -height / 2+160, 0));//now
                if (nowsix == "0") { n0.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "1") { n1.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "2") { n2.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "3") { n3.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "4") { n4.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "5") { n5.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "6") { n6.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "7") { n7.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "8") { n8.Draw(Direct3DProvider.Instance, false); }
                if (nowsix == "9") { n9.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2 + 10, -height / 2 + 180, 0));
                if(Math.Abs(nowLocation - NeXTLocation)<Goukakuhani)
                {
                    teisiiti.Draw(Direct3DProvider.Instance, false);
                }
                else
                {
                    if (NeXTLocation < nowLocation)
                    {
                        over.Draw(Direct3DProvider.Instance, false);
                    }
                    else
                    {
                        ato.Draw(Direct3DProvider.Instance, false);
                    }
                }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2 + 200, -height / 2 +60, 0));
                meter.Draw(Direct3DProvider.Instance, false);
                if (Math.Abs(NeXTLocation - nowLocation) >= 1000)
                {
                    device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, -height / 2 + 100, 0));
                    if (nextone == "0") { r0.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "1") { r1.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "2") { r2.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "3") { r3.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "4") { r4.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "5") { r5.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "6") { r6.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "7") { r7.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "8") { r8.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "9") { r9.Draw(Direct3DProvider.Instance, false); }
                    device.SetTransform(TransformState.World, Matrix.Translation(-width / 2 + 50, -height / 2 + 100, 0));
                    if (nexttwo == "0") { r0.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "1") { r1.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "2") { r2.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "3") { r3.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "4") { r4.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "5") { r5.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "6") { r6.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "7") { r7.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "8") { r8.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "9") { r9.Draw(Direct3DProvider.Instance, false); }
                    device.SetTransform(TransformState.World, Matrix.Translation(-width / 2 + 100, -height / 2 + 100, 0));
                    if (nextthr == "0") { r0.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "1") { r1.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "2") { r2.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "3") { r3.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "4") { r4.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "5") { r5.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "6") { r6.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "7") { r7.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "8") { r8.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "9") { r9.Draw(Direct3DProvider.Instance, false); }
                    device.SetTransform(TransformState.World, Matrix.Translation(-width / 2 + 150, -height / 2 + 100, 0));
                    if (nextfou == "0") { r0.Draw(Direct3DProvider.Instance, false); }
                    if (nextfou == "1") { r1.Draw(Direct3DProvider.Instance, false); }
                    if (nextfou == "2") { r2.Draw(Direct3DProvider.Instance, false); }
                    if (nextfou == "3") { r3.Draw(Direct3DProvider.Instance, false); }
                    if (nextfou == "4") { r4.Draw(Direct3DProvider.Instance, false); }
                    if (nextfou == "5") { r5.Draw(Direct3DProvider.Instance, false); }
                    if (nextfou == "6") { r6.Draw(Direct3DProvider.Instance, false); }
                    if (nextfou == "7") { r7.Draw(Direct3DProvider.Instance, false); }
                    if (nextfou == "8") { r8.Draw(Direct3DProvider.Instance, false); }
                    if (nextfou == "9") { r9.Draw(Direct3DProvider.Instance, false); }
                }
                if (Math.Abs(NeXTLocation - nowLocation) < 1000 && Math.Abs(NeXTLocation - nowLocation) >= 100)
                {
                    device.SetTransform(TransformState.World, Matrix.Translation(-width / 2 + 50, -height / 2 + 100, 0));
                    if (nextone == "0") { r0.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "1") { r1.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "2") { r2.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "3") { r3.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "4") { r4.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "5") { r5.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "6") { r6.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "7") { r7.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "8") { r8.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "9") { r9.Draw(Direct3DProvider.Instance, false); }
                    device.SetTransform(TransformState.World, Matrix.Translation(-width / 2 + 100, -height / 2 + 100, 0));
                    if (nexttwo == "0") { r0.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "1") { r1.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "2") { r2.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "3") { r3.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "4") { r4.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "5") { r5.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "6") { r6.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "7") { r7.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "8") { r8.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "9") { r9.Draw(Direct3DProvider.Instance, false); }
                    device.SetTransform(TransformState.World, Matrix.Translation(-width / 2 + 150, -height / 2 + 100, 0));
                    if (nextthr == "0") { r0.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "1") { r1.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "2") { r2.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "3") { r3.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "4") { r4.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "5") { r5.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "6") { r6.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "7") { r7.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "8") { r8.Draw(Direct3DProvider.Instance, false); }
                    if (nextthr == "9") { r9.Draw(Direct3DProvider.Instance, false); }
                }
                if (Math.Abs(NeXTLocation - nowLocation) < 100 && Math.Abs(NeXTLocation - nowLocation) >= 10)
                {
                    device.SetTransform(TransformState.World, Matrix.Translation(-width / 2 + 100, -height / 2 + 100, 0));
                    if (nextone == "0") { r0.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "1") { r1.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "2") { r2.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "3") { r3.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "4") { r4.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "5") { r5.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "6") { r6.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "7") { r7.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "8") { r8.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "9") { r9.Draw(Direct3DProvider.Instance, false); }
                    device.SetTransform(TransformState.World, Matrix.Translation(-width / 2 + 150, -height / 2 + 100, 0));
                    if (nexttwo == "0") { r0.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "1") { r1.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "2") { r2.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "3") { r3.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "4") { r4.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "5") { r5.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "6") { r6.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "7") { r7.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "8") { r8.Draw(Direct3DProvider.Instance, false); }
                    if (nexttwo == "9") { r9.Draw(Direct3DProvider.Instance, false); }
                }
                if (Math.Abs(NeXTLocation - nowLocation) < 10)
                {
                    device.SetTransform(TransformState.World, Matrix.Translation(-width / 2 + 150, -height / 2 + 100, 0));
                    if (nextone == "0") { r0.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "1") { r1.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "2") { r2.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "3") { r3.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "4") { r4.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "5") { r5.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "6") { r6.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "7") { r7.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "8") { r8.Draw(Direct3DProvider.Instance, false); }
                    if (nextone == "9") { r9.Draw(Direct3DProvider.Instance, false); }
                }
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2 + 150, -height / 2 + 100, 0));

            }
        }
        //500行超えるのでLife２関しての記述はUIDrawerにて
        //Native.VehicleState.Timeから
        public TickResult Tick(int index,string now,double NeXTLocation,double nowLocation,string arrive)
        {
            //var station = BveHacker.Scenario.Route.Stations[index] as Station;
            //arrive = station.DepartureTime.ToString("hhmmss");
            //now = BveHacker.Scenario.TimeManager.Time.ToString("hhmmss");
            arrione = arrive.Substring(0, 1);
            arritwo = arrive.Substring(1, 1);
            arrithr = arrive.Substring(2, 1);
            arrifou = arrive.Substring(3, 1);
            arrifiv = arrive.Substring(4, 1);
            arrisix = arrive.Substring(5, 1);
            nowone = now.Substring(0, 1);
            nowtwo = now.Substring(1, 1);
            nowthr = now.Substring(2, 1);
            nowfou = now.Substring(3, 1);
            nowfiv = now.Substring(4, 1);
            nowsix = now.Substring(5, 1);
            next = ((int)Math.Abs(NeXTLocation - nowLocation)).ToString();
            nextone = next[0].ToString();
            if (NeXTLocation - nowLocation >= 10) { nexttwo = next[1].ToString(); }
            if (NeXTLocation - nowLocation >= 100) { nextthr = next[2].ToString(); }
            if (NeXTLocation - nowLocation >= 1000) { nextfou = next[3].ToString(); }
            else { nextfou = "nothing"; }
            return new MapPluginTickResult();
        }
    }
}
