using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;
public class GameoverLeaderBoard: MonoBehaviour
{
    public GameObject PlayerHolder;

    [Header("Options")]
    public float refreshRate = 1f;

    [Header("UI")]
    public GameObject[] slots;

    [Space]
    public TextMeshProUGUI[] scoreTexts;
    public TextMeshProUGUI[] nameTexts;
    public TextMeshProUGUI[] kdTexts;


    private void Start()
    {
        InvokeRepeating(nameof(Refresh), 1f, refreshRate);
    }


// Cách cũ
    // public void Refresh()
    // {
    //     foreach (var slot in slots)
    //     {
    //         slot.SetActive(false);
    //     }

    //     var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

    //     int i = 0;
    //     foreach (var player in sortedPlayerList)
    //     {
    //         slots[i].SetActive(true);

    //         if (player.NickName == "")
    //             player.NickName = "Unnamed";

    //         nameTexts[i].text = player.NickName;
    //         scoreTexts[i].text = player.GetScore().ToString();

    //         if (player.CustomProperties["kills"] != null)
    //         {
    //             kdTexts[i].text = player.CustomProperties["kills"] + "/" + player.CustomProperties["deaths"];
    //         }
    //         else
    //         {
    //             kdTexts[i].text = $"0/0";
    //         }

    //         i++;
    //     }
    // }

// Cách mới, an toàn hơn, vẫn chưa hoàn thiện
//     public void Refresh()
// {
//     // Ẩn tất cả slot trước
//     foreach (var slot in slots)
//     {
//         slot.SetActive(false);
//     }

//     var sortedPlayerList = PhotonNetwork.PlayerList
//         .OrderByDescending(p => p.GetScore())
//         .ToList();

//     int maxSlots = Mathf.Min(sortedPlayerList.Count, slots.Length);

//     for (int i = 0; i < maxSlots; i++)
//     {
//         var player = sortedPlayerList[i];

//         slots[i].SetActive(true);

//         string nick = string.IsNullOrEmpty(player.NickName) ? "Unnamed" : player.NickName;
//         nameTexts[i].text = nick;
//         scoreTexts[i].text = player.GetScore().ToString();

//         // Cách an toàn lấy K/D
//         if (player.CustomProperties.TryGetValue("kills", out object killsObj) &&
//             player.CustomProperties.TryGetValue("deaths", out object deathsObj))
//         {
//             int kills = (int)killsObj;
//             int deaths = (int)deathsObj;
//             kdTexts[i].text = $"{kills}/{deaths}";
//         }
//         else
//         {
//             kdTexts[i].text = "0/0";
//         }
//     }

//     // Optional: Nếu muốn highlight người chơi local
//     // for (int i = 0; i < maxSlots; i++) { ... if (sortedPlayerList[i] == PhotonNetwork.LocalPlayer) ... }
// }
    public void Refresh()
{
    // Ẩn hết tất cả slot trước để tránh dữ liệu cũ
    for (int i = 0; i < slots.Length; i++)
    {
        slots[i].SetActive(false);
    }

    // Lấy danh sách player còn sống trong room và sắp xếp theo điểm
    var sortedPlayerList = PhotonNetwork.PlayerList
        .OrderByDescending(p => p.GetScore())
        .ToList();

    // Chỉ lặp tối đa số slot có sẵn
    int maxDisplay = Mathf.Min(sortedPlayerList.Count, slots.Length);

    for (int i = 0; i < maxDisplay; i++)
    {
        var player = sortedPlayerList[i];

        slots[i].SetActive(true);

        // Tên player
        string displayName = string.IsNullOrEmpty(player.NickName) ? "Unnamed" : player.NickName;
        nameTexts[i].text = displayName;

        // Score
        scoreTexts[i].text = player.GetScore().ToString();

        // K/D an toàn hơn
        if (player.CustomProperties.TryGetValue("kills", out object killsObj) &&
            player.CustomProperties.TryGetValue("deaths", out object deathsObj))
        {
            kdTexts[i].text = $"{killsObj}/{deathsObj}";
        }
        else
        {
            kdTexts[i].text = "0/0";
        }

        // Optional: Highlight người chơi local (để dễ nhìn)
        // if (player == PhotonNetwork.LocalPlayer)
        //     nameTexts[i].color = Color.yellow; // hoặc bật outline...
    }

    // Nếu muốn hiện "Waiting for more players..." khi ít người, bạn có thể thêm logic ở đây
}
}
