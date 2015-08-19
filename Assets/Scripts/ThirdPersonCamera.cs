using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ThirdPersonCamera : MonoBehaviour {

    public float turnSpeed = 4.0f;
    public Transform player;

    public float height = 1f;
    public float distance = 2f;

    public bool hideBlockingObjects = true;

    private Vector3 offsetX;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        offsetX = new Vector3(0, height, distance);
    }

    Dictionary<Renderer, float> renders = new Dictionary<Renderer, float>();

    void LateUpdate()
    {
        float RotateAxis = Input.GetAxis("RotateCamera") * 10 * Time.deltaTime;

        offsetX = Quaternion.AngleAxis(RotateAxis * turnSpeed, Vector3.up) * offsetX;
        transform.position = player.position + offsetX;
        transform.LookAt(player.position);

        if (hideBlockingObjects)
        {
            Vector3 direction = transform.position - player.position;
            //direction.Normalize();
            
            RaycastHit hit;

            Debug.DrawRay(transform.position, -direction, Color.red);

            if (Physics.Raycast(transform.position, -direction, out hit))
            {
                if (hit.collider.tag != "Player")
                {
                    Renderer r = hit.collider.gameObject.GetComponent<Renderer>();

                    if (r)
                    {
                        foreach (Material m in r.materials)
                        {
                            if (!renders.ContainsKey(r))
                            {
                                renders.Add(r, m.color.a);
                                Debug.Log(m.GetFloat("_Mode"));
                                m.SetFloat("_Mode", 2);
                                Debug.LogFormat("_SrcBlend: {0}",m.GetInt("_SrcBlend"));
                                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                                Debug.LogFormat("_DstBlend: {0}", m.GetInt("_DstBlend"));
                                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                                Debug.LogFormat("_ZWrite: {0}", m.GetInt("_ZWrite"));
                                m.SetInt("_ZWrite", 0);
                                m.DisableKeyword("_ALPHATEST_ON");
                                m.EnableKeyword("_ALPHABLEND_ON");
                                m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                                Debug.Log(m.renderQueue);
                                m.renderQueue = 3000;
                                m.color = new Color(m.color.r, m.color.g, m.color.b, m.color.a * 0.5f);
                                Debug.LogFormat("Alpha: {0}", m.color.a);
                            }
                        }
                    }
                }
                else
                {
                    List<Renderer> toRemove = new List<Renderer>();
                    foreach (Renderer r in renders.Keys)
                    {
                        try
                        {
                            Debug.Log("Removing: " + r.ToString());
                            foreach (Material m in r.materials)
                            {
                                m.SetFloat("_Mode", 0);
                                m.SetInt("_SrcBlend", 1);
                                m.SetInt("_DstBlend", 0);
                                m.SetInt("_ZWrite", 1);
                                m.EnableKeyword("_ALPHATEST_ON");
                                m.DisableKeyword("_ALPHABLEND_ON");
                                m.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                                m.renderQueue = 2000;
                                m.color = new Color(m.color.r, m.color.g, m.color.b, renders[r]);
                            }
                            toRemove.Add(r);
                        }
                        catch (InvalidOperationException e)
                        {
                            //Do nothing
                        }
                        catch (Exception e)
                        {
                            Debug.LogErrorFormat("Un-expectected exception: {0}", e.ToString());
                        }
                        
                    }

                    foreach(Renderer r in toRemove)
                    {
                        renders.Remove(r);
                    }
                    toRemove.Clear();
                }
            }
        }
    }
}
