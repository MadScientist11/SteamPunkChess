using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteampunkChess
{
    public class PromotionPopUpMediator : MonoBehaviour
    {
        [SerializeField] private PromotionPopUp _promotionPopUp;

        public void OnClick_ChoosePiecePromoteTo(int pieceIndex) => _promotionPopUp.ChoosePieceAndProcessPromotion(pieceIndex);
    }
}
