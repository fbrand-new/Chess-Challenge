using ChessChallenge.API;
using System;

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
        Move[] moves = board.GetLegalMoves();
        Move move = BestMove(board);
        return move;
    }

    public int Eval(Board board, bool white)
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
      
        int retval = (whiteVal - blackVal)*(white ? 1 : -1);

        return retval;
        
    }

    public (Move,int) Search(Board board, int depth, bool white)
    {
        if(depth == 0)
        {
            return (new Move {},-Eval(board,white));
        }

        Move[] moves = board.GetLegalMoves();
        Move bestMove = new Move {};
        int max = -1000;
        foreach(Move move in moves)
        {
            board.MakeMove(move);
            (_, int score) = Search(board,depth-1,white);
            score = -score;

            Console.Write(move+"\n");
            Console.Write("score:" + score + "\n");
            if(score >= max)
            {
                Console.Write("Overwriting best move \n");
                max = score;
                bestMove = move;
            }
            board.UndoMove(move);
        }

        return (bestMove,max);
    }

    public Move BestMove(Board board)
    {
        (Move bestMove, int score) = Search(board,3,board.IsWhiteToMove);

        Console.Write("\n\n");
        Console.Write("Best move: " + bestMove);
        Console.Write("\n\n");
        return bestMove;
    } 

}
