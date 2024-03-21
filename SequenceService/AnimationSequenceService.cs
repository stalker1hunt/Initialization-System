using System;
using System.Collections.Generic;
using DG.Tweening;

namespace Game.SequenceService
{
    public class AnimationSequenceService : IAnimationSequenceService
    {
        private readonly Queue<Sequence> _sequences = new Queue<Sequence>();

        private readonly Dictionary<Sequence, List<TweenCallback>> _sequenceActions = new Dictionary<Sequence, List<TweenCallback>>();
        private readonly Dictionary<Sequence, object[]> _sequenceParameters = new Dictionary<Sequence, object[]>();

        private Sequence _currentSequence;
        private bool _isPlaying = false;
        
        public Sequence CreateSequenceWithParameter(IEnumerable<(TweenCallback action, float duration)> steps,  params object[] parameters)
        {
            var sequence = CreateSequence(steps);
            _sequenceParameters[sequence] = parameters;
            return sequence;
        }

        public Sequence CreateSequence(IEnumerable<(TweenCallback action, float duration)> steps)
        {
            var sequence = DOTween.Sequence();
            var actions = new List<TweenCallback>();
            
            foreach (var (action, duration) in steps)
            {
                actions.Add(action);
                sequence.AppendCallback(action).AppendInterval(duration);
            }

            sequence.OnComplete(() => HandleSequenceCompletion(sequence));
            _sequences.Enqueue(sequence);
            _sequenceActions[sequence] = actions;

            if (!_isPlaying)
            {
                PlayNextSequence();
            }

            return sequence;
        }
        
        public object[] GetSequenceParameter(Sequence sequence)
        {
            if (_sequenceParameters.TryGetValue(sequence, out object[] parameters))
            {
                return parameters;
            }

            return Array.Empty<object>(); 
        }

        private void PlayNextSequence()
        {
            if (_sequences.Count > 0)
            {
                _isPlaying = true;
                _currentSequence = _sequences.Dequeue();
                _currentSequence.Play();
            }
        }

        private void HandleSequenceCompletion(Sequence sequence)
        {
            _sequenceActions.Remove(sequence);
            _sequenceParameters.Remove(sequence);
            _currentSequence = null;
            _isPlaying = false;
            PlayNextSequence();
        }

        public void SkipCurrentSequence()
        {
            if (_currentSequence != null && _currentSequence.IsActive())
            {
                foreach (var action in _sequenceActions[_currentSequence])
                {
                    action?.Invoke();
                }

                _currentSequence.Kill();
                _sequenceActions.Remove(_currentSequence);
                _sequenceParameters.Remove(_currentSequence);

                _currentSequence = null;
                _isPlaying = false;
                PlayNextSequence();
            }
        }

        public void KillAllSequences()
        {
            if (_currentSequence != null && _currentSequence.IsActive())
            {
                _currentSequence.Kill();
            }

            foreach (var sequence in _sequences)
            {
                sequence.Kill();
            }

            _sequences.Clear();
            _sequenceActions.Clear();
            _sequenceParameters.Clear();
            _currentSequence = null;
            _isPlaying = false;
        }
        
        
        public bool SequenceQueueContains(Sequence sequence)
        {
            return _sequences.Contains(sequence) || (_currentSequence == sequence && _currentSequence.IsActive());
        }

        public Sequence GetCurrentSequence()
        {
            return _currentSequence;
        }

        public bool IsSequenceQueueEmpty()
        {
            return _sequences.Count == 0 && (_currentSequence == null || !_currentSequence.IsActive());
        }
    }
}
}