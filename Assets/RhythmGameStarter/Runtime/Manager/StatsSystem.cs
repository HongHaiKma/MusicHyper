using System;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    [HelpURL("https://bennykok.gitbook.io/rhythm-game-starter/hierarchy-overview/stats-system")]
    public class StatsSystem : Singleton<StatsSystem>
    {
        [Comment("Responsible for advance stats' config and events.", order = 0)]
        [Title("Hit Level Config", false, 2, order = 1)]
        [Tooltip("Config the hit distance difference for each level, such as Perfect,Ok etc")]
        public HitLevelList levels;

        [Title("Events", 2)]
        [CollapsedEvent]
        public StringEvent onComboStatusUpdate;
        [CollapsedEvent]
        public StringEvent onScoreUpdate;
        [CollapsedEvent]
        public StringEvent onMaxComboUpdate;
        [CollapsedEvent]
        public StringEvent onMissedUpdate;

        #region RUNTIME_FIELD
        [NonSerialized] public int combo;
        [NonSerialized] public int maxCombo;
        [NonSerialized] public int missed;
        [NonSerialized] public int score;
        #endregion

        [Serializable]
        public class HitLevelList : ReorderableList<HitLevel> { }

        [Serializable]
        public class HitLevel
        {
            public string name;
            public float threshold;
            [HideInInspector]
            public int count;
            public float addScore = 1;
            public StringEvent onCountUpdate;
        }

        public void AddMissed(int addMissed)
        {
            missed += addMissed;
            onMissedUpdate.Invoke(missed.ToString());
        }

        void Start()
        {
            UpdateScoreDisplay();

            if (TryGetComponent<SongManager>(out var manager))
            {
                manager.onSongStartPlay.AddListener(() =>
                {
                    if (GameManager.Instance.m_ModePlay != ModePlay.STORY)
                    {
                        score = 0;
                        combo = 0;
                        maxCombo = 0;
                        missed = 0;
                        UpdateScoreDisplay();
                    }
                });
            }
        }

        public void AddCombo(Note _note, int addCombo, float deltaDiff, int addScore)
        {
            // print(deltaDiff);
            combo += addCombo;
            if (combo > maxCombo)
            {
                maxCombo = combo;
                onMaxComboUpdate.Invoke(maxCombo.ToString());
            }

            for (int i = 0; i < levels.values.Count; i++)
            {
                var x = levels.values[i];
                if (deltaDiff <= x.threshold)
                {
                    x.count++;
                    score += (int)(x.addScore);
                    // x.onCountUpdate.Invoke(x.count.ToString());

                    if (x.threshold <= 0.5f)
                    {
                        GameObject go = GameManager.Instance.Sick();
                        NoteLevel noteLevel = go.GetComponent<NoteLevel>();
                        noteLevel.SetPosition(_note);
                        // Helper.DebugLog("Sick");
                    }
                    else if (x.threshold <= 0.7f)
                    {
                        GameObject go = GameManager.Instance.Good();
                        NoteLevel noteLevel = go.GetComponent<NoteLevel>();
                        noteLevel.SetPosition(_note);
                        // Helper.DebugLog("Good");
                    }
                    else if (x.threshold <= 0.85f)
                    {
                        GameObject go = GameManager.Instance.Bad();
                        NoteLevel noteLevel = go.GetComponent<NoteLevel>();
                        noteLevel.SetPosition(_note);
                        // Helper.DebugLog("Bad");
                    }
                    else
                    {
                        GameObject go = GameManager.Instance.Shit();
                        NoteLevel noteLevel = go.GetComponent<NoteLevel>();
                        noteLevel.SetPosition(_note);
                        // Helper.DebugLog("Shit");
                    }

                    UpdateScoreDisplay();
                    onComboStatusUpdate.Invoke(x.name);
                    // print(x.name);
                    return;
                }
            }

            //When no level matched
            onComboStatusUpdate.Invoke("");

        }

        public void UpdateScoreDisplay()
        {
            onScoreUpdate.Invoke(score.ToString());
        }
    }
}