using System;
using System.Collections.Generic;
using nickeltin.Editor.Attributes;
using nickeltin.Extensions.Vector;
using nickeltin.GameData.DataObjects;
using nickeltin.Tweening;
using UnityEngine;

public class ReferencesTest : MonoBehaviour
{
    [SerializeField] private NumberReference m_number;
    [SerializeField] private BoolReference m_bool;
    [SerializeField] private StringReference m_string;

    [SerializeField] [Scene] private string m_scene;

    [SerializeField] [ReorderableList] private List<Thing> m_list;


    [SerializeField] [MinMaxSlider(0, 1)] private Vector2 m_slider;

    [Serializable]
    public struct Thing
    {
        public float hp;
        public float dmg;
        public float weight;
    }

    public GameObject m_tweenTarget;
    
    private void Start()
    {
        m_tweenTarget.LeanScale(Vector3.one.Mult(6), 1).setLoopPingPong();
    }
}

