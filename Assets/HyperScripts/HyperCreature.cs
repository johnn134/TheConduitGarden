using UnityEngine;
using System.Collections;

public class HyperCreature : MonoBehaviour {

	public int w = 0;                       //point on w axis
	public int w_perif = 0;                 //the perifial view of the w axis

    public static HyperCreature instance = null;

    UnityEngine.UI.Image fadeImage;

    void Awake()
    {
        fadeImage = GameObject.Find("Canvas/Image").GetComponent<UnityEngine.UI.Image>();

        fadeImage.material.color = Color.black;

        //declare as singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //add this later for persistant creature
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            WMove(-1);
            WMoveAllHyperObjects();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            WMove(1);
            WMoveAllHyperObjects();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            w_perif++;
            WMoveAllHyperObjects();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            w_perif--;
            WMoveAllHyperObjects();
        }
    }

    public bool FadeOutTransitionStep(float speed)
    {
        if(fadeImage.material.color.a < 1f)
        {
            var fadeMat = fadeImage.material;
            fadeMat.color = new Color(fadeMat.color.r, fadeMat.color.g, fadeMat.color.b, fadeMat.color.a + speed);
            return false;
        }
        return true;

    }

    public bool FadeInTransitionStep(float speed)
    {
        if (fadeImage.material.color.a > 0f)
        {
            var fadeMat = fadeImage.material;
            fadeMat.color = new Color(fadeMat.color.r, fadeMat.color.g, fadeMat.color.b, fadeMat.color.a - speed);
            return false;
        }
        return true;

    }

    void OnDestroy()
    {
        var fadeMat = fadeImage.material;
        fadeMat.color = new Color(fadeMat.color.r, fadeMat.color.g, fadeMat.color.b, 0f);
    }

    public void WMoveAllHyperObjects()
    {
        var allHyper = Object.FindObjectsOfType<HyperObject>();

        foreach (HyperObject hyperObj in allHyper)
            hyperObj.WMove();
    }

	//public function for other objects to call to tell the creature to move along the w axis
	public void WMove(int deltaW){
		if((deltaW > 0 && w != HyperObject.W_RANGE) || (deltaW < 0 && w != 0)){
			w += deltaW;
		}
	}
}
