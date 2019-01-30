using Game;
using Game.GameData;
using Game.UI;
using Patchwork;

namespace PoE2Mods.PartySizeMod
{
    [ModifiesType]
    public class mod_UIPartyManagementIcon : UIPartyManagementIcon
    {
        [ModifiesMember("OnClick")]
        private void OnClickNew()
        {
            if (m_currentPartyMember != null)
            {
                if (isRoster)
                {
                    if (m_currentPartyMember.MemberStatus == PartyMemberStatus.InactiveOnAdventure)
                    {
                        UIWindowManager.ShowMessageBox(UIMessageBox.ButtonStyle.Ok, GuiStringTable.GetText(873), GuiStringTable.Format(896, m_currentPartyMember.Name, string.Empty));
                    }
                    else if (UISingletonHudWindow<UIPartyManager>.Instance.Party.ActiveChildCount < 6)
                    {
                        UISingletonHudWindow<UIPartyManager>.Instance.PartyCharacter(m_currentPartyMember);
                    }
                }
                else if (!m_currentPartyMember.IsPlayer)
                {
                    UISingletonHudWindow<UIPartyManager>.Instance.BenchCharacter(m_currentPartyMember);
                }
                UISingletonHudWindow<UIPartyManager>.Instance.Reload();
            }
        }
    }
}
