﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics.Materials {
    public class CharacterMaterial : XivMaterial {
        #region Fields
        private ShaderResourceView _Diffuse;
        private ShaderResourceView _Specular;
        private ShaderResourceView _Normal;
        private ShaderResourceView _Mask;
        private ShaderResourceView _Table;
        #endregion

        #region Properties
        public ShaderResourceView Diffuse { get { return _Diffuse; } }
        public ShaderResourceView Specular { get { return _Specular; } }
        public ShaderResourceView Normal { get { return _Normal; } }
        public ShaderResourceView Mask { get { return _Mask; } }
        public ShaderResourceView Table { get { return _Table; } }

        public new Effects.CharacterEffect Effect { get { return (Effects.CharacterEffect)base.Effect; } }
        #endregion

        #region Constructor
        public CharacterMaterial(Device device, Assets.MaterialVersion material)
            : base(device, material, EffectCache.Get(device).CharacterEffect) {


            var texCache = TextureCache.Get(device);
            foreach (var texParam in material.ParameterMappings) {
                var tex = material.Textures[texParam.Index];

                string target;
                if (!Utilities.Effect.CharacterParameterMap.TryGetValue(texParam.Id, out target)) {
                    System.Diagnostics.Trace.WriteLine(string.Format("Failed to map parameter with id {0:X8}h (on {1})", texParam.Id, tex.Path));
                    continue;
                }

                switch (target) {
                    case "Diffuse":
                        _Diffuse = texCache.GetResource(tex);
                        break;
                    case "Specular":
                        _Specular = texCache.GetResource(tex);
                        break;
                    case "Normal":
                        _Normal = texCache.GetResource(tex);
                        break;
                    case "Mask":
                        _Mask = texCache.GetResource(tex);
                        break;
                    case "Table":
                        _Table = texCache.GetResource(tex);
                        break;
                    default:
                        System.Diagnostics.Trace.WriteLine(string.Format("Unknown parameter {0} for {1}.", texParam.Id, tex.Path));
                        break;
                }
            }

            CurrentTechniqueName = Utilities.Effect.GetTechniqueName(material);
        }
        #endregion

        #region Apply
        public override void Apply() {
            Effect.Diffuse = this.Diffuse;
            Effect.Specular = this.Specular;
            Effect.Normal = this.Normal;
            Effect.Mask = this.Mask;
            Effect.Table = this.Table;
        }
        #endregion
    }
}