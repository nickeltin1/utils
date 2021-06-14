using System;
using nickeltin.Editor.Attributes;
using nickeltin.Extensions;
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
        [ShowIf("updateRotation")] public bool useDirectionToRotate = true;


        public float distanceTravelled { get; set; }
        public bool stopped = false;
        
        private VertexPath path;

        private void OnEnable()
        {
            if (pathCreator != null) pathCreator.pathUpdated += RefreshPath;
        }

        private void OnDisable()
        {
            if (pathCreator != null) pathCreator.pathUpdated -= RefreshPath;
        }


        private void FixedUpdate()
        {
            if(updateType == UpdateType.FixedUpdate) Update_Internal(Time.fixedDeltaTime);
        }

        private void Update()
        {
            if(updateType == UpdateType.Update) Update_Internal(Time.deltaTime);
        }

        private void LateUpdate()
        {
            if(updateType == UpdateType.LateUpdate) Update_Internal(Time.deltaTime);
        }

        public void SetPath(VertexPath path)
        {
            OnDisable();
            pathCreator = null;
            this.path = path;
        }
        
        private void Update_Internal(float delta)
        {
            if (!stopped)
            {
                if (pathCreator != null) path = pathCreator.path;
                
                if (path == null) return;

                distanceTravelled += speed * delta;
                transform.position = path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                if (updateRotation)
                {
                    if (useDirectionToRotate)
                    {
                        var rot =  path
                            .GetDirectionAtDistance(distanceTravelled, endOfPathInstruction).LookRotation();
                        rot.Set(x: 0, z: 0);
                        transform.rotation = rot;
                    }
                    else
                    {
                        transform.rotation = path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                    }
                }
            }
        }
        
        public void RefreshPath()
        {
            if (pathCreator != null) path = pathCreator.path;
            distanceTravelled = path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}