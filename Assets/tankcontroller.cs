using UnityEngine;
using UnityEngine.InputSystem;

public class tankcontroller : MonoBehaviour
{
    public float movespeed = 5f;
    public float rotatespeed = 90f;
    public float hoverheight = 0.5f;
    public Transform pivot;
    public Transform gunbarrel;

    private Vector2 moveinput;

    void OnMove(InputValue value)
    {
        moveinput = value.Get<Vector2>();
        moveinput = Vector2.ClampMagnitude(moveinput, 1f);
    }

    void OnAttack(InputValue value)
    {
        RaycastHit hit;

        if (Physics.Raycast(gunbarrel.position, pivot.forward, out hit))
        {
            if (hit.collider.tag != "unpaintable")
            {
                hit.collider.GetComponent<Renderer>().material.color = Color.yellow;
            }
        }

        Debug.DrawRay(gunbarrel.position, pivot.forward * 10f, Color.red, 1f);
    }

    void Update()
    {
        pivot.Rotate(Vector3.up * moveinput.x * rotatespeed * Time.deltaTime);

        RaycastHit hit;
        Vector3 rayorigin = transform.position + Vector3.up * 3f;

        if (Physics.Raycast(rayorigin, Vector3.down, out hit, 15f))
        {
            transform.up = Vector3.Lerp(transform.up, hit.normal, Time.deltaTime * 15f);

            Vector3 targetposition = hit.point + transform.up * hoverheight;
            transform.position = Vector3.Lerp(transform.position, targetposition, Time.deltaTime * 20f);

            Vector3 movedir = Vector3.ProjectOnPlane(pivot.forward, hit.normal).normalized;
            transform.Translate(movedir * moveinput.y * movespeed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(pivot.forward * moveinput.y * movespeed * Time.deltaTime, Space.World);
        }
    }
}