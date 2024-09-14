using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager
{
    public void Init(GameManager gameManager , CameraGridManager cameraGridManager, CubeManager cubeManager, SaveManager saveManager , PlayerManager playerManager , UIManager uiManager , CameraManager cameraManager)
    {
        Command.Init(gameManager , cameraGridManager, cubeManager, saveManager , playerManager , uiManager , cameraManager);
    }
}
