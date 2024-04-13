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
        public void CreateModel()
        {
            TimeDrawer timeDrawer = new TimeDrawer();
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
            Model CreatePowerModel(string notch)
            {
                string texFilePath = Path.Combine(Path.GetDirectoryName(timeDrawer.Location), @"picture\P" + notch + ".png");
                RectangleF rectangleF = new RectangleF(0 / 2, 0 / 2, 150, -225);
                Model powerNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return powerNotch;
            }
            Model CreateBrakeModel(string notch)
            { 
                string texFilePath = Path.Combine(Path.GetDirectoryName(timeDrawer.Location), @"picture\B" + notch + ".png");
                RectangleF rectangleF = new RectangleF(-150, 0 / 2, 150, -225);
                Model brakeNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return brakeNotch;
            }
        }
        public void UIDraw()
        {
            Life life = new Life();
            TimeDrawer timeDrawer = new TimeDrawer();
            int width = Direct3DProvider.Instance.PresentParameters.BackBufferWidth;
            int height = Direct3DProvider.Instance.PresentParameters.BackBufferHeight;
            Device device = Direct3DProvider.Instance.Device;
            device.SetTransform(TransformState.World, Matrix.Translation(-width / 2, height / 2, 0));
            if (life.power == 0) { p0.Draw(Direct3DProvider.Instance, false); }
            if (life.power == 1) { p1.Draw(Direct3DProvider.Instance, false); }
            if (life.power == 2) { p2.Draw(Direct3DProvider.Instance, false); }
            if (life.power == 3) { p3.Draw(Direct3DProvider.Instance, false); }
            if (life.power == 4) { p4.Draw(Direct3DProvider.Instance, false); }
            device.SetTransform(TransformState.World, Matrix.Translation(width / 2, height / 2, 0));//(float)Math.Sin(Native.VehicleState.Time.TotalSeconds)*100
            if (life.brake == 0) { b0.Draw(Direct3DProvider.Instance, false); }
            if (life.brake == 1) { b1.Draw(Direct3DProvider.Instance, false); }
            if (life.brake == 2) { b2.Draw(Direct3DProvider.Instance, false); }
            if (life.brake == 3) { b3.Draw(Direct3DProvider.Instance, false); }
            if (life.brake == 4) { b4.Draw(Direct3DProvider.Instance, false); }
            if (life.brake == 5) { b5.Draw(Direct3DProvider.Instance, false); }
            if (life.brake == 6) { b6.Draw(Direct3DProvider.Instance, false); }
            if (life.brake == 7) { b7.Draw(Direct3DProvider.Instance, false); }
            if (life.brake == 8) { b8.Draw(Direct3DProvider.Instance, false); }
            if (life.brake == life.EB) { eb.Draw(Direct3DProvider.Instance, false); }
        }
    }


}