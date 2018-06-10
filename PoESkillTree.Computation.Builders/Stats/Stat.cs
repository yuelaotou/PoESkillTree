﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PoESkillTree.Computation.Common;

namespace PoESkillTree.Computation.Builders.Stats
{
    public class Stat : IStat
    {
        public Stat(
            string identity, Entity entity = default, bool isRegisteredExplicitly = false, Type dataType = null,
            IReadOnlyCollection<Behavior> behaviors = null, bool hasRange = true)
        {
            Identity = identity;
            _hasRange = hasRange;
            Entity = entity;
            IsRegisteredExplicitly = isRegisteredExplicitly;
            DataType = dataType ?? typeof(double);
            Behaviors = behaviors ?? new Behavior[0];
        }

        public static IStat CopyWithSuffix(
            IStat source, string identitySuffix, bool isRegisteredExplicitly = false, Type dataType = null,
            IReadOnlyCollection<Behavior> behaviors = null, bool hasRange = true)
        {
            return new Stat(source.Identity + "." + identitySuffix, source.Entity, isRegisteredExplicitly,
                dataType ?? source.DataType, behaviors, hasRange);
        }

        private readonly bool _hasRange;
        public string Identity { get; }
        public Entity Entity { get; }
        public bool IsRegisteredExplicitly { get; }
        public Type DataType { get; }
        public IReadOnlyCollection<Behavior> Behaviors { get; }

        public IStat Minimum => MinOrMax();
        public IStat Maximum => MinOrMax();

        private IStat MinOrMax([CallerMemberName] string identitySuffix = null) =>
            _hasRange ? CopyWithSuffix(this, identitySuffix, hasRange: false) : null;

        public override string ToString() => Identity;

        public override bool Equals(object obj) =>
            (obj == this) || (obj is IStat other && Equals(other));

        public bool Equals(IStat other) =>
            (other != null) && ToString().Equals(other.ToString()) && Entity == other.Entity;

        public override int GetHashCode() =>
            (Identity, Entity).GetHashCode();
    }
}