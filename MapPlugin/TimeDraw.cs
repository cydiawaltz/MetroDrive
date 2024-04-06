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
using System.Data;
using AtsEx.PluginHost;
using System.Xml.Linq;

namespace MetroDrive
{
    internal class TimeDraw
    {
        public string now;
        public string arrive;
       
        public void DrawModel()
        {
            Device device = Direct3DProvider.Instance.Device;
            //device.SetTransform(TransformState.World, Matrix.Translation(width / 2, height / 2, 0));
        }
        //0~9.:を読み込む（pと同じ）
        //Native.VehicleState.Timeから
       
    }
}
