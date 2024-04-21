using System.ComponentModel;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Sandbox1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<treeNode> nodeList = new List<treeNode>();
            bool isE5 = false;
            string nodeDefinition = "(F,Z) (A,B) (C,X) (B,D) (D,E) (E,R) (A,C) (C,F) (E,G) (D,Y)";

            //E1 - Invalid input format
            string formatRegexPattern = @"(\([A-Z],[A-Z]\)[ ]?)+";
            Match formatMatch = Regex.Match(nodeDefinition, formatRegexPattern);
            if(!formatMatch.Success) { Console.WriteLine("E1"); return; }

            string[] nodeDefinitions = nodeDefinition.Split(' ');

            foreach(string nodeDef in nodeDefinitions)
            {
                string[] splitResult = nodeDef.TrimStart('(').TrimEnd(')').Split(',');

                string parentNode = splitResult[0];
                string childNode = splitResult[1];

                //E2 - Duplicate Pair
                //E3 - Parent has more than two children
                string parentRegexPattern = parentNode + ",[A-Z]";
                MatchCollection parentMatches = Regex.Matches(nodeDefinition, parentRegexPattern);
                for (int i = 0; i < parentMatches.Count; i++)
                {
                    for (int j = i+1; j < parentMatches.Count; j++)
                    {
                        if (parentMatches[i].ToString() == parentMatches[j].ToString()) { Console.WriteLine("E2"); return; }
                    }
                }
                if (parentMatches.Count > 2) { Console.WriteLine("E3"); return; }

                //E5 - input contains cycle (child with more than one parent)
                //A subset of E4 errors were misreported as E5 when two roots share a child node. 
                //Fixed by setting a boolean flag, then we finish processing the list.
                //After processing, we first check for E4 to catch these more specific cases. If we do not find E4, bool is checked, and E5 is reported.
                string childRegexPattern = "[A-Z]," + childNode;
                MatchCollection matches = Regex.Matches(nodeDefinition, childRegexPattern);
                if (matches.Count > 1) { isE5 = true; }

                //Create a node for this child if it doesn't exist, or update its parent node if needed.
                if(!(nodeList.Any(treeNode => treeNode.nodeName == childNode)))
                {
                    treeNode newNode = createNode(childNode);
                    newNode.parentName = parentNode;
                    nodeList.Add(newNode);
                }
                else if(nodeList.Find(treeNode => treeNode.nodeName == childNode).parentName == null)
                {
                    nodeList.Find(treeNode => treeNode.nodeName == childNode).parentName = parentNode;
                }

                //Create a node for the parent if it doesn't exist, with the child in the children list. If it does exist, add this child to children list
                if (!(nodeList.Any(treeNode => treeNode.nodeName == parentNode)))
                {
                    treeNode newNode = createNode(parentNode);
                    treeNode child = nodeList.Find(treenode => treenode.nodeName == childNode);
                    newNode.addChild(child);
                    nodeList.Add(newNode);
                }
                else
                {
                    nodeList.Find(treeNode => treeNode.nodeName == parentNode).addChild(nodeList.Find(treenode => treenode.nodeName == childNode));
                }
            }

            //E4 - Multiple Roots
            //E5 bool check resolves if the error is not E4
            List<treeNode> duplicateRootChecker = nodeList.FindAll(treeNode => treeNode.parentName == null);
            if(duplicateRootChecker.Count > 1)
            {
                Console.WriteLine("E4");
                return;
            }
            if(isE5) { Console.WriteLine("E5"); return; }

            Console.Write("(");
            printTree(nodeList.Find(treeNode => treeNode.parentName == null));
            Console.WriteLine();
            return;
        }

        static void printTree(treeNode node)
        {
            Console.Write(node.nodeName);
            foreach(treeNode child in node.getChildren())
            {
                Console.Write("(");
                printTree(child);
            }
            Console.Write(")");
        }

        public class treeNode
        {
            //TODO: Privatize attributes, add getters and setters.
            public string nodeName;
            public string parentName = null;
            private List<treeNode> children = new List<treeNode>();

            public List<treeNode> getChildren()
            {
                return children;
            }

            public void addChild(treeNode node)
            {
                children.Add(node);
            }
        }

        static treeNode createNode(string name)
        {
            //TODO: Ensure name matches expected format which is [A-Z]{1}
            treeNode node = new treeNode();
            node.nodeName = name;

            return node;
        }
    }
}
