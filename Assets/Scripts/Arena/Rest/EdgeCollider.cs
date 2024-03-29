﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena
{

    public class EdgeCollider : MonoBehaviour
    {
        public float colThickness = 4f;
        public float zPosition = 0f;
        private Vector2 _screenSize;

        public PhysicsMaterial2D bounceMaterial;
        private bool _isOn;

        Dictionary<string, Transform> _colliders = new Dictionary<string, Transform>();

        private void OnEnable()
        {
            ArenaEvents.OnPlayerCharge += ActivateColliders;
        }

        private void Start()
        {
            CreateColliders();
        }

        private void CreateColliders()
        {
            _colliders.Add("Top", new GameObject().transform);
            _colliders.Add("Bottom", new GameObject().transform);
            _colliders.Add("Right", new GameObject().transform);
            _colliders.Add("Left", new GameObject().transform);

            //Generate world spaces point information for position and scale calculations
            Vector3 cameraPos = Camera.main.transform.position;
            _screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f; //Grab the world-space position values of the start and end positions of the screen, then calculate the distance between them and store it as half, since we only need half that value for distance away from the camera to the edge
            _screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;
            //For each Transform/Object in our Dictionary
            foreach (KeyValuePair<string, Transform> valPair in _colliders)
            {
                valPair.Value.gameObject.AddComponent<BoxCollider2D>(); //Add our colliders. Remove the "2D", if you would like 3D colliders.
                valPair.Value.gameObject.GetComponent<BoxCollider2D>().sharedMaterial = bounceMaterial;
                valPair.Value.name = valPair.Key; //Set the object's name to it's "Key" name, and take on "Collider".  i.e: TopCollider
                valPair.Value.parent = transform; //Make the object a child of whatever object this script is on (preferably the camera)
                valPair.Value.gameObject.layer = 8;
                if (valPair.Key == "Left" || valPair.Key == "Right") //Scale the object to the width and height of the screen, using the world-space values calculated earlier
                    valPair.Value.localScale = new Vector3(colThickness, _screenSize.y * 2, colThickness);
                else
                    valPair.Value.localScale = new Vector3(_screenSize.x * 2, colThickness, colThickness);

                valPair.Value.gameObject.SetActive(false);
            }
            //Change positions to align perfectly with outter-edge of screen, adding the world-space values of the screen we generated earlier, and adding/subtracting them with the current camera position, as well as add/subtracting half out objects size so it's not just half way off-screen
            _colliders["Right"].position = new Vector3(cameraPos.x + _screenSize.x + (_colliders["Right"].localScale.x * 0.5f), cameraPos.y, zPosition);
            _colliders["Left"].position = new Vector3(cameraPos.x - _screenSize.x - (_colliders["Left"].localScale.x * 0.5f), cameraPos.y, zPosition);
            _colliders["Top"].position = new Vector3(cameraPos.x, cameraPos.y + _screenSize.y + (_colliders["Top"].localScale.y * 0.5f), zPosition);
            _colliders["Bottom"].position = new Vector3(cameraPos.x, cameraPos.y - _screenSize.y - (_colliders["Bottom"].localScale.y * 0.5f), zPosition);
        }


        public void ActivateColliders()
        {
            if (!_isOn)
            {
                foreach (KeyValuePair<string, Transform> valPair in _colliders)
                {
                    valPair.Value.gameObject.SetActive(true);
                }
                _isOn = true;
            }
            else
            {
                foreach (KeyValuePair<string, Transform> valPair in _colliders)
                {
                    valPair.Value.gameObject.SetActive(false);
                }
                _isOn = false;
            }
        }

        private void OnDestroy()
        {
            ArenaEvents.OnPlayerCharge -= ActivateColliders;
        }
    }
}