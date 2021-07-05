using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using nickeltin.Runtime.StateMachine;
using UnityEngine;

namespace Characters
{
	[Serializable]
	public class Stat
	{
		[SerializeField] protected float _value;

		protected bool _isDirty = true;
		protected float _lastBaseValue;
		public event Action<float> onValueChanged;

		public float baseValue;
		public virtual float Value 
		{
			get 
			{
				if(_isDirty || _lastBaseValue != baseValue) 
				{
					_lastBaseValue = baseValue;
					_value = CalculateFinalValue();
					_isDirty = false;
				}
				return _value;
			}
		}
		
		protected readonly List<StatModifier> _statModifiers = new List<StatModifier>();

		public IReadOnlyCollection<StatModifier> StatModifiers => _statModifiers;
		
		public Stat(float baseValue, params StatModifier[] initialModifiers)
		{
			this.baseValue = baseValue;
			if (initialModifiers != null)
			{
				_isDirty = true;
				for (int i = 0; i < initialModifiers.Length; i++) _statModifiers.Add(initialModifiers[i]);
			}
		}

		public virtual StatModifier AddModifier(StatModifier mod)
		{
			_isDirty = true;
			_statModifiers.Add(mod);
			onValueChanged?.Invoke(Value);
			return mod;
		}

		public virtual bool RemoveModifier(StatModifier mod)
		{
			if (_statModifiers.Remove(mod))
			{
				_isDirty = true;
				onValueChanged?.Invoke(Value);
				return true;
			}
			return false;
		}

		public virtual bool RemoveAllModifiers()
		{
			if (_statModifiers.Count > 0)
			{
				_statModifiers.Clear();
				_isDirty = true;
				onValueChanged?.Invoke(Value);
				return true;
			}

			return false;
		}

		public virtual bool RemoveAllModifiersFromSource(object source)
		{
			int numRemovals = _statModifiers.RemoveAll(mod => mod.source == source);

			if (numRemovals > 0)
			{
				_isDirty = true;
				onValueChanged?.Invoke(Value);
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

			_statModifiers.Sort(CompareModifierOrder);

			for (int i = 0; i < _statModifiers.Count; i++)
			{
				StatModifier mod = _statModifiers[i];

				if (mod.type == StatModifier.Type.Flat)
				{
					finalValue += mod.value;
				}
				else if (mod.type == StatModifier.Type.PercentAdd)
				{
					sumPercentAdd += mod.value;

					if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].type != StatModifier.Type.PercentAdd)
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

		public static implicit operator float(Stat stat) => stat.Value;
	}
}
