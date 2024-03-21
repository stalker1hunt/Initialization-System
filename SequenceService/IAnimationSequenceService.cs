using System.Collections.Generic;
using DG.Tweening;

namespace Game.SequenceService
{
    public interface IAnimationSequenceService
    {
        Sequence CreateSequence(IEnumerable<(TweenCallback action, float duration)> steps);
        void SkipCurrentSequence();
        void KillAllSequences();
        bool SequenceQueueContains(Sequence sequence);
        Sequence GetCurrentSequence();
        bool IsSequenceQueueEmpty();
    }
}