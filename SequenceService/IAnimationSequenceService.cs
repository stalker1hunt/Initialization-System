using DG.Tweening;
using System.Collections.Generic;

namespace Game.SequenceService
{
    /// <summary>
    /// Defines the contract for an animation sequence service that manages and controls animation sequences.
    /// </summary>
    public interface IAnimationSequenceService
    {
        /// <summary>
        /// Creates an animation sequence from a series of steps, each defined by an action and its duration.
        /// </summary>
        /// <param name="steps">A collection of steps, where each step consists of an action (TweenCallback) and its duration (float).</param>
        /// <returns>The created Sequence object.</returns>
        Sequence CreateSequence(IEnumerable<(TweenCallback action, float duration)> steps);

        /// <summary>
        /// Skips the current animation sequence, immediately moving to the next sequence in the queue if available.
        /// </summary>
        void SkipCurrentSequence();

        /// <summary>
        /// Terminates all animation sequences, clearing the queue and stopping any ongoing animations.
        /// </summary>
        void KillAllSequences();

        /// <summary>
        /// Checks if the specified sequence is currently in the animation queue.
        /// </summary>
        /// <param name="sequence">The Sequence object to check.</param>
        /// <returns>True if the sequence is in the queue; otherwise, false.</returns>
        bool SequenceQueueContains(Sequence sequence);

        /// <summary>
        /// Retrieves the current animation sequence being played.
        /// </summary>
        /// <returns>The currently playing Sequence object, or null if no sequence is active.</returns>
        Sequence GetCurrentSequence();

        /// <summary>
        /// Determines whether the animation queue is empty.
        /// </summary>
        /// <returns>True if there are no sequences in the queue; otherwise, false.</returns>
        bool IsSequenceQueueEmpty();
    }
}