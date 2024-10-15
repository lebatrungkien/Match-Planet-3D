using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightManager : MonoBehaviour
{
    [SerializeField] public float damageAmount;
    public GameObject losePanel;
    public Light spotLight;

    public SphereCollider sphereCollider;

    public Transform planetTransform; // planet
    public float maxSpotAngle;
    public float minSpotAngle;
    public int totalLevels = 10;
    private float timeInLight = 0f;
    private float delayBeforeDamage = 1f;
    void Start()
    {
        sphereCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }
    void Update()
    {
        //spot angle tang dan theo level
        if (spotLight != null)
        {
            // Debug.Log("Current Level: " + GameManager.instance.GetCurrentLevel());
            //current level
            float currentLevel = GameManager.instance.GetCurrentLevel();
            float increaseSpotAngleVal = Mathf.Clamp01(currentLevel / totalLevels); //math.clamp01: gioi han 1 gia tri so trong khoang (0,1)
            float currentSpotAngle = Mathf.Lerp(minSpotAngle, maxSpotAngle, increaseSpotAngleVal);
            //Math.Lerp(a,b,c)
            //c=0, return a
            //c=1, return b
            //c = (0, 1), return (0, 1)
            spotLight.spotAngle = currentSpotAngle;
        }

        //tinh toan scale cua collider theo spot angle
        if (spotLight != null && sphereCollider != null && planetTransform != null)
        {
            //tinh toan spot angle cua spot light chieu xuong planet
            RaycastHit hit;
            Vector3 lightDirection = spotLight.transform.forward;
            if (Physics.Raycast(spotLight.transform.position, lightDirection, out hit, spotLight.range))
            {
                if (hit.transform == planetTransform)
                {
                    // set position cua collider tai position ma spot light chieu toi
                    sphereCollider.transform.position = hit.point;

                    //scale cua collider
                    float spotRadiusAtPlanet = Mathf.Tan(Mathf.Deg2Rad * spotLight.spotAngle / 2f) * Vector3.Distance(spotLight.transform.position, hit.point);
                    sphereCollider.radius = spotRadiusAtPlanet;
                }
            }
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (spotLight == null)
    //     {
    //         Debug.LogError("spotLight is not assigned.");
    //         return;
    //     }

    //     if (IsWithinLightRange(other.transform.position))
    //     {
    //         Debug.Log(other.name + " entered the light's area");


    //         HealthManager damageable = other.GetComponent<HealthManager>();
    //         if (damageable != null)
    //         {
    //             if (damageAmount > 0)
    //             {
    //                 damageable.TakenDamage(damageAmount);
    //                 Debug.Log(other.name + " took damage: " + damageAmount);
    //             }
    //             else
    //             {
    //                 Debug.LogWarning("damageAmount is zero or negative: " + damageAmount);
    //             }

    //             if (damageable.currentHealth <= 0)
    //             {
    //                 UIAnimation.instance.ShowLoserPanel();
    //                 losePanel.SetActive(true);
    //                 GameManager.instance.SetElapsedTime();
    //             }
    //         }
    //         else
    //         {
    //             Debug.LogWarning("No HealthManager component found on " + other.name);
    //         }
    //     }
    // }
    private void OnTriggerStay(Collider other)
    {
        if (spotLight == null)
        {
            Debug.LogError("spotLight is not assigned.");
            return;
        }

        if (IsWithinLightRange(other.transform.position))
        {
            // Debug.Log(other.name + " is in the light's area");
            timeInLight += Time.deltaTime;
            if (timeInLight >= delayBeforeDamage)
            {

                HealthManager damageable = other.GetComponent<HealthManager>();
                if (damageable != null)
                {
                    if (damageAmount > 0)
                    {
                        damageable.TakenDamage(damageAmount * Time.deltaTime); //hp bi tru theo thoi gian
                        Debug.Log(other.name + " is taking damage: " + damageAmount * Time.deltaTime);
                    }
                    else
                    {
                        // Debug.LogWarning("damageAmount is zero or negative: " + damageAmount);
                    }

                    if (damageable.currentHealth <= 0)
                    {
                        UIAnimation.instance.ShowLoserPanel();
                        losePanel.SetActive(true);
                        GameManager.instance.SetElapsedTime();
                    }
                }
            }
        }
        else
        {
            timeInLight = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (spotLight == null)
        {
            Debug.LogError("spotLight is not assigned.");
            return;
        }

        if (!IsWithinLightRange(other.transform.position))
        {
            Debug.Log(other.name + " left the light's area");
        }
    }

    private bool IsWithinLightRange(Vector3 position)
    {
        float distance = Vector3.Distance(spotLight.transform.position, position);
        return distance <= spotLight.range;
    }

    private void OnDrawGizmos()
    {
        if (spotLight != null && sphereCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(sphereCollider.transform.position, sphereCollider.radius);
        }
    }
    // [SerializeField] public float damageAmount; // Giá trị mặc định phải là số dương
    // public GameObject losePanel;
    // public Light spotLight;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (IsWithinLightRange(other.transform.position))
    //     {
    //         Debug.Log(other.name + " entered the light's area");

    //         // Kiểm tra nếu đối tượng có component HealthManager
    //         HealthManager damageable = other.GetComponent<HealthManager>();
    //         Debug.Log("Current Health: " + damageable.currentHealth);
    //         Debug.Log("Max Health: " + damageable.maxHealth);
    //         if (damageable != null)
    //         {
    //             if (damageAmount > 0) // Kiểm tra trước khi áp dụng sát thương
    //             {
    //                 damageable.TakenDamage(damageAmount);
    //                 Debug.Log(other.name + " took damage: " + damageAmount);
    //             }
    //             else
    //             {
    //                 Debug.LogWarning("damageAmount is zero or negative: " + damageAmount);
    //             }

    //             if (damageable.currentHealth <= 0)
    //             {
    //                 UIAnimation.instance.ShowLoserPanel();
    //                 losePanel.SetActive(true);
    //                 GameManager.instance.SetElapsedTime();
    //             }
    //         }
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (!IsWithinLightRange(other.transform.position))
    //     {
    //         Debug.Log(other.name + " left the light's area");
    //     }
    // }

    // private bool IsWithinLightRange(Vector3 position)
    // {
    //     float distance = Vector3.Distance(spotLight.transform.position, position);
    //     return distance <= spotLight.range;
    // }

    // private void OnDrawGizmos()
    // {
    //     if (spotLight != null)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawWireSphere(spotLight.transform.position, spotLight.range);
    //     }
    // }


}




