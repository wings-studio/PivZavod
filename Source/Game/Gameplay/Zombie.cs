using FlaxEngine;
using FlaxEngine.Networking;

namespace Game
{
    public class Zombie : Script
    {
        internal NavCrowd Crowd = null;
        internal int ID = -1;
        private Vector3 _targetPos;

        /// <summary>
        /// The target object to follow.
        /// </summary>
        public Actor MoveToTarget;

        /// <summary>
        /// The offset applied to the actor position on moving it.
        /// </summary>
        public Vector3 Offset = new Vector3(0, 100, 0);

        /// <summary>
        /// Agent properties.
        /// </summary>
        public NavAgentProperties Properties = new NavAgentProperties
        {
            Radius = 34.0f,
            Height = 144.0f,
            StepHeight = 35.0f,
            MaxSlopeAngle = 60.0f,
            MaxSpeed = 500.0f,
            CrowdSeparationWeight = 2.0f,
        };

        /// <inheritdoc />
        public override void OnEnable()
        {
            // Register
            PluginManager.GetPlugin<CrowdSystem>().AddAgent(this);
            // Register for replication
            NetworkReplicator.AddObject(this);
            MoveToTarget = Level.FindActors(new Tag(0))[0];
        }

        /// <inheritdoc />
        public override void OnDisable()
        {
            // Unregister
            PluginManager.GetPlugin<CrowdSystem>().RemoveAgent(this);
            // Unregister from replication
            NetworkReplicator.RemoveObject(this);
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
            if (!MoveToTarget || !Crowd)
                return;
            var currentPos = Actor.Position;
            var targetPos = MoveToTarget.Position;

            // Check if need to change target position
            if (targetPos != _targetPos)
            {
                _targetPos = targetPos;
                Crowd.SetAgentMoveTarget(ID, targetPos);
            }

            // Update agent position (calculated by NavCrowd)
            targetPos = Crowd.GetAgentPosition(ID) + Offset;
            Actor.AddMovement(targetPos - currentPos);
        }
    }
}