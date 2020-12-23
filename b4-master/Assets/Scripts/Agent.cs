using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public float radius;
    public float mass;
    public float perceptionRadius;
    public const float k = 1.2f * 100000f;
    public float rank;
    /* Mode
    1 = Pursue and Evade
    2 = Growing Spiral
    3 = Leader Following
    4 = Queueing
    5 = King
    6 = Rook
    7 = Knight
    8 = Queen
    other number = Normal Mode for Part one */
    private int Mode;
    private List<Vector3> path;
    private NavMeshAgent nma;
    private Rigidbody rb;
    private int count = 0;
    private float blood = 5;

    private HashSet<GameObject> perceivedNeighbors = new HashSet<GameObject>();
    private HashSet<GameObject> adjacentWalls = new HashSet<GameObject>();

    void Start()
    {
        //set the mode
        Mode = 10;
        path = new List<Vector3>();
        nma = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        rank = (Vector3.zero - transform.position).magnitude;

        gameObject.transform.localScale = new Vector3(2 * radius, 1, 2 * radius);
        nma.radius = radius;
        rb.mass = mass;
        GetComponent<SphereCollider>().radius = perceptionRadius / 2;
        GameObject.Find("Managers").GetComponent<AgentManager>().Text1.text = "";
        Time.timeScale = 0.5f;

    }

    private void Update()
    {
        if (gameObject.tag == "King" && Mode == 10)
        {
            if(blood == 0)
            {
                GameObject.Find("Managers").GetComponent<AgentManager>().Text1.text = "Queen win";
                Time.timeScale = 0f;
            }
            if(closestTag("Pawn") == null)
            {
                GameObject.Find("Managers").GetComponent<AgentManager>().Text1.text = "King win";
                Time.timeScale = 0f;
            }
        }
        Debug.Log(blood);
        count++;
        switch (Mode)
        {
            //Pursue and Evade
            case 1:
                Debug.Log("Mode 1 *");
                if (Char.GetNumericValue(name, 6) < 5)
                {
                    gameObject.tag = "Player";
                    var temp = gameObject.GetComponent<Renderer>();
                    temp.material.SetColor("_Color", Color.red);
                }
                else
                {
                    gameObject.tag = "Respawn";
                }
                break;
            //Growing Spiral
            case 2:
                Debug.Log("Mode 2 *");
                break;
            //Leader Following
            case 3:
                Debug.Log("Mode 3 *");
                if (Char.GetNumericValue(name, 6) < 1)
                {
                    gameObject.tag = "Player";
                    var temp = gameObject.GetComponent<Renderer>();
                    temp.material.SetColor("_Color", Color.yellow);
                }
                break;
            //Quene
            case 4:
                Debug.Log("Mode 4 *");
                gameObject.tag = "Player";
                if(count < 500) {
                    Debug.Log(count);
                    if (path.Count != 0)
                    {
                        rank = (path[0] - transform.position).magnitude;
                    }
                }
                break;
            case 5:
                Debug.Log("Mode 5, King");
                if (Char.GetNumericValue(name, 6) < 1)
                {
                    gameObject.tag = "King";
                    var temp = gameObject.GetComponent<Renderer>();
                    temp.material.SetColor("_Color", Color.red);
                }
                else
                {
                    gameObject.tag = "Pawn";
                }
                break;
            case 6:
                Debug.Log("Mode 6, Rook");
                if (Char.GetNumericValue(name, 6) < 1)
                {
                    gameObject.tag = "King";
                    var temp = gameObject.GetComponent<Renderer>();
                    temp.material.SetColor("_Color", Color.red);
                }
                else
                {
                    gameObject.tag = "Rook";
                    var temp = gameObject.GetComponent<Renderer>();
                    temp.material.SetColor("_Color", Color.blue);
                }
                break;
            case 7:
                Debug.Log("Mode 7, Knight");
                if (Char.GetNumericValue(name, 6) < 1)
                {
                    gameObject.tag = "King";
                    var temp = gameObject.GetComponent<Renderer>();
                    temp.material.SetColor("_Color", Color.red);
                }
                else if (Char.GetNumericValue(name, 6) < 2)
                {
                    gameObject.tag = "Knight";
                    var temp = gameObject.GetComponent<Renderer>();
                    temp.material.SetColor("_Color", Color.yellow);
                }
                else
                {
                    gameObject.tag = "Pawn";
                }
                break;
            case 8:
                Debug.Log("Mode 8, Queen");
                if (Char.GetNumericValue(name, 6) < 1)
                {
                    gameObject.tag = "Queen";
                    var temp = gameObject.GetComponent<Renderer>();
                    temp.material.SetColor("_Color", Color.black);
                }
                else
                {
                    gameObject.tag = "Pawn";
                }
                break;
            case 10:
                Debug.Log("Mode 8, Queen");
                if (Char.GetNumericValue(name, 6) < 1)
                {
                    gameObject.tag = "King";
                    var temp = gameObject.GetComponent<Renderer>();
                    temp.material.SetColor("_Color", Color.red);
                }
                else if (Char.GetNumericValue(name, 6) == 1)
                {
                    gameObject.tag = "Pawn";
                }
                else if (Char.GetNumericValue(name, 6) < 3)
                {
                    gameObject.tag = "Rook";
                    var temp = gameObject.GetComponent<Renderer>();
                    temp.material.SetColor("_Color", Color.blue);
                }
                else if (Char.GetNumericValue(name, 6) < 5)
                {
                    gameObject.tag = "Knight";
                    var temp = gameObject.GetComponent<Renderer>();
                    temp.material.SetColor("_Color", Color.yellow);
                }
                else if (Char.GetNumericValue(name, 6) < 6)
                {
                    gameObject.tag = "Queen";
                    var temp = gameObject.GetComponent<Renderer>();
                    temp.material.SetColor("_Color", Color.black);
                }
                else
                {
                    gameObject.tag = "Pawn";
                }
                break;
            //Part one
            default:
                Debug.Log("Default Mode *");
                break;
        }
        Debug.Log("The mode is : " + Mode.ToString());




        if (path.Count > 1 && Vector3.Distance(transform.position, path[0]) < 1.1f)
        {
            path.RemoveAt(0);
        } else if (path.Count == 1 && Vector3.Distance(transform.position, path[0]) < 2f)
        {
            path.RemoveAt(0);

            if (path.Count == 0)
            {
                //gameObject.SetActive(false);
                //AgentManager.RemoveAgent(gameObject);
            }
        }

        #region Visualization

        if (false)
        {
            if (path.Count > 0)
            {
                Debug.DrawLine(transform.position, path[0], Color.green);
            }
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i], path[i + 1], Color.yellow);
            }
        }

        if (false)
        {
            foreach (var neighbor in perceivedNeighbors)
            {
                Debug.DrawLine(transform.position, neighbor.transform.position, Color.yellow);
            }
        }

        #endregion
    }

    #region Public Functions

    public void ComputePath(Vector3 destination)
    {
        nma.enabled = true;
        var nmPath = new NavMeshPath();
        nma.CalculatePath(destination, nmPath);
        path = nmPath.corners.Skip(1).ToList();
        //path = new List<Vector3>() { destination };
        //nma.SetDestination(destination);
        nma.enabled = false;
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    #endregion

    #region Incomplete Functions

    private Vector3 ComputeForce()
    {
        //var force = Vector3.zero;
        //var force = CalculateGoalForce(maxSpeed: 5) + CalculateAgentForce() + CalculateWallForce();

        //var force = CalculateSpiralForce(spinSpeed: 5, timeToGrowth: 5);

        //var force = CalculatePursuerForce();

        var force = Vector3.zero;
        Debug.Log("The mode is : " + Mode.ToString());
        switch (Mode)
        {
            //Pursue and Evade
            case 1:
                Debug.Log("Mode 1");
                force = CalculatePursuerForce();
                break;
            //Growing Spiral
            case 2:
                Debug.Log("Mode 2");
                force = CalculateSpiralForce(spinSpeed: 5, timeToGrowth: 1.5f);
                break;
            //Leader Following
            case 3:
                Debug.Log("Mode 3");
                force = CalculateLeaderForce();
                break;
            //Queueing
            case 4:
                Debug.Log("Mode 4");
                force = CalculateQueneForce(maxSpeed: 2);
                break;
            case 5:
                Debug.Log("Mode 5, King");
                force = CalculateKingForce();
                break;
            case 6:
                Debug.Log("Mode 6, Rook");
                if (Input.GetKey(KeyCode.Z))
                {
                    force = CalculateRookForce();
                    force += RookForce();
                }
                else {
                    force = CalculateAgentForce()/100 + CalculateWallForce() * 100;
                    force += RookForce();
                }
                break;
            case 7:
                Debug.Log("Mode 7, Knight");
                int num = 1;
                if (Input.GetKey(KeyCode.X))
                {                  
                    force = CalculateKnightForce();
                    if (this.tag == "Knight")
                    {
                        if (transform.position.y < 0)
                        {
                            num = 1;
                        }
                        if (transform.position.y > 3)
                        {
                            num = -1;
                        }
                        transform.Translate(Vector3.up * Time.deltaTime * num);
                    }
                }
                else
                {
                    force = CalculateAgentForce() / 100 + CalculateWallForce();
                    if (transform.position.y < 1.033333f)
                    {
                        num = 0;
                    }
                    if (transform.position.y > 1.033333f)
                    {
                        num = -2;
                    }
                    transform.Translate(Vector3.up * Time.deltaTime * num);
                }
                break;
            case 8:
                Debug.Log("Mode 8, Queen");
                if (Input.GetKey(KeyCode.M))
                {
                    Debug.Log("M");
                    force = CalculateQueenForce();
                    force += QueenForce();
                }
                else
                {
                    force = CalculateAgentForce() / 100 + CalculateWallForce() * 100;
                    force += QueenForce();
                }
                break;
            case 10:
                int num2 = 1;
                Debug.Log("Mode 10, All");
                if (this.tag == "King")
                {
                    force = CalculateKingForce();
                    if (Input.GetKey(KeyCode.Z))
                    {
                        force += CalculateRookForce();
                    }
                    break;
                }
                if (this.tag == "Pawn")
                {
                    force = CalculateKingForce() + CalculatePawnForce();
                    if (Input.GetKey(KeyCode.M))
                    {
                        force = CalculateQueenForce();
                    }
                    if (blood == 0)
                    {
                        force = CalculatePawnForce();
                    }
                    break;
                }
                if (this.tag == "Queen")
                {
                    if (Input.GetKey(KeyCode.M))
                    {
                        force = CalculateQueenForce();
                        force += QueenForce();
                        break;
                    }
                    else
                    {
                        force = CalculateAgentForce() / 100 + CalculateWallForce() * 100;
                        force += QueenForce();
                    }
                }
                if (this.tag == "Knight")
                {
                    if (Input.GetKey(KeyCode.X))
                    {
                        force = CalculateKnightForce() * 10;
                        if (this.tag == "Knight")
                        {
                            if (transform.position.y < 0)
                            {
                                num2 = 1;
                            }
                            if (transform.position.y > 3)
                            {
                                num2 = -1;
                            }
                            transform.Translate(Vector3.up * Time.deltaTime * num2);
                        }
                        break;
                    }
                    else
                    {
                        force = CalculateAgentForce() / 100 + CalculateWallForce() / 100;
                        if (transform.position.y < 1.033333f)
                        {
                            num2 = 0;
                        }
                        if (transform.position.y > 1.033333f)
                        {
                            num2 = -2;
                        }
                        transform.Translate(Vector3.up * Time.deltaTime * num2);
                    }
                }
                if (this.tag == "Rook")
                {
                    force = Vector3.zero;
                    force = CalculateWallForce() * 100;
                    force += RookForce();
                }
                break;
            //Part one
            default:
            Debug.Log("Part one");
                force = CalculateGoalForce(maxSpeed: 2) + CalculateAgentForce() + CalculateWallForce();
                break;
        }

        if (force != Vector3.zero)
        {
            return force.normalized * Mathf.Min(force.magnitude, Parameters.maxSpeed);
        } else
        {
            return Vector3.zero;
        }
    }
    
    private Vector3 CalculateGoalForce(float maxSpeed)
    {
        if(path.Count == 0)
        {
            return Vector3.zero;
        }

        var temp = path[0] - transform.position;
        var desiredVel = temp.normalized * Mathf.Min(temp.magnitude, maxSpeed);
        var actualVelocity = rb.velocity;
        return mass * (desiredVel - actualVelocity) / Parameters.T;
    }

    private Vector3 CalculateAgentForce()
    {
        var agentForce = Vector3.zero;
        if (count < 2)
        {
            return agentForce;
        }
        foreach (var n in perceivedNeighbors) 
        {
            if (!AgentManager.IsAgent(n))
            {
                continue;
            }

            var neighbor = AgentManager.agentsObjs[n];
            var dir = (transform.position - neighbor.transform.position).normalized;
            var tangent = Vector3.Cross(Vector3.up, dir);
            var overlap = (radius + neighbor.radius) - Vector3.Distance(transform.position, n.transform.position);

            agentForce += (Parameters.A * Mathf.Exp(overlap / Parameters.B)) * dir;
            agentForce += Parameters.k * Mathf.Max(overlap, 0) * dir;
            agentForce -= Parameters.Kappa * (overlap > 0f ? overlap : 0) * Vector3.Dot(rb.velocity - neighbor.GetVelocity(), tangent) * tangent;
        }

        return agentForce;
    }

    private Vector3 CalculateWallForce()
    {
        var wallForce = Vector3.zero;
        if (count < 2)
        {
            return wallForce;
        }
        foreach (var n in adjacentWalls)
        {
            if (!WallManager.IsWall(n))
            {
                continue;
            }

            var dir = (transform.position - n.transform.position).normalized;
            var overlap = (radius + 0.5f) - Vector3.Distance(transform.position, n.transform.position);
            var tangent = Vector3.Cross(Vector3.up, dir);
            wallForce += Parameters.A * Mathf.Exp(overlap / Parameters.B) * dir;
            wallForce += Parameters.k * Mathf.Max(overlap, 0) * dir;
            wallForce -= Parameters.Kappa * (overlap > 0f ? overlap : 0) * Vector3.Dot(rb.velocity, tangent) * tangent;

        }

        return wallForce;
    }

    private Vector3 CalculatePursuerForce()
    {
        var pursuerForce = Vector3.zero;
        if (this.tag == "Player")
        {
            var dir = (closestTag("Respawn").transform.position - transform.position).normalized;
            var desiredVel = dir.normalized * Mathf.Min(dir.magnitude, 5);
            var actualVelocity = rb.velocity;
            var force = (desiredVel - actualVelocity) / Parameters.T;
            pursuerForce = force * 2 + CalculateAgentForce()/5 + CalculateWallForce();
            if (pursuerForce != Vector3.zero)
            {
                return pursuerForce;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            var dir = (transform.position - closestTag("Player").transform.position).normalized;
            var desiredVel = dir.normalized * Mathf.Min(dir.magnitude, 5);
            var actualVelocity = rb.velocity;
            var force = (desiredVel - actualVelocity) / Parameters.T;
            pursuerForce = force * 2 + CalculateAgentForce()/5 + CalculateWallForce();

            //Avoid Corner
            var wallForce = Vector3.zero;
            foreach (var n in adjacentWalls)
            {
                if (!WallManager.IsWall(n))
                {
                    continue;
                }
                if ((transform.position - n.transform.position).magnitude > 2f)
                {
                    continue;
                }
                
                var tempW = Vector3.zero - transform.position;
                var tangentW = Vector3.Cross(Vector3.up, tempW);
                var desiredVelW = 5 * tangentW.normalized * Mathf.Min(tangentW.magnitude, 5);
                var actualVelocityW = rb.velocity;
                wallForce = mass * (desiredVelW - 10 * actualVelocityW) / Parameters.T + CalculateGoalForce(maxSpeed: 5);
            }



            if (pursuerForce != Vector3.zero)
            {
                return pursuerForce + wallForce;
            }
            else
            {
                return Vector3.zero;
            }


        }
    }

    private Vector3 CalculateKingForce()
    {
        var pursuerForce = Vector3.zero;
        if (this.tag == "Pawn")
        {
            var dir = (closestTag("King").transform.position - transform.position).normalized;
            var desiredVel = dir.normalized * Mathf.Min(dir.magnitude, 5);
            var actualVelocity = rb.velocity;
            var force = (desiredVel * 3 - actualVelocity) / Parameters.T;
            pursuerForce = force * 2 + CalculateAgentForce() / 5 + CalculateWallForce();
            if (pursuerForce != Vector3.zero)
            {
                return pursuerForce;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            if (closestTag("Pawn") != null)
            {
                var dir = (transform.position - closestTag("Pawn").transform.position).normalized;
                var desiredVel = dir.normalized * Mathf.Min(dir.magnitude, 5);
                var actualVelocity = rb.velocity;
                var force = (desiredVel - actualVelocity) / Parameters.T;
                pursuerForce = force * 2 + CalculateAgentForce() / 5 + CalculateWallForce();

                //Avoid Corner
                var wallForce = Vector3.zero;
                foreach (var n in adjacentWalls)
                {
                    if (!WallManager.IsWall(n))
                    {
                        continue;
                    }
                    if ((transform.position - n.transform.position).magnitude > 2f)
                    {
                        continue;
                    }

                    var tempW = Vector3.zero - transform.position;
                    var tangentW = Vector3.Cross(Vector3.up, tempW);
                    var desiredVelW = 5 * tangentW.normalized * Mathf.Min(tangentW.magnitude, 5);
                    var actualVelocityW = rb.velocity;
                    wallForce = mass * (desiredVelW - 10 * actualVelocityW) / Parameters.T + CalculateGoalForce(maxSpeed: 5);
                }




                if (pursuerForce != Vector3.zero)
                {
                    return pursuerForce + wallForce;
                }
                else
                {
                    return Vector3.zero;
                }
            }
            return CalculateAgentForce() / 100 + CalculateWallForce();


        }
    }

    private Vector3 CalculateRookForce()
    {
        var pursuerForce = Vector3.zero;
        if (this.tag == "King")
        {
            var dir = (closestTag("Rook").transform.position - transform.position).normalized;
            var desiredVel = dir.normalized * Mathf.Min(dir.magnitude, 5);
            var actualVelocity = rb.velocity;
            var force = (desiredVel * 5 - actualVelocity) / Parameters.T;
            pursuerForce = force * 2 + CalculateAgentForce() / 5 + CalculateWallForce();
            if (pursuerForce != Vector3.zero)
            {
                return pursuerForce * 5;
            }
            else
            {
                return Vector3.zero;
            }
        }
        return CalculateAgentForce() / 100 + CalculateWallForce();
    }

    private Vector3 CalculateKnightForce()
    {
        var pursuerForce = Vector3.zero;
        if (this.tag == "Knight")
        {
            if (closestTag("Pawn") != null)
            {
                var dir = ((closestTag("King").transform.position + (closestTag("Pawn").transform.position)) / 2 - transform.position).normalized;
                var desiredVel = dir.normalized * Mathf.Min(dir.magnitude, 5);
                var actualVelocity = rb.velocity;
                var force = (desiredVel * 3 - actualVelocity) / Parameters.T;
                pursuerForce = force * 2 + CalculateAgentForce() / 5 + CalculateWallForce();
                if (pursuerForce != Vector3.zero)
                {
                    return pursuerForce * 5;
                }
                else
                {
                    return Vector3.zero;
                }
            }
        }
        return CalculateAgentForce() / 100 + CalculateWallForce() / 100;
    }

    private Vector3 CalculateQueenForce()
    {
        var pursuerForce = Vector3.zero;
        if (this.tag == "Pawn")
        {
            var inRange = closestTag("Queen").transform.position - transform.position;
            var dir = (closestTag("Queen").transform.position - transform.position).normalized;
            if (Math.Abs(inRange.x) > 5 || Math.Abs(inRange.z) > 5)
            {
                return CalculateAgentForce() / 50 + CalculateWallForce();
            }
            var desiredVel = dir.normalized * Mathf.Min(dir.magnitude, 5);
            var actualVelocity = rb.velocity;
            var force = (desiredVel * 5 - actualVelocity) / Parameters.T;
            pursuerForce = force * 2 + CalculateAgentForce() / 5 + CalculateWallForce();
            if (pursuerForce != Vector3.zero)
            {
                return pursuerForce;
            }
            else
            {
                return CalculateAgentForce() / 100 + CalculateWallForce();
            }
        }
        return CalculateAgentForce() / 50 + CalculateWallForce();
    }

    private Vector3 CalculatePawnForce()
    {
        var pursuerForce = Vector3.zero;
        if (this.tag == "Pawn")
        {
            /*var inRange = closestTag("Rook").transform.position - transform.position;           
            if (Math.Abs(inRange.x) < 0.8f && Math.Abs(inRange.z) < 0.8f)
            {
                blood = 0;
            }
            inRange = closestTag("Knight").transform.position - transform.position;
            if (Math.Abs(inRange.x) < 0.8f && Math.Abs(inRange.z) < 0.8f)
            {
                blood = 0;
            }*/
            if (blood == 0)
            {
                var dir = (closestTag("Wall").transform.position - transform.position).normalized;
                var desiredVel = dir.normalized * Mathf.Min(dir.magnitude, 5);
                var actualVelocity = rb.velocity;
                var force = (desiredVel * 5 - actualVelocity) / Parameters.T;
                pursuerForce = force * 2 + CalculateAgentForce() / 5 + CalculateWallForce();
                if (pursuerForce != Vector3.zero)
                {
                    return pursuerForce;
                }
                else
                {
                    return CalculateAgentForce() / 100 + CalculateWallForce();
                }
            }
        }
        return CalculateAgentForce() / 50 + CalculateWallForce();
    }

    private Vector3 RookForce()
    {
        var force = Vector3.zero;
        if (this.tag == "Rook")
        {
            if (Input.GetKey(KeyCode.A))
            {
                force += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                force += Vector3.right;
            }
            if (Input.GetKey(KeyCode.W))
            {
                force += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                force += Vector3.back;
            }
        }
        return force * 3;
    }

    private Vector3 QueenForce()
    {
        var force = Vector3.zero;
        if (this.tag == "Queen")
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                force += Vector3.left;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                force += Vector3.right;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                force += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                force += Vector3.back;
            }
        }
        return force * 3;
    }


    private Vector3 CalculateSpiralForce(float spinSpeed, float timeToGrowth)
    {
        var spiralForce = Vector3.zero;
        var otherForce = CalculateAgentForce() + 1000 * CalculateWallForce();
        var goalForce = CalculateGoalForce(maxSpeed: 5);
        if (path.Count == 0)
        {
            return spiralForce;
        }
        var temp = path[0] - transform.position;
        var tangent = Vector3.Cross(Vector3.up, temp);
        var desiredVel = spinSpeed * tangent.normalized * Mathf.Min(tangent.magnitude, spinSpeed);
        var actualVelocity = rb.velocity;
        spiralForce += mass * (desiredVel - (2 * spinSpeed) * actualVelocity) / Parameters.T;
        if(count < timeToGrowth * 100) {
            spiralForce += otherForce + goalForce;
        }
        else
        {
            spiralForce += otherForce - goalForce;
        }
        return spiralForce;
    }

    //Leader Following Mode
    private Vector3 CalculateLeaderForce()
    {
        if (this.tag == "Player")
        {
            var leadForce = CalculateGoalForce(maxSpeed: 3) + CalculateAgentForce() + CalculateWallForce();
            var force = leadForce.normalized * Mathf.Min(leadForce.magnitude, Parameters.maxSpeed);
            if (force != Vector3.zero)
            {
                return force;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {

            var destination = Vector3.zero;
            if (path.Count != 0)
            {
                destination = path[0];
            }
            var target = closestTag("Player").transform.position;
            float angle = Vector3.SignedAngle(target, transform.position, destination - closestTag("Player").transform.position);

            var goalDirection = (closestTag("Player").transform.position - transform.position).normalized;
            var prefForce = (((goalDirection * Mathf.Min(goalDirection.magnitude, 1)) - rb.velocity) / Parameters.T);
            var leadForce = prefForce + CalculateAgentForce() + CalculateWallForce();
            var force = leadForce.normalized * Mathf.Min(leadForce.magnitude, Parameters.maxSpeed);

            Debug.Log(angle);
            //Move Out
            if((destination - target).magnitude > (destination - transform.position).magnitude)
            {
                var tempL = Vector3.zero;
                if (angle >= 0f)
                {
                    tempL = closestTag("Player").transform.position - transform.position;
                }
                else if (angle < 0f)
                {
                    tempL = -(closestTag("Player").transform.position - transform.position);
                }


                var tangentL = Vector3.Cross(Vector3.up, tempL);
                var desiredVelL = 5 * tangentL.normalized * Mathf.Min(tangentL.magnitude, 5);

                var temp = target - transform.position;
                var desiredVel = temp.normalized * Mathf.Min(temp.magnitude, 5);
                var actualVelocity = rb.velocity;
                var tagForce = mass * (desiredVel - actualVelocity) / Parameters.T;

                force += mass * (desiredVelL - 10 * actualVelocity) / Parameters.T + 2 * tagForce;
            }
            if (force != Vector3.zero)
            {
                return force;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    //Quene Following Mode
    private Vector3 CalculateQueneForce(float maxSpeed)
    {
        var target = Vector3.zero;
        if (closestDestTag("Player") == null)
        {
            if (path.Count != 0)
            {
                target = path[0];
            }           
        }
        else {
            target = closestDestTag("Player").transform.position;
        }
        var dir = (transform.position - target).normalized;
        float x = 0;
        float z = 0;
        if (dir.z > 0)
        {
            if (dir.x < 0)
            {
                x = -0.3f;
                z = 0.3f;
            }
            else if (dir.x > 0)
            {
                x = 0.3f;
                z = 0.3f;
            }
            else
            {
                z = 0.3f;
            }
        }
        else if (dir.z < 0)
        {
            if (dir.x < 0)
            {
                x = -0.3f;
                z = -0.3f;
            }
            else if (dir.x > 0)
            {
                x = 0.3f;
                z = -0.3f;
            }
            else
            {
                z = -0.3f;
            }
        }
        else if (dir.z == 0)
        {
            if (dir.x < 0)
            {
                x = -0.3f;
            }
            else if (dir.x > 0)
            {
                x = 0.3f;
            }
        }
        target = new Vector3(target.x + x, 0, target.z + z);

        var temp = target - transform.position;
        var desiredVel = temp.normalized * Mathf.Min(temp.magnitude, maxSpeed);
        var actualVelocity = rb.velocity;
        var prefForce = mass * (desiredVel - actualVelocity) / Parameters.T;
        var force = prefForce + CalculateAgentForce() + CalculateWallForce();

        if (force != Vector3.zero)
        {
            return force;
        }
        else
        {
            return Vector3.zero;
        }
    }



    public void ApplyForce()
    {
        var force = ComputeForce();
        force.y = 0;

        //rb.AddForce(force * 10, ForceMode.Force);
        rb.AddForce(force / mass, ForceMode.Acceleration);
    }

    public GameObject closestTag(string tag)
    {
        GameObject[] neighbor;
        neighbor = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject n in neighbor)
        {
            Vector3 diff = n.transform.position - position;
            float dis = diff.sqrMagnitude;
            if (dis < distance)
            {
                closest = n;
                distance = dis;
            }
        }
        return closest;
    }

    public GameObject closestDestTag(string tag)
    {
        GameObject[] neighbor;
        neighbor = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject n in neighbor)
        {
            Vector3 diff = n.transform.position - position;
            float dis = diff.sqrMagnitude;
            if (dis < distance && n.GetComponent<Agent>().rank < rank)
            {
                closest = n;
                distance = dis;
            }
        }
        return closest;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (AgentManager.IsAgent(other.gameObject))
        {
            perceivedNeighbors.Add(other.gameObject);
            if (this.tag == "King" && other.tag == "Pawn")
            {
                blood -= 0.5f;
            }
            if (this.tag == "Pawn" && (other.tag == "Rook" || other.tag == "Knight"))
            {
                blood = 0;
            }
        }
        if (WallManager.IsWall(other.gameObject))
        {
            adjacentWalls.Add(other.gameObject);
            if (this.tag == "Pawn" && blood == 0)
            {
                gameObject.SetActive(false);
                AgentManager.RemoveAgent(gameObject);
            }
        }
        
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (perceivedNeighbors.Contains(other.gameObject))
        {
            perceivedNeighbors.Remove(other.gameObject);
        }
        if (adjacentWalls.Contains(other.gameObject))
        {
            adjacentWalls.Remove(other.gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }

    public void OnCollisionExit(Collision collision)
    {
        
    }

    #endregion
}
