using System;
using System.Collections.Generic;
using Game;
using Game.AI;
using Game.GameData;
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
            if (slot >= 5)
            {
                if (PartyInteractionPoints.Length - 1 < slot)
                    Array.Resize(ref PartyInteractionPoints, slot + 1);

                if (PartyInteractionPoints[slot] == null)
                    PartyInteractionPoints[slot] = new GameObject().transform;
                
                var neighbor = PartyInteractionPoints[slot - 1];
                var position = neighbor.transform.position + PartyInteractionPoints[slot].transform.right;
                var rotation = neighbor.transform.rotation;

                PartyInteractionPoints[slot].transform.SetPositionAndRotation(position, rotation);
            }

            if (PartyInteractionPoints == null || slot >= PartyInteractionPoints.Length || slot < 0 || PartyInteractionPoints[slot] == null)
            {
                return base.transform.position;
            }

            if (secondary)
            {
                Vector3 hitPosition = PartyInteractionPoints[slot].position + PartyInteractionPoints[slot].right;
                Vector3 origin = hitPosition + Vector3.up * 2f;
                RaycastHit result;
                if (GameUtilities.Raycast(origin, Vector3.down, out result, float.PositiveInfinity, LayerUtility.WalkableLayersMask))
                {
                    hitPosition = result.point;
                }
                NavMeshUtility.SamplePosition(hitPosition, out hitPosition, 5f, NavMeshUtility.WalkableAreasMask);
                return hitPosition;
            }

            return PartyInteractionPoints[slot].position;
        }
    }
}
