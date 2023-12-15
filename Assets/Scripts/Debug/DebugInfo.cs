using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text FPS, coords, intendedVel, deFactoVel, interactableObject;
    [SerializeField] private float secondsBetweenUpdate = 0.25f;
    [SerializeField] private float smoothStepFPSCounter = 0.2f;

    private float timer;
    private float fpsDeltaTime;

    private Player player;

    private void Start()
    {
        player = GameMaster.Instance.PlayerInstance;
    }

    private void OnEnable()
    {
        GameMaster.Instance.OnSetNewPlayer += UpdatePlayer;
        player = GameMaster.Instance.PlayerInstance;
        timer = secondsBetweenUpdate;
        fpsDeltaTime = 0.0f;
    }

    private void OnDisable()
    {
        GameMaster.Instance.OnSetNewPlayer -= UpdatePlayer;
        
    }

    private void LateUpdate()
    {
        if (gameObject.activeSelf)
        {
            float deltaTime = Time.deltaTime;
            timer += deltaTime;
            fpsDeltaTime += (deltaTime - fpsDeltaTime) * smoothStepFPSCounter;

            if(timer >= secondsBetweenUpdate)
            {
                float fpsDisplay = 1.0f / fpsDeltaTime;
                Vector3 playerPosition = player.transform.position;
                Vector2 intendedVel = player.IntendedVel;
                Vector2 deFactoVel = player.HorizontalVel;
                string interactableObject = (player.InteractObject != null) ? player.InteractObject.ToString() : "null";

                FPS.text = $"FPS: {Mathf.Floor(fpsDisplay)}";
                coords.text = $"Coords: X:{playerPosition.x} Y:{playerPosition.y} Z:{playerPosition.z}";
                this.intendedVel.text = $"Intended Velocity: X:{intendedVel.x} Z:{intendedVel.y}";
                this.deFactoVel.text = $"Defacto Velocity: X:{deFactoVel.x} Z:{deFactoVel.y}";
                this.interactableObject.text = $"Interactable Object: {interactableObject}";

                timer = 0.0f;
            }
        }

        
        
    }

    private void UpdatePlayer(Player player)
    {
        this.player = player;
    }
}
