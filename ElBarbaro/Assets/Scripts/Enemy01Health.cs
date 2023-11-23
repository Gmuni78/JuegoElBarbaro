using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy01Health : MonoBehaviour
{
    //Variable de salud de vidad con un valor cargado.
    [SerializeField]
    private int startingHealth = 20;
    //Tiempo entre da�os al enemigo.
    [SerializeField]
    private float timeSinceLastHit;
    //Tiempo de desaparici�n del enemigo.
    [SerializeField]
    private float dissapearSpeed = 2f;
    //Variable con el valor del da�o.
    [SerializeField]
    private int currentHealth;

    //control del tiempo.
    private float timer = 0f;
    //Carga de la animaci�n.
    private Animator anim;
    //Coger NavMex para poder utilizarlo
    private NavMeshAgent nav;
    //Saber si est� muerto.
    private bool isAlive;
    //Cogemos el rigigbody del enemigo
    private Rigidbody rigidBbody;
    //Coger el collider
    private CapsuleCollider capsuleCollider;
    //Preguntar si ha desaparecido.
    private bool dissapearEnemy = false;
    //BoxCollider del arma
    private BoxCollider weaponCollider;
    //Variable para que emita sonido al enemigo.
    private AudioSource audio;
    //Para reproducir el audio herido.
    [SerializeField]
    private AudioClip hurtAudio;
    //Para reproducir el audio muerto.
    [SerializeField]
    private AudioClip dieAudio;

    //Hacemos una propiedad de di est� vivo.
    public bool IsAlive
    {
        //Devolvemos si esta vivo o muerto.
        get { return isAlive; } 
    }
    void Start()
    {
        //Cogemos el BoxCllider hijo.
        weaponCollider = GetComponentInChildren<BoxCollider>();
        //Inicializar las variables.
        rigidBbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        isAlive = true;
        currentHealth = startingHealth;
        audio = GetComponent<AudioSource>();
    }

  
    void Update()
    {
        //Cogemos el tiempo real.
        timer += Time.deltaTime;
        //Hundimos el personaje.
        if (dissapearEnemy)
        {
            //Movemos el cadaver al interior de la plataforma.
            transform.Translate(Vector3.down * dissapearSpeed * Time.deltaTime);
        }
    }
    //Cuando colisiona el cuchillo con el enemigo.
    private void OnTriggerEnter(Collider other)
    {
        //Preguntamos si se le puede golpear y si esta vivo.
        if (timer >= timeSinceLastHit && !GameManager.instance.GameOver)
        {
            //Si golpea el cuchillo.
            if (other.tag == "PlayerWeapon")
            {
                //Llamamos el m�todo de descaontar vida
                takehit();
                //Reiniciamos el tiempo entre golpes
                timer = 0f;
            }
        }
    }
    //M�tod de quitar vida
    private void takehit()
    {
        //Si tiene vida
        if (currentHealth > 0)
        {
            //Emite el sonido herido una sola vez.
            audio.PlayOneShot(hurtAudio);
            anim.Play("Hurt");
            currentHealth -= 10;
        }
        //Si est� el enemigo muerto 
        if (currentHealth<=0)
        {
            isAlive = false;
            //Llamamos al m�todo de animaci�n de morirse.
            KillEmemy();
        }
    }
    //M�todo de activar animaci�n de muerte de enemigo
    private void KillEmemy()
    {
        //Desactivar el Collider.
        capsuleCollider.enabled = false;
        //Activar la animaci�n de morirse.
        //Emite el sonido muerto una sola vez.
        audio.PlayOneShot(dieAudio);
        anim.SetTrigger("EnemyDie");
        //Activar la funci�n manual del rigidbody.
        rigidBbody.isKinematic = true;
        //activar la coroutine.
        StartCoroutine(removeEnemy());
        //Desactivamos el Collider del arma
        weaponCollider.enabled = false;
        //Desactivar el camino.
        nav.enabled = false;
    }
    // Tiempo que tarde en desaparecer el personaje.
    IEnumerator removeEnemy()
    {
        //Hacer desaparecer el enemigo a los 2 segundos.
        yield return new WaitForSeconds(2f);
        dissapearEnemy = true;
        //Destruye el objeto al cabo de 1 segundo.
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
