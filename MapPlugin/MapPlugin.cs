using System;
using AtsEx.PluginHost.Plugins;
using BveTypes.ClassWrappers;
using FastMember;
using TypeWrapping;
using ObjectiveHarmonyPatch;
using AtsEx.PluginHost.Native;
using AtsEx.PluginHost.MapStatements;
using System.Collections.Generic;
using Mackoy.Bvets;
using System.Timers;
using System.IO.Pipes;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;

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
        Pause pause;
        int atc;
        bool hideHorn;
        //フラグ
        bool isOverATC;
        bool isDelay;
        bool isEB;
        bool isTeituu;
        bool isGood;
        bool isGreat;
        bool isEBStop;
        bool isOverRun;
        bool isRestart;
        bool isFixOver;//「停止位置を修正します」みたいな感じのウィザード
        int time;
        bool isBonus;//ボーナスがある時はこれで時刻表を追加する
        bool isTimeOut;
        bool isTaiken;
        int pointerIndex;
        bool isEnterPressed;
        bool isExitScenario;
        bool isVoiceOn;
        int addStaPosi;
        int addStaArrival;
        bool isUIOff;
        bool isGame;//ゲーム実行中意外はFormをオフに
        bool isPause;
        Task task;
        int timer;
        public MapPluginMain(PluginBuilder builder) : base(builder)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Launch();
            }
            life = new Life();
            timeDrawer = new TimeDrawer();
            uIDrawer = new UIDrawer();
            pause = new Pause();
            Native.BeaconPassed += new BeaconPassedEventHandler(BeaconPassed);
            Native.HornBlown += new HornBlownEventHandler(life.OnHorn);
            BveHacker.MainFormSource.KeyDown += OnKeyDown;
            FlagStart();
            //MemoryMappedFile a = MemoryMappedFile.CreateNew("ScenarioOpen", 4096);
            //sendtounity = a.CreateViewAccessor();
            ClassMemberSet assistantDrawerMembers = BveHacker.BveTypes.GetClassInfoOf<AssistantDrawer>();
            FastMethod drawMethod = assistantDrawerMembers.GetSourceMethodOf(nameof(AssistantDrawer.Draw));
            HarmonyPatch drawPatch = HarmonyPatch.Patch(Name, drawMethod.Source, PatchType.Prefix);
            timeDrawer.CreateModel(Location);
            uIDrawer.CreateModel(Location);
            pause.CreateModel(Location);
            drawPatch.Invoked += (sender, e) =>
            {
                timeDrawer.Patch(NeXTLocation, nowLocation,isUIOff);
                uIDrawer.UIDraw(power, brake, EB, life.isTeituu, life.life,isUIOff);
                uIDrawer.LifeDraw(life.life,isUIOff);
                pause.PauseMenuDrawer(pointerIndex, isEnterPressed, isExitScenario, isVoiceOn,isUIOff);
                return PatchInvokationResult.DoNothing(e);
            };
            Namespace ns = Namespace.GetUserNamespace("CydiaWaltz").Child("MetroDrive");
            Identifier difficulty = new Identifier(ns, "Level");
            IReadOnlyList<IHeader> headers = BveHacker.MapHeaders.GetAll(difficulty);
            for (int i = 0; i < headers.Count; i++)
            {
                IHeader header = headers[i];
                if (header.Name.FullName == "Easy") { life.OnStartEasy(); }
            }
            Identifier bonusCheck = new Identifier(ns, "isBonus");
            IReadOnlyList<IHeader> bheader = BveHacker.MapHeaders.GetAll(bonusCheck);
            for (int i = 0; i < bheader.Count; i++)
            {
                IHeader header = bheader[i];
                if (header.Name.FullName == "true") { isBonus = true; }
                else { isBonus = false; }
            }
            Identifier taikenCheck = new Identifier(ns, "isTaiken");
            IReadOnlyList<IHeader> cheader = BveHacker.MapHeaders.GetAll(taikenCheck);
            for (int i = 0; i < cheader.Count; i++)
            {
                IHeader header = cheader[i];
                if (header.Name.FullName == "true") { isTaiken = true; }
                else { isTaiken = false; }
            }
            Identifier addStationLocation = new Identifier(ns, "addLocation");
            IReadOnlyList<IHeader> dheader = BveHacker.MapHeaders.GetAll(addStationLocation);
            for (int i = 0; i < dheader.Count; i++)
            {
                IHeader header = dheader[i];
                addStaPosi = int.Parse(header.Name.FullName);
            }
            Identifier addStationArrival = new Identifier(ns, "addArrival");
            IReadOnlyList<IHeader> eheader = BveHacker.MapHeaders.GetAll(addStationArrival);
            for (int i = 0; i < eheader.Count; i++)
            {
                IHeader header = eheader[i];
                TimeSpan a = TimeSpan.Parse(header.Name.FullName);
                addStaArrival = (int)a.TotalMilliseconds;
            }
            //計器照明を有効に
            InputEventArgs inputEventArgs = new InputEventArgs(-2, 15);
            BveHacker.KeyProvider.KeyDown_Invoke(inputEventArgs);
            BveHacker.KeyProvider.KeyUp_Invoke(inputEventArgs);
            InputEventArgs inputEventArgs2 = new InputEventArgs(2, 1);
            BveHacker.KeyProvider.KeyDown_Invoke(inputEventArgs2);
            BveHacker.KeyProvider.KeyUp_Invoke(inputEventArgs2);
            //毎秒実行
            System.Timers.Timer timer = new System.Timers.Timer(2000);
            timer.Elapsed += FlagBuilder;
            timer.Start();
            isTimeOut = false;
            MessageBox.Show("MetroDriveプラグインが読み込まれました");
            NamedPipe((int)NeXTLocation);
            isPause= false;
        }
        public override void Dispose()
        {
            Native.BeaconPassed -= BeaconPassed;
            Native.HornBlown -= life.OnHorn;
            //Formを非表示にする処理
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
            index = BveHacker.Scenario.Route.Stations.CurrentIndex + 1;//Index
            nowLocation = Native.VehicleState.Location;//現在位置を設定
            NeXTLocation = BveHacker.Scenario.Route.Stations[index].Location;//次駅位置
            EB = 9;//EB
            //power = BveHacker.Handles.Power.Notch;//PowerNotch
            //brake = BveHacker.Handles.Brake.Notch;//BrakeNotch
            power = Native.Handles.Power.Notch;
            brake = Native.Handles.Brake.Notch;
            nowMilli = BveHacker.Scenario.TimeManager.TimeMilliseconds;//Now
            speed = Native.VehicleState.Speed;//speed
            now = BveHacker.Scenario.TimeManager.Time.ToString("hhmmss");
            arrive = station.DepartureTime.ToString("hhmmss");
            if (brake == 0) { }
            else
            {
                for (int i = 0; i == 3; i++)
                {
                    InputEventArgs inputEventArgs = new InputEventArgs(1, 0);
                    BveHacker.KeyProvider.KeyDown_Invoke(inputEventArgs);
                    BveHacker.KeyProvider.KeyUp_Invoke(inputEventArgs);
                }
            }
            if (pass == true && NeXTLocation == nowLocation)
            {
                OnPass();
            }
            life.NewUpdate(isOverATC, isDelay, isEB, isTeituu, isGood, isGreat, isEBStop, isOverRun, nowLocation, NeXTLocation, isRestart);
            timeDrawer.Tick(index, now, NeXTLocation, nowLocation, arrive);
            uIDrawer.tick(life.life);
            if (isExitScenario)
            {
                //情報を送り、BVEのFormを非表示にする

                BveHacker.MainForm.UnloadScenario();
            }

            return new MapPluginTickResult();
        }
        public void NamedPipe(int writething)
        {
            //名前付きパイプのテスト
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.Out))
            {
                try
                {
                    pipeClient.Connect(1000);
                    using (StreamWriter writer = new StreamWriter(pipeClient))
                    {
                        writer.WriteLine(writething);
                        writer.Flush(); // これによりデータがパイプに即座に書き込まれる
                    }
                }
                catch (TimeoutException)
                {
                    MessageBox.Show("接続がタイムアウトされました。ポーズメニューより「運転終了」を選択した上、ゲームを再起動してください。");
                    isTimeOut = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //名前付きパイプの受信
        public void ReceivePipe()
        {
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.In))
            {
                try
                {
                    pipeClient.Connect(2000);
                    using (StreamReader reader = new StreamReader(pipeClient))
                    {
                        while (true)
                        {
                            string mes = reader.ReadLine();
                            if (mes == null) break;
                            string filedirectory = BveHacker.ScenarioInfo.DirectoryName;
                            /*switch (mes)
                            {
                                case "10"://Unity→BVEは10番台
                                    //BVEのMainFormをオンにする
                                    break;
                                case "11"://シナリオ1の読み込み
                                    BveHacker.MainForm.OpenScenario(Path.Combine(filedirectory, "1.txt"));
                                    break;
                                case "12"://シナリオ1の読み込み
                                    BveHacker.MainForm.OpenScenario(Path.Combine(filedirectory, "2.txt"));
                                    break;
                                case "13"://シナリオ1の読み込み
                                    BveHacker.MainForm.OpenScenario(Path.Combine(filedirectory, "3.txt"));
                                    break;
                                case "14"://シナリオ1の読み込み
                                    BveHacker.MainForm.OpenScenario(Path.Combine(filedirectory, "4.txt"));
                                    break;
                                case "15"://シナリオ1の読み込み
                                    BveHacker.MainForm.OpenScenario(Path.Combine(filedirectory, "5.txt"));
                                    break;
                                case "16"://シナリオ1の読み込み
                                    BveHacker.MainForm.OpenScenario(Path.Combine(filedirectory, "6.txt"));
                                    break;
                                case "17"://シナリオ1の読み込み
                                    BveHacker.MainForm.OpenScenario(Path.Combine(filedirectory, "7.txt"));
                                    break;
                                        case "18"://シナリオ1の読み込み
                                    BveHacker.MainForm.OpenScenario(Path.Combine(filedirectory, "8.txt"));
                                    break;
                                case "19"://シナリオ1の読み込み
                                    BveHacker.MainForm.OpenScenario(Path.Combine(filedirectory, "9.txt"));
                                    break;
                                case "20"://シナリオ1の読み込み
                                    BveHacker.MainForm.OpenScenario(Path.Combine(filedirectory, "10.txt"));
                                    break;
                            }*/
                            ScenarioInfo scenarioinfo = BveHacker.ScenarioInfo.FromFile(Path.Combine(filedirectory, (int.Parse(mes)-10).ToString()+".txt"));
                            BveHacker.MainForm.LoadScenario(scenarioinfo, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void BeaconPassed(BeaconPassedEventArgs e)
        {
            switch (e.Type)
            {
                case 10://信号0
                    atc = 0;
                    if (isVoiceOn) { /*音を再生する処理*/}
                    break;
                case 18://ATC信号40
                    atc = 40;
                    break;
                case 19://ATC45
                    atc = 45;
                    break;
                case 21://ATC55
                    atc = 55;
                    break;
                case 23: //ATC65
                    atc = 65;
                    break;
                case 25://ATC75
                    atc = 75;
                    break;
                case 26://Atc80
                    atc = 80;
                    break;
                case 900://隠し警笛開始
                    hideHorn = true;
                    break;
                case 901://隠し警笛終了
                    hideHorn = false;
                    break;
                case 902:
                    //「墨堤通り」ダイアログ
                    break;
                case 903:
                    //発車判定
                    break;
                case 904:
                    //停車/通過判定
                    break;
                case 905:
                    //「次は停車駅です」とか
                    break;
            }
        }
        void FlagBuilder(object sender, ElapsedEventArgs e)
        {
            if (speed < atc && power > 0) { isOverATC = true; }
            if (nowMilli - arriveMilli < 0)
            {
                if (pass == true) {
                    if (Math.Abs(nowLocation - NeXTLocation) > life.GoukakuHani)
                    { isDelay = true; }
                    if (Math.Abs(nowLocation - NeXTLocation) < life.GoukakuHani && speed > 0)
                    { isDelay = true; }
                }
                else {
                    if (nowLocation < NeXTLocation)
                    {
                        isDelay = true;
                    }
                }
            }
            if (Math.Abs(arriveMilli - nowMilli) < 1000 && NeXTLocation == nowLocation)
            { isTeituu = true; }
            if (brake == EB && speed > 5 && NeXTLocation - nowLocation < 150)
            { isEBStop = true; }
            if (brake == EB && speed > 5 && NeXTLocation - nowLocation >= 150)
            { isEB = true; }
            life.life--;
        }
        void OnStop()//停車時に一回だけ呼び出される
        {
            if (NeXTLocation - nowLocation > life.GoukakuHani)
            { isOverRun = true; }
            if (Math.Abs(nowLocation - NeXTLocation) < 0.5 && Math.Abs(nowMilli - arriveMilli) >= 2000)
            { isGood = true; }
            if (Math.Abs(nowLocation - NeXTLocation) < 0.5 && Math.Abs(nowMilli - arriveMilli) < 2000)
            { isGreat = true; }
        }
        void OnPass()
        {
            if (Math.Abs(arriveMilli - nowMilli) < 1000 && Math.Abs(NeXTLocation - nowLocation) < 5)
            { isTeituu = true; }
        }
        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P) { pause.OnInputP(); }
            if (e.KeyCode == Keys.Return) { isEnterPressed = true; }
            if(e.KeyCode == Keys.R)
            {
                if(isUIOff)
                {
                    isUIOff = false;
                }
                else
                {
                    isUIOff = true;
                }
            }
        }
        void EndBVE(int writething)
        {
            NamedPipe(writething);
            BveHacker.MainForm.CreateDirectXDevices();
        }
        void FlagStart()
        {
            isOverATC = false;
            isRestart = false;
            isEB = false;
            isEBStop = false;
            isTeituu = false;
            isGood = false;
            isGreat = false;
        }
        void FixOverRun()//オーバーランを直す
        {
            isFixOver = true;
            speed = 20;
            if (nowLocation - NeXTLocation > life.GoukakuHani)
            {
                speed = 0;
                isFixOver = false;
            }
        }
        public void OnAddStation()
        {
            StationList stations = BveHacker.Scenario.Route.Stations;
            try
            {
                Station newStation = new Station("茅場町")
                {
                    Location = addStaPosi,
                    ArrivalTimeMilliseconds = addStaArrival,
                    Pass = false,
                    IsTerminal = true
                };
                stations.Insert(newStation);
            }
            catch(Exception e)
            {
                MessageBox.Show("茅場町駅のロードに失敗しました。運転を終了します。エラーメッセージ:"+e);
                //BVEを終了するメソッドを呼び出
                EndBVE(1);
            }
        }
        void FormControll()
        {

        }
        /*void LoadScenario(string path)
        {
            BveHacker.MainForm.OpenScenario(path);
        }*/
        /*
         *メモ
         *終了コード（EndBVEのwritething）
         *0→
         *1→茅場町追加ミス
         */
    }
 }
