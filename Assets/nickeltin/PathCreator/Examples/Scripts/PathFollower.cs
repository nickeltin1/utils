using System;
using UnityEngine;

namespace nickeltin.PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        [Serializable] 
        public enum UpdateType
        {
            Update, FixedUpdate, LateUpdate
        }
    
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public UpdateType updateType = UpdateType.Update;
        public float speed = 5;
        public bool updateRotation = true;


        public float distanceTravelled { get; set; }
        public bool stopped = false;
        
        private void Start() 
        {
            if (pathCreator != null) pathCreator.pathUpdated += OnPathChanged;
        }


        private void FixedUpdate()
        {
            if(updateType == UpdateType.FixedUpdate) Update_Internal();
        }

        private void Update()
        {
            if(updateType == UpdateType.Update) Update_Internal();
        }

        private void LateUpdate()
        {
            if(updateType == UpdateType.LateUpdate) Update_Internal();
        }
        
        private void Update_Internal()
        {
            if (pathCreator != null && !stopped)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                if(updateRotation) transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }
        
        private void OnPathChanged() 
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}