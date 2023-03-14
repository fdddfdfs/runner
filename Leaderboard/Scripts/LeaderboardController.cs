using System.Collections.Generic;
using System;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;

namespace fdddfdfs.Leaderboard
{
    public class LeaderboardController : Menu
    {
        private const string LeaderboardEntityPrefabName = "UI/Leaderboard/LeaderboardEntity";

        [SerializeField] private List<LeaderboardEntityData> _leaderboardEntityData;
        [SerializeField] private RectTransform _leaderboardEntitiesParent;
        [SerializeField] private Text _nickname;
        [SerializeField] private Text _score;

        private LeaderboardEntityData[] _sortedLeaderboardEntities;
        private LeaderboardEntity _leaderboardEntityPrefab;
        private List<LeaderboardEntity> _entities;

        private CallResult<LeaderboardFindResult_t> _leaderboardCallResult;
        private CallResult<LeaderboardScoresDownloaded_t> _leaderboardDownloaded;
        private CallResult<LeaderboardScoreUploaded_t> _leaderboardUploaded;
        private SteamLeaderboard_t _currentLeaderboard;

        private string _currentLeaderboardName;
        private bool _isInitialized;


        private void RefreshLeaderboard(string leaderboardName)
        {
            if (SteamManager.Initialized)
            {
                FindLeaderboard(leaderboardName);
            }
            else
            {
                DownloadedLeaderboard(default, true);
            }
        }

        public void SetCurrentLeaderboard(string leaderboardName)
        {
            _currentLeaderboardName = leaderboardName;

            if (!_isInitialized)
            {
                Initialize();
            }
            
            RefreshLeaderboard(_currentLeaderboardName);
        }

        public void UploadResult(int score)
        {
            if (!SteamManager.Initialized) return;
            
            _leaderboardUploaded.Set(
                SteamUserStats.UploadLeaderboardScore(
                    _currentLeaderboard,
                    ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest,
                    score,
                    null,
                    0));
        }

        public override void ChangeMenuActive(bool active)
        {
            base.ChangeMenuActive(active);

            if (active)
            {
                if (_currentLeaderboard.m_SteamLeaderboard == 0)
                {
                    RefreshLeaderboard(_currentLeaderboardName);
                }
                else
                {
                    _leaderboardDownloaded.Set(SteamUserStats.DownloadLeaderboardEntries(
                        _currentLeaderboard,
                        ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal,
                        1,
                        100));
                }
            }
        }

        private void Initialize()
        {
            _leaderboardUploaded = CallResult<LeaderboardScoreUploaded_t>.Create(LeaderboardUpdated);
            _leaderboardCallResult = CallResult<LeaderboardFindResult_t>.Create(GetLeaderboard);
            _leaderboardDownloaded = CallResult<LeaderboardScoresDownloaded_t>.Create(DownloadedLeaderboard);

            _isInitialized = true;
        }

        private void Awake()
        {
            _leaderboardEntityPrefab = LoadLeaderboardEntityPrefab();
            SortLeaderboardEntityData(_leaderboardEntityData);
            _sortedLeaderboardEntities = _leaderboardEntityData.ToArray();
            _entities = new List<LeaderboardEntity>();
            RecreateLeaderboardPlayer(_sortedLeaderboardEntities.Length);
            for (int i = 0; i < _entities.Count; i++)
            {
                SetPlayerInfoInLeaderboard(i, _sortedLeaderboardEntities[i]);
            }

            _score.text = "Score";
            _nickname.text = "Nickname";
        }

        private void LeaderboardUpdated(LeaderboardScoreUploaded_t leaderboardScoreUploadedT, bool bIOFailure)
        {
            Debug.Log("success: " + leaderboardScoreUploadedT.m_bSuccess);
            Debug.Log("changed: " + leaderboardScoreUploadedT.m_bScoreChanged);
        }

        private void GetLeaderboard(LeaderboardFindResult_t respond, bool success)
        {
            _currentLeaderboard = respond.m_hSteamLeaderboard;
            _leaderboardDownloaded.Set(SteamUserStats.DownloadLeaderboardEntries(
                respond.m_hSteamLeaderboard,
                ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal,
                1,
                100));
        }

        private void FindLeaderboard(string leaderboardName)
        {
            _leaderboardCallResult.Set(SteamUserStats.FindOrCreateLeaderboard(leaderboardName,ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending,ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric));
        }

        private void SetPlayerInfoInLeaderboard(int index, string nickname, int score, Texture2D avatar)
        {
            Sprite avatarSprite = Sprite.Create(
                avatar,
                new Rect(0, 0, avatar.width, avatar.height),
                new Vector2(0f, 0f));
            _entities[index].Init(avatarSprite, nickname, score);
        }

        private void SetPlayerInfoInLeaderboard(int index, LeaderboardEntityData data)
        {
            _entities[index].Init(data);
        }

        private void DownloadedLeaderboard(LeaderboardScoresDownloaded_t respond, bool success)
        {
            RecreateLeaderboardPlayer(respond.m_cEntryCount + _sortedLeaderboardEntities.Length);

            int[] details = new int[200];
            for (int i = 0; i < respond.m_cEntryCount; i++)
            {
                LeaderboardEntry_t leaderboardEntry = default;
                if (SteamManager.Initialized)
                {
                    SteamUserStats.GetDownloadedLeaderboardEntry(
                        respond.m_hSteamLeaderboardEntries,
                        i,
                        out leaderboardEntry,
                        details,
                        200);
                }

                Texture2D avatarTexture = GetPlayerAvatar(leaderboardEntry.m_steamIDUser);

                SetPlayerInfoInLeaderboard(
                    i,
                    SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser),
                    leaderboardEntry.m_nScore,
                    avatarTexture);
            }

            for (int i = 0; i < _sortedLeaderboardEntities.Length; i++)
            {
                SetPlayerInfoInLeaderboard(respond.m_cEntryCount + i, _sortedLeaderboardEntities[i]);
            }

            SortLeaderboardEntity(_entities);
            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].transform.SetSiblingIndex(i);
                _entities[i].SetIndex(i + 1);
            }
        }

        private void RecreateLeaderboardPlayer(int newEntitiesCount)
        {
            _leaderboardEntitiesParent.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                _leaderboardEntityPrefab.RectTransform.sizeDelta.y * newEntitiesCount);

            int oldCount = _entities.Count;
            for (int i = _entities.Count; i < newEntitiesCount; i++)
            {
                GameObject newEntity = Instantiate(
                    _leaderboardEntityPrefab.gameObject,
                    _leaderboardEntitiesParent.transform);
                _entities.Add(newEntity.GetComponent<LeaderboardEntity>());
            }

            for (int i = 0; i < newEntitiesCount; i++)
            {
                _entities[i].gameObject.SetActive(true);
            }

            for (int i = newEntitiesCount; i < oldCount; i++)
            {
                _entities[i].gameObject.SetActive(false);
            }
        }

        private static void SortLeaderboardEntityData(List<LeaderboardEntityData> leaderboardEntities)
        {
            leaderboardEntities.Sort(LeaderboardEntityData.LeaderboardEntityDataComparer);
        }

        private static void SortLeaderboardEntity(List<LeaderboardEntity> leaderboardEntities)
        {
            leaderboardEntities.Sort(LeaderboardEntity.LeaderboardEntityComparison);
        }

        private static LeaderboardEntity LoadLeaderboardEntityPrefab()
        {
            GameObject leaderboardEntityObject = Resources.Load(LeaderboardEntityPrefabName) as GameObject;
            if (leaderboardEntityObject == null)
            {
                throw new Exception($"Resources folder do not contain {LeaderboardEntityPrefabName}");
            }

            return leaderboardEntityObject.GetComponent<LeaderboardEntity>();
        }

        private static Texture2D GetPlayerAvatar(CSteamID playerID)
        {
            int avatar = SteamFriends.GetMediumFriendAvatar(playerID);
            SteamUtils.GetImageSize(avatar, out uint width, out uint height);
            byte[] mas = new byte[width * height * 4];
            SteamUtils.GetImageRGBA(avatar, mas, mas.Length);
            Texture2D avatarTexture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
            Color[] colors = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colors[((int)height - 1 - y) * height + x] = new Color(
                        mas[y * width * 4 + x * 4] / 255f,
                        mas[y * width * 4 + x * 4 + 1] / 255f,
                        mas[y * width * 4 + x * 4 + 2] / 255f,
                        1);
                }
            }

            avatarTexture.SetPixels(colors);
            avatarTexture.Apply();

            return avatarTexture;
        }
    }
}
