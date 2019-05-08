using Game.UI;
using Patchwork;

namespace PoE2Mods.PartySizeMod
{
    [ModifiesType]
    public class mod_UIPartyPortraitBar : UIPartyPortraitBar
    {
        [NewMember]
        protected new void Awake()
        {
            base.Spacing = 1.1f;
            base.m_Portraits = new UIPartyPortrait[6];

            base.Awake();
        }
    }
}
