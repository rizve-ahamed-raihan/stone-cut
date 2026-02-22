using UnityEngine;

public class StoneCutter : MonoBehaviour
{
    public GameObject holeEffect; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // মাউস ক্লিক চেক
        {
            Debug.Log("১. মাউস ক্লিক ডিটেক্ট হয়েছে!"); 

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("২. রে-কাস্ট (Raycast) কোনো অবজেক্টে লেগেছে: " + hit.collider.gameObject.name);

                if (hit.collider.gameObject.CompareTag("Stone"))
                {
                    Debug.Log("৩. সফল! পাথরের গায়ে ক্লিক লেগেছে।");
                    CreateHole(hit.point, hit.normal);
                }
                else
                {
                    Debug.LogWarning("৪. ক্লিক লেগেছে কিন্তু অবজেক্টের ট্যাগ 'Stone' নয়! আপনার ট্যাগ চেক করুন।");
                }
            }
            else
            {
                Debug.Log("৫. রে-কাস্ট কোনো অবজেক্ট খুঁজে পায়নি। ক্যামেরার সামনে কি কিছু আছে?");
            }
        }
    }

    void CreateHole(Vector3 position, Vector3 normal)
    {
        if (holeEffect != null)
        {
            Instantiate(holeEffect, position, Quaternion.LookRotation(normal));
        }
        else
        {
            Debug.LogError("ভুল: আপনি Inspector-এ holeEffect অ্যাসাইন করেননি!");
        }
    }
}