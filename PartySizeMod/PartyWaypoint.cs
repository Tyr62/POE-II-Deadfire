using System;
using Game;
using Onyx;
using Patchwork;
using UnityEngine;

namespace PoE2Mods.PartySizeMod
{
    [ModifiesType]
    public class mod_PartyWaypoint : PartyWaypoint
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
                if (Waypoints.Length - 1 < slot)
                    Array.Resize(ref Waypoints, slot + 1);

                if (Waypoints[slot] == null)
                    Waypoints[slot] = new GameObject();

                var neighbor = Waypoints[slot - 1];
                var position = neighbor.transform.position + Waypoints[slot].transform.right;
                var rotation = neighbor.transform.rotation;

                Waypoints[slot].transform.SetPositionAndRotation(position, rotation);
            }
            
            if (slot < 0 || slot >= Waypoints.Length || Waypoints[slot] == null)
            {
                return base.transform.position;
            }

            if (secondary)
            {
                Vector3 vector = Waypoints[slot].transform.position + Waypoints[slot].transform.right;
                Vector3 origin = vector + Vector3.up * 2f;
                RaycastHit result;
                if (GameUtilities.Raycast(origin, Vector3.down, out result, float.PositiveInfinity, -5))
                {
                    vector = result.point;
                }
                return vector;
            }

            return Waypoints[slot].transform.position;
        }
    }
}
