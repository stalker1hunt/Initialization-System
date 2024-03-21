using System.Collections.Generic;
using Game.AnimationSequence;
using DG.Tweening;

namespace Game.SequenceService
{
    public class AnimationSequenceServiceTests
    {
        [Test]
        public void AnimationSequenceService_AddsSequenceToQueue()
        {
            var service = new AnimationSequenceService();
            var sequence = service.CreateSequence(new List<(TweenCallback, float)> { 
                (() => {}, 1.0f) 
            });

            Assert.IsTrue(service.SequenceQueueContains(sequence));
        }
    }
}