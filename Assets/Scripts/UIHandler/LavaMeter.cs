using Dinojump.Schemas;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class LavaMeter : MonoBehaviour
{
    VisualElement lavaBar;
    VisualElement lavaMeter;

    float onePercentScale;

    List<PlayerBarData> allPlayerBars;

    // Start is called before the first frame update
    void OnEnable()
    {
        allPlayerBars = new List<PlayerBarData>();
        var root = GetComponent<UIDocument>().rootVisualElement;
        lavaMeter = root.Q<VisualElement>("LavaMeterBG");
        lavaBar = root.Q<VisualElement>("LavaLevel");

        //lavameter height is 300
        onePercentScale = 300 / 100;

        lavaMeter.Clear();

        foreach (var player in GameManager.Instance.playerList)
        {
            VisualElement vE = new VisualElement();
            var barData = new PlayerBarData()
            {
                Position = player.Value.position.y,
                VisualElement = vE,
                PlayerKey = player.Key,
                IsPlayer = player.Key == GameManager.Instance.myPlayerKey,
                IsDead = player.Value.isDead
            };

            vE.AddToClassList(barData.IsPlayer ? "playerbar" : "enemybar");

            lavaMeter.Add(vE);
            allPlayerBars.Add(barData);
        }

        lavaBar = new VisualElement();
        lavaBar.AddToClassList("lavabar");
        lavaMeter.Add(lavaBar);
    }

    // Update is called once per frame
    void Update()
    {
        var lavaObject = GameObject.Find("LavaContainer").GetComponent<LavaController>();
        var lavaLevel = lavaObject.floorSchema.position.y > 0 ? lavaObject.floorSchema.position.y : 0;
        var zeroPercent = lavaLevel -300;
        if (zeroPercent < 0) zeroPercent = 0;
        UpdatePlayerPositions(allPlayerBars);
        
        float onePercentWorld = (allPlayerBars.Max(m => m.Position) - zeroPercent) / 100;
        
        foreach (var player in allPlayerBars)
        {
            if (player.IsDead)
            {
                player.VisualElement.style.display = DisplayStyle.None;
                return;
            }
             var playerPercentage = ((player.Position - zeroPercent ) / onePercentWorld) - 1;
            player.VisualElement.style.bottom = ((playerPercentage * onePercentScale) - 300);
        }

        var lavaPercentage = (lavaLevel - zeroPercent) / onePercentWorld;
        var lavaPosition = ((lavaPercentage * onePercentScale) - 290);
        lavaBar.style.bottom = lavaPosition;
    }

    void UpdatePlayerPositions(List<PlayerBarData> playerData)
    {
        foreach (var p in playerData)
        {
            p.Position = GameManager.Instance.playerList[p.PlayerKey].position.y;
            p.IsDead = GameManager.Instance.playerList[p.PlayerKey].isDead;
        }
    }
}
public class PlayerBarData
{
    public bool IsPlayer { get; set; }
    public string PlayerKey { get; set; }
    public float Position { get; set; }

    public bool IsDead {get;set;}
    public VisualElement VisualElement { get; set; }
}
