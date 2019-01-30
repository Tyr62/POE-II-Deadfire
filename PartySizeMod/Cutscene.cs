using Game;
using Game.GameData;
using Onyx;
using Patchwork;
using UnityEngine;

namespace PoE2Mods.PartySizeMod
{
    [ModifiesType]
    public class mod_Cutscene : Cutscene
    {
        [ModifiesMember("AddPartyToActorList")]
        protected void AddPartyToActorListNew()
        {
            foreach (PartyMember activePartyMember in SingletonBehavior<PartyManager>.Instance.GetActivePartyMembers())
            {
                int absoluteFormationIndex = SingletonBehavior<PartyManager>.Instance.GetAbsoluteFormationIndex(activePartyMember);
                if (!(activePartyMember == null))
                {
                    if (activePartyMember.IsSecondaryPartyMember())
                    {
                        if (AnimalCompanionsFollowOwner && activePartyMember.PartyMemberData.MemberType == PartyMemberType.AnimalCompanion)
                        {
                            AIBehavior behavior = AIBehaviorManager.AllocateFollowBehavior();
                            activePartyMember.AIController.BehaviorStack.PushBehavior(behavior);
                        }
                    }
                    else
                    {
                        GameObject gameObject = activePartyMember.gameObject;
                        if (!activePartyMember.IsPrimaryPartyMember())
                        {
                            activePartyMember.AIController.IgnoreAsCutsceneObstacle = true;
                        }
                        if (ActiveShot.UsePartyStartLocation && ActiveShot.PartyStartLocation != null)
                        {
                            CutsceneWaypoint cutsceneWaypoint = new CutsceneWaypoint();
                            cutsceneWaypoint.owner = gameObject;
                            cutsceneWaypoint.MoveType = MovementType.Teleport;
                            cutsceneWaypoint.TeleportVFX = null;

                            if (absoluteFormationIndex < 5)
                            {
                                if (ActiveShot.PartyStartLocation.Waypoints.Length < absoluteFormationIndex && ActiveShot.PartyStartLocation.Waypoints[absoluteFormationIndex] != null)
                                {
                                    cutsceneWaypoint.Location = ActiveShot.PartyStartLocation.Waypoints[absoluteFormationIndex].transform;
                                    SpawnWaypointList.Add(cutsceneWaypoint);
                                }
                                else
                                {
                                    Debug.Log($"ActiveShot.PartyStartLocation.Waypoints: {absoluteFormationIndex} is null or too high.");
                                }
                            }
                            else
                            {
                                cutsceneWaypoint.Location = ActiveShot.PartyStartLocation.Waypoints[4].transform;
                                SpawnWaypointList.Add(cutsceneWaypoint);
                            }
                        }
                        if (ActiveShot.UsePartyMoveLocation && ActiveShot.PartyMoveLocation != null)
                        {
                            CutsceneWaypoint cutsceneWaypoint2 = new CutsceneWaypoint();
                            cutsceneWaypoint2.owner = gameObject;
                            cutsceneWaypoint2.MoveType = ActiveShot.PartyMoveType;
                            cutsceneWaypoint2.TeleportVFX = null;

                            if (absoluteFormationIndex < 5)
                            {
                                if (ActiveShot.PartyMoveLocation.Waypoints.Length < absoluteFormationIndex && ActiveShot.PartyMoveLocation.Waypoints[absoluteFormationIndex] != null)
                                {
                                    cutsceneWaypoint2.Location = ActiveShot.PartyMoveLocation.Waypoints[absoluteFormationIndex].transform;
                                    cutsceneWaypoint2.SetFacingOnArrival = ActiveShot.PartyFaceOnArrival;
                                    MoveWaypointList.Add(cutsceneWaypoint2);
                                }
                                else
                                {
                                    Debug.Log($"ActiveShot.PartyMoveLocation.Waypoints: {absoluteFormationIndex} is null or too high.");
                                }
                            }
                            else
                            {
                                cutsceneWaypoint2.Location = ActiveShot.PartyMoveLocation.Waypoints[4].transform;
                                cutsceneWaypoint2.SetFacingOnArrival = ActiveShot.PartyFaceOnArrival;
                                MoveWaypointList.Add(cutsceneWaypoint2);
                            }
                        }
                        if (!ActorList.Contains(gameObject))
                        {
                            ActorList.Add(gameObject);
                        }
                    }
                }
            }
        }
    }
}
