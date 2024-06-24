using RoadToAAA.ProjectClock.Core;
using RoadToAAA.ProjectClock.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Managers
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private GameObject HandExplosionPrefab;

        private StaticPool _handExplosionsPool;
        private List<ParticleSystem> _particles;

        #region UnityMessages
        private void Awake()
        {
            _handExplosionsPool = new StaticPool(HandExplosionPrefab, 4);
            _particles = new List<ParticleSystem>();
        }

        private void OnEnable()
        {
            EventManager<Clock, Clock>.Instance.Subscribe(EEventType.OnNewClockSelected, NewClockSelected);
        }

        private void OnDisable()
        {
            EventManager<Clock, Clock>.Instance.Unsubscribe(EEventType.OnNewClockSelected, NewClockSelected);
        }
        #endregion

        private void NewClockSelected(Clock newClock, Clock oldClock)
        {
            if (oldClock == null) return;

            Transform handTransform = oldClock.HandTransform;

            if (!_handExplosionsPool.HasItems()) return;

            GameObject HandExplosionGameObject = _handExplosionsPool.Get(true);
            HandExplosionGameObject.transform.position = handTransform.position;
            ParticleSystem particleSystem = HandExplosionGameObject.transform.GetChild(0).GetComponent<ParticleSystem>();

            HandExplosionGameObject.transform.rotation = Quaternion.Euler(0.0f, HandExplosionGameObject.transform.rotation.eulerAngles.y, handTransform.rotation.eulerAngles.z);

            ParticleSystem.MainModule particleMain = particleSystem.main;
            particleMain.startColor = oldClock.ClockParameters.HandColor;

            
            _particles.Add(particleSystem);
            particleSystem.Play();
            StartCoroutine(ParticleCompletitionDespawn(HandExplosionGameObject, particleSystem));
        }

        private IEnumerator ParticleCompletitionDespawn(GameObject particleSystemGameObejct, ParticleSystem particleSystem)
        {
            yield return new WaitForSeconds(particleSystem.main.duration + particleSystem.main.startLifetime.constant);

            _handExplosionsPool.Release(particleSystemGameObejct);
            _particles.Remove(particleSystem);
        }
    }
}
