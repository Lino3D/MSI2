using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree
{
   public class Node<T>
    {
        private T data;
        private Node<T> Left;
        private Node<T> Right;

        public T value
        {
            get { return data; }
            set { data = value; }
        }
        public Node()
            {
         
            }
        public Node(T data) : this(data, null, null) { }
       // public Node(T data, Node<T> Left) : this(data, Left,null) { }
        public Node(T data, Node<T> L, Node<T> R)
        {
            this.data = data;
            this.Left = L;
            this.Right = R;
        }





    }
}
