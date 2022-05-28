﻿using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MainMenu : Menu
    {
        [SerializeField] private Button continueButton;

        protected override bool DisableOnStart => false;

        protected override void Start()
        {
            if (continueButton == null)
            {
                Debug.LogWarning("Continue button is not assigned", this);
            }
            else
            {
                var gameLoader = Container.Get<IGameLoader>();
                continueButton.interactable = gameLoader.HasAnyGameProgress;
            }
            base.Start();
        }
    }
}