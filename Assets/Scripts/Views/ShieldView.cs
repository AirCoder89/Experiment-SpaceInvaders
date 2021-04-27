using System.Collections.Generic;
using Core;
using Models.SystemConfigs;
using UnityEngine;

namespace Views
{
    public class ShieldView : GameView
    {
        private Dictionary<string, ShieldPiece> _pieces;
        private ShieldConfig _config;
        
        public ShieldView(string inName, ShieldConfig inConfig) : base(inName)
        {
            _config = inConfig;
            GeneratePieces();
        }

        public override void Destroy()
        {
            base.Destroy();

            foreach (var piece in _pieces)
                piece.Value.Destroy();
            
            _pieces?.Clear();
            _pieces = null;
            _config = null;
        }

        private void GeneratePieces()
        {
            _pieces = new Dictionary<string, ShieldPiece>();
            for (var i = 0; i < _config.shieldDimension.y; i++)
            {
                var isLast = i == _config.shieldDimension.y - 1;
                for (var j = 0; j < _config.shieldDimension.x; j++)
                {
                    if (j == (int) (_config.shieldDimension.x / 2) && isLast) continue;
                    var pieceName = $"Piece [{i},{j}]";
                    var piece = new ShieldPiece(pieceName, _config, _config.meshPiece);
                    piece.SetParent(gameObject.transform);
                    piece.gameObject.transform.localScale = _config.pieceScale;
                    UpdatePosition(piece.gameObject, new Utils.Array2D.Vector2Int(i,j));
                    _pieces.Add(pieceName, piece);
                }
            }
        }

        public float GetWidth()
        {
            return (_config.pieceScale.x + _config.spacing) * 5;
        }
        
        private void UpdatePosition(GameObject inObject, Utils.Array2D.Vector2Int inLocation)
            => inObject.transform.position = LocationToPosition(inLocation);
        
        private Vector3 LocationToPosition(Utils.Array2D.Vector2Int inLocation)
        {
            var spacing = new Vector3(_config.spacing * inLocation.y, -_config.spacing * inLocation.x, 0f);
            var position = new Vector3(_config.pieceScale.x * inLocation.y, -_config.pieceScale.y * inLocation.x, 0f);
            return position + spacing;
        }
       
    }
}