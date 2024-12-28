using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    [Header("Settings")]
    public float swayClamp = 0.09f;

    [Space]
    public float smoothing = 3f;

    private Vector3 origin;


    // Start is called before the first frame update
    void Start()
    {
        origin = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(x: Input.GetAxisRaw("Mouse X"),y: Input.GetAxisRaw("Mouse Y"));

        input.x = Mathf.Clamp(value: input.x, min: -swayClamp, max: swayClamp);
        input.y = Mathf.Clamp(value: input.y, min: -swayClamp, max: swayClamp);

        Vector3 target = new Vector3(-input.x, -input.y, z: 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, target + origin, Time.deltaTime * smoothing);
    }
}
