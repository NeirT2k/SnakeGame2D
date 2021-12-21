using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MouseFollowScript : MonoBehaviour
{
    public float speed = 0.5f;
    public float smoothTime = 0.5f;
    public Transform bodyObject;
    public List<Transform> bodyParts = new List<Transform>();
    public float spawnFoodEveryXSeconds = 1;
    public GameObject foodPrelab;
    public float speedWhileRunnin = 6.5f;
    public float speedWhileWalking = 3.5f;
    public float bodyPartFollowTimeWalking = 0.19f;
    public float bodyPartFollowTimeRunning = 0.1f;
    bool running = false;
    private int foodCounter;
    public int growOnEveryXFood;
    public float growthRate = 0.1f;
    public float bodyPartOverTimeFollow = 0.19f;
    private Vector3 currentSize = Vector3.one;
    // Start is called before the first frame update
    void Start()
    {
        SpawnFoodManager();
    }

    // Update is called once per frame
    void Update()
    {
        Running();
        MouseFollowRotate();
    }
    
    void FixedUpdate()
    {
        MoveFoward();
        ApplyingStuffForBody();
    }
    
    void MouseFollowRotate()
	{
        Vector3 mousePos;
        Vector3 diff;
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        diff = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
    
    void MoveFoward()
	{
        transform.position += transform.up * speed * Time.deltaTime;
    }
    

    void OnCollisionEnter2D(Collision2D other)
    {
        
        
        if (other.transform.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            foodCounter++;
            if (SizeUp(foodCounter) == false)
            {
                if (bodyParts.Count == 0)
                {
                    Vector3 currentPos = transform.position;
                    Transform newBodyPart = Instantiate(bodyObject, currentPos, Quaternion.identity) as Transform;
                    newBodyPart.localScale = currentSize;
                    newBodyPart.GetComponent<SnakeBody>().overTime = bodyPartOverTimeFollow;
                    bodyParts.Add(newBodyPart);
                }
                else
                {
                    Vector3 currentPos = bodyParts[bodyParts.Count - 1].position;
                    Transform newBodyPart = Instantiate(bodyObject, currentPos, Quaternion.identity) as Transform;
                    newBodyPart.localScale = currentSize;
                    newBodyPart.GetComponent<SnakeBody>().overTime = bodyPartOverTimeFollow;
                    bodyParts.Add(newBodyPart);
                }
            }
			else
			{
                currentSize += Vector3.one * growthRate;
                bodyPartOverTimeFollow += 0.04f;
                transform.localScale = currentSize;
                foreach(Transform bodyPart_ith in bodyParts)
				{
                    bodyPart_ith.localScale = currentSize;
                    bodyPart_ith.GetComponent<SnakeBody>().overTime = bodyPartOverTimeFollow;
				}
			}
        }
        
    } 
    
    void SpawnFoodManager()
	{
        StartCoroutine("CallEveryFewSeconds", spawnFoodEveryXSeconds);
	}
    IEnumerator CallEveryFewSeconds(float x)
	{
        while(true)
        {
            yield return new WaitForSeconds(x);
            Vector3 randomNewFoodPosition = new Vector3(
                Random.Range(
                    Random.Range(transform.position.x - 10, transform.position.x - 5),
                    Random.Range(transform.position.x + 5, transform.position.x + 10)),
                Random.Range(
                    Random.Range(transform.position.y - 10, transform.position.y - 5),
                    Random.Range(transform.position.y + 5, transform.position.y + 10)),
                0);
            GameObject newFood = Instantiate(foodPrelab, randomNewFoodPosition, Quaternion.identity) as GameObject;
            GameObject foodParent = GameObject.Find("Food");
            newFood.transform.parent = foodParent.transform;
        }
        
    }
    
    void Running()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            speed = speedWhileRunnin;
            running = true;
            bodyPartOverTimeFollow = bodyPartFollowTimeRunning;
        }
        if (Input.GetMouseButtonUp(0))
        {
            speed = speedWhileWalking;
            running = false;
            bodyPartOverTimeFollow = bodyPartFollowTimeWalking;
        }
        if (running == true)
        {
            //GlowingSnake(true);
        }
        else
        {
            bodyPartOverTimeFollow = bodyPartFollowTimeWalking;
        }
    }
    bool SizeUp(int x)
	{
        if (x%growOnEveryXFood==0)
        {
            return true;
        
        }
        else
            return false; 
	}
    void GlowingSnake(bool key)
    {
        foreach (Transform bodyParts_x in bodyParts)
        {
            bodyParts_x.Find("glow").gameObject.SetActive(key);
        }

    }
    
    private Vector3 headV;
    void ApplyingStuffForBody()
    {
        transform.localScale = Vector3.SmoothDamp(transform.localScale, currentSize, ref headV, 0.5f);
        foreach (Transform bodyPart_x in bodyParts)
        {
            bodyPart_x.localScale = transform.localScale;
            bodyPart_x.GetComponent<SnakeBody>().overTime = bodyPartOverTimeFollow;
        }
    }
}
