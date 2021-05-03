using UnityEngine;
using UnityEngine.UI;

namespace UI.Leaderboards
{
    public class LeaderBoardItem : MonoBehaviour
    {
        private bool _isClear;
        private Text _txt;
        private Text _targetText
        {
            get
            {
                if (_txt == null) _txt = GetComponentInChildren<Text>();
                return _txt;
            }
        }

        public void Clear()
        {
            _isClear = true;
            gameObject.SetActive(false);
        }
        
        public void BindData(int inRank, string inName, int inScore)
        {
            _isClear = false;
            gameObject.SetActive(true);
            _targetText.text = $"   {inRank}- {inName.ToUpper()}			                     {inScore}";
        }
    }
}