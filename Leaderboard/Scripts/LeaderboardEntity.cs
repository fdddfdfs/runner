using System;
using UnityEngine;
using UnityEngine.UI;

namespace fdddfdfs.Leaderboard
{
    [RequireComponent(typeof(RectTransform))]
    public class LeaderboardEntity : MonoBehaviour
    {
        [SerializeField] private Text _number;
        [SerializeField] private Image _avatar;
        [SerializeField] private Text _nickname;
        [SerializeField] private Text _score;

        private RectTransform _rectTransform;
        private bool _isRectTransformNull = true;
        private int _scoreInt;

        public RectTransform RectTransform
        {
            get
            {
                if (_isRectTransformNull)
                {
                    _isRectTransformNull = false;
                    _rectTransform = GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }

        public static readonly Comparison<LeaderboardEntity> LeaderboardEntityComparison =
            (x, y) => y._scoreInt - x._scoreInt;

        public void Init(LeaderboardEntityData data)
        {
            _avatar.sprite = data.Avatar;
            _nickname.text = data.Nickname;
            _score.text = data.Score.ToString();
            _scoreInt = data.Score;
        }

        public void Init(Sprite avatar, string nickname, int score)
        {
            _avatar.sprite = avatar;
            _nickname.text = nickname;
            _score.text = score.ToString();
            _scoreInt = score;
        }

        public void SetIndex(int number)
        {
            _number.text = number.ToString();
        }
    }
}
