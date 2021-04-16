using System;
using System.Collections.Generic;
using UnityEngine;

namespace nickeltin.Other
{
    [Serializable]
    public class SerializableArray3D<T> where T : new()
    {
        [SerializeField] private Vector3Int m_size;
        [SerializeField] private X[] m_x;

        public Vector3Int Size => m_size;

        public SerializableArray3D(Vector3Int size)
        {
            m_size = size;
                
            m_x = new X[size.x];
            for (int xi = 0; xi < size.x; xi++)
            {
                this.m_x[xi].m_y = new Y[size.y];
                for (int yi = 0; yi < size.y; yi++)
                {
                    this.m_x[xi].m_y[yi].m_z = new T[size.z];
                    for (int zi = 0; zi < size.z; zi++)
                    {
                        this.m_x[xi].m_y[yi].m_z[zi] = new T();
                    }
                }
            }
        }

        public SerializableArray3D(int sizeX, int sizeY, int sizeZ) : this(new Vector3Int(sizeX,sizeY,sizeZ)) { }

        [Serializable] public struct X { public Y[] m_y; }

        [Serializable] public struct Y { public T[] m_z; }

        public List<T> GetItems()
        {
            List<T> items = new List<T>();

            for (int z = 0; z < m_size.z; z++)
            {
                for (int y = 0; y < m_size.y; y++)
                {
                    for (int x = 0; x < m_size.x; x++)
                    {
                        var item = this[x, y, z];
                        if(item != null) items.Add(item);
                    }
                }
            }

            return items;
        }
        
        public bool TryGetNeighbour(Vector3Int neighbourOf, Vector3Int neighbourDirection, out T neighbour)
        {
            Vector3Int dir = new Vector3Int(Mathf.Clamp(neighbourDirection.x, -1, 1),
                Mathf.Clamp(neighbourDirection.y,-1, 1),
                Mathf.Clamp(neighbourDirection.z,-1, 1)) + neighbourOf;

            if (InRange(dir))
            {
                neighbour = this[dir.x, dir.y, dir.z];
                return neighbour != null;
            }

            neighbour = default(T);
            return false;
        }

        public bool InRange(Vector3Int indexes) => InRange(indexes.x, indexes.y, indexes.z);

        public bool InRange(int x, int y, int z)
        {
            if (x >= 0 && x < m_size.x && y >= 0 && y < m_size.y && z >= 0 && z < m_size.z) return true;
            return false;
        }
        
        public T this[int x, int y, int z]
        {
            get => m_x[x].m_y[y].m_z[z];
            set => m_x[x].m_y[y].m_z[z] = value;
        }
    }
}