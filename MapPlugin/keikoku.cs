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
using System.Threading.Tasks;
using System.Threading;
using static System.Collections.Specialized.BitVector32;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace MetroDrive.MapPlugin
{
    internal class Keikoku
    {
        Device device;
        int width;
        int height;
        Model eB;
        Model eBStop;
        Model over;
        Model restart;
        Model stopSoon;
        Model leave;
        Model h16;
        Model h17;
        Model h18;
        Model h19;
        Model pass;
        Model stop;
        Model gameover;

        Model bokutei;
        Thread thread;
        bool isDrawing;
        string drawingName;
        string stationName;
        bool isGameOver;
        int endTimer = -1000;
        public void CreateModel(string Location)
        {
            eB = CreateModels(@"picture\keikoku\EB.png",-150,-60,300,120);
            eBStop = CreateModels(@"picture\keikoku\EBStop.png", -210, -60, 420, 120);
            over = CreateModels(@"picture\keikoku\over.png", -150, -60, 300, 120);
            restart = CreateModels(@"picture\keikoku\restart.png", -210, -60, 420, 120);
            stopSoon= CreateModels(@"picture\keikoku\stopsoon.png", -250, -30, 500, 60);
            //ダイアログ
            bokutei = CreateModels(@"picture\station\bokutei.png", -175, -30, 350, 60);
            //駅系
            leave = CreateModels(@"picture\station\leave.png", 0, -60, 240, 120);
            pass = CreateModels(@"picture\station\pass.png", 0, -60, 240, 120);
            stop = CreateModels(@"picture\station\stop.png", 0, -60, 240, 120);
            h16 = CreateModels(@"picture\station\h16.png", -230, -60, 300, 120);
            h17 = CreateModels(@"picture\station\h17.png", -230, -60, 300, 120);
            h18 = CreateModels(@"picture\station\h18.png", -230, -60, 300, 120);
            h19 = CreateModels(@"picture\station\h19.png", -230, -60, 300, 120);
            gameover = CreateModels(@"picture\station\over.png", -275, -75, 550, 150);
            Model CreateModels(string path, float x, float y, float sizex, float sizey)
            {
                string texFilePath = Path.Combine(Path.GetDirectoryName(Location), path);
                RectangleF rectangleF = new RectangleF(x, y, sizex, -sizey);
                Model brakeNotch = Model.CreateRectangleWithTexture(rectangleF, 0, 0, texFilePath);//四角形の3Dモデル
                return brakeNotch;
            }
        }
        public void Patch(bool isEB,bool isEBStop,bool isOver,bool isRestart,double speed)
        {
            device = Direct3DProvider.Instance.Device;
            width = Direct3DProvider.Instance.PresentParameters.BackBufferWidth;
            height = Direct3DProvider.Instance.PresentParameters.BackBufferHeight;
            if (isGameOver)
            {
                if (!(endTimer == 0))
                {
                    endTimer = endTimer + 10;
                }
                device.SetTransform(TransformState.World, Matrix.Translation(endTimer, 300, 0));
                gameover.Draw(Direct3DProvider.Instance, false);
                isEB = false;
                isEBStop = false;
                isOver = false;
                isRestart = false;
                isDrawing = false;
            }
            device.SetTransform(TransformState.World, Matrix.Translation(0 , 300 , 0));
            if(isEB)
            {
                eB.Draw(Direct3DProvider.Instance, false);
            }
            if(isEBStop)
            {
                eBStop.Draw(Direct3DProvider.Instance, false);
            }
            if(isOver)
            {
                over.Draw(Direct3DProvider.Instance, false);
                if(speed > 0)
                {
                    device.SetTransform(TransformState.World, Matrix.Translation(0, 200, 0));
                    stopSoon.Draw(Direct3DProvider.Instance, false);
                }
            }
            if(isRestart)
            {
                device.SetTransform(TransformState.World, Matrix.Translation(0, 300, 0));
                restart.Draw(Direct3DProvider.Instance, false);
            }
            if(isDrawing)
            {
                device.SetTransform(TransformState.World, Matrix.Translation(0, 300, 0));
                switch (drawingName)
                {
                    case "bokutei":
                        bokutei.Draw(Direct3DProvider.Instance, false);
                    break;
                    case "leave":
                        leave.Draw(Direct3DProvider.Instance, false);
                    break;
                    case "pass":
                        pass.Draw(Direct3DProvider.Instance, false);
                    break;
                    case "stop":
                        stop.Draw(Direct3DProvider.Instance, false);
                    break;
                }
                device.SetTransform(TransformState.World, Matrix.Translation(0, 300, 0));
                switch (stationName)
                {
                    case "入谷":
                        h19.Draw(Direct3DProvider.Instance, false);
                        break;
                    case "上野":
                        h18.Draw(Direct3DProvider.Instance, false);
                    break;
                    case "仲御徒町":
                        h17.Draw(Direct3DProvider.Instance, false);
                    break;
                    case "秋葉原":
                        h16.Draw(Direct3DProvider.Instance, false);
                    break;
                }
            }
            
        }
        public void DrawDialog(string model,string station,int stoptime)
        {
            thread = new Thread(() => DrawingMain(model,station,stoptime));
            thread.Start();
        }
        public void GameOver(string sharedmes)
        {
            Thread thread2 = new Thread(() => GameOverDraw(sharedmes));
            thread2.Start();
        }
        async void GameOverDraw(string sharedmes)
        {
            isGameOver = true;
            await Task.Delay(5000);
            sharedmes = "end";
        }
        async void DrawingMain(string model, string station,int dialogTimer)
        {
            isDrawing = true;
            drawingName = model;
            stationName = station;
            await Task.Delay(dialogTimer);
            isDrawing = false;
        }
    }
}

