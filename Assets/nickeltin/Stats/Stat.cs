using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Characters
{
	[Serializable]
	public class Stat
	{
		public float baseValue;

		protected bool m_isDirty = true;
		protected float m_lastBaseValue;

		[SerializeField] protected float m_value;
		public virtual float Value {
			get {
				if(m_isDirty || m_lastBaseValue != baseValue) {
					m_lastBaseValue = baseValue;
					m_value = CalculateFinalValue();
					m_isDirty = false;
				}
				return m_value;
			}
		}
		
		protected readonly List<StatModifier> m_statModifiers;

		public IReadOnlyCollection<StatModifier> StatModifiers => m_statModifiers;

		public Stat()
		{
			m_statModifiers = new List<StatModifier>();
		}

		public Stat(float baseValue, [Optional] params StatModifier[] initialModifiers) : this()
		{
			this.baseValue = baseValue;
			if (initialModifiers != null)
			{
				for (int i = 0; i < initialModifiers.Length; i++) AddModifier(initialModifiers[i]);
			}
		}

		public virtual StatModifier AddModifier(StatModifier mod)
		{
			m_isDirty = true;
			m_statModifiers.Add(mod);
			return mod;
		}

		public virtual bool RemoveModifier(StatModifier mod)
		{
			if (m_statModifiers.Remove(mod))
			{
				m_isDirty = true;
				return true;
			}
			return false;
		}

		public virtual bool RemoveAllModifiers()
		{
			if (m_statModifiers.Count > 0)
			{
				m_statModifiers.Clear();
				m_isDirty = true;
				return true;
			}

			return false;
		}

		public virtual bool RemoveAllModifiersFromSource(object source)
		{
			int numRemovals = m_statModifiers.RemoveAll(mod => mod.source == source);

			if (numRemovals > 0)
			{
				m_isDirty = true;
				return true;
			}
			return false;
		}

		protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
		{
			if (a.order < b.order) return -1;
			if (a.order > b.order) return 1;
			if (a.order == b.order) return 0;
			return 0;
		}
		
		protected virtual float CalculateFinalValue()
		{
			float finalValue = baseValue;
			float sumPercentAdd = 0;

			m_statModifiers.Sort(CompareModifierOrder);

			for (int i = 0; i < m_statModifiers.Count; i++)
			{
				StatModifier mod = m_statModifiers[i];

				if (mod.type == StatModifier.Type.Flat)
				{
					finalValue += mod.value;
				}
				else if (mod.type == StatModifier.Type.PercentAdd)
				{
					sumPercentAdd += mod.value;

					if (i + 1 >= m_statModifiers.Count || m_statModifiers[i + 1].type != StatModifier.Type.PercentAdd)
					{
						finalValue *= 1 + sumPercentAdd;
						sumPercentAdd = 0;
					}
				}
				else if (mod.type == StatModifier.Type.PercentMult)
				{
					finalValue *= 1 + mod.value;
				}
			}

			// Workaround for float calculation errors, like displaying 12.00001 instead of 12
			return (float)Math.Round(finalValue, 4);
		}
	}
}
