using UnityEngine;
using EzySlice; // এই লাইনটি এরর দেখালে বুঝবেন লাইব্রেরি ঠিকমতো ইন্সটল হয়নি

public class StoneSlicer : MonoBehaviour
{
    public Material stoneInsideMaterial; // পাথরের ভেতরের অংশের ম্যাটেরিয়াল
    public LayerMask sliceableLayer;      // কোন লেয়ারের অবজেক্ট কাটা যাবে

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, sliceableLayer))
            {
                // স্লাইস ফাংশন কল করা
                SliceObject(hit.collider.gameObject, hit.point, ray.direction);
            }
        }
    }

    void SliceObject(GameObject obj, Vector3 hitPoint, Vector3 direction)
    {
        // মাউস ক্লিকের পজিশন এবং ক্যামেরার অ্যাঙ্গেল অনুযায়ী কাটার প্লেন সেট করা
        // এখানে Vector3.up কে আপনার প্রয়োজনমতো পরিবর্তন করতে পারেন
        SlicedHull hull = obj.Slice(hitPoint, Vector3.up); 

        if (hull != null)
        {
            // কাটা অংশের উপরের এবং নিচের ম্যাস তৈরি করা
            GameObject upperHull = hull.CreateUpperHull(obj, stoneInsideMaterial);
            GameObject lowerHull = hull.CreateLowerHull(obj, stoneInsideMaterial);

            if (upperHull != null && lowerHull != null)
            {
                // নতুন টুকরোগুলোতে ফিজিক্স যোগ করা
                ConfigureSlicedPart(upperHull);
                ConfigureSlicedPart(lowerHull);

                // মূল আস্ত পাথরটি ডিলিট করে দেওয়া
                Destroy(obj);
            }
        }
    }

    void ConfigureSlicedPart(GameObject part)
    {
        part.AddComponent<MeshCollider>().convex = true;
        Rigidbody rb = part.AddComponent<Rigidbody>();
        part.layer = LayerMask.NameToLayer("Sliceable"); // যাতে নতুন টুকরোকেও আবার কাটা যায়
        
        // টুকরোটি যাতে ছিটকে যায় তার জন্য একটু ফোর্স দেওয়া
        rb.AddExplosionForce(100f, part.transform.position, 1f);
    }
}