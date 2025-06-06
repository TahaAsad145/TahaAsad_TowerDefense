using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 4f;
    private int currentWayPoint = 0;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if(animator != null)
        {
            animator.Play("Walk");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Length == 0 || currentWayPoint >= waypoints.Length) return;

        Transform targ = waypoints[currentWayPoint];

        transform.position = Vector3.MoveTowards(transform.position, targ.position, speed * Time.deltaTime);

        Debug.Log("Current speed: " + speed);


        if (Vector3.Distance(transform.position, targ.position) < 0.5f)
        {
            currentWayPoint++;
            if (currentWayPoint >= waypoints.Length)
            {
                Destroy(gameObject);
                Debug.Log("Live Lost | Enemy Reached the end!");
            }
        }
    }
}
