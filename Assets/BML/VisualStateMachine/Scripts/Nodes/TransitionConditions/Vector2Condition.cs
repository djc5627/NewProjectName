using System;
using System.Collections.Generic;
using MyAssets.ScriptableObjects.Variables;
using MyAssets.ScriptableObjects.Variables.ValueReferences;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
[HideReferenceObjectPicker]
public class Vector2Condition : ITransitionCondition
{
    [HideLabel, Required, SerializeField, HideReferenceObjectPicker]
    private Vector2Reference targetParameter;

    [HideLabel] public Comparison xCompare;
    [HideLabel] public Comparison yCompare;

    private string parentTransitionName = "";

    
    public enum Comparator
    {
        Ignore,
        LessThan,
        GreaterThan,
        AbsGreaterThan,
        AbsLessThan,
    }
    
    [Serializable]
    public struct Comparison
    {
        public Comparator comparator;
        public FloatValueReference value;
    }

    public void Init(string transitionName)
    {
        parentTransitionName = transitionName;
    }

    public bool Evaluate(List<TriggerVariable> receivedTriggers)
    {
        bool xIs = Compare(xCompare, targetParameter.Value.x);
        bool yIs = Compare(yCompare, targetParameter.Value.y);
        return xIs && yIs;
    }

    public override string ToString()
    {
        return $"{targetParameter.Name}.X {xCompare.comparator} {xCompare.value.Name}.X &&" +
               $" {targetParameter.Name}.Y {yCompare.comparator} {yCompare.value.Name}.Y";
    }

    private bool Compare(Comparison comparison, float paramValue)
    {
        switch (comparison.comparator)
        {
            case Comparator.GreaterThan:
                return paramValue > comparison.value.Value;
            case Comparator.LessThan:
                return paramValue < comparison.value.Value;
            case Comparator.AbsGreaterThan:
                return Mathf.Abs(paramValue) > comparison.value.Value;
            case Comparator.AbsLessThan:
                return Mathf.Abs(paramValue) < comparison.value.Value;
            case Comparator.Ignore:
                return true;
        }
        
        Debug.Log(this.ToString());

        return false;
    }
}