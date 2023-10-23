using System.Security.Cryptography.X509Certificates;

public class Notakto
{
    int boards = 0;
    bool[] data;
    byte[] sums;
    int lastBoard = 0;
    int lastPosition = 0;

    public Notakto(int boards)
    {
        this.boards = boards;
        data = new bool[9 * boards];
        sums = new byte[8 * boards]; 

        //sums ->
            // Na horizontal: Cima, Meio, Baixo
            // Na vertical: Esquerda, Meio, Direita
            // Na diagonal: Direita, Esquerda
    }
    
    /// <summary>
    /// Obtém a última jogada
    /// </summary>
    public (int board, int position) GetLast()
        => (lastBoard, lastPosition);

    /// <summary>
    /// Joga em uma posição em um tabuleiro
    /// </summary>
    public void Play(int board, int position)
    {
        lastBoard = board;
        lastPosition = position;

        data[9 * board + position] = true;
        
        int sumInitialPos = 8 * board; 
        // Na vertical: Esquerda, Meio, Direita
        sums[sumInitialPos + position % 3] += 1;
        // Na horizontal: Cima, Meio, Baixo
        sums[sumInitialPos + 3 + position / 3] += 1;

        // Na diagonal: Direita, Esquerda
        if (position % 4 == 0)
            sums[sumInitialPos + 6] += 1;

        if (position % 2 == 0 && position % 8 != 0)
            sums[sumInitialPos + 7] += 1;
    }
    
    /// <summary>
    /// Testa e retorna verdadeiro se você pode jogar em um tabuleiro
    /// </summary>
    public bool CanPlay(int board)
    {
        int boardInArr = board * 8;
        for (int i = boardInArr; i < boardInArr + 8; i++) {
            if (sums[i] >= 3)
                return false;
        }

        return true;
    }
    
    /// <summary>
    /// Retorna verdadeiro se o jogo acabou
    /// </summary>
    public bool GameEnded()
    {
        for (int i = 0; i < boards; i++)
            if (CanPlay(i))
                return false;
        
        return true;
    }

    /// <summary>
    /// Cria uma cópia indentica do estado.
    /// </summary>
    public Notakto Clone()
    {
        Notakto copy = new Notakto(boards);
        Array.Copy(
            this.data, 
            copy.data, 
            this.data.Length
        );
        Array.Copy(
            this.sums, 
            copy.sums, 
            this.sums.Length
        );
        return copy;
    }

    /// <summary>
    /// Obtém próximas jogadas válidas
    /// </summary>
    public IEnumerable<Notakto> Next()
    {
        // Seu código aqui...
        List<Notakto> validMove = new();

        for(int i = 0; i < data.Length; i++)
        {
            if (!CanPlay(i / 9))
                continue;

            if (!data[i])
            {
                Notakto newState = this.Clone();
                newState.Play(i / 9, i % 9);

                validMove.Add(
                    newState
                );
            }
        }

        return validMove;
    }
    public void Print(){
        string str = "";
        int i = 1;
        int board = 1;
        foreach(var item in this.data)
        {
            str += item ? " X " : " . ";
            str += "|";
            
            if(i % 3 == 0)
                str += "\n";
            if(board % 9 == 0)
                str += "\n\n";

            board += 1;
            i += 1;
        }

        Console.WriteLine(str);
    }
}