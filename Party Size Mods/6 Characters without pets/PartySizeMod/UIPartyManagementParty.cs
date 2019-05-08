using Game;
using Game.UI;
using Patchwork;

namespace PoE2Mods.PartySizeMod
{
    [ModifiesType]
    public class mod_UIPartyManagementParty : UIPartyManagementParty
    {
        [ModifiesMember("Reload")]
        public void ReloadNew()
        {
            Populate(0);
            foreach (PartyMemberData activePrimaryPartyMember in UISingletonHudWindow<UIPartyManager>.Instance.GetActivePrimaryPartyMembers())
            {
                if (activePrimaryPartyMember != null && !UISingletonHudWindow<UIPartyManager>.Instance.PendingToBench.Contains(activePrimaryPartyMember))
                {
                    ActivateClone<UIPartyManagementIcon>().SetPartyMember(activePrimaryPartyMember);
                }
            }
            for (int i = 0; i < UISingletonHudWindow<UIPartyManager>.Instance.PendingToParty.Count; i++)
            {
                PartyMemberData partyMemberData = UISingletonHudWindow<UIPartyManager>.Instance.PendingToParty[i];
                if (partyMemberData != null)
                {
                    ActivateClone<UIPartyManagementIcon>().SetPartyMember(partyMemberData);
                }
            }
            PartyCount.text = IntUtils.ToStringLocal(base.ActiveChildCount) + "/6";
        }
    }
}
