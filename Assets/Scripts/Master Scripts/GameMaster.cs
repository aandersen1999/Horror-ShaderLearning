using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }
    public AudioManager Audio_Manager { get; private set; }
    public UIMaster UIMaster { get; private set; }
    public Player PlayerInstance { get; private set; }

    [SerializeField] private AudioClip bgm;
    [SerializeField] private List<SceneData> datas = new List<SceneData>();
    [SerializeField] private GameObject UIPrefab;

    #region MonoBehavior
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Audio_Manager = GetComponent<AudioManager>();
        UIMaster = GetComponent<UIMaster>();

        GameObject UI = Instantiate(UIPrefab);
        DontDestroyOnLoad(UI);
        UIMaster.SetUIContainer(UI.GetComponent<UIContainer>());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {

    }


    private void Update()
    {
        
    }
    #endregion

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UIMaster.BlackScreen(false);

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
        Audio_Manager.OverrideBGM(bgm);


    }

    public void SetPlayer(Player player)
    {
        PlayerInstance = player;
    }

    public void ChangeScene(int index)
    {
        UIMaster.BlackScreen(true);
        SceneManager.LoadScene(index);
    }
}
