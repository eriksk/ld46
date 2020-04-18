using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD46
{
    public static class TransformExtensions
    {
        public static GameObject Instantiate(this GameObject instance, Transform parent = null)
        {
            var f = GameObject.Instantiate(instance, Vector3.zero, Quaternion.identity);
            f.transform.localPosition = Vector3.zero;
            f.transform.localRotation = Quaternion.identity;
            f.transform.localScale = Vector3.one;
            f.transform.SetParent(parent);
            f.transform.localScale = Vector3.one;
            f.transform.localPosition = Vector3.zero;
            f.transform.localRotation = Quaternion.identity;
            return f;
        }

        public static T GetComponentInChildrenDeep<T>(this Transform transform) where T : Component
        {
            var component = transform.GetComponent<T>();

            if (component != null)
            {
                return component;
            }

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var childComponent = child.GetComponentInChildrenDeep<T>();
                if (childComponent != null)
                {
                    return childComponent;
                }
            }
            return null;
        }

        public static IEnumerable<Transform> Parents(this Transform t)
        {
            if(t == null) return new Transform[0];
            
            return new[]{ t }.Concat(Parents(t.parent));
        }

        public static IEnumerable<Transform> Children(this Transform t)
        {
            for (var i = 0; i < t.childCount; i++)
            {
                yield return t.GetChild(i);
            }
        }

        public static void DestroyAllChildren(this Transform t)
        {
            for (var i = 0; i < t.childCount; i++)
            {
                var child = t.GetChild(i);
                Object.Destroy(child.gameObject);
            }
        }

        public static IEnumerable<Transform> ChildrenDeep(this Transform instance)
        {
            for (var i = 0; i < instance.childCount; i++)
            {
                var child = instance.GetChild(i);
                yield return child;
                var grandChildren = child.ChildrenDeep();
                foreach (var grandChild in grandChildren)
                {
                    yield return grandChild;
                }
            }
        }
    }
}