using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : Singleton<GameMaster>
{
    private AudioManager audio_Manager;
    public AudioManager Audio_Manager { get { return audio_Manager; } }
    public Player PlayerInstance { get; private set; }
    [field:SerializeField] public Player PlayerPrefab { get; private set; }

    [SerializeField] private AudioClip bgm;
    [SerializeField] private List<SceneData> datas = new();
    [SerializeField] private UIMaster UIPrefab;

    public int spawnerIDForNextRoom = 0;

    public event Action<Player> OnSetNewPlayer;
    public event Action<Camera> OnChangeMainCam;

    #region MonoBehavior
    protected override void Awake()
    {
        base.Awake();

        if(!TryGetComponent(out audio_Manager))
        {
            audio_Manager = gameObject.AddComponent<AudioManager>();
        }
        
    }

    private void Start()
    {
        if(UIMaster.IsInstanceNull)
            Instantiate(UIPrefab);
        LoadSceneData(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadSceneData;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadSceneData;
    }
    #endregion

    private void LoadSceneData(Scene scene, LoadSceneMode mode)
    {

        try
        {
            if (scene.buildIndex == 0)
            {
                Destroy(gameObject);
                return;
            }
            bgm = datas[scene.buildIndex].BGM;
        }
        catch (ArgumentOutOfRangeException)
        {
            bgm = null;
        }
        catch (NullReferenceException)
        {
            bgm = null;
        }
        Audio_Manager.OverrideBGM(bgm);


    }

    public void SetPlayer(Player player)
    {
        PlayerInstance = player;
        OnSetNewPlayer?.Invoke(player);
    }

    public void SetMainCamera(Camera cam)
    {
        OnChangeMainCam?.Invoke(cam);
    }

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
