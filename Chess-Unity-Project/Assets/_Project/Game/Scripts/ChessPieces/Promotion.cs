using System.Collections.Generic;
using UnityEngine;


namespace SteampunkChess
{
    public class Promotion : ISpecialMove
    {
        public void ProcessSpecialMove()
        {
            //GameObject promotionPanel = Resources.Load<GameObject>("PromotionPanel");
            //Transform canvas = Object.FindObjectOfType<MainCanvas>().gameObject.transform;
            //FactoryInjector.gameController.WaitingForUserInput = true;
            //PromotionHelper promotionHelper = Object.Instantiate(promotionPanel, canvas).GetComponent<PromotionHelper>();
            //promotionHelper.ProcessPromotion(ref moveList,ref chessPieces, pieceCreator);
        }
        //public SpecialMoveStringBuilder GetStringBuilder(MoveInfo moveInfo)
        //{
        //    return new PromotionSpecialMoveStringBuilder(moveInfo);
        //}

        public void ProcessSpecialMove(List<Movement> moveList, PieceArrangement pieceArrangement)
        {
            throw new System.NotImplementedException();
        }
    }
    
}