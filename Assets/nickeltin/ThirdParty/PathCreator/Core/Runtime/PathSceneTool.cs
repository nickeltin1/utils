using UnityEngine;
using System;

namespace nickeltin.PathCreation.Examples
{
    [ExecuteInEditMode]
    public abstract class PathSceneTool : MonoBehaviour
    {
        public event Action onDestroyed;
        public PathCreator pathCreator;
        public bool autoUpdate = true;

        protected VertexPath path => pathCreator.path;

        public void TriggerUpdate() => PathUpdated();
        
        protected virtual void OnDestroy() => onDestroyed?.Invoke();

        protected abstract void PathUpdated();
    }
}
