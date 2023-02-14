using System;
using UnityEngine;

namespace fdddfdfs.Leaderboard
{
    [CreateAssetMenu(fileName = "LeaderboardEntityData", menuName = "Leaderboard/EntityData")]
    public class LeaderboardEntityData : ScriptableObject
    {
        [SerializeField] private Sprite _avatar;
        [SerializeField] private string _nickname;
        [SerializeField] private int _score;

        public Sprite Avatar => _avatar;

        public string Nickname => _nickname;

        public int Score => _score;

        public static readonly Comparison<LeaderboardEntityData> LeaderboardEntityDataComparer =
            (x, y) => y.Score - x.Score;
    }
}
