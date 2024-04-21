# What is this?

A simple solution written to parse a provided string representation of a binary tree, validate for errors, and print a simple text-based representation of that tree. 

* The input string is formatted as "(A,B) (A,C)"
* each node is separated by one space, each label is exactly one uppercase character, and there are a minimum of two nodes.
* The errors to check for include sanitizing the input format, checking for multiple roots, circular references, and multi-child parents or multi-parent children.

Once these errors are validated, the string should be printed in the format `(A(B)(C))`. An example is provided. 
