using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    protected float ySpeed = 0.75f;
    protected float xSpeed = 1.0f;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }


    protected virtual void UpdateMotor(Vector3 input)
    {
        // moveDelta zur�cksetzen
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        // Richtung vom sprite wenn man rechts o. links geht
        if (moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // r�cksto� hinzuf�gen
        moveDelta += pushDirection;


        //Reduziere den r�chsto� bei jedem Frame mit der Erholungsgeschwindigkeit
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);




        //sicherstellen das wir uns in diese richtung bewegen k�nnen indem wir eine box zuerst hingeben wenn "null" zur�ckgegeben wird k�nnen wir uns bewegen
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Bewegung
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Bewegung
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }

}
