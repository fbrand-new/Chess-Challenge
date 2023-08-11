using ChessChallenge.API;
using System;
using System.Diagnostics;

public class MyBot : IChessBot
{
    int pawnVal;
    int knightVal;
    int bishopVal;
    int rookVal;
    int queenVal;

    public MyBot()
    {
        this.pawnVal = 10;
        this.knightVal = 30;
        this.bishopVal = 30;
        this.rookVal = 50;
        this.queenVal = 120;
    }

    public Move Think(Board board, Timer timer)
    {
        Move move = BestMove(board);
        return move;
    }

    public int Eval(Board board)
    {
        PieceList[] all_pieces = board.GetAllPieceLists();
      
        int whiteVal = 0;
        int blackVal = 0;
    
        foreach(PieceList pieces in all_pieces)
        {
            int piece_count = pieces.Count;
            bool white_piece = pieces.IsWhitePieceList;
            PieceType piece_type = pieces.TypeOfPieceInList;

            // Console.Write("piece_count: " + piece_count + "\n");
            // Console.Write("piece_type: " + piece_type + "\n");

            int val = 0; 
            if(piece_type == PieceType.Pawn)
            {
                val = pawnVal;
            }
            else if(piece_type == PieceType.Knight)
            {
                val = knightVal;
            }
            else if(piece_type == PieceType.Bishop)
            {
                val = bishopVal;
            }
            else if(piece_type == PieceType.Rook)
            {
                val = rookVal;
            }
            else if(piece_type == PieceType.Queen)
            {
                val = queenVal;
            }

            if(white_piece)
            {
                whiteVal += piece_count*val;
            }
            else
            {
                blackVal += piece_count*val;
            }
    
        }
     
        // Console.Write("iswhite: " + white);
        // Console.WriteLine("whiteval:" + whiteVal);
        // Console.WriteLine("blackVal:" + blackVal);
        int retval = (whiteVal - blackVal)*(board.IsWhiteToMove? 1 : -1);

        return retval;
        
    }

    public double Search(Board board, int depth,double beta,
                            double alpha, ref Move bestMove,
                            int maxDepth)
    {

        if(depth == 0)
        {
            return Eval(board);
        }

        Move[] moves = board.GetLegalMoves();
        
        if(moves.Length == 0)
        {
            Debug.Write("No possible move?");
            if (board.IsInCheck())
            {
                return double.NegativeInfinity; //checkmate
            }

            return 0; //stalemate
        }


        foreach(Move move in moves)
        {
            board.MakeMove(move);
            double score = -Search(board,depth-1,
                                    -alpha,-beta,ref bestMove,maxDepth);
            if(depth == maxDepth)
            {
                // Console.Write(move + "\n");
                // Console.Write("score:" + score + "\n");
                // Console.Write("beta:" + beta + "\n");
            }

            if(score >= beta)
            {
                board.UndoMove(move);
                return beta;
            }

            if(score > alpha)
            {
                if(depth == maxDepth)
                {
                    bestMove = move;
                }
                alpha = score;
            }
            board.UndoMove(move);
        }

        return alpha;
    }

    public Move BestMove(Board board)
    {
        Move bestMove = new Move {};
        int maxDepth = 5;
        double score = Search(board,maxDepth,
                                    Double.PositiveInfinity,
                                    Double.NegativeInfinity,
                                    ref bestMove,maxDepth);

        // Console.Write("\n\n");
        // Console.Write("Best move: " + bestMove);
        // Console.Write("\n\n");
        return bestMove;
    } 

}
