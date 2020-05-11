using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;

namespace ImguiLogModule {
    public class ImguiLogSubModule: MBSubModuleBase {
        private List<string> messages = new List<string>();
        private readonly Regex stripTagsRegex = new Regex(@"<.*?>", RegexOptions.Compiled);
        private readonly Regex takenPrisonerRegex = new Regex(@"taken prisoner", RegexOptions.Compiled);
        // TODO CampaignEvents.HeroGainedSkill would be better for performance.
        // see TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.DefaultNotificationsCampaignBehavior.OnHeroGainedSkill() for the reference.
        private readonly Regex skillPointRegex = new Regex(@"skill point", RegexOptions.Compiled);
        private bool showMsgLog = false;

        protected override void OnSubModuleLoad() {
            InformationManager.DisplayMessageInternal += DisplayMessage;
        }

        protected override void OnSubModuleUnloaded() {
            InformationManager.DisplayMessageInternal -= DisplayMessage;
        }

        protected override void OnApplicationTick(float dt) {
            if (IsActive()) {
                if (Input.IsKeyPressed(InputKey.NumpadMinus)) { // TODO should be configurable
                    showMsgLog = !showMsgLog;
                }
                if (showMsgLog) {
                    // TODO might be a good idea to add some checkboxes and even text input for filtering
                    Imgui.BeginMainThreadScope();
                    Imgui.Begin("IM message log");
                    foreach (var msg in messages) {
                        Imgui.Text(msg);
                    }
                    Imgui.End();
                    Imgui.EndMainThreadScope();
                }
            }
        }

        private void DisplayMessage(InformationMessage msg) {
            if (IsAcceptingMessages() || skillPointRegex.IsMatch(msg.Information)) {
                string information = stripTagsRegex.Replace(msg.Information, ""); // TODO wtf with all the HTML tags inside? (img src works in game log)
                if (!takenPrisonerRegex.IsMatch(information)) {
                    if (Campaign.Current != null) {
                        // TODO since 1.2.0 we sometimes end up with an empty string here
                        var hour = (int)(CampaignTime.Now.ToHours % 24);
                        var minute = (int)(CampaignTime.Now.ToMinutes % 60);
                        messages.Add($"{CampaignTime.Now} {hour:d2}:{minute:d2} - {information}");
                    } else {
                        messages.Add($"{DateTime.Now} - {information}");
                    }
                }
            }
        }

        // TODO any better condition?
        // Campaign map seems like a most logical state where this should be active.
        // But then, there is no harm at showing this in any state really and that might be useful in some cases.
        // Can't be bothered in Missions (in particular, battles) though (probably).
        private static bool IsActive() => Mission.Current is null;

        // TODO any better condition?
        // basically we're interested in campaign map messages and not interested in combat log messages (except skill points)
        private static bool IsAcceptingMessages() => Mission.Current is null;
    }
}
