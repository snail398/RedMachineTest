using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CircleController : MonoBehaviour
{
    public Circle circle = new Circle();
    SpriteRenderer sprite;
    SpriteRenderer areaSprite;
    List<Circle> circlesList;
    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void InitCircle(Team team, float speed, Vector2 direction, float radius,SpriteRenderer areaSprite, float currentSpeed)
    {
        circle.team = team;
        sprite.color = team.color;
        circle.baseSpeed = speed;
        circle.direction = direction;
        this.areaSprite = areaSprite;
        transform.localScale = new Vector2(radius, radius);
        circle.scale = transform.localScale;
        circle.position = transform.position;
        circle.currentSpeed = currentSpeed;
    }

    private void FixedUpdate()
    {
        circlesList = gameManager.GetCirclesList();
        transform.position += circle.direction * circle.currentSpeed; //moving on playing area
        circle.position = transform.position;
        CheckEdgeCollision();
        CheckOtherCircleCollision();
    }
    
    void CheckEdgeCollision()
    {
        if (sprite.bounds.center.x -sprite.bounds.extents.x <= -areaSprite.bounds.extents.x || sprite.bounds.center.x + sprite.bounds.extents.x >= areaSprite.bounds.extents.x)
        {
            transform.position = new Vector2(transform.position.x - Mathf.Sign(sprite.bounds.center.x)*1 ,transform.position.y);
            circle.direction.Set(-circle.direction.x, circle.direction.y, circle.direction.z);
        }

        if (sprite.bounds.center.y - sprite.bounds.extents.y <= -areaSprite.bounds.extents.y || sprite.bounds.center.y + sprite.bounds.extents.y >= areaSprite.bounds.extents.y)
        {
            transform.position = new Vector2(transform.position.x , transform.position.y - Mathf.Sign(sprite.bounds.center.y) * 1);
            circle.direction.Set(circle.direction.x, -circle.direction.y, circle.direction.z);
        }
    }

    void CheckOtherCircleCollision()
    {
        foreach (var tempCircle in  GameObject.FindGameObjectsWithTag("circle"))
        {
            if (tempCircle != null)
            {
                Vector3 fromTo = sprite.bounds.center - tempCircle.GetComponent<SpriteRenderer>().bounds.center;
                float distance = fromTo.magnitude;
                if (distance <= sprite.bounds.extents.x + tempCircle.GetComponent<SpriteRenderer>().bounds.extents.x && distance > 0)
                {
                    //collision
                    //check the other team circle collision
                    if (!tempCircle.GetComponent<CircleController>().GetTeam().Equals(circle.team))
                    {
                        Vector3 damage = new Vector3(0.1f, 0.1f, 0);
                        transform.localScale -= damage;
                        //check radius . Kill both if one's radius less then 0.2f???
                        if (transform.localScale.x < 0.2f || tempCircle.transform.localScale.x < 0.2f)
                        {
                            Destroy(gameObject);
                            Destroy(tempCircle.gameObject);
                            circlesList.Remove(gameObject.GetComponent<CircleController>().circle);
                            circlesList.Remove(tempCircle.GetComponent<CircleController>().circle);
                            gameManager.KillFirstTeamCircle();
                            gameManager.KillSecondTeamCircle();
                        }
                    }
                    //set new direction
                    Vector2 newDirection = (fromTo.normalized + circle.direction).normalized; 
                    circle.direction.Set(newDirection.x, newDirection.y, circle.direction.z);
                }
            }
        }
    }

    public void StartMoving()
    {
        circle.currentSpeed = circle.baseSpeed;
    }

    public void StopMoving()
    {
        circle.currentSpeed = 0f;
    }

    public Team GetTeam()
    {
        return circle.team;
    }

}
