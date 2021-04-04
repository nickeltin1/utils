using UnityEngine;


namespace SimpleMan.EventSystem.Demo
{
    public class Jumper : MonoBehaviour
    {
        ////******            FIELDS          	******\\
        public GameEvent gameEvent;
        public float force = 10;


        private Rigidbody _rigidbody;




        //******    	    METHODS  	  	    ******\\
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }


        private void OnEnable()
        {
            gameEvent.AddListener(OnMessageReceive);
        }


        private void OnDisable()
        {
            gameEvent.RemoveListener(OnMessageReceive);
        }


        private void OnMessageReceive()
        {
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    } 
}