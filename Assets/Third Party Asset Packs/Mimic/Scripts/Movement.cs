using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MimicSpace
{
    /// <summary>
    /// This is a very basic movement script, if you want to replace it
    /// Just don't forget to update the Mimic's velocity vector with a Vector3(x, 0, z)
    /// </summary>
    public class Movement : MonoBehaviour
    {
        [Header("Controls")]
        [Tooltip("Body Height from ground")]
        [Range(0.5f, 25f)]
        public float height = 1f;
        public float speed = 1f;
        Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 2f;
        private Vector3 randomDirection = Vector3.zero;
        public float changeDirectionInterval = 1f; // Time interval to change direction
        private float directionChangeTimer = 0f;
        Mimic myMimic;

        [Header("Layer Settings")]
        [Tooltip("Layers to ignore during ground detection.")]
        public LayerMask groundDetectionMask;

        private void Start()
        {
            myMimic = GetComponent<Mimic>();
            GenerateRandomDirection(); // Generate the initial random direction
        }

        void Update()
        {
            // Update the timer and change direction if the interval has elapsed
            directionChangeTimer += Time.deltaTime;
            if (directionChangeTimer >= changeDirectionInterval)
            {
                GenerateRandomDirection();
                directionChangeTimer = 0f;
            }

            SetVelcoity();
            AdjustHeight();
        }

        private void AdjustHeight()
        {
            // Adjust the object's height based on ground detection
            RaycastHit hit;
            Vector3 destHeight = transform.position;
            if (Physics.Raycast(transform.position + Vector3.up * 5f, -Vector3.up, out hit, ~groundDetectionMask))
            {
                destHeight = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);
            }
            transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);
        }

        private void SetVelcoity()
        {
            // Smoothly interpolate velocity towards the random direction
            velocity = Vector3.Lerp(velocity, randomDirection * speed, velocityLerpCoef * Time.deltaTime);

            // Assigning velocity to the mimic to assure great leg placement
            myMimic.velocity = velocity;

            // Move the object
            transform.position = transform.position + velocity * Time.deltaTime;
        }

        // Generate a new random direction in the XZ plane
        private void GenerateRandomDirection()
        {
            float randomX = Random.Range(-0.3f, 0.3f);
            float randomZ = Random.Range(-0.3f, 0.3f);
            randomDirection = new Vector3(randomX, 0f, randomZ).normalized;
        }
    }
}