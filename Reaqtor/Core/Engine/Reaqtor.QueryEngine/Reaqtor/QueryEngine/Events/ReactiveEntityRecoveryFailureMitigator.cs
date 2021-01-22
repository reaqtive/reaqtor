// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine.Events
{
    /// <summary>
    /// Base class for mitigators used upon failure to load a reactive entity during recovery.
    /// </summary>
    internal class ReactiveEntityRecoveryFailureMitigator
    {
        /// <summary>
        /// Called for every type of mitigation. Allows the derived class to run some actions before performing the mitigation.
        /// </summary>
        /// <param name="entity">The entity for which a mitigation is applied.</param>
        protected virtual void OnAll(ReactiveEntity entity) { }

        /// <summary>
        /// Called upon applying an <see cref="ReactiveEntityRecoveryFailureMitigation.Ignore"/> mitigation.
        /// </summary>
        /// <param name="entity">The entity for which a mitigation is applied.</param>
        /// <returns>true if the mitigation was applied; otherwise, false.</returns>
        protected virtual bool OnIgnore(ReactiveEntity entity) => true;

        /// <summary>
        /// Called upon applying a <see cref="ReactiveEntityRecoveryFailureMitigation.Remove"/> mitigation.
        /// </summary>
        /// <param name="entity">The entity for which a mitigation is applied.</param>
        /// <returns>true if the mitigation was applied; otherwise, false.</returns>
        protected virtual bool OnRemove(ReactiveEntity entity) => false;

        /// <summary>
        /// Called upon applying a <see cref="ReactiveEntityRecoveryFailureMitigation.Regenerate"/> mitigation.
        /// </summary>
        /// <param name="entity">The entity for which a mitigation is applied.</param>
        /// <returns>true if the mitigation was applied; otherwise, false.</returns>
        protected virtual bool OnRegenerate(ReactiveEntity entity) => false;

        /// <summary>
        /// Applies the specified <paramref name="mitigation"/> for the specified <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">The entity for which a mitigation is applied.</param>
        /// <param name="mitigation">The mitigation to apply.</param>
        /// <returns>true if a mitigation was applied; otherwise, false.</returns>
        public bool DoMitigate(ReactiveEntity entity, ReactiveEntityRecoveryFailureMitigation mitigation)
        {
            OnAll(entity);

            return mitigation switch
            {
                ReactiveEntityRecoveryFailureMitigation.Ignore => OnIgnore(entity),
                ReactiveEntityRecoveryFailureMitigation.Remove => OnRemove(entity),
                ReactiveEntityRecoveryFailureMitigation.Regenerate => OnRegenerate(entity),
                _ => false,
            };
        }
    }
}
