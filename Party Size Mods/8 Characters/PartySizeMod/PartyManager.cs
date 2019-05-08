using System;
using Game;
using Game.GameData;
using Patchwork;
using UnityEngine;

namespace PoE2Mods.PartySizeMod
{
    [ModifiesType]
    public class mod_PartyManager : PartyManager
    {
        [ModifiesMember("AddToParty")]
        public bool AddToPartyNew(Guid newPartyMemberGuid, PartyMemberType partyMemberType, PartyMemberStatus status, PartyMemberData existingPartyMemberData, AddToPartyFlags flags)
        {
            if (newPartyMemberGuid == Guid.Empty)
            {
                Debug.LogError("Failed to add party member: Empty Guid.");
                return false;
            }
            if (m_guidToPartyMemberDataDictionary.ContainsKey(newPartyMemberGuid))
            {
                return false;
            }
            if (!AllTimePartyMembers.Contains(newPartyMemberGuid))
            {
                AllTimePartyMembers.Add(newPartyMemberGuid);
            }
            if (partyMemberType == PartyMemberType.Primary && IsActiveStatus(status))
            {
                if ((flags & AddToPartyFlags.AllowRedirectToBench) != 0 && GodChallengeGameData.AnyEnabledChallenge(GodChallengeGameData.ProhibitPartyMembersPredicate))
                {
                    return false;
                }
                if (m_activePrimaryPartyMembers.Count >= 8)
                {
                    Debug.LogError("Trying to add a primary party member to the party when already at max primary party members limit!");
                    return false;
                }
            }
            PartyMemberData partyMemberData = existingPartyMemberData;
            if (partyMemberData == null)
            {
                partyMemberData = new PartyMemberData();
            }
            GameObject objectByID = InstanceID.GetObjectByID(newPartyMemberGuid);
            if ((bool)objectByID)
            {
                PartyMember partyMember = (!(bool)objectByID) ? null : objectByID.GetComponent<PartyMember>();
                if (partyMember == null)
                {
                    partyMember = ResourceManager.AddComponent<PartyMember>(objectByID);
                }
                AIController component = objectByID.GetComponent<AIController>();
                if (component != null)
                {
                    component.AssignPartyMemberBehavior();
                }
                partyMember.PartyMemberData = partyMemberData;
            }
            if (!(bool)objectByID && partyMemberData.MemberStatus == PartyMemberStatus.NotInParty)
            {
                Debug.LogError("Trying to add party member with no game object reference! Guid: " + newPartyMemberGuid.ToString());
                return false;
            }
            if (!(bool)objectByID && status == PartyMemberStatus.Active)
            {
                status = PartyMemberStatus.InactiveAvailable;
            }
            partyMemberData.InitializePartyMemberData(newPartyMemberGuid, partyMemberType);
            m_guidToPartyMemberDataDictionary.Add(newPartyMemberGuid, partyMemberData);
            if (existingPartyMemberData != null)
            {
                RestorePartyMemberStatus(newPartyMemberGuid, status);
            }
            else
            {
                SetPartyMemberStatus(newPartyMemberGuid, status, flags);
            }
            return true;
        }
    }

}
