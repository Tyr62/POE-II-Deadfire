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
                        var gameObject = activePartyMember.gameObject;
                        if (!activePartyMember.IsPrimaryPartyMember())
                            activePartyMember.AIController.IgnoreAsCutsceneObstacle = true;

                        if (ActiveShot.UsePartyStartLocation && ActiveShot.PartyStartLocation != null)
                        {
                            if (ActiveShot.PartyStartLocation.Waypoints.Length - 1 < absoluteFormationIndex)
                                Array.Resize(ref ActiveShot.PartyStartLocation.Waypoints, absoluteFormationIndex + 1);

                            if (ActiveShot.PartyStartLocation.Waypoints[absoluteFormationIndex] == null)
                            {
                                ActiveShot.PartyStartLocation.Waypoints[absoluteFormationIndex] = new GameObject();
                                var neighbor = ActiveShot.PartyStartLocation.Waypoints[absoluteFormationIndex - 1] ?? ActiveShot.PartyStartLocation.Waypoints[0];
                                var position = neighbor.transform.position + Vector3.right;
                                var rotation = neighbor.transform.rotation;

                                ActiveShot.PartyStartLocation.Waypoints[absoluteFormationIndex].transform.SetPositionAndRotation(position, rotation);
                            }

                            var cutsceneWaypoint = new CutsceneWaypoint();
                            cutsceneWaypoint.owner = gameObject;
                            cutsceneWaypoint.MoveType = MovementType.Teleport;
                            cutsceneWaypoint.TeleportVFX = null;
                            cutsceneWaypoint.Location = ActiveShot.PartyStartLocation.Waypoints[absoluteFormationIndex].transform;

                            SpawnWaypointList.Add(cutsceneWaypoint);
                        }

                        if (ActiveShot.UsePartyMoveLocation && ActiveShot.PartyMoveLocation != null)
                        {
                            if (ActiveShot.PartyMoveLocation.Waypoints.Length - 1 < absoluteFormationIndex)
                                Array.Resize(ref ActiveShot.PartyMoveLocation.Waypoints, absoluteFormationIndex + 1);

                            if (ActiveShot.PartyMoveLocation.Waypoints[absoluteFormationIndex] == null)
                            {
                                ActiveShot.PartyMoveLocation.Waypoints[absoluteFormationIndex] = new GameObject();
                                var neighbor = ActiveShot.PartyMoveLocation.Waypoints[absoluteFormationIndex - 1] ?? ActiveShot.PartyMoveLocation.Waypoints[0];
                                var position = neighbor.transform.position + Vector3.right;
                                var rotation = neighbor.transform.rotation;

                                ActiveShot.PartyMoveLocation.Waypoints[absoluteFormationIndex].transform.SetPositionAndRotation(position, rotation);
                            }

                            var cutsceneWaypoint = new CutsceneWaypoint();
                            cutsceneWaypoint.owner = gameObject;
                            cutsceneWaypoint.MoveType = MovementType.Teleport;
                            cutsceneWaypoint.TeleportVFX = null;
                            cutsceneWaypoint.Location = ActiveShot.PartyMoveLocation.Waypoints[absoluteFormationIndex].transform;

                            SpawnWaypointList.Add(cutsceneWaypoint);

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