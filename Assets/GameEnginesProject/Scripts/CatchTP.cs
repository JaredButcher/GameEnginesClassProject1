/* For a large trigger zone under a parcore section to teleport you back to the start if you fall, because dieing was too annoying
 *
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CatchTP : MonoBehaviour
{
    [Tooltip("Teleport player here upon entering the catch")]
    public GameObject TPDestination;

    private GameObject objToMove = null;

    private void OnTriggerStay(Collider collider) {
        //If a player enters the trigger box, move to tp point and arrest velocity
        if(collider.gameObject.GetComponent<PlayerCharacterController>()){
            objToMove = collider.gameObject;
        }
    }

    private void LateUpdate() {
        //Has to run after the CharacterController and PlayerCharacterController
        if (objToMove) {
            objToMove.GetComponent<PlayerCharacterController>().characterVelocity = new Vector3(0, 0, 0);
            objToMove.GetComponent<CharacterController>().SimpleMove(Vector3.zero);
            objToMove.transform.position = TPDestination.transform.position;
            objToMove = null;
        }
    }
}
