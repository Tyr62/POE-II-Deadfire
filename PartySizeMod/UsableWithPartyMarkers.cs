using Game;
using Patchwork;
using UnityEngine;

namespace PoE2Mods.PartySizeMod
{
    [ModifiesType]
    public class mod_UsableWithPartyMarkers : UsableWithPartyMarkers
    {

        [NewMember]
        protected new void Awake()
        {
            base.PartyInteractionPoints = new Transform[6];

            base.Awake();
        }

        [ModifiesMember("CreatePartyMarkers")]
        public void CreatePartyMarkersNew()
        {
            Vector2[] array = new Vector2[]
            {
                new Vector2(-1f, -1.5f),
                new Vector2(1f, -1.5f),
                new Vector2(-1f, 0f),
                new Vector2(1f, 0f),
                new Vector2(-1f, 1.5f),
                new Vector2(1f, 1.5f)
            };
            for (int i = 0; i < 6; i++)
            {
                GameObject gameObject = ResourceManager.CreateEmptyGameObject("pm" + (i + 1));
                SceneTransitionMarker sceneTransitionMarker = ResourceManager.AddComponent<SceneTransitionMarker>(gameObject);
                sceneTransitionMarker.slot = i;
                gameObject.transform.parent = base.transform;
                gameObject.transform.localPosition = new Vector3(array[i].x, 0f, array[i].y);
                Vector3 position;
                if (NavMeshUtility.GetPositionOnNavMesh(gameObject.transform.position, LayerUtility.WalkableLayersMask, -1, out position))
                {
                    gameObject.transform.position = position;
                }
                this.PartyInteractionPoints[i] = gameObject.transform;
            }
        }
    }
}
