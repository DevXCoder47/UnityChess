using Board;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public enum Colour
    {
        White,
        Black
    }

    public enum Type
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }
    public class Piece : MonoBehaviour
    {
        [SerializeField] private Colour colour;
        [SerializeField] private Type type;
        [SerializeField] private int price;

        public List<Cell> MoveVariants { get; private set; }
        public Cell CellOccupied { get; private set; }

        public void MoveToCell(Cell cell)
        {

        }

        private void CalculateVariants(List<List<Cell>> chessBoardCells)
        {

        }

        private void PromoteToPiece(Type type)
        {

        }

        public void Die()
        {

        }
    }
}
