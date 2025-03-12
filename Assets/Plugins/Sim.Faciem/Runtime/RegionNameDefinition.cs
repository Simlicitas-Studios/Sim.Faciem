﻿using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sim.Faciem
{
    [CreateAssetMenu(fileName = "Region", menuName = "Faciem/Region")]
    public class RegionNameDefinition : ScriptableObject
    {
        [FormerlySerializedAs("Name")] [SerializeField]
        private string _name;

        public bool SourceCodeGeneration;

        public MonoScript SourceFile;

        public RegionName Name => RegionName.From(_name);
        
        #if UNITY_EDITOR

        internal Action<RegionNameDefinition> ExecuteCodeGeneration { get; set; }
        
        private void OnValidate()
        {
            ExecuteCodeGeneration?.Invoke(this);
        }

        #endif
    }
}