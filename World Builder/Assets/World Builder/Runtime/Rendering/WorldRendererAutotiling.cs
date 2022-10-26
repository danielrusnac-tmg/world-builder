﻿using System.Collections.Generic;
using Autotiler;
using UnityEngine;
using WorldBuilder.Data;

namespace WorldBuilder.Rendering
{
    public class WorldRendererAutotiling : WorldRenderer<WorldGridByte>, IAutotileMap
    {
        [SerializeField] private AutotileGenerator _generator;

        private HashSet<Vector3Int> _dirtyCells = new HashSet<Vector3Int>();

        public int Width => DataLayer.Width;
        public int Height => DataLayer.Height;
        public int Length => DataLayer.Length;
        public Vector3 CellSize => World.Layout.CellSize;
        public Vector3 WorldPosition(int x, int y, int z) => World.Layout.WorldPosition(x, y, z);
        public byte GetValue(int x, int y, int z) => DataLayer.Get(x, y, z);

        private bool IsInitialized => IsDataLayerValid;

        protected override void OnValidate()
        {
            base.OnValidate();

            if (_generator != null)
                _generator.Map = this;
        }

        private void OnEnable()
        {
            SubscribeToWorld();

            if (_generator != null)
                _generator.Map = this;
        }

        private void OnDisable()
        {
            UnsubscribeFromWorld();
        }

        private void Start()
        {
            RegenerateAll();
        }

        protected override void OnDataLayerFound(WorldGridByte dataLayer)
        {
            UnsubscribeFromWorld();
            SubscribeToWorld();
            dataLayer.CellChanged += OnCellChanged;
        }

        protected override void OnDataLayerLost()
        {
            if (DataLayer != null)
                DataLayer.CellChanged -= OnCellChanged;

            UnsubscribeFromWorld();
        }

        [ContextMenu(nameof(RegenerateAll))]
        private void RegenerateAll()
        {
            if (IsInitialized)
                _generator.RegenerateAll();
        }

        [ContextMenu(nameof(DestroyAll))]
        private void DestroyAll()
        {
            _generator.DestroyAll();
        }

        private void OnCellChanged(int x, int y, int z)
        {
            _dirtyCells.Add(new Vector3Int(x, y, z));
        }

        private void OnWorldChanged()
        {
            _generator.RegenerateDirty(_dirtyCells);
            _dirtyCells.Clear();
        }
        
        private void OnWorldChangedAll()
        {
            _generator.RegenerateAll();
            _dirtyCells.Clear();
        }

        private void SubscribeToWorld()
        {
            if (World != null)
            {
                World.Changed += OnWorldChanged;
                World.ChangedAll += OnWorldChangedAll;
            }
        }

        private void UnsubscribeFromWorld()
        {
            if (World != null)
            {
                World.Changed -= OnWorldChanged;
                World.ChangedAll -= OnWorldChangedAll;
            }
        }
    }
}