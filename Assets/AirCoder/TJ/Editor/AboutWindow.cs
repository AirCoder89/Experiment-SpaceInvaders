using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AirCoder.TJ.Editor
{
    public class AboutWindow : EditorWindow
    {
        [MenuItem("Tools/About TJS")]
        static void ShowAboutWindow()
        {
            AboutWindow w = EditorWindow.GetWindowWithRect<AboutWindow>(new Rect(100, 100, 470, 360), true, "TJS - Tween Job System");
            w.position = new Rect(100, 100, 570, 360);
        }

        private static GUIContent s_MonoLogo, s_AgeiaLogo, s_Header;

        private const string scrollText1 = "Scripting powered by AirCoder 2021.";
        private const string scrollText2 = "TJS version 1.0.0";
        private const string scrollText3 = "";
        private const string scrollText4 = "TJS it's an optimized animation engine for Unity with API made to boost efficiency,";
        private const string scrollText5 = "intuitiveness and ease of use.";
       
        private const string githubLink = "https://github.com/AirCoder89/TJS-Tween-Job-System-";
        private const string linkedInLink = "https://www.linkedin.com/in/atef-sassi-a84334124";
        private const string facebookLink = "https://www.facebook.com/Sassi.3atef";
           
        private static void LoadLogos()
        {
            if (s_MonoLogo != null)
                return;
            s_MonoLogo = EditorGUIUtility.IconContent("console.infoicon");
            s_Header = EditorGUIUtility.IconContent("AboutWindow.MainHeader");
        }

        public void OnEnable()
        {
            EditorApplication.update += UpdateScroll;
            m_LastScrollUpdate = EditorApplication.timeSinceStartup;
        }

        public void OnDisable()
        {
            EditorApplication.update -= UpdateScroll;
        }

        float m_TextYPos = 120;
        float m_TextInitialYPos = 120;
        float m_TotalCreditsHeight = Mathf.Infinity;

        double m_LastScrollUpdate = 0.0f;

        public void UpdateScroll()
        {
            double deltaTime = EditorApplication.timeSinceStartup - m_LastScrollUpdate;
            m_LastScrollUpdate = EditorApplication.timeSinceStartup;

            if (GUIUtility.hotControl != 0)
                return;

            m_TextYPos -= 40f * (float)deltaTime;
            if (m_TextYPos < -m_TotalCreditsHeight)
                m_TextYPos = m_TextInitialYPos;
            Repaint();
        }

        bool m_ShowDetailedVersion = false;
        public void OnGUI()
        {
            LoadLogos();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label(s_Header, GUIStyle.none);

            ListenForSecretCodes();

            GUILayout.BeginHorizontal();
            GUILayout.Space(52f); // Ident version information

            if (Event.current.type == EventType.ValidateCommand)
                return;

            GUILayout.EndHorizontal();
            GUILayout.Space(4);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();

            float creditsWidth = position.width - 10;
            float chunkOffset = m_TextYPos;

            Rect scrollAreaRect = GUILayoutUtility.GetRect(10, m_TextInitialYPos);
            GUI.BeginGroup(scrollAreaRect);
           // foreach (string nameChunk in AboutWindowNames.Names(null, true))
                //chunkOffset = DoCreditsNameChunk(nameChunk, creditsWidth, chunkOffset);
            chunkOffset = DoCreditsNameChunk(scrollText1, creditsWidth, chunkOffset);
            chunkOffset = DoCreditsNameChunk(scrollText2, creditsWidth, chunkOffset);
            chunkOffset = DoCreditsNameChunk(scrollText3, creditsWidth, chunkOffset);
            chunkOffset = DoCreditsNameChunk(scrollText4, creditsWidth, chunkOffset);
            chunkOffset = DoCreditsNameChunk(scrollText5, creditsWidth, chunkOffset);

            m_TotalCreditsHeight = chunkOffset - m_TextYPos;
            GUI.EndGroup();

            HandleScrollEvents(scrollAreaRect);

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.Label(s_MonoLogo);
            
            //GUILayout.BeginVertical();
            GUILayout.BeginVertical();
            var linkStyle = new GUIStyle(GUI.skin.label);
            linkStyle.normal.textColor = Color.cyan;
            linkStyle.focused.textColor = Color.yellow;
            linkStyle.active.textColor = Color.gray;
            if (GUILayout.Button($"- Github:         {githubLink}", linkStyle))
            {
                Application.OpenURL(githubLink);
            }
            if (GUILayout.Button($"- LinkedIn:     {linkedInLink}", linkStyle))
            {
                Application.OpenURL(githubLink);
            }
            if (GUILayout.Button($"- Facebook:   {facebookLink}", linkStyle))
            {
                Application.OpenURL(githubLink);
            }
            GUILayout.Label($"- Email :          sassi.3atef@gmail.com");
          
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

         
            GUILayout.Label(InternalEditorUtility.GetUnityCopyright(), "MiniLabel");
            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label(InternalEditorUtility.GetLicenseInfo(), "AboutWindowLicenseLabel");
            GUILayout.EndVertical();
            GUILayout.Space(5);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
        }

        private void HandleScrollEvents(Rect scrollAreaRect)
        {
            int id = GUIUtility.GetControlID(FocusType.Passive);

            switch (Event.current.GetTypeForControl(id))
            {
                case EventType.MouseDown:
                    if (scrollAreaRect.Contains(Event.current.mousePosition))
                    {
                        GUIUtility.hotControl = id;
                        Event.current.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == id)
                    {
                        m_TextYPos += Event.current.delta.y;
                        m_TextYPos = Mathf.Min(m_TextYPos, m_TextInitialYPos);
                        m_TextYPos = Mathf.Max(m_TextYPos, -m_TotalCreditsHeight);
                        Event.current.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == id)
                    {
                        GUIUtility.hotControl = 0;
                        Event.current.Use();
                    }
                    break;
            }
        }

        private static float DoCreditsNameChunk(string nameChunk, float creditsWidth, float creditsChunkYOffset)
        {
            float creditsNamesHeight = EditorStyles.wordWrappedLabel.CalcHeight(new GUIContent(), creditsWidth);
            Rect creditsNamesRect = new Rect(5, creditsChunkYOffset, creditsWidth, creditsNamesHeight);
            GUI.Label(creditsNamesRect, nameChunk, EditorStyles.wordWrappedLabel);
            return creditsNamesRect.yMax;
        }
        
        private int m_InternalCodeProgress;
        private void ListenForSecretCodes()
        {
            if (Event.current.type != EventType.KeyDown || (int)Event.current.character == 0)
                return;

            if (SecretCodeHasBeenTyped("internal", ref m_InternalCodeProgress))
            {
                bool enabled = !EditorPrefs.GetBool("DeveloperMode", false);
                EditorPrefs.SetBool("DeveloperMode", enabled);
                ShowNotification(new GUIContent(string.Format(L10n.Tr("Developer Mode {0}"), (enabled ? L10n.Tr("On") : L10n.Tr("Off")))));
                EditorUtility.RequestScriptReload();

                // Repaint all views to show/hide debug repaint indicator
                InternalEditorUtility.RepaintAllViews();
            }
        }

        private bool SecretCodeHasBeenTyped(string code, ref int characterProgress)
        {
            if (characterProgress < 0 || characterProgress >= code.Length || code[characterProgress] != Event.current.character)
                characterProgress = 0;

            // Don't use else here. Even if key was mismatch, it should still be recognized as first key of sequence if it matches.
            if (code[characterProgress] == Event.current.character)
            {
                characterProgress++;

                if (characterProgress >= code.Length)
                {
                    characterProgress = 0;
                    return true;
                }
            }
            return false;
        }

    }
}
