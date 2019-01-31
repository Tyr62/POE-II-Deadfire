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
                            activePartyMember.AIController.IgnoreAsCutsceneObstacle = true;

                        if (ActiveShot.UsePartyStartLocation && ActiveShot.PartyStartLocation != null)
                        {
                            try
                            {
                                CutsceneWaypoint cutsceneWaypoint = new CutsceneWaypoint();
                                cutsceneWaypoint.owner = gameObject;
                                cutsceneWaypoint.MoveType = MovementType.Teleport;
                                cutsceneWaypoint.TeleportVFX = null;

                                if (absoluteFormationIndex < 5)
                                    cutsceneWaypoint.Location = ActiveShot.PartyStartLocation.Waypoints[absoluteFormationIndex].transform;
                                else
                                {
                                    var transform = ActiveShot.PartyMoveLocation.Waypoints[0].transform;
                                    transform.position += new Vector3(absoluteFormationIndex + 0.5f, 0f, absoluteFormationIndex + 0.5f);

                                    cutsceneWaypoint.Location = transform;
                                }

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
                                cutsceneWaypoint2.MoveType = ActiveShot.PartyMoveType;
                                cutsceneWaypoint2.TeleportVFX = null;
                                cutsceneWaypoint2.SetFacingOnArrival = ActiveShot.PartyFaceOnArrival;

                                if (absoluteFormationIndex < 5)
                                    cutsceneWaypoint2.Location = ActiveShot.PartyMoveLocation.Waypoints[absoluteFormationIndex].transform;
                                else
                                {
                                    var transform = ActiveShot.PartyMoveLocation.Waypoints[0].transform;
                                    transform.position += new Vector3(absoluteFormationIndex + 0.5f, 0f, absoluteFormationIndex + 0.5f);

                                    cutsceneWaypoint2.Location = transform;
                                }

                                MoveWaypointList.Add(cutsceneWaypoint2);
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