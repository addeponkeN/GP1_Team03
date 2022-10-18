using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefPlayer;
    [SerializeField] private Transform[] _spawnPoints;

    private List<Player> _players;
    private GameObject _playerContainer;

    private void Awake()
    {
        _players = new();
        _playerContainer = new GameObject("PlayerContainer");
    }

    public void AddPlayers(int count)
    {
        for(int i = 0; i < count; i++)
        {
            var player = Instantiate(_prefPlayer, _playerContainer.transform)
                .GetComponent<Player>();
            _players.Add(player);
        }
    }

    public Player GetPlayer(int id)
    {
        return _players[id];
    }
    
}