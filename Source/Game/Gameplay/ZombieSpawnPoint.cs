using FlaxEngine;
#if FLAX_EDITOR
using System.IO;
using FlaxEditor;
using FlaxEditor.SceneGraph;
#endif

namespace Game
{
    /// <summary>
    /// Spawn Point actor placed on a level to mark zombies spawn location.
    /// </summary>
    public class ZombieSpawnPoint : Actor
    {
#if FLAX_EDITOR
        static ZombieSpawnPoint()
        {
            ViewportIconsRenderer.AddCustomIcon(typeof(ZombieSpawnPoint), Content.LoadAsync<Texture>(Path.Combine(Globals.ProjectContentFolder, "Textures/drip_stand_512.flax")));
            SceneGraphFactory.CustomNodesTypes.Add(typeof(ZombieSpawnPoint), typeof(ZombieSpawnPointNode));
        }
#endif

        /// <inheritdoc />
        public override void OnEnable()
        {
            base.OnEnable();
#if FLAX_EDITOR
            ViewportIconsRenderer.AddActor(this);
#endif
            LevelsManager.Instance?.ZombieSpawnPoints.Add(this);
        }

        /// <inheritdoc />
        public override void OnDisable()
        {
            LevelsManager.Instance?.ZombieSpawnPoints.Remove(this);
#if FLAX_EDITOR
            ViewportIconsRenderer.RemoveActor(this);
#endif
            base.OnDisable();
        }
    }

#if FLAX_EDITOR
    /// <summary>Custom actor node for Editor.</summary>
    [HideInEditor]
    public sealed class ZombieSpawnPointNode : ActorNodeWithIcon
    {
        /// <inheritdoc />
        public ZombieSpawnPointNode(Actor actor)
            : base(actor)
        {
        }
    }
#endif
}
