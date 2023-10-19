using System.Globalization;
using System.Net.WebSockets;
using System.Runtime.InteropServices;

public class Node
{
    public Notakto State { get; set; }
    public float Evaluation { get; set; } = 0;
    public List<Node> Children { get; set; } = new();
    public bool Expanded { get; set; } = false;
    public bool YouPlays { get; set; } = true;

    public Node Play(int board, int position)
    {
        // Seu código aqui...
        foreach(Node child in this.Children)
            if (child.State.GetLast() == (board, position))
                return child;
        
        return this;
    }

    public void Expand(int deep)
    {
        if (deep == 0)
            return;

        // Seu código aqui...
        if (Expanded)
        {
            foreach (var child in Children)
            {
                child.Expand(deep - 1);
            }   
        }

        Expanded = true;
        foreach (var item in State.Next())
        {
            Children.Add(new Node(){
                State = item
            });
        }
    }

    public Node PlayBest()
    {
        // Seu código aqui...
        float max = float.NegativeInfinity;
        Node bestNode = this.Children[0];

        foreach (Node child in this.Children)
            if (max < child.Evaluation)
                bestNode = child;
        
        return bestNode;
    }

    public float MiniMax()
    {
        if (this.isTerminalNode())
        {
            this.Evaluation = eval();
            return this.Evaluation;
        }
        
        bool maximize = this.YouPlays;

        float value;
        if (maximize)
        {
            value = float.NegativeInfinity;
            
            foreach (Node child in this.Children)
                value = Math.Max(value, child.MiniMax());

            return value;
        }
        else
        {
            value = float.PositiveInfinity;
            
            foreach (Node child in this.Children)
                value = Math.Min(value, child.MiniMax());

            return value;
        }
    }

    private float eval()
    {
        if (this.YouPlays)
            if (this.State.GameEnded())
                return float.NegativeInfinity;
                    
        if (!this.YouPlays)
            if (this.State.GameEnded())
                return float.PositiveInfinity;
        
        return 0;
    }

    private bool isTerminalNode() 
        => Children.Count == 0;
}