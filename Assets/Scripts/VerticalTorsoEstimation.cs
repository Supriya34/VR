using UnityEngine;

namespace Vrsys
{
    public class VerticalTorsoEstimation : MonoBehaviour
    {
        [Tooltip("The head transform which is used to align the body to. If nothing is specified, transform of parent GameObject will be used.")]
        public Transform headTransform;
        public bool lookingDown = false;
        private void Awake()
        {
            if (headTransform == null)
            {
                headTransform = transform.parent;
            }
        }

        public static float Remap(float input, float oldLow, float oldHigh, float newLow, float newHigh) {
            //https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/#post-6960476
            float t = Mathf.InverseLerp(oldLow, oldHigh, input);
            return Mathf.Lerp(newLow, newHigh, t);
        }

        void Update()
        {
            // TODO Exercise 1.6
            //https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html
            //Task 1 The body stays aligned with the global Y-axis.
            var y_rotation = headTransform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, y_rotation, 0);
            
            //Task 2 The body slides back slightly as the head tilts downward.
            var x_rotation = headTransform.eulerAngles.x;
            //print(x_rotation) //what values do we have?
            transform.position = headTransform.position; //head and body have their coord origin in the same place
            lookingDown = (x_rotation > 0 && x_rotation < 90);
            if(lookingDown){
                var z_offset = Remap(x_rotation,0,90,0,-0.3f);
                transform.Translate(0,0,z_offset,Space.Self);
            }
        }
    }
}
