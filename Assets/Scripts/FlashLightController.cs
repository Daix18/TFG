using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    public static FlashLightController THIS;

    //Velocidad del offset.
    [SerializeField] float speed = 3.0f;

    //Gameobject seguir
    GameObject goFollow;

    // Start is called before the first frame update
    void Start()
    {
        goFollow = Camera.main.gameObject;
    }

    private void Awake()
    {
        THIS = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Va a transformar la posicion de la linterna basandose en la posicion de la camara mas el offset que nosotros le pongamos.
        transform.position = goFollow.transform.position;

        //Lo mismo pero para la rotacion.
        transform.rotation = Quaternion.Slerp(transform.rotation, goFollow.transform.rotation, speed * Time.deltaTime);
    }
}
