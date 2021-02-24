/* Just stores and selects the difficulty settings. 
 * The script for taking damage was modifed to check the difficuly values.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Difficulty : MonoBehaviour
{
    [System.Serializable]
    public struct s_Difficulty {
        public string displayName;
        public float damage;
        public float enemyDamage;
    }

    public s_Difficulty[] difficulties;
    public int selectedDifficulty = 0;

    void Start()
    {
		//Should perset
        DontDestroyOnLoad(this.gameObject);
    }

    public s_Difficulty GetDifficulty() {
        return difficulties[selectedDifficulty];
    }

	//Toggles though the difficulty settings by traversing the difficlty array
    public void cycleDifficulties(Text text) {
        selectedDifficulty = ++selectedDifficulty % difficulties.Length;
		//Is passed the difficulty toggling button, changes the text
        text.text = GetDifficulty().displayName;
    }
}
