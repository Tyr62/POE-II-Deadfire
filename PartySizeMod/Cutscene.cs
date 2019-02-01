using System;
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
                if (absoluteFormationIndex >= 5)
                    Debug.Log($"AddPartyToActorList: {activePartyMember.name} {absoluteFormationIndex}");

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
                            activePartyMember.AIController.IgnoreAsCutsceneObstacle = true;

                        if (ActiveShot.UsePartyStartLocation && ActiveShot.PartyStartLocation != null)
                        {
                            try
                            {
                                CutsceneWaypoint cutsceneWaypoint = new CutsceneWaypoint();
                                cutsceneWaypoint.owner = gameObject;
                                cutsceneWaypoint.MoveType = MovementType.Teleport;
                                cutsceneWaypoint.TeleportVFX = null;

                                if (absoluteFormationIndex >= 5 && ActiveShot.PartyStartLocation.Waypoints.Length - 1 < absoluteFormationIndex)
                                {
                                    Array.Resize(ref ActiveShot.PartyStartLocation.Waypoints, absoluteFormationIndex + 1);
                                    var neighbor = ActiveShot.PartyStartLocation.Waypoints[absoluteFormationIndex - 1];
                                    var position = neighbor.transform.position + new Vector3(absoluteFormationIndex * 0.5f, 0f, absoluteFormationIndex * 0.5f);
                                    var rotation = neighbor.transform.rotation;

                                    ActiveShot.PartyStartLocation.Waypoints[absoluteFormationIndex] = new GameObject();
                                    ActiveShot.PartyStartLocation.Waypoints[absoluteFormationIndex].transform.SetPositionAndRotation(position, rotation);
                                }

                                cutsceneWaypoint.Location = ActiveShot.PartyStartLocation.Waypoints[absoluteFormationIndex].transform;
     
                                SpawnWaypointList.Add(cutsceneWaypoint);
                            }
                            catch (System.Exception ex)
                            {
                                Debug.Log($"ActiveShot.PartyStartLocation.Waypoints: {absoluteFormationIndex} is null or too high.");
                                Debug.Log($"Exception: {ex.ToString()}");
                            }
                        }

                        if (ActiveShot.UsePartyMoveLocation && ActiveShot.PartyMoveLocation != null)
                        {
                            try
                            {
                                CutsceneWaypoint cutsceneWaypoint2 = new CutsceneWaypoint();
                                cutsceneWaypoint2.owner = gameObject;
                                cutsceneWaypoint2.MoveType = MovementType.Teleport;
                                cutsceneWaypoint2.TeleportVFX = null;

                                if (absoluteFormationIndex >= 5 && ActiveShot.PartyMoveLocation.Waypoints.Length - 1 < absoluteFormationIndex)
                                {
                                    Array.Resize(ref ActiveShot.PartyMoveLocation.Waypoints, absoluteFormationIndex + 1);
                                    var neighbor = ActiveShot.PartyMoveLocation.Waypoints[absoluteFormationIndex - 1];
                                    var position = neighbor.transform.position + new Vector3(absoluteFormationIndex * 0.5f, 0f, absoluteFormationIndex * 0.5f);
                                    var rotation = neighbor.transform.rotation;

                                    ActiveShot.PartyMoveLocation.Waypoints[absoluteFormationIndex] = new GameObject();
                                    ActiveShot.PartyMoveLocation.Waypoints[absoluteFormationIndex].transform.SetPositionAndRotation(position, rotation);
                                }

                                cutsceneWaypoint2.Location = ActiveShot.PartyMoveLocation.Waypoints[absoluteFormationIndex].transform;

                                SpawnWaypointList.Add(cutsceneWaypoint2);
                            }
                            catch (System.Exception ex)
                            {
                                Debug.Log($"ActiveShot.PartyMoveLocation.Waypoints: {absoluteFormationIndex} is null or too high.");
                                Debug.Log($"Exception: {ex.ToString()}");
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