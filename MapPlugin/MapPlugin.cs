using System;
using AtsEx.PluginHost.Plugins;
using BveTypes.ClassWrappers;
using FastMember;
using TypeWrapping;
using ObjectiveHarmonyPatch;

namespace AtsExCsTemplate.MapPlugin
{
    /// <summary>
    /// プラグインの本体
    /// </summary>
    [PluginType(PluginType.MapPlugin)]
    internal class MapPluginMain : AssemblyPluginBase
    {
        /// <summary>
        /// プラグインが読み込まれた時に呼ばれる
        /// 初期化を実装する
        /// </summary>
        /// <param name="builder"></param>
        public MapPluginMain(PluginBuilder builder) : base(builder)
        {
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

        /// <summary>
        /// プラグインが解放されたときに呼ばれる
        /// 後処理を実装する
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// シナリオ読み込み中に毎フレーム呼び出される
        /// </summary>
        /// <param name="elapsed">前回フレームからの経過時間</param>
        public override TickResult Tick(TimeSpan elapsed)
        {
            return new MapPluginTickResult();
        }
    }
}
