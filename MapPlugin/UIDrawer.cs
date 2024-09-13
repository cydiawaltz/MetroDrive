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
    internal class UIDrawer
    {
        public Model p0;
        public Model p1;
        public Model p2;
        public Model p3;
        public Model p4;
        public Model b0;
        public Model b1;
        public Model b2;
        public Model b3;
        public Model b4;
        public Model b5;
        public Model b6;
        public Model b7;
        public Model b8;
        public Model eb;
        Model teituu;
        Model lifeModel;
        Model l0;
        Model l1;
        Model l2;
        Model l3;
        Model l4;
        Model l5;
        Model l6;
        Model l7;
        Model l8;
        Model l9;
        string lifeone;
        string lifetwo;
        string lifethree;
        public void CreateModel(string Location)
        {
            p0 = CreatePowerModel("0");
            p1 = CreatePowerModel("1");
            p2 = CreatePowerModel("2");
            p3 = CreatePowerModel("3");
            p4 = CreatePowerModel("4");
            //brake
            b0 = CreateBrakeModel("0");
            b1 = CreateBrakeModel("1");
            b2 = CreateBrakeModel("2");
            b3 = CreateBrakeModel("3");
            b4 = CreateBrakeModel("4");
            b5 = CreateBrakeModel("5");
            b6 = CreateBrakeModel("6");
            b7 = CreateBrakeModel("7");
            b8 = CreateBrakeModel("8");
            eb = CreateBrakeModel("9");
            teituu = CreateAnyModel(@"picture\UI\teituu.png", 0, 0, 400, 150);
            lifeModel = CreateAnyModel(@"picture\life\life.png", 0, 0, 150, 80);
            l0 = CreatelifeModel("0");
            l1 = CreatelifeModel("1");
            l2 = CreatelifeModel("2");
            l3 = CreatelifeModel("3");
            l4 = CreatelifeModel("4");
            l5 = CreatelifeModel("5");
            l6 = CreatelifeModel("6");
            l7 = CreatelifeModel("7");
            l8 = CreatelifeModel("8");
            l9 = CreatelifeModel("9");
            Model CreatePowerModel(string notch)
            {
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\P" + notch + ".png");
                RectangleF rectangleF = new RectangleF(0 / 2, 0 / 2, 150, -225);
                Model powerNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return powerNotch;
            }
            Model CreateBrakeModel(string notch)
            { 
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\B" + notch + ".png");
                RectangleF rectangleF = new RectangleF(-150, 0 / 2, 150, -225);
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
            Model CreatelifeModel(string num)
            {
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), @"picture\life\" + num + ".png");
                RectangleF rectangleF = new RectangleF(-150, 0 / 2, 40, -80);
                Model brakeNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return brakeNotch;
            }
        }
        public void UIDraw(int power,int brake,int EB,bool isTeituu,int life,bool isUIOff)
        {
            if(isUIOff == false)
            {
                int width = Direct3DProvider.Instance.PresentParameters.BackBufferWidth;
                int height = Direct3DProvider.Instance.PresentParameters.BackBufferHeight;
                Device device = Direct3DProvider.Instance.Device;
                device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));
                if (power == 0) { p0.Draw(Direct3DProvider.Instance, false); }
                if (power == 1) { p1.Draw(Direct3DProvider.Instance, false); }
                if (power == 2) { p2.Draw(Direct3DProvider.Instance, false); }
                if (power == 3) { p3.Draw(Direct3DProvider.Instance, false); }
                if (power == 4) { p4.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2, height / 2, 0));//(float)Math.Sin(Native.VehicleState.Time.TotalSeconds)*100
                if (brake == 0) { b0.Draw(Direct3DProvider.Instance, false); }
                if (brake == 1) { b1.Draw(Direct3DProvider.Instance, false); }
                if (brake == 2) { b2.Draw(Direct3DProvider.Instance, false); }
                if (brake == 3) { b3.Draw(Direct3DProvider.Instance, false); }
                if (brake == 4) { b4.Draw(Direct3DProvider.Instance, false); }
                if (brake == 5) { b5.Draw(Direct3DProvider.Instance, false); }
                if (brake == 6) { b6.Draw(Direct3DProvider.Instance, false); }
                if (brake == 7) { b7.Draw(Direct3DProvider.Instance, false); }
                if (brake == 8) { b8.Draw(Direct3DProvider.Instance, false); }
                if (brake == EB) { eb.Draw(Direct3DProvider.Instance, false); }
                device.SetTransform(TransformState.World, Matrix.Translation(0 / 2 - 200, 0 / 2 + 150, 0));
                if (isTeituu == true) { teituu.Draw(Direct3DProvider.Instance, false); }
            }
        }
        public void LifeDraw(int life,bool isUIOff)
        {
            if(isUIOff == false)
            {
                Device device = Direct3DProvider.Instance.Device;
                int width = Direct3DProvider.Instance.PresentParameters.BackBufferWidth;
                int height = Direct3DProvider.Instance.PresentParameters.BackBufferHeight;
                if (life >= 100)
                {
                    device.SetTransform(TransformState.World, Matrix.Translation(width / 2-150, -height / 2+250, 0));
                    if (lifeone == "0") { l0.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "1") { l1.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "2") { l2.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "3") { l3.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "4") { l4.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "5") { l5.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "6") { l6.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "7") { l7.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "8") { l8.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "9") { l9.Draw(Direct3DProvider.Instance, false); }
                    device.SetTransform(TransformState.World, Matrix.Translation(width / 2-110, -height / 2+250, 0));
                    if (lifetwo == "0") { l0.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "1") { l1.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "2") { l2.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "3") { l3.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "4") { l4.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "5") { l5.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "6") { l6.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "7") { l7.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "8") { l8.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "9") { l9.Draw(Direct3DProvider.Instance, false); }
                    device.SetTransform(TransformState.World, Matrix.Translation(width / 2-70, -height / 2+250, 0));
                    if (lifethree == "0") { l0.Draw(Direct3DProvider.Instance, false); }
                    if (lifethree == "1") { l1.Draw(Direct3DProvider.Instance, false); }
                    if (lifethree == "2") { l2.Draw(Direct3DProvider.Instance, false); }
                    if (lifethree == "3") { l3.Draw(Direct3DProvider.Instance, false); }
                    if (lifethree == "4") { l4.Draw(Direct3DProvider.Instance, false); }
                    if (lifethree == "5") { l5.Draw(Direct3DProvider.Instance, false); }
                    if (lifethree == "6") { l6.Draw(Direct3DProvider.Instance, false); }
                    if (lifethree == "7") { l7.Draw(Direct3DProvider.Instance, false); }
                    if (lifethree == "8") { l8.Draw(Direct3DProvider.Instance, false); }
                    if (lifethree == "9") { l9.Draw(Direct3DProvider.Instance, false); }
                }
                if (life > 10 && life < 100)
                {
                    device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 110, -height / 2+250, 0));
                    if (lifeone == "0") { l0.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "1") { l1.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "2") { l2.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "3") { l3.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "4") { l4.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "5") { l5.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "6") { l6.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "7") { l7.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "8") { l8.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "9") { l9.Draw(Direct3DProvider.Instance, false); }
                    device.SetTransform(TransformState.World, Matrix.Translation(width / 2 -70, -height / 2 + 250, 0));
                    if (lifetwo == "0") { l0.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "1") { l1.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "2") { l2.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "3") { l3.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "4") { l4.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "5") { l5.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "6") { l6.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "7") { l7.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "8") { l8.Draw(Direct3DProvider.Instance, false); }
                    if (lifetwo == "9") { l9.Draw(Direct3DProvider.Instance, false); }
                }
                if(life == 10)
                {
                    device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 110, -height / 2 + 250, 0));
                    l1.Draw(Direct3DProvider.Instance, false);
                    device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 70, -height / 2 + 250, 0));
                    l0.Draw(Direct3DProvider.Instance, false);
                }
                if (life < 10)
                {
                    device.SetTransform(TransformState.World, Matrix.Translation(width / 2-110, -height / 2+250, 0));
                    l0.Draw(Direct3DProvider.Instance, false);
                    device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 70, -height / 2+250, 0));
                    if (lifeone == "0") { l0.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "1") { l1.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "2") { l2.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "3") { l3.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "4") { l4.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "5") { l5.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "6") { l6.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "7") { l7.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "8") { l8.Draw(Direct3DProvider.Instance, false); }
                    if (lifeone == "9") { l9.Draw(Direct3DProvider.Instance, false); }
                }
                if(life<=0)
                {
                    device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 110, -height / 2 + 250, 0));
                    l0.Draw(Direct3DProvider.Instance, false);
                    device.SetTransform(TransformState.World, Matrix.Translation(width / 2 - 70, -height / 2 + 250, 0));
                    l0.Draw(Direct3DProvider.Instance, false);
                }
                device.SetTransform(TransformState.World, Matrix.Translation(width / 2 -450, -height / 2 + 250, 0));
                lifeModel.Draw(Direct3DProvider.Instance, false);
            }
        }
        public TickResult tick(int life)
        {
            if(life > 0)
            {
                lifeone = life.ToString().Substring(0, 1);
            }
            if(life >10)
            {
                lifetwo = life.ToString().Substring(1, 1);
            }
            if(life >100)
            {
                lifethree = life.ToString().Substring(2, 1);
            }
            return new MapPluginTickResult();
        }
    }
}