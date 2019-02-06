using System;
using Game;
using Onyx;
using Patchwork;
using UnityEngine;

namespace PoE2Mods.PartySizeMod
{
    [ModifiesType]
    public class mod_UsableWithPartyMarkers : UsableWithPartyMarkers
    {
        [ModifiesMember("GetMarkerPosition")]
        public Vector3 GetMarkerPositionNew(PartyMember partyMember, bool reverse)
        {
            int absoluteFormationIndex = SingletonBehavior<PartyManager>.Instance.GetAbsoluteFormationIndex(partyMember);
            bool secondary = partyMember.IsSecondaryPartyMember();
            return GetMarkerPositionBySlot((!reverse) ? absoluteFormationIndex : (6 - absoluteFormationIndex), secondary);
        }

        [ModifiesMember("GetMarkerPositionBySlot")]
        private Vector3 GetMarkerPositionBySlotNew(int slot, bool secondary)
        {
            if (PartyInteractionPointsIsNull() || slot < 0)
                return base.transform.position;

            if (slot >= 5)
            {
                if (PartyInteractionPoints.Length - 1 < slot)
                    Array.Resize(ref PartyInteractionPoints, slot + 1);

                if (PartyInteractionPoints[slot] == null)
                    PartyInteractionPoints[slot] = new GameObject().transform;
                else
                    return PartyInteractionPoints[slot].transform.position;

                var neighbor = PartyInteractionPoints[slot - 1] ?? PartyInteractionPoints[0];
                var position = neighbor.transform.position + Vector3.right;
                var rotation = neighbor.transform.rotation;

                PartyInteractionPoints[slot].transform.SetPositionAndRotation(position, rotation);
            }

            if (slot >= PartyInteractionPoints.Length)
                return base.transform.position;

            if (secondary)
            {
                Vector3 hitPosition = PartyInteractionPoints[slot].position + PartyInteractionPoints[slot].right;
                Vector3 origin = hitPosition + Vector3.up * 2f;
                RaycastHit result;
                if (GameUtilities.Raycast(origin, Vector3.down, out result, float.PositiveInfinity, LayerUtility.WalkableLayersMask))
                    hitPosition = result.point;

                NavMeshUtility.SamplePosition(hitPosition, out hitPosition, 5f, NavMeshUtility.WalkableAreasMask);
                return hitPosition;
            }

            return PartyInteractionPoints[slot].position;
        }

        [NewMember]
        private bool PartyInteractionPointsIsNull()
        {
            if (PartyInteractionPoints == null)
                return true;

            for (var i = 0; i < PartyInteractionPoints.Length; i++)
            {
                if (PartyInteractionPoints[i] != null)
                    return false;
            }

            return true;
        }
    }
}
