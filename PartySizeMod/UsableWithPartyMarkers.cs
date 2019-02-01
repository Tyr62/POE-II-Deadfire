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
        //[ModifiesMember("CreatePartyMarkers")]
        //public void CreatePartyMarkersNew()
        //{
        //    Vector2[] array = new Vector2[6]
        //    {
        //        new Vector2(-1f, -1.5f),
        //        new Vector2(1f, -1.5f),
        //        new Vector2(-1f, 0f),
        //        new Vector2(1f, 0f),
        //        new Vector2(-1f, 1.5f),
        //        new Vector2(1f, 1.5f)
        //    };
        //    for (int i = 0; i < 6; i++)
        //    {
        //        GameObject gameObject = ResourceManager.CreateEmptyGameObject("pm" + (i + 1));
        //        SceneTransitionMarker sceneTransitionMarker = ResourceManager.AddComponent<SceneTransitionMarker>(gameObject);
        //        sceneTransitionMarker.slot = i;
        //        gameObject.transform.parent = base.transform;
        //        gameObject.transform.localPosition = new Vector3(array[i].x, 0f, array[i].y);
        //        Vector3 navMeshPosition;
        //        if (NavMeshUtility.GetPositionOnNavMesh(gameObject.transform.position, LayerUtility.WalkableLayersMask, -1, out navMeshPosition))
        //        {
        //            gameObject.transform.position = navMeshPosition;
        //        }
        //        PartyInteractionPoints[i] = gameObject.transform;
        //    }
        //}

        [ModifiesMember("GetMarkerPosition")]
        public Vector3 GetMarkerPositionNew(PartyMember partyMember, bool reverse)
        {
            int absoluteFormationIndex = SingletonBehavior<PartyManager>.Instance.GetAbsoluteFormationIndex(partyMember);
            bool secondary = partyMember.IsSecondaryPartyMember();
            return GetMarkerPositionBySlot((!reverse) ? absoluteFormationIndex : (6 - absoluteFormationIndex), secondary);
        }
    }
}
