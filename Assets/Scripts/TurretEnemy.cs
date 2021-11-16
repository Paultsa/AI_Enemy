using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    public GameObject targetLocation;
    public GameObject ammo;
    public GameObject ammoSpawn;
    public GameObject gunRotator;

    public Vector3 gravity;
    
    
    public float force;
    public float waitBetweenShots;

    private int directionMultiplier;
    
    // Start is called before the first frame update
    void Start()
    {
        gravity = Physics.gravity;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(targetLocation.transform.position);

        if(gameObject.transform.position.z < targetLocation.transform.position.z)
        {
            directionMultiplier = -1;
        } else
        {
            directionMultiplier = 1;
        }
    }

    public void Shoot()
    {
        StartCoroutine(ShootBalls());
    }

    IEnumerator ShootBalls()
    {
        //Kulmat lasketaan
        Vector3[] direction = HitTargetBySpeed(ammoSpawn.transform.position, targetLocation.transform.position, gravity, force);

        //Tykki kääntyy
        Debug.Log("Tykin x rotaatio tulisi mennä numeroon: " + Mathf.Atan(direction[0].y / direction[0].z) * Mathf.Rad2Deg * directionMultiplier);
        gunRotator.GetComponent<RotateGun>().xAngle = Mathf.Atan(direction[0].y / direction[0].z) * Mathf.Rad2Deg * directionMultiplier;

        yield return new WaitUntil(() => gunRotator.GetComponent<RotateGun>().rotating == false);

        //Vihollinen ampuu
        GameObject projectile = Instantiate(ammo, ammoSpawn.transform.position, Quaternion.identity);
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
        projectileRB.AddRelativeForce(direction[0], ForceMode.Impulse);

        yield return new WaitForSeconds(waitBetweenShots);

        //Tykki kääntyy
        gunRotator.GetComponent<RotateGun>().xAngle = Mathf.Atan(direction[1].y / direction[1].z) * Mathf.Rad2Deg * directionMultiplier;

        yield return new WaitUntil(() => gunRotator.GetComponent<RotateGun>().rotating == false);


        //Vihollinen ampuu
        GameObject projectile2 = Instantiate(ammo, ammoSpawn.transform.position, Quaternion.identity);
        Rigidbody projectileRB2 = projectile2.GetComponent<Rigidbody>();
        projectileRB2.AddRelativeForce(direction[1], ForceMode.Impulse);

    }

    public Vector3[] HitTargetBySpeed(Vector3 startPosition, Vector3 targetPosition, Vector3 gravityBase, float launchSpeed)
    {
        //Turretista targettiin vektori
        Vector3 AtoB = targetPosition - startPosition;

        //Horisontaalinen vektori turretista targettiin
        Vector3 horizontal = GetHorizontalVector(AtoB, gravityBase, startPosition);
        float horizontalDistance = horizontal.magnitude;

        //Vertikaalinen vektori turretista targettiin
        Vector3 vertical = GetVerticalVector(AtoB, gravityBase, startPosition);
        float verticalDistance = vertical.y;

        float x2 = horizontalDistance * horizontalDistance;
        float v2 = launchSpeed * launchSpeed;
        float v4 = launchSpeed * launchSpeed * launchSpeed * launchSpeed;
        float gravMag = gravity.magnitude;

        float launchTest = v4 - (gravMag * ((gravMag * x2) + (2 * verticalDistance)));
        Debug.Log("LAUNCHTEST " + launchTest);

        Vector3[] launch = new Vector3[2];

        //Jos launchTest on negatiivinen, tiedetään, että on pakko ampua 45 asteen kulmaan, jotta pallo lentää mahdollisimman pitkälle
        //Jos launchTest on positiivinen, tiedetään, että voidaan osua kohteeseen, joten lasketaan kummatkin kulmat
        if(launchTest < 0)
        {
            launch[0] = (horizontal.normalized * launchSpeed * Mathf.Cos(45.0f * Mathf.Deg2Rad)) 
                - (gravityBase.normalized * launchSpeed * Mathf.Sin(45.0f * Mathf.Deg2Rad));

            launch[1] = (horizontal.normalized * launchSpeed * Mathf.Cos(45.0f * Mathf.Deg2Rad))
                - (gravityBase.normalized * launchSpeed * Mathf.Sin(45.0f * Mathf.Deg2Rad));
        }
        else
        {
            float[] tanAngle = new float[2];
            tanAngle[0] = (v2 - Mathf.Sqrt(v4 - gravMag * ((gravMag * x2) + (2 * verticalDistance * v2)))) / (gravMag * horizontalDistance);

            //HUOM! Alussa v2 + ....
            tanAngle[1] = (v2 + Mathf.Sqrt(v4 - gravMag * ((gravMag * x2) + (2 * verticalDistance * v2)))) / (gravMag * horizontalDistance);

            float[] finalAngle = new float[2];
            finalAngle[0] = Mathf.Atan(tanAngle[0]);
            finalAngle[1] = Mathf.Atan(tanAngle[1]);

            Debug.Log("Kulmat ovat " + finalAngle[0] * Mathf.Rad2Deg + " ja " + finalAngle[1] * Mathf.Rad2Deg);

            launch[0] = (horizontal.normalized * launchSpeed * Mathf.Cos(finalAngle[0])) 
                - (gravityBase.normalized * launchSpeed * Mathf.Sin(finalAngle[0]));

            launch[1] = (horizontal.normalized * launchSpeed * Mathf.Cos(finalAngle[1])) 
                - (gravityBase.normalized * launchSpeed * Mathf.Sin(finalAngle[1]));
        }


        return launch;
    }

    public Vector3 GetHorizontalVector(Vector3 AtoB, Vector3 gravityVector, Vector3 startPos)
    {
        Vector3 output;
        Vector3 perpendicular = Vector3.Cross(AtoB, gravityVector);
        perpendicular = Vector3.Cross(gravityVector, perpendicular);
        output = Vector3.Project(AtoB, perpendicular);
        Debug.DrawRay(startPos, output, Color.green, 10f);
        return output;
    }

    public Vector3 GetVerticalVector(Vector3 AtoB, Vector3 gravityVector, Vector3 startPos)
    {
        Vector3 output;
        output = Vector3.Project(AtoB, gravityVector);
        Debug.DrawRay(startPos, output, Color.cyan, 10f);
        return output;
    }
}
