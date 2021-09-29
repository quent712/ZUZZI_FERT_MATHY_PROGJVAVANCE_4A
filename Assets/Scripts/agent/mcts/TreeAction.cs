using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private LinkedList<Node> children;

    public Node parent;

    public Action state; //Etat du joueur
    public Register data;

    public Node(Register data){
        this.data = data;
        this.children = new LinkedList<Node>();
    }
    public Node(Node parent, Register data){
        this.parent = parent;
        this.data = data;
        this.children = new LinkedList<Node>();
    }
    public Node AddChild(Register data)
    {
        Node n = new Node(data);
        children.AddFirst(n);
        return n;
    }

    public LinkedList<Node> getPossibleAction(){ //Recup les fils 
        return children;
    }
    public int nbChilden(){ //recup nombre de fils
        return children.Count;
    }

    public void setState(Action p){ //set un etat
        this.state = p;
    }

    public static void Retropropagation(Node node){  
        int i = 0;
        int validate = node.data.a;
        while(node.parent != null){
            node.parent.data.a += validate; //ON Incrémente le data de win du parent
            node.parent.data.b++; //On incrémente le data de try du parent
            node = node.parent;
            //if(i++ > 10000) break;

        }
    }

    public Node Exist(Action p ){
        if(children != null){
            foreach(var child in children){
                if(child.state == p){
                    return child;
                }
            }
        }
        return null;
    }
    public Node GetChild(int i)
    {
        foreach (Node n in children)
            if (--i == 0)
                return n;
        return null;
    }

}
